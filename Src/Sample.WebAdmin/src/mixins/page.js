import BaseMixin from "@/mixins/base";
import ApiService from "@/services/api";
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
			if (!this.$route.params.controller) return null;
			return this.getController({
				code: this.$route.params.controller,
				firstOrDefault: true,
			});
		},
	},

	methods: {
		...mapActions(["auth/load", "layout/load"]),
		...mapActions("layout", ["setPageTitle", "setBreadcrumb"]),

		async prepareInput({ inputs, fields }) {
			let result = {};
			for (let field of fields) {
				let input = inputs[field.key];
				let value = input.value;

				if (field.type === "media") {
					if (input.file) {
						let { url } = await ApiService.static.upload(input.file);
						input.file = null;
						input.value = url;
						value = url;
					}
				} else if (field.type === "datetime") {
					if (input.value) value = this.$moment(input.value).valueOf();
				}

				if (value !== undefined) result[field.key] = value;
			}

			return result;
		},

		_validatePage() {},

		_pageTitle() {
			if (this.controller) return this.controller.name;
			return this.$route.meta?.title;
		},

		_breadcrumbs() {
			return [];
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

		try {
			await this._validatePage();
		} catch (e) {
			this.redirectToErrorPage();
			throw e;
		}

		this.setPageTitle(await this._pageTitle());
		this.setBreadcrumb(await this._breadcrumbs());

		this.initialing = false;
		this.loading = false;
		if (this._loaded) await this._loaded();
	},
};
