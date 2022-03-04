<script>
import { mapGetters } from "vuex";
import BaseMixin from "@/mixins/base";

export default {
	name: "AdminLayout-LeftBar",
	mixins: [BaseMixin],

	props: {
		value: { type: Boolean, default: null },
	},

	data: () => ({
		search: null,
	}),

	computed: {
		...mapGetters(["layout/menu"]),

		menu() {
			let menu = this["layout/menu"];
			if (!this.search) return menu;

			let items = [];
			for (let menuItem of menu) {
				if (menuItem.type === "controller") items.push(menuItem);
				else {
					for (let childrenItem of menuItem.items) items.push(childrenItem);
				}
			}

			items = items.filter((x) => x.name.includes(this.search));

			return items;
		},

		_value: {
			get() {
				return this.value;
			},
			set(value) {
				this.$emit("input", value);
			},
		},
	},
};
</script>

<template>
	<v-navigation-drawer v-model="_value" color="#1E1E2D" app dark>
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

		<v-list>
			<v-list-item>
				<v-list-item-content>
					<v-treeview
						class="menu"
						:items="menu"
						item-key="code"
						item-children="items"
						:expand-icon="null"
						open-on-click
					>
						<template #label="{ item }">
							<v-icon v-if="item.icon">
								{{ "mdi-" + item.icon }}
							</v-icon>

							{{ item.name | strTitlecase }}
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
	</v-navigation-drawer>
</template>

<style lang="scss" scoped>
.menu ::v-deep {
	.v-treeview-node__toggle {
		display: none;
	}
}
</style>
