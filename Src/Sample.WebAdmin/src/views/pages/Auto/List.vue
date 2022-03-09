<script>
export default {
	name: "auto-list",
	components: {
		BaseListPage: () => import("@/components/BaseAuto/Page/List"),
	},
};
</script>

<template>
	<BaseListPage />
</template>
