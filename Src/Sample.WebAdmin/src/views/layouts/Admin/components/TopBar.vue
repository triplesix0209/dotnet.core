<script>
import BaseMixin from "@/mixins/base";
import { mapGetters } from "vuex";

export default {
	name: "AdminLayout-TopBar",
	mixins: [BaseMixin],

	components: {
		UserMenu: () => import("./UserMenu"),
	},

	props: {
		toggleLeftBar: { type: Boolean, default: null },
	},

	computed: {
		...mapGetters("layout", ["pageTitle", "breadcrumbs"]),

		path() {
			return this.$route.path;
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
	<v-app-bar height="110" elevation="3" color="white" app>
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

			<v-spacer />

			<UserMenu v-slot="{ attrs, on }">
				<v-avatar v-bind="attrs" v-on="on" size="45" rounded v-ripple>
					<v-img :src="currentUser.avatarLink" />
				</v-avatar>
			</UserMenu>
		</div>

		<div class="header-divider" />

		<v-scroll-y-reverse-transition hide-on-leave>
			<div :key="path" class="header-row">
				<h4>{{ pageTitle | strFormat("titleCase") }}</h4>

				<v-divider v-if="breadcrumbs.length > 0" class="mx-2" vertical />

				<v-breadcrumbs
					v-if="breadcrumbs.length > 0"
					class="pa-0"
					:items="breadcrumbs"
				>
					<template #divider>
						<v-icon>mdi-chevron-right</v-icon>
					</template>

					<template v-slot:item="{ item }">
						<router-link v-if="item.href" :to="{ path: item.href }">
							{{ item.text | strFormat("capitalize") }}
						</router-link>

						<span v-else>
							{{ item.text | strFormat("capitalize") }}
						</span>
					</template>
				</v-breadcrumbs>
			</div>
		</v-scroll-y-reverse-transition>
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
