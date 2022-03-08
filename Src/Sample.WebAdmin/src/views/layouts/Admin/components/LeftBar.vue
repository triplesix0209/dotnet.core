<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	name: "AdminLayout-LeftBar",
	mixins: [BaseMixin],

	props: {
		menu: { type: Array, require: true },
		currentMenu: { type: Object, require: true },
		toggle: { type: Boolean, default: null },
	},

	data: () => ({
		search: null,
		active: { old: [], current: [] },
		open: [],
		searchActive: null,
	}),

	computed: {
		filteredMenu() {
			let items = [];

			for (let menu of this.menu) {
				if (
					[CONST.MENU_TYPE_PAGE, CONST.MENU_TYPE_CONTROLLER].includes(menu.type)
				)
					items.push(menu);
				else if (menu.type === CONST.MENU_TYPE_GROUP) {
					for (let child of menu.items) {
						items.push(child);
					}
				}
			}

			if (!this.search) return items;
			return items.filter((x) =>
				this.$strFormat(x.name.trim(), "clearVietnameseSign").includes(
					this.$options.filters.strClearVietnameseSign(this.search.trim()),
				),
			);
		},

		_toggle: {
			get() {
				return this.toggle;
			},
			set(value) {
				this.$emit("update:toggle", value);
			},
		},
	},

	watch: {
		$route() {
			if (this.currentMenu.code !== this.active.current[0].code)
				this.selectMenu([this.currentMenu]);
		},
	},

	methods: {
		selectMenu(value) {
			this.search = null;
			this.searchActive = null;
			let menuItem = value.length === 0 ? null : value[0];

			let parentItem = null;
			if (menuItem !== null) {
				for (let menu of this.menu) {
					if (
						menu.type === CONST.MENU_TYPE_CONTROLLER &&
						menu.code === menuItem.code
					)
						break;

					if (
						menu.type === CONST.MENU_TYPE_GROUP &&
						menu.items.some((x) => x.code === menuItem.code)
					) {
						parentItem = menu;
						break;
					}
				}
			}

			let oldMenuItem =
				this.active.old.length === 0 ? null : this.active.old[0];
			if (!menuItem) {
				if (this.active.old.length === 0) {
					this.$emit("menu:change", null);
					return;
				}

				this.$nextTick(() => (this.active.current = this.active.old));
			} else if (
				this.active.old.length === 0 ||
				this.active.old[0].code !== menuItem.code
			) {
				if (parentItem !== null) this.open = [parentItem];
				this.active.current = [menuItem];
				this.active.old = [menuItem];
				this.$emit("menu:change", menuItem);
			}

			if (!oldMenuItem || !menuItem) return;
			if (oldMenuItem.code === menuItem.code) return;
			if (!menuItem.path) return;

			if (menuItem.path === this.$route.path) return;
			this.$router.push({ path: menuItem.path });
		},
	},

	updated() {
		if (!this.currentMenu) return;
		if (this.active.current.length > 0) return;
		this.selectMenu([this.currentMenu]);
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
						v-model="search"
						placeholder="Tìm kiếm..."
						filled
						rounded
						dense
						clearable
						hide-details
					/>
				</v-list-item-content>
			</v-list-item>
		</v-list>

		<v-divider />

		<v-scroll-x-reverse-transition hide-on-leave>
			<v-list v-show="!search" class="menu">
				<v-list-item class="px-0">
					<v-list-item-content class="pa-0">
						<v-treeview
							:items="menu"
							:open.sync="open"
							:active.sync="active.current"
							color="info"
							item-key="code"
							item-children="items"
							:expand-icon="null"
							open-on-click
							activatable
							return-object
							transition
							@update:active="selectMenu"
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
			<v-list v-show="search">
				<v-list-item-group v-model="searchActive" color="info">
					<v-list-item
						v-for="item in filteredMenu"
						:key="item.code"
						@click="selectMenu([item])"
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
.menu ::v-deep {
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
