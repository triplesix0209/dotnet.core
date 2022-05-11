<script>
import PageMixin from "@/mixins/page";

export default {
	name: "permission-proup-update",
	mixins: [PageMixin],

	components: {
		BaseUpdatePage: () => import("@/components/Core/Page/Update"),
		PermissionTable: () => import("./components/PermissionTable"),
	},

	data: () => ({ data: null, permissionItems: [] }),

	methods: {
		async loaded() {
			await this.loadListPermission();
		},

		async change({ value, field }) {
			if (field.key === "hierarchyParentId")
				await this.loadListPermission({ parentId: value });
		},

		async loadListPermission({ parentId } = {}) {
			let { data } = await this.requestApi({
				method: "get",
				url: "/admin/permissionGroup/permissionValue",
				params: { id: parentId ?? this.data.hierarchyParentId },
			});

			let permissionItems = data;
			for (let item of permissionItems) {
				let oldItem = this.permissionItems.find((x) => x.code === item.code);
				let dataItem = this.data.listPermissionValue.find(
					(x) => x.code === item.code,
				);
				item.select = { value: dataItem.value };
				if (!oldItem) item.select = { value: dataItem.value };
				else item.select = oldItem.select;
			}

			this.permissionItems = permissionItems;
		},

		prepareSubmitData(data) {
			data.listPermissionValue = [];
			for (let item of this.permissionItems) {
				data.listPermissionValue.push({
					code: item.code,
					value: item.select.value,
				});
			}

			return data;
		},
	},
};
</script>

<template>
	<BaseUpdatePage
		:data.sync="data"
		:prepare-submit-data="prepareSubmitData"
		@loaded="loaded"
		@change="change"
	>
		<template #field-listPermissionValue="{ field }">
			<v-label>{{ field.name | strFormat("capitalize") }}</v-label>
			<PermissionTable v-model="permissionItems" />
		</template>
	</BaseUpdatePage>
</template>
