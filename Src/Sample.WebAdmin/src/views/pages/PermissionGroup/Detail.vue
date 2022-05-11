<script>
import PageMixin from "@/mixins/page";

export default {
	name: "permission-proup-detail",
	mixins: [PageMixin],

	components: {
		BaseDetailPage: () => import("@/components/Core/Page/Detail"),
		PermissionTable: () => import("./components/PermissionTable"),
	},

	data: () => ({ data: null }),
};
</script>

<template>
	<BaseDetailPage :data.sync="data">
		<template #field-listPermissionValue="{ field }">
			<v-label>{{ field.name | strFormat("capitalize") }}</v-label>
			<PermissionTable v-model="data.listPermissionValue" readonly />
		</template>
	</BaseDetailPage>
</template>
