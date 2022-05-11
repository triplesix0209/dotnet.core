<script>
export default {
	name: "auto-create",
	components: {
		BaseCreatePage: () => import("@/components/Core/Page/Create"),
	},
};
</script>

<template>
	<BaseCreatePage />
</template>
