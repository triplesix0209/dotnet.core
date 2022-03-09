<script>
export default {
	name: "auto-create",
	components: {
		BaseCreatePage: () => import("@/components/BaseAuto/Page/Create"),
	},
};
</script>

<template>
	<BaseCreatePage />
</template>
