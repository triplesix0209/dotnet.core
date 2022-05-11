<script>
export default {
	name: "auto-detail",
	components: {
		BaseDetailPage: () => import("@/components/Core/Page/Detail"),
	},
};
</script>

<template>
	<BaseDetailPage />
</template>
