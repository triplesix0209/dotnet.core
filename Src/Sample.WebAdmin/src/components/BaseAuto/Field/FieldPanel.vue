<script>
import BaseMixin from "@/mixins/base";

export default {
	name: "field-panel",
	mixins: [BaseMixin],

	components: {
		FieldItem: () => import("@/components/BaseAuto/Field/FieldItem"),
	},

	props: {
		fields: { type: Array, required: true },
		groups: { type: Array, default: () => [] },
		inputMode: { type: Boolean, default: false },
	},

	data() {
		let value = {};
		for (let field of this.fields) {
			value[field.key] = undefined;
		}

		return { value };
	},

	computed: {
		renderFields() {
			return this.fields.filter((x) => x.render);
		},
	},
};
</script>

<template>
	<v-container>
		<v-row>
			<v-col
				v-for="field in renderFields"
				:key="field.key"
				:cols="field.gridCol"
			>
				<FieldItem :field="field" :input-mode="inputMode" />
			</v-col>
		</v-row>
	</v-container>
</template>
