<script>
export default {
	name: "auto-detail",
	components: {
		BaseDetailPage: () => import("@/components/BaseAuto/Page/Detail"),
	},
};
</script>

<template>
	<BaseDetailPage />
</template>
