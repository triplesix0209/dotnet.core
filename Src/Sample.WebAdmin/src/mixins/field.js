import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	mixins: [BaseMixin],

	props: {
		field: { type: Object, required: true },
		fieldBase: { type: Object },
		value: { type: Object },
		data: { type: Object },
		objectId: { type: String },
		mode: {
			type: String,
			default: "input",
			validator(value) {
				return ["detail", "list", "input"].includes(value);
			},
		},
	},

	data() {
		return {
			loading: false,
			input: this._inputFromValue(),
		};
	},

	computed: {
		fieldRules() {
			return [];
		},

		fieldReadonly() {
			if (
				this._isOperator([
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
				!this._isOperator([
					CONST.FIELD_OPERATOR_EQUAL,
					CONST.FIELD_OPERATOR_NULL,
					CONST.FIELD_OPERATOR_NOT_NULL,
				])
			)
				result += ` (${this.operatorText})`;
			return result;
		},

		fieldPlaceholder() {
			if (
				this.field.defaultValue === undefined ||
				this.field.defaultValue === null
			)
				return null;

			if (
				this._isOperator([
					CONST.FIELD_OPERATOR_NULL,
					CONST.FIELD_OPERATOR_NOT_NULL,
				])
			)
				return this.$strFormat(this.operatorText, "capitalize");

			if (this.field.enum)
				return this.$strFormat(
					this.field.enum[this.field.defaultValue.toString()],
					"capitalize",
				);
			return this.field.defaultValue.toString();
		},

		fieldDisplayValue() {
			return this.data[this.field.key];
		},

		fieldEmptyValue() {
			return "(không có)";
		},

		fieldHint() {
			return this.field.description;
		},

		canUndo() {
			let checkValue =
				this.input.prevValue !== undefined &&
				this.input.prevValue !== null &&
				this.input.value !== this.input.prevValue;
			if (!this.field.operator) return checkValue;

			let checkOperator =
				this.input.prevOperator !== undefined &&
				this.input.prevOperator !== null &&
				this.input.operator !== this.input.prevOperator;
			return checkValue || checkOperator;
		},

		operatorText() {
			if (!this.input || !this.input.operator) return null;
			return this.field.operator[this.input.operator];
		},

		isListOperator() {
			return this._isOperator([
				CONST.FIELD_OPERATOR_IN,
				CONST.FIELD_OPERATOR_NOT_IN,
			]);
		},

		isEmptyFieldData() {
			return (
				!this.data ||
				this.data[this.field.key] === undefined ||
				this.data[this.field.key] === null
			);
		},
	},

	watch: {
		"value"() {
			this.input = this._inputFromValue();
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
		undo() {
			this.input.value = this.input.prevValue;
			this.input.operator = this.input.prevOperator;
		},

		detailUrl({ id, field } = {}) {
			if (!id) throw Error("id is invaild");
			if (!field) field = this.field;

			let controller = field.modelController?.code;
			if (!controller) return null;

			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller,
				id,
			});
		},

		raiseChangeInput({ value, field } = {}) {
			if (!field) field = this.field;
			this.$emit("change", { value, field });
		},

		_isOperator(operators) {
			if (!Array.isArray(operators)) operators = [operators];
			return (
				this.input &&
				!!this.input.operator &&
				operators.includes(this.input.operator)
			);
		},

		_inputFromValue() {
			let input = this.value;
			if (input === undefined || input === null) input = {};

			if (!input.operator && !!this.field.operator)
				input.operator = Object.getOwnPropertyNames(this.field.operator)[0];

			return input;
		},
	},
};
