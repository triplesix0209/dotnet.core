<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-update-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/Core/FieldPanel"),
	},

	props: {
		api: { type: String },
		prepareSubmitData: { type: Function },
	},

	data: () => ({
		data: null,
		inputs: null,
	}),

	computed: {
		id() {
			return this.$route.params.id;
		},

		detailMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_DETAIL,
				firstOrDefault: true,
			});
		},

		updateMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_UPDATE,
				firstOrDefault: true,
			});
		},

		detailUrl() {
			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller: this.controller.code,
				id: this.id,
			});
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
					href: this.detailUrl,
				},
				{
					text: `Chỉnh sửa`,
				},
			];
		},

		async _loaded() {
			let { data } = await this.requestApi({
				controllerMethod: this.detailMethod,
				path: { id: this.id },
				toggleLoading: true,
			});

			this.data = data;
			this.$emit("update:data", this.data);
		},

		_raiseChangeInput(payload) {
			this.$emit("change", payload);
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

		async submit() {
			let data = await this.prepareInput({
				fields: this.updateMethod.inputFields,
				inputs: this.inputs,
			});
			if (this.prepareSubmitData) data = await this.prepareSubmitData(data);

			await this.doSubmit({
				successMessage: `đã chỉnh sửa ${this.controller.name} thành công`,
				handler: async () => {
					await this.requestApi({
						controllerMethod: this.updateMethod,
						path: { id: this.id },
						data,
					});

					this.$router.push({ path: this.detailUrl });
				},
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
		<v-form ref="form" :disabled="loading" @submit.stop.prevent="submit">
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
					<FieldPanel
						v-model="inputs"
						:data="data"
						:fields="updateMethod.inputFields"
						:groups="updateMethod.inputFieldGroups"
						:object-id="id"
						mode="input"
						@change="_raiseChangeInput"
					>
						<template
							v-for="field in updateMethod.inputFields"
							v-slot:[`field-${field.key}`]
						>
							<slot
								:name="`field-${field.key}`"
								:field-list="updateMethod.inputFields"
								:field="field"
							/>
						</template>
					</FieldPanel>
				</v-col>
			</v-row>

			<v-row>
				<v-col class="d-flex justify-end" cols="12">
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
		</v-form>
	</v-container>
</template>
