<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "input-field-enum",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("@/components/BaseAuto/Field/FieldOperator"),
	},

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			return rules;
		},

		items() {
			let items = [];

			for (let item in this.field.enum)
				items.push({
					value: item,
					text: this.$strFormat(this.field.enum[item], "capitalize"),
				});

			return items;
		},
	},
};
</script>

<template>
	<v-select
		class="input-field"
		v-model="input.value"
		:items="items"
		:rules="fieldRules"
		:readonly="fieldReadonly"
		:label="fieldLabel"
		:counter="field.max"
		:placeholder="fieldPlaceholder"
		:persistent-placeholder="fieldPlaceholder !== null"
		:hint="fieldHint"
		:multiple="isListOperator"
		persistent-hint
		clearable
		small-chips
	>
		<template #prepend>
			<FieldOperator v-model="input.operator" :field="field" />
		</template>

		<template #append>
			<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
		</template>
	</v-select>
</template>
