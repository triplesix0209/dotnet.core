<script>
import PageMixin from "@/mixins/page";

export default {
	name: "permission-proup-changelog",
	mixins: [PageMixin],

	components: {
		BaseChangeLogPage: () => import("@/components/Core/Page/ChangeLog"),
		PermissionTable: () => import("./components/PermissionTable"),
	},

	data: () => ({ data: null }),
};
</script>

<template>
	<BaseChangeLogPage :data.sync="data">
		<template #field-listPermissionValue="{ field, mode }">
			<v-label>{{ field.name | strFormat("capitalize") }}</v-label>
			<PermissionTable
				v-model="JSON.parse(data[mode]).listPermissionValue"
				readonly
			/>
		</template>
	</BaseChangeLogPage>
</template>
