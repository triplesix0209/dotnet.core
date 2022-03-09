<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-create-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/BaseAuto/Field/FieldPanel"),
	},

	data: () => ({
		inputs: null,
	}),

	computed: {
		createMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_CREATE,
				firstOrDefault: true,
			});
		},
	},

	methods: {
		_validatePage() {
			if (!this.controller) throw Error("controller is invalid");
			if (!this.createMethod) throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.createMethod.permissionCodes,
					this.createMethod.permissionOperator,
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
					text: `Tạo`,
				},
			];
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
			await this.doSubmit({
				successMessage: `Đã tạo ${this.controller.name} thành công`,
				handler: async () => {
					let { data: id } = await this.requestApi({
						controllerMethod: this.createMethod,
						data: await this.prepareInput({
							fields: this.createMethod.inputFields,
							inputs: this.inputs,
						}),
					});

					this.$router.push({
						path: CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
							controller: this.controller.code,
							id,
						}),
					});
				},
			});
		},
	},
};
</script>

<template>
	<v-container v-if="initialing">
		<v-row>
			<v-col>
				<v-skeleton-loader type="article,actions" />
			</v-col>
		</v-row>
	</v-container>

	<v-container v-else>
		<v-form ref="form" :disabled="loading" @submit.stop.prevent="submit">
			<v-row class="pt-5" align="end">
				<v-col class="d-flex pt-0 pl-0 justify-end" cols="12">
					<v-btn class="ml-2" :disabled="loading" small @click="cancel">
						Hủy bỏ
					</v-btn>

					<v-btn
						class="ml-2"
						type="submit"
						color="success"
						:disabled="loading || !inputs"
						small
					>
						Tạo
						<v-icon dark right> mdi-pencil </v-icon>
					</v-btn>
				</v-col>
			</v-row>

			<v-row>
				<v-col>
					<v-card>
						<FieldPanel
							v-model="inputs"
							:fields="this.createMethod.inputFields"
							mode="input"
						/>
					</v-card>
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
						:disabled="loading || !inputs"
						small
					>
						Tạo
						<v-icon dark right> mdi-pencil </v-icon>
					</v-btn>
				</v-col>
			</v-row>
		</v-form>
	</v-container>
</template>
