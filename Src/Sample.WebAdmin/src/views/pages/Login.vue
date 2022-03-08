<script>
import PageMixin from "@/mixins/page";
import { mapActions } from "vuex";

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

					let redirect = this.$route.query.redirect;
					if (!redirect) redirect = "/";
					this.$router.push({ path: redirect });
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
		<v-main>
			<v-card class="screen-center" width="280">
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
		</v-main>
	</v-app>
</template>

<style lang="scss" scoped>
#app {
	background-image: url("@/assets/background.jpg");
	background-size: cover;
}
</style>
