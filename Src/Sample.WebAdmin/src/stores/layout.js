import ApiService from "@/services/api";

const CONST = {
	ROUTE_ADMIN_NAME: "admin",
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
		page: null,
	},

	mutations: {
		setLayout(state, data) {
			state.layout = data;
		},

		setPage(state, data) {
			state.page = data;
		},
	},

	getters: {
		layout: (state) => state.layout,
		menu: (state) => state.layout?.menu ?? [],
		controller: (state) => state.layout?.controllers ?? [],
		method: (state) => state.layout?.methods ?? [],
		page: (state) => state.page,
		pageTitle: (state) => state.page?.title ?? null,
	},

	actions: {
		async load(context) {
			if (context.getters.layout !== null) return context.getters.layout;

			let responses = {};
			for (let key in ApiService.admin) {
				let admin = ApiService.admin[key];
				responses[key] = (await admin.get({ url: "/_metadata" })).data;
			}

			let layout = { menu: [], controllers: [], methods: [] };
			for (let key in responses) {
				let { controllerGroups, controllers, methods } = responses[key];

				for (let group of controllerGroups) {
					let index = layout.menu.findIndex(
						(x) => x.type === CONST.MENU_TYPE_GROUP && x.code === group.code,
					);

					if (index === -1)
						layout.menu.push({
							type: CONST.MENU_TYPE_GROUP,
							...group,
							items: [],
						});
					else layout.menu[index] = { ...layout.menu[index], ...group };
				}

				for (let controller of controllers) {
					layout.controllers.push(controller);

					if (controller.render === false) continue;

					if (controller.group === null) {
						layout.menu.push({
							type: CONST.MENU_TYPE_CONTROLLER,
							code: controller.code,
							name: controller.name,
							icon: controller.icon,
						});
					} else {
						let group = layout.menu.find(
							(x) =>
								x.type === CONST.MENU_TYPE_GROUP && x.code === controller.group,
						);
						if (group === null)
							throw Error(`group '${controller.group}' not found`);

						let index = group.items.findIndex(
							(x) =>
								x.type === CONST.MENU_TYPE_CONTROLLER &&
								x.code === controller.code,
						);

						if (index === -1)
							group.items.push({
								type: CONST.MENU_TYPE_CONTROLLER,
								code: controller.code,
								name: controller.name,
								icon: controller.icon,
							});
						else {
							group.items[index].name = controller.name;
							group.items[index].icon = controller.icon;
						}
					}
				}

				for (let method of methods) {
					layout.methods.push({ id: layout.methods.length, ...method });
				}
			}

			context.commit("setLayout", layout);
			return layout;
		},

		setPageTitle(context, title) {
			let page = context.getters.page;
			if (!page) page = {};

			page.title = title?.trim();

			context.commit("setPage", page);
		},
	},
};
