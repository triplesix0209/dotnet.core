<script>
export default {
	name: "auto-list",
	components: {
		BaseListPage: () => import("@/components/Core/Page/List"),
	},
};
</script>

<template>
	<BaseListPage />
</template>
