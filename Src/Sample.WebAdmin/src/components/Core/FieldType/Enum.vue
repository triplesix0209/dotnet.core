<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-enum",
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

		fieldDisplayValue() {
			return this.$strFormat(
				this.field.enum[this.data[this.field.key]],
				"capitalize",
			);
		},
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">-</div>
		<div v-else>
			<v-chip>
				{{ field.enum[data[field.key]] | strFormat("capitalize") }}
			</v-chip>
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
		<v-select
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
		</v-select>
	</div>
</template>
