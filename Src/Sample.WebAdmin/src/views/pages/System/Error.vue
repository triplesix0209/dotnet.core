<script>
export default {
	name: "error",

	data() {
		let errorCode = this.$route.params.code;
		if (errorCode === undefined || errorCode === null) errorCode = "404";

		return { errorCode };
	},
};
</script>

<template>
	<v-app
		:style="{ backgroundImage: `url(/assets/error-${this.errorCode}.png)` }"
	/>
</template>

<style lang="scss" scoped>
#app {
	background-size: contain;
	background-position: center;
	background-repeat: no-repeat;
}
</style>
