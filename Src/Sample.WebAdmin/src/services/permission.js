import Store from "@/stores/_index";

const service = {};

service.check = function (targetPermission) {
	let currentUser = Store.getters["auth/currentUser"];
	if (currentUser === null) return false;
	if (currentUser.accessLevel === 0) return true;
	if (currentUser.permissions === null) return false;

	if (typeof targetPermission === "function")
		targetPermission = targetPermission();

	if (typeof targetPermission === "string")
		return currentUser.permissions.includes(targetPermission);

	if (Array.isArray(targetPermission)) {
		for (let permission of targetPermission) {
			if (
				typeof item === "string" &&
				!currentUser.permissions.includes(permission)
			)
				return false;

			if (
				Array.isArray(permission) &&
				permission.every((x) => !currentUser.permissions.includes(x))
			)
				return false;
		}

		return true;
	}
};

export default service;
