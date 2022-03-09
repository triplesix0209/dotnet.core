<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	name: "data-table",
	mixins: [BaseMixin],

	props: {
		fields: { type: Array, required: true },
		loading: { type: Boolean },
		items: { types: Array },
		total: { type: Number },
		pagination: { type: Object, default: () => ({}) },
		controller: { type: String },
	},

	components: {
		FieldItem: () => import("@/components/BaseAuto/Field/FieldItem"),
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
	},

	methods: {
		detailUrl(item) {
			if (!this.controller) return null;

			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller: this.controller,
				id: item.id,
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
				<FieldItem
					:key="key"
					:field="fields.find((x) => x.key === key)"
					:field-list="fields"
					:data="item"
					mode="list"
				/>
			</template>

			<template v-slot:[`item.[action]`]="{ item }">
				<div class="text-right">
					<v-tooltip
						v-if="canPerformDetail({ controller: controller, id: item.id })"
						open-delay="100"
						color="rgba(0, 0, 0, 1)"
						left
					>
						<template #activator="{ on, attrs }">
							<v-btn
								v-bind="attrs"
								v-on="on"
								class="m-1"
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
