<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "input-field-string",
	mixins: [FieldMixin],

	props: {
		value: { type: String },
	},

	data: () => ({ items: ["Foo", "Bar", "Fizz", "Buzz"] }),

	computed: {
		rules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			if (this.field.min !== null)
				rules.push(
					(v) =>
						(v && v.length >= this.field.min) ||
						`Phải >= ${this.$numeral(this.field.min)} ký tự`,
				);

			if (this.field.max !== null)
				rules.push(
					(v) =>
						(v && v.length <= this.field.max) ||
						`Phải <= ${this.$numeral(this.field.max)} ký tự`,
				);

			return rules;
		},
	},
};
</script>

<template>
	<v-text-field
		class="input-field"
		v-model="inputValue"
		:label="field.name | strFormat('capitalize')"
		:placeholder="field.defaultValue"
		:hint="field.description"
		:rules="rules"
		:counter="field.max"
		:persistent-placeholder="field.defaultValue !== null"
		persistent-hint
		clearable
	>
		<template #prepend>
			<v-select
				v-if="inputOperators.length > 0"
				class="ma-0 pa-0"
				:items="inputOperators"
				hide-details
			/>
		</template>
	</v-text-field>
</template>
