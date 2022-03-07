<script>
export default {
	name: "auto-list",
	components: {
		BaseListPage: () => import("@/components/BaseAuto/Page/BaseListPage"),
	},
};
</script>

<template>
	<BaseListPage />
</template>
