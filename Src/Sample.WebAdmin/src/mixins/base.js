import { mapGetters } from "vuex";
import PermissionService from "@/services/permission";

export default {
	computed: {
		...mapGetters("auth", ["isAuthenticated", "currentUser"]),
	},

	methods: {
		getController({ code } = {}) {
			let result = this.$store.getters["layout/controller"];

			if (code) result = result.filter((x) => x.code === code);

			return result;
		},

		getMethod({ type, controller } = {}) {
			let result = this.$store.getters["layout/method"];

			if (type) result = result.filter((x) => x.type === type);
			if (controller)
				result = result.filter((x) => x.controller === controller);

			return result;
		},

		checkPermission(targetPermission) {
			return PermissionService.check(targetPermission);
		},
	},
};
