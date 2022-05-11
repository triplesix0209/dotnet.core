import ApiService from "@/services/api";

const CONST = {
	MENU_TYPE_PAGE: "page",
	MENU_TYPE_GROUP: "group",
	MENU_TYPE_CONTROLLER: "controller",

	METHOD_TYPE_LIST: 1,
	METHOD_TYPE_DETAIL: 2,
	METHOD_TYPE_CREATE: 3,
	METHOD_TYPE_UPDATE: 4,
	METHOD_TYPE_DELETE: 5,
	METHOD_TYPE_RESTORE: 6,
	METHOD_TYPE_LIST_CHANGELOG: 7,
	METHOD_TYPE_DETAIL_CHANGELOG: 8,
	METHOD_TYPE_EXPORT: 9,

	PERMISSION_AND: 1,
	PERMISSION_OR: 2,

	FIELD_OPERATOR_EQUAL: "equal",
	FIELD_OPERATOR_NOT_EQUAL: "notEqual",
	FIELD_OPERATOR_NULL: "isNull",
	FIELD_OPERATOR_NOT_NULL: "notNull",
	FIELD_OPERATOR_IN: "in",
	FIELD_OPERATOR_NOT_IN: "notIn",
	FIELD_OPERATOR_BETWEEN: "between",
	FIELD_OPERATOR_NOT_BETWEEN: "notBetween",

	generateMethodUrl(type, { controller, id } = {}) {
		if (!controller) throw Error("method controller is invalid");
		if (
			!id &&
			[
				CONST.METHOD_TYPE_DETAIL,
				CONST.METHOD_TYPE_UPDATE,
				CONST.METHOD_TYPE_DETAIL_CHANGELOG,
			].includes(type)
		)
			throw Error("object id of controller is invalid");

		switch (type) {
			case CONST.METHOD_TYPE_LIST:
				return `/${controller}`;
			case CONST.METHOD_TYPE_DETAIL:
				return `/${controller}/${id}`;
			case CONST.METHOD_TYPE_CREATE:
				return `/${controller}/create`;
			case CONST.METHOD_TYPE_UPDATE:
				return `/${controller}/${id}/update`;
			case CONST.METHOD_TYPE_LIST_CHANGELOG:
				return `/${controller}/${id}/changelog`;
		}

		throw Error(`method type '${type}' is invalid`);
	},
};

export { CONST };

export default {
	namespaced: true,

	state: {
		layout: null,
		pageTitle: null,
		breadcrumbs: [],
	},

	mutations: {
		setLayout(state, data) {
			state.layout = data;
		},

		setPageTitle(state, data) {
			state.pageTitle = data;
		},

		setBreadcrumb(state, data) {
			state.breadcrumbs = data;
		},
	},

	getters: {
		layout: (state) => state.layout,
		controller: (state) => state.layout?.controllers ?? [],
		method: (state) => state.layout?.methods ?? [],
		pageTitle: (state) => state.pageTitle,
		breadcrumbs: (state) => state.breadcrumbs,
	},

	actions: {
		async load(context) {
			if (context.getters.layout !== null) return context.getters.layout;

			let responses = {};
			for (let key in ApiService.admin) {
				let admin = ApiService.admin[key];
				responses[key] = (await admin.get({ url: "/admin/_metadata" })).data;
			}

			let layout = { controllers: [], methods: [] };
			for (let key in responses) {
				let { controllerGroups, controllers, methods } = responses[key];

				for (let controller of controllers) {
					layout.controllers.push({
						...controller,
						group: controller.group
							? controllerGroups.find((x) => x.code === controller.group)
							: null,
					});
				}

				for (let method of methods) {
					let m = {
						id: layout.methods.length,
						api: key,
						...method,
						method: method.method.toLowerCase(),
					};

					if (m.modelController) m.modelController.api = key;

					layout.methods.push(m);
				}
			}

			context.commit("setLayout", layout);
			return layout;
		},

		setPageTitle(context, pageTitle) {
			context.commit("setPageTitle", pageTitle);
		},

		setBreadcrumb(context, breadcrumbs) {
			context.commit("setBreadcrumb", breadcrumbs);
		},
	},
};
