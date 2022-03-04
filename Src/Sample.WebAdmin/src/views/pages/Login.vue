<script>
import { mapActions } from "vuex";
import PageMixin from "@/mixins/page";

export default {
	name: "login",
	mixins: [PageMixin],

	data: () => ({
		metadata: { requireAuth: false, loadLayout: false },
		errors: [],
		form: {
			username: null,
			password: null,
		},
		rules: {
			username: [(v) => !!v || "Tên đăng nhập không được phép bỏ trống"],
			password: [(v) => !!v || "Mật khẩu không được phép bỏ trống"],
		},
	}),

	methods: {
		...mapActions("auth", ["login"]),

		submit() {
			this.doSubmit({
				toggleLoadingOnDone: false,

				handler: async () => {
					this.errors = [];

					await this.login(this.form);

					this.$router.push({ name: "home" });
				},

				error: (e) => {
					this.errors.push(e.message);
				},
			});
		},
	},
};
</script>

<template>
	<v-app>
		<v-card class="login-form" width="280">
			<v-card-text>
				<v-img src="assets/logo.png" height="100" contain />
				<div class="text-center title mt-5">Đăng nhập hệ thống</div>

				<v-divider class="my-5" />

				<v-alert
					v-if="errors && errors.length > 0"
					type="error"
					border="left"
					text
				>
					<div v-for="error in errors" :key="error">{{ error }}</div>
				</v-alert>

				<v-form ref="form" :disabled="loading" @submit.stop.prevent="submit">
					<v-text-field
						v-model="form.username"
						prepend-inner-icon="mdi-account"
						:rules="rules.username"
						filled
						rounded
					/>

					<v-text-field
						type="password"
						v-model="form.password"
						prepend-inner-icon="mdi-key"
						:rules="rules.password"
						filled
						rounded
					/>

					<v-btn type="submit" color="primary" :loading="loading" block>
						Đăng nhập
					</v-btn>
				</v-form>
			</v-card-text>
		</v-card>
	</v-app>
</template>

<style lang="scss" scoped>
#app {
	background-image: url("@/assets/background.jpg");
	background-size: cover;
}

.login-form {
	position: absolute;
	top: 50%;
	left: 50%;
	transform: translate(-50%, -50%);
}
</style>
