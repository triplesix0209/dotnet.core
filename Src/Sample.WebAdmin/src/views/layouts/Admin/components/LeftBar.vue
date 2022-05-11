<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";
import Router from "@/router";
import { mapGetters } from "vuex";

export default {
	name: "AdminLayout-LeftBar",
	mixins: [BaseMixin],

	props: {
		toggle: { type: Boolean, default: null },
	},

	data: () => ({
		treeMenu: { open: [], active: [], old: [] },
		listMenu: { search: null, active: null },
	}),

	computed: {
		...mapGetters("layout", ["layout"]),

		_toggle: {
			get() {
				return this.toggle;
			},
			set(value) {
				this.$emit("update:toggle", value);
			},
		},

		pageRoutes() {
			let result =
				Router.options.routes.find((x) => x.meta?.root)?.children ?? [];

			return result.filter(
				(x) => !x.controller && x.meta && (!x.meta.menu || !x.meta.menu.hide),
			);
		},

		treeMenuItems() {
			let menu = [];
			if (!this.layout) return menu;

			// #region [group item]

			for (let controller of this.layout.controllers) {
				if (!controller.render || !controller.group) continue;
				if (
					menu.some(
						(x) =>
							x.type === CONST.MENU_TYPE_GROUP &&
							x.code === controller.group.code,
					)
				)
					continue;

				menu.push({
					type: CONST.MENU_TYPE_GROUP,
					code: controller.group.code,
					name: controller.group.name,
					icon: controller.group.icon,
					sortOrder: 0,
					items: [],
				});
			}

			for (let route of this.pageRoutes) {
				if (!route.meta || !route.meta.menu || !route.meta.menu.group) continue;

				let groupCode = route.meta.menu.group.code;
				let groupName = route.meta.menu.group.name;
				let groupIcon = route.meta.menu.group.icon;
				if (
					(!groupCode || groupCode.trim() === "") &&
					(!groupName || groupName.trim() === "")
				)
					continue;

				if (!groupName || groupName.trim() === "") groupName = groupCode.trim();
				if (!groupCode || groupCode.trim() === "") {
					groupCode = this.$strFormat(groupName.trim(), "clearVietnameseSign");
					groupCode = this.$strFormat(groupCode, "kebabCase");
				}
				if (!groupIcon || groupIcon.trim() === "") groupIcon = null;

				if (
					menu.some(
						(x) => x.type === CONST.MENU_TYPE_GROUP && x.code === groupCode,
					)
				)
					continue;

				menu.push({
					type: CONST.MENU_TYPE_GROUP,
					code: groupCode,
					name: groupName,
					icon: groupIcon,
					sortOrder: route.meta.menu.sortOrder ?? 0,
					items: [],
				});
			}

			// #endregion

			// #region [controller item]

			for (let controller of this.layout.controllers) {
				let listMethod = this.getMethod({
					type: CONST.METHOD_TYPE_LIST,
					controller: controller.code,
					firstOrDefault: true,
				});
				if (!listMethod) continue;

				let item = {
					type: CONST.MENU_TYPE_CONTROLLER,
					code: controller.code,
					name: `Danh sách ${controller.name}`,
					icon: controller.icon,
					sortOrder: 0,
					path: CONST.generateMethodUrl(listMethod.type, {
						controller: controller.code,
					}),
				};

				let group = controller.group?.code;
				if (group) {
					group = menu.find(
						(x) =>
							x.type === CONST.MENU_TYPE_GROUP &&
							x.code === controller.group.code,
					);
				}

				if (group) group.items.push(item);
				else menu.push(item);
			}

			for (let route of this.pageRoutes) {
				let item = {
					type: CONST.MENU_TYPE_PAGE,
					code: route.name,
					path: route.path,
					sortOrder: route.meta?.menu?.sortOrder ?? 0,
				};

				if (route.meta && route.meta.title && route.meta.title.trim() !== "")
					item.name = route.meta.title.trim();
				else item.name = item.code;

				if (
					route.meta &&
					route.meta.menu &&
					route.meta.menu.icon &&
					route.meta.menu.icon.trim() !== ""
				)
					item.icon = route.meta.menu.icon.trim();
				else item.icon = null;

				let group = route.meta?.menu?.group?.code;
				if (!group || group.trim() === "") {
					group = route.meta?.menu?.group?.name;
					if (group && group.trim() !== "")
						group = this.$$strFormat(group, "kebabCase");
				}

				if (group) {
					group = menu.find(
						(x) => x.type === CONST.MENU_TYPE_GROUP && x.code === group,
					);
				}

				if (group) group.items.push(item);
				else menu.push(item);
			}

			// #endregion

			// #region [filter & sort]

			menu = menu.filter(
				(x) => x.type !== CONST.MENU_TYPE_GROUP || x.items.length > 0,
			);

			menu.sort((a, b) => a.sortOrder - b.sortOrder);

			// #endregion

			return menu;
		},

		listMenuItems() {
			let menu = [];
			if (!this.layout) return menu;

			// #region [controller item]

			for (let controller of this.layout.controllers) {
				let listMethod = this.getMethod({
					type: CONST.METHOD_TYPE_LIST,
					controller: controller.code,
					firstOrDefault: true,
				});
				if (!listMethod) continue;

				let item = {
					type: CONST.MENU_TYPE_CONTROLLER,
					code: controller.code,
					name: controller.name,
					icon: controller.icon,
					sortOrder: 0,
					path: CONST.generateMethodUrl(listMethod.type, {
						controller: controller.code,
					}),
				};

				menu.push(item);
			}

			for (let route of this.pageRoutes) {
				let item = {
					type: CONST.MENU_TYPE_PAGE,
					code: route.name,
					path: route.path,
					sortOrder: route.meta?.menu?.sortOrder ?? 0,
				};

				if (route.meta && route.meta.title && route.meta.title.trim() !== "")
					item.name = route.meta.title.trim();
				else item.name = item.code;

				if (
					route.meta &&
					route.meta.menu &&
					route.meta.menu.icon &&
					route.meta.menu.icon.trim() !== ""
				)
					item.icon = route.meta.menu.icon.trim();
				else item.icon = null;

				menu.push(item);
			}

			// #endregion

			// #region [filter & sort]

			if (this.search) {
				menu = menu.filter((x) =>
					this.$strFormat(x.name, "clearVietnameseSign")
						.toLowerCase()
						.includes(
							this.$strFormat(this.search, "clearVietnameseSign").toLowerCase(),
						),
				);
			}

			menu.sort((a, b) => a.sortOrder - b.sortOrder);

			// #endregion

			return menu;
		},
	},

	watch: {
		$route() {
			this._syncActiveMenu(true);
		},
	},

	methods: {
		selectTreeMenu(value, { pushRoute } = {}) {
			// clear list menu
			this.listMenu.active = null;
			this.listMenu.search = null;

			// #region [skip case]

			if (
				this.$route.meta?.menu?.hide &&
				value.length > 0 &&
				this.$route.name === value[0].code
			) {
				if (this.treeMenu.active.length > 0) this.treeMenu.active = [];
				if (this.treeMenu.old.length > 0) this.treeMenu.old = [];
				return;
			}

			if (value.length === 0) {
				if (this.treeMenu.old.length !== 0)
					this.$nextTick(() => (this.treeMenu.active = this.treeMenu.old));
				return;
			}

			// #endregion

			if (pushRoute === undefined) pushRoute = true;

			let parentItem = null;
			let selectItem = null;
			for (let menu of this.treeMenuItems) {
				if (menu.type === CONST.MENU_TYPE_GROUP) {
					for (let child of menu.items) {
						if (child.code === value[0].code) {
							parentItem = menu;
							selectItem = child;
							break;
						}
					}

					if (selectItem) break;
				} else if (menu.type === value[0].type && menu.code === value[0].code) {
					selectItem = menu;
					break;
				}
			}

			let isChanged =
				this.treeMenu.old.length === 0 ||
				this.treeMenu.old[0].type !== selectItem.type ||
				this.treeMenu.old[0].code !== selectItem.code;
			this.treeMenu.open = parentItem ? [parentItem] : [];
			this.treeMenu.active = [selectItem];
			this.treeMenu.old = this.treeMenu.active;

			if (!isChanged) return;
			this.$emit("menu:change", selectItem);

			if (pushRoute) this.$router.push({ path: selectItem.path });
		},

		_syncActiveMenu(resync = false) {
			if (!resync && this.treeMenu.active.length > 0) return;
			if (!this.layout) return;
			let selectItem = null;

			if (
				[
					"auto-list",
					"auto-detail",
					"auto-create",
					"auto-update",
					"auto-changelog",
				].includes(this.$route.name)
			) {
				selectItem = {
					type: CONST.MENU_TYPE_CONTROLLER,
					code: this.$route.params.controller,
				};
			} else if (this.$route.meta && this.$route.meta.controller) {
				selectItem = {
					type: CONST.MENU_TYPE_CONTROLLER,
					code: this.$route.meta.controller,
				};
			} else {
				selectItem = {
					type: CONST.MENU_TYPE_PAGE,
					code: this.$route.name,
				};
			}

			this.selectTreeMenu([selectItem], { pushRoute: false });
		},
	},

	updated() {
		this._syncActiveMenu();
	},
};
</script>

<template>
	<v-navigation-drawer v-model="_toggle" color="#1E1E2D" app dark>
		<v-list>
			<v-list-item>
				<v-list-item-content>
					<v-img src="/assets/logo.png" max-height="50" contain />
				</v-list-item-content>
			</v-list-item>

			<v-list-item>
				<v-list-item-content>
					<v-text-field
						v-model="listMenu.search"
						placeholder="Tìm kiếm..."
						hide-details
						clearable
						rounded
						filled
						dense
					/>
				</v-list-item-content>
			</v-list-item>
		</v-list>

		<v-divider />

		<v-scroll-x-reverse-transition hide-on-leave>
			<v-list v-show="!listMenu.search" class="tree-menu">
				<v-list-item class="px-0">
					<v-list-item-content class="pa-0">
						<v-treeview
							:items="treeMenuItems"
							:open.sync="treeMenu.open"
							:active.sync="treeMenu.active"
							color="info"
							item-key="code"
							item-children="items"
							:expand-icon="null"
							open-on-click
							return-object
							transition
							activatable
							@update:active="selectTreeMenu"
						>
							<template #label="{ item }">
								<v-icon v-if="item.icon">
									{{ "mdi-" + item.icon }}
								</v-icon>

								{{ item.name | strFormat("capitalize") }}
							</template>

							<template #append="{ item, open }">
								<v-icon v-if="item.type === 'group'">
									{{ open ? "mdi-chevron-down" : "mdi-chevron-right" }}
								</v-icon>
							</template>
						</v-treeview>
					</v-list-item-content>
				</v-list-item>
			</v-list>
		</v-scroll-x-reverse-transition>

		<v-scroll-x-transition hide-on-leave>
			<v-list v-show="listMenu.search">
				<v-list-item-group v-model="listMenu.active" color="info">
					<v-list-item
						v-for="item in listMenuItems"
						:key="item.code"
						@click="selectTreeMenu([item])"
					>
						<v-list-item-icon v-if="item.icon">
							<v-icon>
								{{ "mdi-" + item.icon }}
							</v-icon>
						</v-list-item-icon>
						<v-list-item-content>
							<v-list-item-title>
								{{ item.name | strFormat("capitalize") }}
							</v-list-item-title>
						</v-list-item-content>
					</v-list-item>
				</v-list-item-group>
			</v-list>
		</v-scroll-x-transition>
	</v-navigation-drawer>
</template>

<style lang="scss" scoped>
.tree-menu ::v-deep {
	.v-treeview-node__root {
		.v-treeview-node__toggle {
			display: none;
		}

		.v-treeview-node__level:first-child {
			display: none;
		}
	}
}
</style>
