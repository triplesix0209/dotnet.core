<script>
export default {
	name: "auto-update",
	components: {
		BaseUpdatePage: () => import("@/components/BaseAuto/Page/Update"),
	},
};
</script>

<template>
	<BaseUpdatePage />
</template>
