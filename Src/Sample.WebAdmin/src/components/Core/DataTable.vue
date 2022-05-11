<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	name: "data-table",
	mixins: [BaseMixin],

	props: {
		objectId: { type: String },
		fields: { type: Array, required: true },
		loading: { type: Boolean },
		items: { types: Array },
		total: { type: Number },
		pagination: { type: Object, default: () => ({ page: 1 }) },
		api: { type: String },
		controller: { type: String },
		hideAction: { type: Boolean, default: false },
	},

	components: {
		FieldItem: () => import("@/components/Core/FieldItem"),
	},

	computed: {
		_pagination: {
			get() {
				return this.pagination;
			},
			set(value) {
				let pagination = this.pagination;
				if (
					Object.getOwnPropertyNames(pagination).length ===
					Object.getOwnPropertyNames(value).length
				)
					pagination = value;
				else pagination = { ...value, page: pagination.page };

				if (pagination.page === undefined || pagination.page === null)
					pagination.page = 1;

				this.$emit("update:pagination", pagination);
			},
		},

		pageSize() {
			return this._pagination.itemsPerPage ?? 10;
		},

		pageCount() {
			return Math.ceil(this.total / this.pageSize);
		},

		headers() {
			let headers = [];

			for (let field of this.fields) {
				if (!field.render) continue;

				headers.push({
					value: field.key,
					text: this.$strFormat(field.name, "capitalize"),
					sortable: !!field.sortColumn,
					align: field.type === "number" ? "end" : "start",
				});
			}

			headers.push({
				value: "[action]",
				text: "",
				sortable: false,
			});

			return headers;
		},

		deleteMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller,
				type: CONST.METHOD_TYPE_DELETE,
				firstOrDefault: true,
			});
		},

		restoreMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller,
				type: CONST.METHOD_TYPE_RESTORE,
				firstOrDefault: true,
			});
		},
	},

	methods: {
		detailUrl(item) {
			if (!this.controller) return null;

			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller: this.controller,
				id: item.id,
			});
		},

		async deleteItem(item) {
			await this.doSubmit({
				form: null,
				toggleLoading: false,
				confirmMessage: `bạn chắc chắn muốn khóa mục`,
				successMessage: `đã khóa thành công`,
				handler: async () => {
					await this.requestApi({
						controllerMethod: this.deleteMethod,
						path: { id: item.id },
					});

					this.$emit("item-deleted");
				},
			});
		},

		async restoreItem(item) {
			await this.doSubmit({
				form: null,
				confirm: true,
				toggleLoading: false,
				confirmMessage: `bạn chắc chắn muốn mở khóa mục`,
				successMessage: `đã mở khóa thành công`,
				handler: async () => {
					await this.requestApi({
						controllerMethod: this.restoreMethod,
						path: { id: item.id },
					});

					this.$emit("item-restored");
				},
			});
		},
	},
};
</script>

<template>
	<div class="data-table">
		<v-data-table
			class="elevation-1"
			:headers="headers"
			:items="items"
			:loading="loading"
			:footer-props="{ 'items-per-page-option': [pageSize] }"
			:options.sync="_pagination"
			hide-default-footer
			multi-sort
		>
			<template
				v-for="{ value: key } in headers"
				v-slot:[`item.${key}`]="{ item }"
			>
				<slot
					:name="`field-${key}`"
					:field="fields.find((x) => x.key === key)"
					:field-list="fields"
					:item="item"
				>
					<FieldItem
						:key="key"
						:data="item"
						:field="fields.find((x) => x.key === key)"
						:field-list="fields"
						:object-id="objectId"
						mode="list"
					/>
				</slot>
			</template>

			<template v-slot:[`item.[action]`]="{ item, index }">
				<div class="text-right">
					<v-tooltip
						v-if="
							canPerformDelete({ controller: controller, id: item.id }) &&
							!item.isDeleted
						"
						open-delay="100"
						color="rgba(0, 0, 0, 1)"
						left
					>
						<template #activator="{ on, attrs }">
							<v-btn
								v-if="!hideAction"
								v-bind="attrs"
								v-on="on"
								class="ma-1"
								color="orange"
								:disabled="loading"
								x-small
								fab
								dark
								@click="deleteItem(item)"
							>
								<v-icon dark> mdi-lock-outline </v-icon>
							</v-btn>
						</template>
						<span>Khóa</span>
					</v-tooltip>

					<v-tooltip
						v-if="
							canPerformRestore({ controller: controller, id: item.id }) &&
							item.isDeleted
						"
						open-delay="100"
						color="rgba(0, 0, 0, 1)"
						left
					>
						<template #activator="{ on, attrs }">
							<v-btn
								v-if="!hideAction"
								v-bind="attrs"
								v-on="on"
								class="ma-1"
								color="red"
								:disabled="loading"
								x-small
								fab
								dark
								@click="restoreItem(item)"
							>
								<v-icon dark> mdi-lock-open-variant </v-icon>
							</v-btn>
						</template>
						<span>Mở khóa</span>
					</v-tooltip>

					<v-tooltip
						v-if="canPerformDetail({ controller: controller, id: item.id })"
						open-delay="100"
						color="rgba(0, 0, 0, 1)"
						left
					>
						<template #activator="{ on, attrs }">
							<v-btn
								v-if="!hideAction"
								v-bind="attrs"
								v-on="on"
								class="ma-1"
								color="primary"
								:disabled="loading"
								:to="{ path: detailUrl(item) }"
								x-small
								fab
							>
								<v-icon dark> mdi-chevron-right </v-icon>
							</v-btn>
						</template>
						<span>Xem chi tiết</span>
					</v-tooltip>

					<slot
						name="external-action"
						v-bind:loading="loading"
						v-bind:item="item"
						v-bind:index="index"
					/>
				</div>
			</template>
		</v-data-table>

		<v-pagination
			v-if="pageCount > 1"
			class="mt-2"
			v-model="_pagination.page"
			:length="pageCount"
		/>
	</div>
</template>
