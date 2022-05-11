<script>
import FieldMixin from "@/mixins/field";
import FieldDatetime from "@/components/Core/FieldType/Datetime";
import FieldEnum from "@/components/Core/FieldType/Enum";
import FieldId from "@/components/Core/FieldType/Id";
import FieldMedia from "@/components/Core/FieldType/Media";
import FieldNumber from "@/components/Core/FieldType/Number";
import FieldPassword from "@/components/Core/FieldType/Password";
import FieldString from "@/components/Core/FieldType/String";
import FieldTextArea from "@/components/Core/FieldType/TextArea";
import FieldHtml from "@/components/Core/FieldType/Html";
import FieldList from "@/components/Core/FieldType/List";

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
				case "html":
					return FieldHtml;
				case "password":
					return FieldPassword;
				case "textarea":
					return FieldTextArea;
				case "enum":
					return FieldEnum;
				case "datetime":
					return FieldDatetime;
				case "media":
					return FieldMedia;
				case "list":
					return FieldList;
			}

			return null;
		},
	},
};
</script>

<template>
	<div v-if="!fieldComponent">{{ field.key }}</div>

	<router-link
		v-else-if="
			mode === 'list' &&
			field.type === 'id' &&
			field.key !== 'id' &&
			data[field.key]
		"
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
		:object-id="objectId"
		:mode="mode"
		:data="data"
		v-model="input"
		@change="raiseChangeInput"
	/>
</template>
