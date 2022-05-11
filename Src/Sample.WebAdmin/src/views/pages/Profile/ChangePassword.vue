<script>
import PageMixin from "@/mixins/page";

export default {
	name: "profile",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/Core/FieldPanel"),
	},

	data: () => ({
		inputs: null,

		fields: [
			{
				key: "oldPassword",
				name: "mật khẩu cũ",
				type: "password",
				isRequired: true,
			},
			{
				key: "newPassword",
				name: "mật khẩu mới",
				type: "password",
				isRequired: true,
			},
			{
				key: "rePassword",
				name: "nhập lại mật khẩu mới",
				type: "password",
				isRequired: true,
			},
		],
	}),

	methods: {
		cancel() {
			this.$router.push({ name: "profile" });
		},

		async submit() {
			let data = await this.prepareInput({
				fields: this.fields,
				inputs: this.inputs,
			});

			await this.doSubmit({
				successMessage: `đã thay đổi mật khẩu`,
				handler: async () => {
					if (data.newPassword !== data.rePassword)
						throw Error("mật khẩu mới không giống nhau");

					await this.requestApi({
						method: "put",
						url: "/identity/changePassword",
						data: {
							oldPassword: data.oldPassword,
							newPassword: data.newPassword,
						},
					});

					this.$router.push({ name: "profile" });
				},
			});
		},
	},
};
</script>

<template>
	<v-form ref="form" :disabled="loading" @submit.stop.prevent="submit">
		<v-container>
			<v-row class="pt-5" align="end">
				<v-col class="d-flex justify-end" cols="12">
					<v-btn
						v-permission="'profile.update'"
						class="ml-2"
						:disabled="loading"
						small
						@click="cancel"
					>
						Hủy bỏ
					</v-btn>

					<v-btn
						v-permission="'profile.update'"
						type="submit"
						class="ml-2"
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
					<FieldPanel v-model="inputs" :fields="fields" mode="input" />
				</v-col>
			</v-row>

			<v-row align="end">
				<v-col class="d-flex justify-end" cols="12">
					<v-btn
						v-permission="'profile.update'"
						class="ml-2"
						:disabled="loading"
						small
						@click="cancel"
					>
						Hủy bỏ
					</v-btn>

					<v-btn
						v-permission="'profile.update'"
						type="submit"
						class="ml-2"
						color="success"
						:disabled="loading"
						small
					>
						Ghi nhận
						<v-icon dark right> mdi-pencil </v-icon>
					</v-btn>
				</v-col>
			</v-row>
		</v-container>
	</v-form>
</template>
