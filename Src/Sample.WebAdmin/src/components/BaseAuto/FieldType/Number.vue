<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-number",
	mixins: [FieldMixin],

	components: {
		FieldLink: () => import("@/components/BaseAuto/Field/FieldLink"),
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

		fieldDisplayValue() {
			return this.$numeral(this.data[this.field.key], "0,0");
		},
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">-</div>
		<div v-else>
			{{ data[field.key] | numeral("0,0") }}
		</div>
	</div>

	<div v-else-if="mode === 'detail'">
		<v-text-field
			v-if="data"
			v-model="fieldDisplayValue"
			:label="fieldLabel"
			:placeholder="fieldEmptyValue"
			:hint="fieldHint"
			persistent-placeholder
			persistent-hint
			readonly
		>
			<template #append>
				<FieldLink :field="fieldBase" :data="data" />
			</template>
		</v-text-field>
	</div>

	<div v-else>
		<v-text-field
			v-if="!isListOperator"
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
				<FieldOperator
					v-if="input.operator"
					v-model="input.operator"
					:field="field"
				/>
			</template>

			<template #append>
				<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
			</template>
		</v-combobox>
	</div>
</template>
