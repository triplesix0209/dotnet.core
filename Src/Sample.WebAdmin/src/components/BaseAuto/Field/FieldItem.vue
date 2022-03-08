<script>
import FieldMixin from "@/mixins/field";
import InputFieldString from "@/components/BaseAuto/Field/InputFieldString";

export default {
	name: "field-item",
	mixins: [FieldMixin],

	props: {
		inputMode: { type: Boolean, default: false },
	},

	computed: {
		fieldComponent() {
			if (this.inputMode) {
				if (this.field.type === "string") return InputFieldString;
			}

			return null;
		},
	},
};
</script>

<template>
	<component v-model="inputValue" :is="fieldComponent" :field="field" />
</template>
