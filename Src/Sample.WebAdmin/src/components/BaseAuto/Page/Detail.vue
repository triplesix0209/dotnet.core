<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-detail-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/BaseAuto/Field/FieldPanel"),
	},

	data: () => ({
		data: null,
	}),

	computed: {
		detailMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_DETAIL,
				firstOrDefault: true,
			});
		},

		id() {
			return this.$route.params.id;
		},
	},

	methods: {
		_validatePage() {
			if (!this.controller) throw Error("controller is invalid");
			if (!this.detailMethod) throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.detailMethod.permissionCodes,
					this.detailMethod.permissionOperator,
				)
			)
				throw Error("access is denied");
		},

		_breadcrumbs() {
			return [
				{
					text: `Danh sách`,
					href: CONST.generateMethodUrl(CONST.METHOD_TYPE_LIST, {
						controller: this.controller.code,
					}),
				},
				{
					text: `Chi tiết`,
				},
			];
		},

		async _loaded() {
			await this.loadData();
		},

		async loadData({ force } = {}) {
			if (!force && this.loading) return;

			let { data } = await this.requestApi({
				controllerMethod: this.detailMethod,
				path: { id: this.id },
			});

			this.data = data;
		},
	},
};
</script>

<template>
	<v-container v-if="!initialing">
		<v-row>
			<v-col>
				<v-card>
					<FieldPanel
						:data="data"
						:fields="this.detailMethod.detailFields"
						mode="detail"
					/>
				</v-card>
			</v-col>
		</v-row>
	</v-container>
</template>
