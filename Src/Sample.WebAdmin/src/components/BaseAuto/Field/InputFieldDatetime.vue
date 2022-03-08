<script>
import FieldMixin from "@/mixins/field";
import { CONST } from "@/stores/layout";

export default {
	name: "input-field-datetime",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("@/components/BaseAuto/Field/FieldOperator"),
	},

	data: () => ({ dates: [null, null] }),

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			return rules;
		},

		isBetweenOperator() {
			return this.isOperator([
				CONST.FIELD_OPERATOR_BETWEEN,
				CONST.FIELD_OPERATOR_NOT_BETWEEN,
			]);
		},

		dateDisplay1: {
			get() {
				if (!this.dates[0]) return null;
				return this.$moment(this.dates[0]).format("DD/MM/YYYY HH:mm");
			},
			set() {},
		},

		dateDisplay2: {
			get() {
				if (!this.dates[1]) return null;
				return this.$moment(this.dates[1]).format("DD/MM/YYYY HH:mm");
			},
			set() {},
		},
	},

	watch: {
		"dates"() {
			let value = this.dates.map((x) => (x ? x : null));
			if (!this.isBetweenOperator) value = value[0];

			this.input = { ...this.input, value };
		},

		"input.operator"(val, old) {
			if (
				[CONST.FIELD_OPERATOR_NULL, CONST.FIELD_OPERATOR_NOT_NULL].includes(val)
			) {
				this.dates = [null, null];
				this.input = { ...this.input, value: this.dates[0] };
				return;
			}

			let isListVal = [
				CONST.FIELD_OPERATOR_BETWEEN,
				CONST.FIELD_OPERATOR_NOT_BETWEEN,
			].includes(val);
			let isListOld =
				!!old &&
				[
					CONST.FIELD_OPERATOR_BETWEEN,
					CONST.FIELD_OPERATOR_NOT_BETWEEN,
				].includes(old);
			if (isListVal !== isListOld) {
				this.dates = [null, null];
				this.input = { ...this.input, value: this.dates };
				return;
			}
		},
	},
};
</script>

<template>
	<div class="input-field">
		<datetime
			ref="datetime-1"
			input-class="d-none"
			type="datetime"
			v-model="dates[0]"
		/>

		<datetime
			ref="datetime-2"
			input-class="d-none"
			type="datetime"
			v-model="dates[1]"
		/>

		<FieldOperator
			class="input-operator"
			v-model="input.operator"
			:field="field"
		/>

		<v-text-field
			class="input-datetime"
			:class="{ 'split-date': isBetweenOperator }"
			v-model="dateDisplay1"
			:rules="fieldRules"
			:label="fieldLabel"
			:counter="field.max"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			persistent-hint
			clearable
			readonly
			@click="(e) => $refs[`datetime-1`].open(e)"
			@click:clear="input.value = null"
		>
			<template #append>
				<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
				<v-icon
					@click="(e) => $refs[`datetime-1`].open(e)"
					:color="input.value ? 'primary' : ''"
				>
					mdi-calendar
				</v-icon>
			</template>
		</v-text-field>

		<v-icon v-if="isBetweenOperator">mdi-minus</v-icon>

		<v-text-field
			v-if="isBetweenOperator"
			class="input-datetime"
			:class="{ 'split-date': isBetweenOperator }"
			v-model="dateDisplay2"
			:rules="fieldRules"
			:label="fieldLabel"
			:counter="field.max"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			persistent-hint
			clearable
			readonly
			@click="(e) => $refs[`datetime-2`].open(e)"
			@click:clear="input.value = null"
		>
			<template #append>
				<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
				<v-icon
					@click="(e) => $refs[`datetime-2`].open(e)"
					:color="input.value ? 'primary' : ''"
				>
					mdi-calendar
				</v-icon>
			</template>
		</v-text-field>
	</div>
</template>

<style lang=scss scoped>
.input-operator {
	display: inline-flex;
	vertical-align: middle;
	margin: 4px 9px 4px 0;
}

.input-datetime {
	display: inline-flex;
	width: calc(100% - 45px);

	&.split-date {
		width: calc((100% - 69px) / 2);
	}
}
</style>
