<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-list-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/BaseAuto/Field/FieldPanel"),
	},

	props: {},

	data: () => ({
		page: 1,
		size: 10,
		pageCount: null,
		total: null,
		items: [],
	}),

	computed: {
		listMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_LIST,
				firstOrDefault: true,
			});
		},
	},

	watch: {},

	methods: {
		_validatePage() {
			if (!this.controller) throw Error("controller is invalid");
			if (!this.listMethod) throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.listMethod.permissionCodes,
					this.listMethod.permissionOperator,
				)
			)
				throw Error("access is denied");
		},

		async _loaded() {
			await this.loadData({ setPage: 1 });
		},

		async loadData({ force, setPage } = {}) {
			if (!force && this.loading) return;
			if (setPage) this.page = setPage;

			let params = { page: this.page, size: this.size };

			let { meta, data } = await this.requestApi({
				controllerMethod: this.listMethod,
				params,
			});

			this.total = meta.total;
			this.pageCount = meta.pageCount;
			this.items = data;
		},
	},
};
</script>

<template>
	<v-container v-if="!initialing">
		<v-form :disabled="loading" @submit.stop.prevent="loadData">
			<v-card>
				<FieldPanel :fields="this.listMethod.filterFields" input-mode />

				<v-card-actions>
					<v-btn block type="submit" color="primary" :loading="loading">
						Lấy dữ liệu
					</v-btn>
				</v-card-actions>
			</v-card>
		</v-form>
	</v-container>
</template>
