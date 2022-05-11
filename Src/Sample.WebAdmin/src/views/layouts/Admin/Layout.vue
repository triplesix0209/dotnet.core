<script>
import BaseMixin from "@/mixins/base";
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
	}),

	computed: {
		...mapGetters("lightbox", ["mediaList", "mediaIndex"]),

		path() {
			return this.$route.path;
		},
	},

	methods: {
		menuChanged() {
			if (this.$refs.topBar.isMenuAutoHide()) this.toggleLeftBar = false;
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
			<TopBar ref="topBar" :toggle-left-bar.sync="toggleLeftBar" />

			<LeftBar :toggle.sync="toggleLeftBar" @menu:change="menuChanged" />

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
