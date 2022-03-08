<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "input-field-number",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("@/components/BaseAuto/Field/FieldOperator"),
	},

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			if (this.field.min !== null)
				rules.push(
					(v) =>
						(v !== undefined && v !== null && v >= this.field.min) ||
						`Phải >= ${this.$numeral(this.field.min)}`,
				);

			if (this.field.max !== null)
				rules.push(
					(v) =>
						(v !== undefined && v !== null && v <= this.field.max) ||
						`Phải <= ${this.$numeral(this.field.max)}`,
				);

			return rules;
		},
	},
};
</script>

<template>
	<v-text-field
		v-if="!isListOperator"
		class="input-field"
		type="number"
		v-model="input.value"
		:rules="fieldRules"
		:readonly="fieldReadonly"
		:label="fieldLabel"
		:placeholder="fieldPlaceholder"
		:persistent-placeholder="fieldPlaceholder !== null"
		:hint="fieldHint"
		persistent-hint
		clearable
	>
		<template #prepend>
			<FieldOperator v-model="input.operator" :field="field" />
		</template>

		<template #append>
			<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
		</template>
	</v-text-field>

	<v-combobox
		v-else
		class="input-field"
		type="number"
		v-model="input.value"
		:rules="fieldRules"
		:readonly="fieldReadonly"
		:label="fieldLabel"
		:placeholder="fieldPlaceholder"
		:persistent-placeholder="fieldPlaceholder !== null"
		:hint="fieldHint"
		persistent-hint
		clearable
		multiple
		small-chips
		deletable-chips
	>
		<template #prepend>
			<FieldOperator v-model="input.operator" :field="field" />
		</template>

		<template #append>
			<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
		</template>
	</v-combobox>
</template>
