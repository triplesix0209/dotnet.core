import { CONST } from "@/stores/layout";
import ApiService from "@/services/api";
import PermissionService from "@/services/permission";
import { mapGetters, mapActions } from "vuex";

export default {
	computed: {
		...mapGetters("auth", ["isAuthenticated", "currentUser"]),
	},

	methods: {
		...mapActions("lightbox", ["showLightbox"]),

		$numeral(value, format) {
			return this.$options.filters.numeral(value, format);
		},

		$strFormat(value, type) {
			return this.$options.filters.strFormat(value, type);
		},

		getController({ code, firstOrDefault } = {}) {
			let result = this.$store.getters["layout/controller"];

			if (code) result = result.filter((x) => x.code === code);

			if (firstOrDefault) return result.length > 0 ? result[0] : null;
			return result;
		},

		getMethod({ type, controller, firstOrDefault } = {}) {
			let result = this.$store.getters["layout/method"];

			if (type) result = result.filter((x) => x.type === type);
			if (controller)
				result = result.filter((x) => x.controller === controller);

			if (firstOrDefault) return result.length > 0 ? result[0] : null;
			return result;
		},

		checkPermission(targetPermission, checkOperator = CONST.PERMISSION_AND) {
			if (checkOperator === CONST.PERMISSION_AND)
				return PermissionService.check(targetPermission);
			return PermissionService.check([targetPermission]);
		},

		redirectToErrorPage({ errorCode } = {}) {
			if (errorCode === undefined || errorCode === null) errorCode = 404;
			this.$router.push({ name: "error", params: { code: errorCode } });
		},

		async confirm({ message }) {
			return await this.$confirm(message, {
				persistent: true,
			});
		},

		toastSuccess({ message = "Đã xử lý thành công" } = {}) {
			this.$toast.success(message);
		},

		toastWarning({ message } = {}) {
			this.$toast.warning(message);
		},

		toastError(error) {
			let { code, message, data } = error;
			const h = this.$createElement;
			let msg;

			if (code === "bad_client_request") {
				let details = [h("p", message)];
				for (let key in data) {
					let errors = [h("b", `${key}: `)];
					if (data[key].length === 1) errors.push(h("span", data[key]));
					else {
						errors.push(h("br"));
						for (let error of data[key]) {
							errors.push(h("span", ` - ${error}`));
						}
					}
					details.push(h("p", errors));
				}
				msg = h("div", details);
			} else {
				msg = message;
			}

			console.error(error);
			this.$toast.error(msg);
		},

		async requestApi({
			controllerMethod,
			path,
			params,
			data,
			form,
			responseType,
			accessTokenField,
			accessToken,
			refreshToken,
			loadingField,
			toggleLoading,
		} = {}) {
			if (!controllerMethod)
				throw Error("parameter 'controllerMethod' is invalid");

			if (loadingField === undefined) loadingField = "loading";
			if (toggleLoading === undefined) toggleLoading = true;

			try {
				if (toggleLoading && loadingField in this) this[loadingField] = true;

				let response = await ApiService.admin[controllerMethod.api][
					controllerMethod.method
				]({
					url: controllerMethod.url,
					path,
					params,
					data,
					form,
					responseType,
					accessTokenField,
					accessToken,
					refreshToken,
				});

				if (toggleLoading && loadingField in this) this[loadingField] = false;

				return response;
			} catch (e) {
				if (toggleLoading && loadingField in this) this[loadingField] = false;
				this.toastError(e);
				throw e;
			}
		},

		async doSubmit({
			handler,
			error,
			form,
			loadingField,
			toggleLoading,
			confirmMessage,
			successMessage,
		} = {}) {
			if (form === undefined) form = "form";
			if (loadingField === undefined) loadingField = "loading";
			if (toggleLoading === undefined) toggleLoading = true;

			try {
				if (form) if (!this.$refs[form].validate()) return;
				if (
					confirmMessage &&
					!(await this.confirm({ message: confirmMessage }))
				)
					return;

				if (toggleLoading && loadingField in this) this[loadingField] = true;
				let result = await handler();
				if (successMessage) this.toastSuccess({ message: successMessage });
				if (toggleLoading && loadingField in this) this[loadingField] = false;

				return result;
			} catch (e) {
				this.toastError(e);
				if (error) await error(e);
				if (toggleLoading && loadingField in this) this[loadingField] = false;
			}
		},

		canPerformList({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_LIST,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformDetail({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_DETAIL,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformCreate({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_CREATE,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformUpdate({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_UPDATE,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformDelete({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_DELETE,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformRestore({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_RESTORE,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformChangeLog({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_LIST_CHANGELOG,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},

		canPerformExport({ controller }) {
			let method = this.getMethod({
				type: CONST.METHOD_TYPE_LIST_EXPORT,
				controller,
				firstOrDefault: true,
			});
			if (!method) return false;

			return this.checkPermission(
				method.permissionCodes,
				method.permissionOperator,
			);
		},
	},
};
