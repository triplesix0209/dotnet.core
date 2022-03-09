<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";
import Router from "@/router";
import { mapGetters } from "vuex";

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
		...mapGetters(["layout/layout", "layout/menu"]),
		...mapGetters("lightbox", ["mediaList", "mediaIndex"]),

		controller() {
			if (!this.$route.params.controller) return null;
			return this.getController({
				code: this.$route.params.controller,
				firstOrDefault: true,
			});
		},

		path() {
			return this.$route.path;
		},

		menu() {
			let result = this["layout/menu"].map((x, i) =>
				x.type === CONST.MENU_TYPE_GROUP
					? this._generateMenuItem({ group: x, sortOrder: i })
					: this._generateMenuItem({ menu: x, sortOrder: i }),
			);

			let routes =
				Router.options.routes.find((x) => x.name === CONST.ROUTE_ADMIN_NAME)
					?.children ?? [];
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
						x.type === CONST.MENU_TYPE_GROUP &&
						x.name.trim().toLowerCase() === groupName.toLowerCase(),
				);
				if (!group) {
					group = this._generateMenuItem({
						group: {
							type: CONST.MENU_TYPE_GROUP,
							code: this.$strFormat(
								this.$strFormat(groupName, "clearVietnameseSign"),
								"kebabCase",
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
				.filter((x) => x.type !== CONST.MENU_TYPE_GROUP || x.items.length > 0);

			return result;
		},

		currentMenu() {
			let route = (
				Router.options.routes.find((x) => x.name === CONST.ROUTE_ADMIN_NAME)
					?.children ?? []
			).find((x) => x.path === this.$route.path);

			for (let menu of this.menu) {
				if (menu.type === CONST.MENU_TYPE_PAGE && route) {
					if (menu.code === `${CONST.MENU_TYPE_PAGE}-${route.name}`)
						return menu;
				} else if (
					menu.type === CONST.MENU_TYPE_CONTROLLER &&
					this.controller
				) {
					if (menu.code === this.controller.code) return menu;
				} else if (menu.type === CONST.MENU_TYPE_GROUP) {
					for (let child of menu.items) {
						if (
							child.type === CONST.MENU_TYPE_CONTROLLER &&
							this.controller &&
							child.code === this.controller.code
						)
							return child;
						else if (
							child.type === CONST.MENU_TYPE_PAGE &&
							route &&
							child.code === `${CONST.MENU_TYPE_PAGE}-${route.name}`
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
					type: CONST.MENU_TYPE_PAGE,
					code: `${CONST.MENU_TYPE_PAGE}-${route.name}`,
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

			menu.method = this.getMethod({
				controller: menu.code,
				firstOrDefault: true,
			});
			menu.name = menu.method.name;
			menu.sortOrder = sortOrder;
			menu.path = CONST.generateMethodUrl(menu.method.type, {
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
	<v-main>
		<div v-if="!['layout/layout']" class="screen-center text-center">
			<v-progress-circular
				color="primary"
				:size="70"
				:width="7"
				indeterminate
			/>
			<h3 class="mt-5">Đang nạp trang...</h3>
			<h4 class="grey--text">Xin vui lòng chờ.</h4>
		</div>

		<template v-else>
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

			<v-scroll-y-transition hide-on-leave>
				<router-view :key="path" />
			</v-scroll-y-transition>
		</template>
	</v-main>
</template>

<style lang="scss" scoped>
.v-main {
	background-color: #eef0f8;
}
</style>
