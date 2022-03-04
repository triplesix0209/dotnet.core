import ApiService from "@/services/api";

const MENU_TYPE_GROUP = "group";
const MENU_TYPE_CONTROLLER = "controller";

export default {
	namespaced: true,

	state: {
		layout: null,
	},

	mutations: {
		setLayout(state, data) {
			state.layout = data;
		},
	},

	getters: {
		layout: (state) => state.layout,
		menu: (state) => state.layout?.menu ?? [],
		controller: (state, code) =>
			state.layout?.controllers.find((x) => x.code === code),
		method: (state, filter = {}) =>
			state.layout?.controllers.find(
				(x) =>
					(!filter.id || x.id === filter.id) &&
					(!filter.controller || x.controller === filter.controller) &&
					(!filter.type || x.type === filter.type),
			),
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
						(x) => x.type === MENU_TYPE_GROUP && x.code === group.code,
					);

					if (index === -1)
						layout.menu.push({ type: MENU_TYPE_GROUP, ...group, items: [] });
					else layout.menu[index] = { ...layout.menu[index], ...group };
				}

				for (let controller of controllers) {
					layout.controllers.push(controller);

					if (controller.render === false) continue;

					if (controller.group === null) {
						layout.menu.push({
							type: MENU_TYPE_CONTROLLER,
							code: controller.code,
							name: controller.name,
							icon: controller.icon,
						});
					} else {
						let group = layout.menu.find(
							(x) => x.type === MENU_TYPE_GROUP && x.code === controller.group,
						);
						if (group === null)
							throw Error(`group '${controller.group}' not found`);

						let index = group.items.findIndex(
							(x) =>
								x.type === MENU_TYPE_CONTROLLER && x.code === controller.code,
						);

						if (index === -1)
							group.items.push({
								type: MENU_TYPE_CONTROLLER,
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
	},
};
