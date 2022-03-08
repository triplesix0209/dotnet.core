import BaseMixin from "@/mixins/base";

export default {
	mixins: [BaseMixin],

	props: {
		value: {},
		field: { type: Object, required: true },
	},

	data() {
		return {
			loading: false,
			inputValue: null,
			inputOperatorValue: null,
		};
	},

	computed: {
		rules() {
			return [];
		},

		inputOperators() {
			let result = [];
			if (!this.field.operator) return result;

			for (let key in this.field.operator) {
				console.log(this.field.operator[key]);
			}

			return result;
		},
	},

	watch: {
		value() {
			this.inputValue = this.value;
		},

		inputValue() {
			this.$emit("input", this.inputValue);
		},
	},
};
