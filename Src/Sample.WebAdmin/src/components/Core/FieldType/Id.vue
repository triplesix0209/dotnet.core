<script>
import FieldMixin from "@/mixins/field";
import { CONST } from "@/stores/layout";

export default {
	name: "field-type-id",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("./FieldComponent/FieldOperator"),
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
			if (!this.field.modelController) return null;
			return this.getMethod({
				api: this.field.modelController.api,
				controller: this.field.modelController.code,
				type: CONST.METHOD_TYPE_LIST,
				firstOrDefault: true,
			});
		},

		modelFieldKey() {
			return (
				this.modelListMethod?.itemFields.find((x) => x.isModelKey)?.key ?? "id"
			);
		},

		modelFieldText() {
			return this.modelListMethod?.itemFields.find((x) => x.isModelText)?.key;
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

			if (this.field.operator) {
				if (input && input.value) {
					params["id.operator"] = input.operator;
					if (!Array.isArray(input.value)) params["id.value"] = input.value;
					else {
						for (let i = 0; i < input.value.length; i++)
							params[`id.value[${i}]`] = input.value[i];
					}
				}
			} else if (this.field.type === "parentId" && this.objectId) {
				params["id.operator"] = CONST.FIELD_OPERATOR_NOT_EQUAL;
				params["id.value"] = this.objectId;
			}

			if (this.modelListMethod) {
				let { data } = await this.requestApi({
					controllerMethod: this.modelListMethod,
					params,
				});

				this.items = data;
			}
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
			<router-link
				v-if="field.key !== 'id'"
				:to="{ path: detailUrl({ id: data[field.key] }) }"
			>
				{{ data[field.key] }}
			</router-link>
			<span v-else> {{ data[field.key] }}</span>
		</div>
	</div>

	<div v-else-if="mode === 'detail'">
		<v-text-field
			v-if="data"
			v-model="fieldDisplayValue"
			:label="fieldLabel"
			:placeholder="fieldEmptyValue"
			:hint="fieldHint"
			persistent-placeholder
			persistent-hint
			readonly
		>
			<template #append>
				<FieldLink :field="field" :data="data" />
			</template>
		</v-text-field>
	</div>

	<div v-else>
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
			@change="raiseChangeInput({ value: input.value })"
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
