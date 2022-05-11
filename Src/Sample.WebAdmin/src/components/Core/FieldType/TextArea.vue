<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-string",
	mixins: [FieldMixin],

	components: {
		FieldLink: () => import("./FieldComponent/FieldLink"),
		FieldOperator: () => import("./FieldComponent/FieldOperator"),
	},

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			if (this.field.min !== undefined && this.field.min !== null)
				rules.push(
					(v) =>
						!v ||
						v.length >= this.field.min ||
						`Phải >= ${this.$numeral(this.field.min)} ký tự`,
				);

			if (this.field.max !== undefined && this.field.max !== null)
				rules.push(
					(v) =>
						!v ||
						v.length <= this.field.max ||
						`Phải <= ${this.$numeral(this.field.max)} ký tự`,
				);

			return rules;
		},
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">-</div>
		<div v-else>{{ data[field.key] }}</div>
	</div>

	<div v-else-if="mode === 'detail'">
		<v-textarea
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
		</v-textarea>
	</div>

	<div v-else>
		<v-textarea
			v-if="!isListOperator"
			v-model="input.value"
			:rules="fieldRules"
			:readonly="fieldReadonly"
			:label="fieldLabel"
			:counter="field.max"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			persistent-hint
			clearable
			@change="raiseChangeInput({ value: input.value })"
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
		</v-textarea>

		<v-combobox
			v-else
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
			@change="raiseChangeInput({ value: input.value })"
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
