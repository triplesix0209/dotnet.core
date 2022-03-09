<script>
import FieldMixin from "@/mixins/field";
import FieldDatetime from "@/components/BaseAuto/FieldType/Datetime";
import FieldEnum from "@/components/BaseAuto/FieldType/Enum";
import FieldId from "@/components/BaseAuto/FieldType/Id";
import FieldMedia from "@/components/BaseAuto/FieldType/Media";
import FieldNumber from "@/components/BaseAuto/FieldType/Number";
import FieldString from "@/components/BaseAuto/FieldType/String";

export default {
	name: "field-item",
	mixins: [FieldMixin],

	props: { fieldList: { type: Array } },

	computed: {
		fieldDisplay() {
			if (this.field.displayBy)
				return this.fieldList.find((x) => x.key === this.field.displayBy);
			return this.field;
		},

		fieldComponent() {
			switch (this.fieldDisplay.type) {
				case "id":
				case "parentId":
					return FieldId;
				case "number":
					return FieldNumber;
				case "string":
					return FieldString;
				case "enum":
					return FieldEnum;
				case "datetime":
					return FieldDatetime;
				case "media":
					return FieldMedia;
			}

			return null;
		},
	},
};
</script>

<template>
	<div v-if="!fieldComponent">{{ field.key }}</div>

	<router-link
		v-else-if="mode === 'list' && field.type === 'id' && data[field.key]"
		:to="{ path: detailUrl({ id: data[field.key] }) }"
	>
		<component
			:is="fieldComponent"
			:field="fieldDisplay"
			:field-base="field"
			:data="data"
			:mode="mode"
			v-model="input"
		/>
	</router-link>

	<component
		v-else
		:is="fieldComponent"
		:field="fieldDisplay"
		:field-base="field"
		:data="data"
		:mode="mode"
		v-model="input"
	/>
</template>
