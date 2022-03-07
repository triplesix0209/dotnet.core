<script>
import { mapGetters } from "vuex";
import BaseMixin from "@/mixins/base";
import { CONST as layoutConst } from "@/stores/layout";
import Router from "@/router";

export default {
	name: "AdminLayout",
	mixins: [BaseMixin],

	components: {
		TopBar: () => import("./components/TopBar"),
		LeftBar: () => import("./components/LeftBar"),
	},

	data: () => ({
		toggleLeftBar: null,
		activeMenu: null,
	}),

	computed: {
		...mapGetters(["layout/menu"]),

		controller() {
			if (!this.$route.params.controller) return null;
			return this.getController({ code: this.$route.params.controller })[0];
		},

		menu() {
			let result = this["layout/menu"].map((x, i) =>
				x.type === layoutConst.MENU_TYPE_GROUP
					? this._generateMenuItem({ group: x, sortOrder: i })
					: this._generateMenuItem({ menu: x, sortOrder: i }),
			);

			let routes =
				Router.options.routes.find(
					(x) => x.name === layoutConst.ROUTE_ADMIN_NAME,
				)?.children ?? [];
			for (let route of routes) {
				let groupName = route.meta.group?.trim();
				if (!groupName) {
					result.push(
						this._generateMenuItem({ route, sortOrder: result.length }),
					);
					continue;
				}

				let group = result.find(
					(x) =>
						x.type === layoutConst.MENU_TYPE_GROUP &&
						x.name.trim().toLowerCase() === groupName.toLowerCase(),
				);
				if (!group) {
					group = this._generateMenuItem({
						group: {
							type: layoutConst.MENU_TYPE_GROUP,
							code: this.$options.filters.strKebabCase(
								this.$options.filters.strClearVietnameseSign(groupName),
							),
							name: groupName,
							items: [],
						},
					});
					result.push(group);
				}

				group.items.push(
					this._generateMenuItem({ route, sortOrder: group.items.length }),
				);
				group.items.sort((a, b) => a.sortOrder - b.sortOrder);
			}

			result.sort((a, b) => a.sortOrder - b.sortOrder);

			result = result
				.filter((x) => x.permissionVaild)
				.filter(
					(x) => x.type !== layoutConst.MENU_TYPE_GROUP || x.items.length > 0,
				);

			return result;
		},

		currentMenu() {
			let route = (
				Router.options.routes.find(
					(x) => x.name === layoutConst.ROUTE_ADMIN_NAME,
				)?.children ?? []
			).find((x) => x.path === this.$route.path);

			for (let menu of this.menu) {
				if (menu.type === layoutConst.MENU_TYPE_PAGE && route) {
					if (menu.code === `${layoutConst.MENU_TYPE_PAGE}-${route.name}`)
						return menu;
				} else if (
					menu.type === layoutConst.MENU_TYPE_CONTROLLER &&
					this.controller
				) {
					if (menu.code === this.controller.code) return menu;
				} else if (menu.type === layoutConst.MENU_TYPE_GROUP) {
					for (let child of menu.items) {
						if (
							child.type === layoutConst.MENU_TYPE_CONTROLLER &&
							this.controller &&
							child.code === this.controller.code
						)
							return child;
						else if (
							child.type === layoutConst.MENU_TYPE_PAGE &&
							route &&
							child.code === `${layoutConst.MENU_TYPE_PAGE}-${route.name}`
						)
							return child;
					}
				}
			}

			return null;
		},
	},

	methods: {
		menuChanged() {
			if (this.$refs.topBar.isMenuAutoHide()) this.toggleLeftBar = false;
		},

		_generateMenuItem({ group, route, menu, sortOrder } = {}) {
			if (sortOrder === undefined) sortOrder = 0;

			if (group) {
				return {
					...group,
					permissionVaild: true,
					sortOrder,
					items: group.items.map((x, i) =>
						this._generateMenuItem({ menu: x, sortOrder: i }),
					),
				};
			}

			if (route) {
				return {
					type: layoutConst.MENU_TYPE_PAGE,
					code: `${layoutConst.MENU_TYPE_PAGE}-${route.name}`,
					name: route.meta?.title ?? route.name,
					icon: route.meta?.icon,
					sortOrder: route.meta?.sortOrder ?? sortOrder,
					route,
					path: route.path,
					permissionVaild: route.meta?.permission
						? this.checkPermission(route.meta?.permission)
						: true,
				};
			}

			menu.method = this.getMethod({ controller: menu.code })[0];
			menu.name = menu.method.name;
			menu.sortOrder = sortOrder;
			menu.path = layoutConst.generateMethodUrl(menu.method.type, {
				controller: menu.method.controller,
			});

			if (menu.method.permissionCodes.length === 0) {
				menu.permissionVaild = true;
			} else {
				menu.permissionVaild = this.checkPermission(
					menu.method.permissionCodes,
					menu.method.permissionOperator,
				);
			}

			return menu;
		},
	},
};
</script>

<template>
	<v-app>
		<TopBar
			ref="topBar"
			:current-menu="currentMenu"
			:toggle-left-bar.sync="toggleLeftBar"
		/>

		<LeftBar
			:menu="menu"
			:current-menu="currentMenu"
			:toggle.sync="toggleLeftBar"
			@menu:change="menuChanged"
		/>

		<v-main>
			<router-view />
		</v-main>
	</v-app>
</template>

<style lang="scss" scoped>
.v-main {
	background-color: #eef0f8;
}
</style>
