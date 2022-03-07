<script>
import BaseMixin from "@/mixins/base";
import { CONST as layoutConst } from "@/stores/layout";

export default {
	name: "AdminLayout-TopBar",
	mixins: [BaseMixin],

	components: {
		UserMenu: () => import("./UserMenu"),
	},

	props: {
		currentMenu: { type: Object, require: true },
		toggleLeftBar: { type: Boolean, default: null },
	},

	computed: {
		id() {
			return this.$route.params.id;
		},

		title() {
			if (!this.currentMenu) return;
			return this.currentMenu.name;
		},

		breadcrumbs() {
			let result = [];
			if (!this.currentMenu) return result;

			result.push({
				text: this.currentMenu.name,
				href: this.currentMenu.path,
			});

			if (this.currentMenu.method) {
				let methods = this.getMethod({
					controller: this.currentMenu.method.controller,
				});

				if (
					[
						layoutConst.METHOD_TYPE_UPDATE,
						layoutConst.METHOD_TYPE_LIST_CHANGELOG,
					].includes(this.currentMenu.method.type)
				) {
					let method = methods.find(
						(x) => x.type === layoutConst.METHOD_TYPE_DETAIL,
					);
					if (
						method &&
						this.checkPermission(
							method.permissionCodes,
							method.permissionOperator,
						)
					) {
						result.push({
							text: method.name,
							href: layoutConst.generateMethodUrl(method.type, {
								controller: method.controller,
								id: this.id,
							}),
						});
					}
				}

				if (
					[
						layoutConst.METHOD_TYPE_DETAIL,
						layoutConst.METHOD_TYPE_CREATE,
						layoutConst.METHOD_TYPE_UPDATE,
						layoutConst.METHOD_TYPE_LIST_CHANGELOG,
					].includes(this.currentMenu.method.type)
				) {
					let method = methods.find(
						(x) => x.type === layoutConst.METHOD_TYPE_LIST,
					);
					if (
						method &&
						this.checkPermission(
							method.permissionCodes,
							method.permissionOperator,
						)
					) {
						result.push({
							text: method.name,
							href: layoutConst.generateMethodUrl(method.type, {
								controller: method.controller,
							}),
						});
					}
				}
			}

			return result.reverse();
		},

		_toggleLeftBar: {
			get() {
				return this.toggleLeftBar;
			},
			set(value) {
				this.$emit("update:toggle-left-bar", value);
			},
		},
	},

	methods: {
		isMenuAutoHide() {
			let element = this.$refs.toggleMenu?.$el;
			if (!element) return false;

			let display = getComputedStyle(element).display;
			return display !== "none";
		},
	},
};
</script>

<template>
	<v-app-bar height="100" elevation="3" color="white" app>
		<div class="header-row">
			<v-btn
				ref="toggleMenu"
				class="d-lg-none"
				@click="_toggleLeftBar = !_toggleLeftBar"
				icon
			>
				<v-icon v-if="!_toggleLeftBar" dark> mdi-menu </v-icon>
				<v-icon v-else dark> mdi-close </v-icon>
			</v-btn>

			<v-toolbar-title>
				{{ title | strCapitalize }}
			</v-toolbar-title>

			<v-spacer />

			<UserMenu />
		</div>

		<div class="header-divider" />

		<div class="header-row">
			<v-breadcrumbs class="pa-0" :items="breadcrumbs">
				<template #divider>
					<v-icon>mdi-chevron-right</v-icon>
				</template>

				<template v-slot:item="{ item }">
					<v-breadcrumbs-item :to="item.href" :disabled="item.disabled">
						{{ item.text | strCapitalize }}
					</v-breadcrumbs-item>
				</template>
			</v-breadcrumbs>
		</div>
	</v-app-bar>
</template>

<style lang="scss" scoped>
header ::v-deep {
	.v-toolbar__content {
		flex-wrap: wrap;
		padding-left: 0;
		padding-right: 0;

		.header-row {
			width: 100%;
			display: flex;
			align-items: center;
			padding: 0 15px;
		}

		.header-divider {
			width: 100%;
			display: flex;
			border-top: 1px solid #eef0f8;
		}
	}
}
</style>
