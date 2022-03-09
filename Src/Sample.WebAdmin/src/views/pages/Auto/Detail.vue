<script>
export default {
	name: "auto-list",
	components: {
		BaseDetailPage: () => import("@/components/BaseAuto/Page/Detail"),
	},
};
</script>

<template>
	<BaseDetailPage />
</template>
