<script>
export default {
	name: "auto-list",
	components: {
		BaseListPage: () => import("@/components/BaseAuto/Page/ListPage"),
	},
};
</script>

<template>
	<BaseListPage />
</template>
