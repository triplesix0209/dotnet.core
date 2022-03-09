<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-update-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/BaseAuto/Field/FieldPanel"),
	},

	data: () => ({
		data: null,
		inputs: null,
	}),

	computed: {
		detailMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_DETAIL,
				firstOrDefault: true,
			});
		},

		updateMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_UPDATE,
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
			if (!this.detailMethod || !this.updateMethod)
				throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.detailMethod.permissionCodes,
					this.detailMethod.permissionOperator,
				) ||
				!this.checkPermission(
					this.updateMethod.permissionCodes,
					this.updateMethod.permissionOperator,
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
					href: CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
						controller: this.controller.code,
						id: this.id,
					}),
				},
				{
					text: `Chỉnh sửa`,
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

		async submit() {
			console.log(this.input);
		},

		async cancel() {
			if (!(await this.$confirm("Bạn chắc chắn muốn hủy thao tác?"))) return;

			this.$router.push({
				path: CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
					controller: this.controller.code,
					id: this.id,
				}),
			});
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
		<v-form :disabled="loading" @submit.stop.prevent="submit">
			<v-row class="pt-5" align="end">
				<v-col cols="12">
					ID: <b>{{ id }}</b>
				</v-col>

				<v-col class="pt-0 pr-0" cols="10" sm="5">
					<p v-if="data.createDatetime" class="ma-0">
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

					<p v-if="data.updateDatetime" class="ma-0">
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
				</v-col>

				<v-col class="d-flex pt-0 pl-0 justify-end" cols="12" sm="7">
					<v-btn class="ml-2" :disabled="loading" small @click="cancel">
						Hủy bỏ
					</v-btn>

					<v-btn
						class="ml-2"
						type="submit"
						color="success"
						:disabled="loading"
						small
					>
						Ghi nhận
						<v-icon dark right> mdi-pencil </v-icon>
					</v-btn>
				</v-col>
			</v-row>

			<v-row>
				<v-col>
					<v-card>
						<FieldPanel
							v-model="inputs"
							:data="data"
							:fields="this.updateMethod.inputFields"
							mode="input"
						/>
					</v-card>
				</v-col>
			</v-row>
		</v-form>
	</v-container>
</template>
