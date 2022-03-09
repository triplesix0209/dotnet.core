<script>
import BaseMixin from "@/mixins/base";

export default {
	name: "field-panel",
	mixins: [BaseMixin],

	components: {
		FieldItem: () => import("@/components/BaseAuto/Field/FieldItem"),
	},

	props: {
		value: { type: Object },
		data: { type: Object },
		fields: { type: Array, required: true },
		groups: { type: Array, default: () => [] },
		fieldSm: [Number, String],
		fieldMd: [Number, String],
		fieldLg: [Number, String],
		mode: {
			type: String,
			default: "input",
			validator(value) {
				return ["detail", "list", "input"].includes(value);
			},
		},
	},

	data() {
		return { inputs: this._inputFromValue() };
	},

	computed: {
		renderFields() {
			return this.fields.filter((x) => x.render);
		},
	},

	watch: {
		value() {
			let inputs = this._inputFromValue();
			if (JSON.stringify(inputs) !== JSON.stringify(this.inputs))
				this.inputs = inputs;
		},

		inputs: {
			deep: true,
			handler() {
				this.$emit("input", this.inputs);
			},
		},
	},

	methods: {
		_inputFromValue() {
			let result = {};

			for (let field of this.fields) {
				result[field.key] = { value: null, operator: null };

				if (!!this.data && this.data[field.key] !== undefined) {
					let value = this.data[field.key];

					if (field.type === "enum") value = value.toString();

					result[field.key].value = value;
					result[field.key].prevValue = value;
				}

				if (!!this.value && !!this.value[field.key])
					result[field.key] = this.value[field.key];
			}

			return result;
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
				:sm="fieldSm ? fieldSm : field.gridCol"
				:md="fieldMd ? fieldMd : field.gridCol"
				:lg="fieldLg ? fieldLg : field.gridCol"
				cols="12"
			>
				<FieldItem
					v-model="inputs[field.key]"
					:data="data"
					:field="field"
					:field-list="fields"
					:mode="mode"
				/>
			</v-col>
		</v-row>
	</v-container>
</template>
