import BaseMixin from "@/mixins/base";
import { mapActions } from "vuex";

export default {
	mixins: [BaseMixin],

	data: () => ({
		metadata: {
			requireAuth: true,
			loadLayout: true,
		},

		initialing: true,
		loading: true,
	}),

	computed: {
		controller() {
			let controller = this.$route.params?.controller;
			if (!controller) controller = this.$route.meta?.controller;
			if (!controller) return null;

			return this.getController({ code: controller, firstOrDefault: true });
		},
	},

	methods: {
		...mapActions(["auth/load", "layout/load"]),
		...mapActions("layout", ["setPageTitle", "setBreadcrumb"]),

		_pageTitle() {
			if (this.controller) return this.controller.name;
			return this.$route.meta?.title;
		},
	},

	async mounted() {
		if (!this.isAuthenticated) await this["auth/load"]();

		if (this.metadata.requireAuth === true && !this.isAuthenticated) {
			let redirect = this.$route.fullPath;
			if (redirect.startsWith("/login")) redirect = undefined;
			this.$router.push({ name: "login", query: { redirect } });
			return;
		}

		if (this.metadata.loadLayout === true) {
			try {
				await this["layout/load"]();
			} catch (e) {
				this.redirectToErrorPage();
			}
		}

		if (this._validatePage) {
			try {
				await this._validatePage();
			} catch (e) {
				this.redirectToErrorPage();
				throw e;
			}
		}

		this.setPageTitle(await this._pageTitle());
		if (this._breadcrumbs) this.setBreadcrumb(await this._breadcrumbs());
		else this.setBreadcrumb([]);

		this.initialing = false;
		this.loading = false;
		if (this._loaded) await this._loaded();
		this.$emit("loaded");
	},
};
