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

		changelogUrl() {
			if (!this.controller) return null;

			return CONST.generateMethodUrl(CONST.METHOD_TYPE_LIST_CHANGELOG, {
				controller: this.controller.code,
				id: this.id,
			});
		},

		updateUrl() {
			if (!this.controller) return null;

			return CONST.generateMethodUrl(CONST.METHOD_TYPE_UPDATE, {
				controller: this.controller.code,
				id: this.id,
			});
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
	<v-container v-if="initialing || !data">
		<v-row>
			<v-col>
				<v-skeleton-loader type="article,actions" />
			</v-col>
		</v-row>
	</v-container>

	<v-container v-else>
		<v-row class="pt-5" align="end">
			<v-col cols="12">
				ID: <b>{{ id }}</b>
			</v-col>

			<v-col class="pt-0 pr-0" cols="10" sm="5">
				<div :class="{ 'object-change-container': canPerformChangeLog }">
					<v-tooltip v-if="canPerformChangeLog" right color="rgba(0,0,0,1)">
						<template #activator="{ on, attrs }">
							<router-link
								:to="{ path: changelogUrl }"
								class="change-log-button"
							>
								<v-btn
									v-bind="attrs"
									v-on="on"
									:disabled="loading"
									color="primary"
									small
								>
									<v-icon small>mdi-file-clock-outline</v-icon>
								</v-btn>
							</router-link>
						</template>
						<span>Xem lịch sử thay đổi</span>
					</v-tooltip>

					<p v-if="data.createDatetime" class="create-datetime">
						Tạo lúc:
						<v-tooltip color="rgba(0,0,0,1)" bottom>
							<template #activator="{ on, attrs }">
								<b v-bind="attrs" v-on="on">
									{{ data.createDatetime | moment("from") }}
								</b>
							</template>
							<span>
								{{ data.createDatetime | moment("DD/MM/YYYY HH:mm") }}
							</span>
						</v-tooltip>
					</p>

					<p v-if="data.updateDatetime" class="update-datetime">
						Sửa lúc:
						<v-tooltip color="rgba(0,0,0,1)" bottom>
							<template #activator="{ on, attrs }">
								<b v-bind="attrs" v-on="on">
									{{ data.updateDatetime | moment("from") }}
								</b>
							</template>
							<span>
								{{ data.updateDatetime | moment("DD/MM/YYYY HH:mm") }}
							</span>
						</v-tooltip>
					</p>
				</div>
			</v-col>

			<v-col class="d-flex pt-0 pl-0 justify-end" cols="12" sm="7">
				<v-btn
					v-if="canPerformUpdate"
					class="ml-2"
					color="success"
					:disabled="loading"
					:to="{ path: updateUrl }"
					small
				>
					Sửa
					<v-icon dark right> mdi-pencil </v-icon>
				</v-btn>

				<slot name="external-button" v-bind:loading="loading" />
			</v-col>
		</v-row>

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

<style lang="scss" scoped>
.object-change-container {
	display: grid;
	grid-template-areas:
		"button create"
		"button update";
	grid-template-columns: 60px auto;
	align-items: center;
}

.change-log-button {
	grid-area: button;
	margin-right: 10px;
}

.create-datetime {
	grid-area: create;
	margin: 0;
}

.update-datetime {
	grid-area: update;
	margin: 0;
}
</style>
