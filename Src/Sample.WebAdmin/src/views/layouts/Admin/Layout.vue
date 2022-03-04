<script>
export default {
	name: "AdminLayout",

	components: {
		TopBar: () => import("./components/TopBar"),
		LeftBar: () => import("./components/LeftBar"),
	},

	data: () => ({ toggleLeftBar: null }),
};
</script>

<template>
	<v-app>
		<TopBar :toggle-left-bar.sync="toggleLeftBar" />

		<LeftBar v-model="toggleLeftBar" />

		<v-main>
			<router-view />
		</v-main>
	</v-app>
</template>
