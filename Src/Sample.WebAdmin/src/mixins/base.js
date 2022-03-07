import { mapGetters } from "vuex";
import PermissionService from "@/services/permission";
import { CONST as layoutConst } from "@/stores/layout";

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

		async confirm({ message }) {
			return await this.$confirm(message, {
				persistent: true,
			});
		},

		checkPermission(
			targetPermission,
			checkOperator = layoutConst.PERMISSION_AND,
		) {
			if (checkOperator === layoutConst.PERMISSION_AND)
				return PermissionService.check(targetPermission);
			return PermissionService.check([targetPermission]);
		},
	},
};
