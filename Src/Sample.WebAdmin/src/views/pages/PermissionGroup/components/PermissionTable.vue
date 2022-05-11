<script>
export default {
	name: "permission-table",

	props: {
		value: { type: Array },
		readonly: { type: Boolean },
	},

	data: () => ({
		headers: [
			{
				value: "categoryName",
				text: "Nhóm",
				sortable: true,
			},
			{
				value: "code",
				text: "Mã quyền",
				sortable: true,
			},
			{
				value: "name",
				text: "Tên gọi",
				sortable: true,
			},
			{
				value: "value",
				text: "Giá trị",
				sortable: true,
			},
		],

		enumValues: [
			{ value: 0, text: "Kế thừa" },
			{ value: 1, text: "Cho phép" },
			{ value: -1, text: "Cấm" },
		],
	}),

	methods: {
		itemColor(data, item) {
			let value = item.value;
			if (value === 0) value = data.actualValue ? 1 : -1;
			if (value === 1) return "green";
			if (value === -1) return "red";
		},

		itemText(data, item) {
			let valueText = this.enumValues.find((x) => x.value === item.value).text;
			if (item.value !== 0) return valueText;

			let actualValueText = data.actualValue ? 1 : -1;
			actualValueText = this.enumValues.find(
				(x) => x.value === actualValueText,
			).text;
			return `${valueText}: ${actualValueText}`;
		},
	},
};
</script>

<template>
	<v-data-table
		class="mt-1 elevation-1"
		:headers="headers"
		:items="value"
		:items-per-page="-1"
		hide-default-footer
	>
		<template v-slot:[`item.categoryName`]="{ item }">
			{{ item.categoryName | strFormat("capitalize") }}
		</template>

		<template v-slot:[`item.name`]="{ item }">
			{{ item.name | strFormat("capitalize") }}
		</template>

		<template v-slot:[`item.value`]="{ item }">
			<template v-if="readonly">
				<v-chip v-if="item.value === 1" color="green" dark> Cho phép </v-chip>
				<v-chip v-else-if="item.value === -1" color="red" dark> Cấm </v-chip>
				<v-chip v-else :color="item.actualValue ? 'green' : 'red'" dark>
					Kế thừa: {{ item.actualValue ? "Cho phép" : "Cấm" }}
				</v-chip>
			</template>

			<v-select v-else v-model="item.select" :items="enumValues" return-object>
				<template v-slot:selection="{ item: selectedItem }">
					<v-chip :color="itemColor(item, selectedItem)" dark small>
						{{ itemText(item, selectedItem) }}
					</v-chip>
				</template>
			</v-select>
		</template>
	</v-data-table>
</template>
