import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	mixins: [BaseMixin],

	props: {
		field: { type: Object, required: true },
		value: { type: Object },
		inputMode: { type: Boolean, default: false },
	},

	data() {
		return {
			renderKey: 0,
			loading: false,
			input: this.inputFromValue(),
		};
	},

	computed: {
		fieldRules() {
			return [];
		},

		fieldReadonly() {
			if (
				this.isOperator([
					CONST.FIELD_OPERATOR_NULL,
					CONST.FIELD_OPERATOR_NOT_NULL,
				])
			)
				return true;

			return false;
		},

		fieldLabel() {
			let result = this.$strFormat(this.field.name, "capitalize");

			if (
				this.operatorText &&
				!this.isOperator([
					CONST.FIELD_OPERATOR_EQUAL,
					CONST.FIELD_OPERATOR_IS,
					CONST.FIELD_OPERATOR_NULL,
					CONST.FIELD_OPERATOR_NOT_NULL,
				])
			)
				result += ` (${this.operatorText})`;
			return result;
		},

		fieldPlaceholder() {
			if (
				this.isOperator([
					CONST.FIELD_OPERATOR_NULL,
					CONST.FIELD_OPERATOR_NOT_NULL,
				])
			)
				return this.$strFormat(this.operatorText, "capitalize");

			return this.field.defaultValue;
		},

		fieldHint() {
			return this.field.description;
		},

		canUndo() {
			let checkValue =
				this.input.prevValue !== undefined &&
				this.input.prevValue !== null &&
				this.input.value !== this.input.prevValue;

			let checkOperator =
				!this.field.operator ||
				(this.input.prevOperator !== undefined &&
					this.input.prevOperator !== null &&
					this.input.operator !== this.input.prevOperator);

			return checkValue || checkOperator;
		},

		operatorText() {
			if (!this.input || !this.input.operator) return null;
			return this.field.operator[this.input.operator];
		},

		isListOperator() {
			return this.isOperator([
				CONST.FIELD_OPERATOR_IN,
				CONST.FIELD_OPERATOR_NOT_IN,
			]);
		},
	},

	watch: {
		"value"() {
			this.input = this.inputFromValue();
		},

		"input"() {
			this.$emit("input", this.input);
		},

		"input.operator"(val, old) {
			if (
				[CONST.FIELD_OPERATOR_NULL, CONST.FIELD_OPERATOR_NOT_NULL].includes(val)
			) {
				this.input = { ...this.input, value: null };
				return;
			}

			let isListVal = [
				CONST.FIELD_OPERATOR_IN,
				CONST.FIELD_OPERATOR_NOT_IN,
			].includes(val);
			let isListOld =
				!!old &&
				[CONST.FIELD_OPERATOR_IN, CONST.FIELD_OPERATOR_NOT_IN].includes(old);
			if (isListVal !== isListOld) {
				this.input = { ...this.input, value: null };
				return;
			}
		},
	},

	methods: {
		isOperator(operators) {
			if (!Array.isArray(operators)) operators = [operators];
			return (
				this.input &&
				!!this.input.operator &&
				operators.includes(this.input.operator)
			);
		},

		undo() {
			this.input.value = this.input.prevValue;
			this.input.operator = this.input.prevOperator;
		},

		inputFromValue() {
			let input = this.value;
			if (input === undefined || input === null) input = {};

			if (!input.operator && !!this.field.operator)
				input.operator = Object.getOwnPropertyNames(this.field.operator)[0];

			return input;
		},
	},
};
