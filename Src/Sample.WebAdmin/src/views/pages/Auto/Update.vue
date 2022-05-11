<script>
export default {
	name: "auto-update",
	components: {
		BaseUpdatePage: () => import("@/components/Core/Page/Update"),
	},
};
</script>

<template>
	<BaseUpdatePage />
</template>
