<script>
import { mapGetters, mapActions } from "vuex";

export default {
	name: "App",

	computed: {
		...mapGetters("lightbox", ["mediaList", "mediaIndex"]),
	},

	methods: {
		...mapActions("lightbox", ["closeLightbox"]),
	},
};
</script>

<template>
	<v-app>
		<router-view />

		<CoolLightBox
			:items="mediaList"
			:index="mediaIndex"
			@close="closeLightbox"
		/>
	</v-app>
</template>

<style lang="scss">
@import "assets/style.scss";
</style>
