import { mapActions } from "vuex";
import BaseMixin from "@/mixins/base";

export default {
	mixins: [BaseMixin],

	data: () => ({
		loading: true,
		metadata: {
			requireAuth: true,
			loadLayout: true,
		},
	}),

	computed: {
		controller() {
			if (!this.$route.params.controller) return null;
			return this.getController({ code: this.$route.params.controller })[0];
		},
	},

	methods: {
		...mapActions(["auth/load", "layout/load"]),

		async doSubmit({ handler, error, formRef, toggleLoadingOnDone } = {}) {
			if (formRef === undefined) formRef = "form";
			if (toggleLoadingOnDone === undefined) toggleLoadingOnDone = true;

			try {
				if (!this.$refs[formRef].validate()) return;

				this.loading = true;
				let result = await handler();
				if (toggleLoadingOnDone) this.loading = false;

				return result;
			} catch (e) {
				await error(e);
				this.loading = false;
			}
		},
	},

	async mounted() {
		if (!this.isAuthenticated) await this["auth/load"]();

		if (this.metadata.requireAuth === true && !this.isAuthenticated) {
			this.$router.push({ name: "login" });
			return;
		}

		if (this.metadata.loadLayout === true) {
			try {
				await this["layout/load"]();
			} catch (e) {
				this.$router.push({ name: "error" });
			}
		}

		this.loading = false;
		if (this.loaded) await this.loaded();
	},
};
