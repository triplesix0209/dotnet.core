<script>
import FieldMixin from "@/mixins/field";
import FieldItemString from "@/components/BaseAuto/Field/FieldItemString";
import FieldItemNumber from "@/components/BaseAuto/Field/FieldItemNumber";
import FieldItemEnum from "@/components/BaseAuto/Field/FieldItemEnum";
import FieldItemDatetime from "@/components/BaseAuto/Field/FieldItemDatetime";
import FieldItemId from "@/components/BaseAuto/Field/FieldItemId";

export default {
	name: "field-item",
	mixins: [FieldMixin],

	computed: {
		fieldComponent() {
			if (this.inputMode) {
				if (this.field.type === "number") return FieldItemNumber;
				if (this.field.type === "string") return FieldItemString;
				if (this.field.type === "enum") return FieldItemEnum;
				if (this.field.type === "dateTime") return FieldItemDatetime;
				if (["id", "parentId"].includes(this.field.type)) return FieldItemId;
			}

			return null;
		},
	},
};
</script>

<template>
	<component
		v-if="fieldComponent"
		:is="fieldComponent"
		:field="field"
		:input-mode="inputMode"
		v-model="input"
	/>

	<div v-else>{{ field.key }}</div>
</template>
