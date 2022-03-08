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
		let data = {};
		for (let field of this.fields) {
			data[field.key] = null;
		}

		return { data };
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
		<v-row class="pb-3">
			<v-col
				v-for="field in renderFields"
				class="pb-0"
				:key="field.key"
				:sm="field.gridCol"
				cols="12"
			>
				<FieldItem
					v-model="data[field.key]"
					:field="field"
					:input-mode="inputMode"
				/>
			</v-col>
		</v-row>
	</v-container>
</template>
