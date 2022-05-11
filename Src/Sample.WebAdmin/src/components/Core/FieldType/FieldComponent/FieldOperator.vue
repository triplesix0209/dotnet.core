<script>
import BaseMixin from "@/mixins/base";

export default {
	name: "field-operator",
	mixins: [BaseMixin],

	props: {
		field: { type: Object, required: true },
		value: { type: String },
	},

	data() {
		let operatorValue = this.value
			? this.value
			: Object.getOwnPropertyNames(this.field.operator)[0];

		let operatorIndex = Object.getOwnPropertyNames(
			this.field.operator,
		).findIndex((x) => x === operatorValue);

		return { operatorIndex };
	},

	computed: {
		items() {
			let result = [];
			if (!this.field.operator) return result;

			for (let key in this.field.operator)
				result.push({ value: key, text: this.field.operator[key] });

			return result;
		},
	},

	watch: {
		value() {
			this.operatorIndex = Object.getOwnPropertyNames(
				this.field.operator,
			).findIndex((x) => x === this.value);
		},

		operatorIndex(val, old) {
			if (val === undefined) {
				this.$nextTick(() => (this.operatorIndex = old ?? 0));
				return;
			}

			this.$emit("input", Object.getOwnPropertyNames(this.field.operator)[val]);
		},
	},
};
</script>

<template>
	<v-menu v-if="items.length > 0" offset-y>
		<template #activator="{ on, attrs }">
			<v-btn v-bind="attrs" v-on="on" icon>
				<v-icon :color="operatorIndex !== 0 ? 'primary' : ''">
					mdi-filter-menu
				</v-icon>
			</v-btn>
		</template>

		<v-list>
			<v-list-item-group v-model="operatorIndex" color="primary">
				<v-list-item v-for="item in items" :key="item.value">
					<v-list-item-title>
						{{ item.text | strFormat("capitalize") }}
					</v-list-item-title>
				</v-list-item>
			</v-list-item-group>
		</v-list>
	</v-menu>
</template>
