<script>
export default {
	name: "auto-changelog",
	components: {
		BaseChangeLogPage: () => import("@/components/Core/Page/ChangeLog"),
	},
};
</script>

<template>
	<BaseChangeLogPage />
</template>
