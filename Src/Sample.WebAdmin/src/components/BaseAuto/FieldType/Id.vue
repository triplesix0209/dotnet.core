<script>
import FieldMixin from "@/mixins/field";
import { CONST } from "@/stores/layout";

export default {
	name: "field-type-id",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("@/components/BaseAuto/Field/FieldOperator"),
	},

	data: () => ({ items: [], search: null }),

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			return rules;
		},

		modelListMethod() {
			return this.getMethod({
				controller: this.field.modelController.code,
				type: CONST.METHOD_TYPE_LIST,
				firstOrDefault: true,
			});
		},

		modelFieldKey() {
			return (
				this.modelListMethod.itemFields.find((x) => x.isModelKey)?.key ?? "id"
			);
		},

		modelFieldText() {
			return this.modelListMethod.itemFields.find((x) => x.isModelText)?.key;
		},
	},

	watch: {
		async search() {
			if (!this.search) return;
			await this.loadItem();
		},

		async value() {
			this.input = this.value;
			await this.loadItem({ input: this.input });
		},

		input() {
			this.$emit("input", this.input);
		},
	},

	methods: {
		async loadItem({ input } = {}) {
			let params = { search: this.search };

			if (input && input.value) {
				params["id.operator"] = input.operator;
				if (!Array.isArray(input.value)) params["id.value"] = input.value;
				else {
					for (let i = 0; i < input.value.length; i++)
						params[`id.value[${i}]`] = input.value[i];
				}
			}

			let { data } = await this.requestApi({
				controllerMethod: this.modelListMethod,
				params,
			});

			this.items = data;
		},
	},

	async mounted() {
		await this.loadItem({ input: this.value });
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">-</div>
		<div v-else>
			<router-link :to="{ path: detailUrl({ id: data[field.key] }) }">
				{{ data[field.key] }}
			</router-link>
		</div>
	</div>

	<div v-else class="input-field">
		<v-autocomplete
			v-model="input.value"
			:items="items"
			:loading="loading"
			:search-input.sync="search"
			:rules="fieldRules"
			:readonly="fieldReadonly"
			:label="fieldLabel"
			:counter="field.max"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			:multiple="isListOperator"
			:item-value="modelFieldKey"
			:item-text="modelFieldText"
			persistent-hint
			hide-no-data
			hide-selected
			no-filter
			clearable
		>
			<template #prepend>
				<FieldOperator
					v-if="input.operator"
					v-model="input.operator"
					:field="field"
				/>
			</template>

			<template #append>
				<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
			</template>

			<template #item="{ item }">
				{{ item[modelFieldText] | strFormat("capitalize") }}
			</template>

			<template #selection="{ item }">
				{{ item[modelFieldText] | strFormat("capitalize") }}
			</template>
		</v-autocomplete>
	</div>
</template>
