<script>
import FieldMixin from "@/mixins/field";
import InputFieldString from "@/components/BaseAuto/Field/InputFieldString";
import InputFieldEnum from "@/components/BaseAuto/Field/InputFieldEnum";
import InputFieldDatetime from "@/components/BaseAuto/Field/InputFieldDatetime";
import InputFieldId from "@/components/BaseAuto/Field/InputFieldId";

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
				if (this.field.type === "enum") return InputFieldEnum;
				if (this.field.type === "dateTime") return InputFieldDatetime;
				if (this.field.type === "id") return InputFieldId;
			}

			return null;
		},
	},
};
</script>

<template>
	<component :is="fieldComponent" :field="field" v-model="input" />
</template>
