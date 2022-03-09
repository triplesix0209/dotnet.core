<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-media",
	mixins: [FieldMixin],

	components: {
		FieldOperator: () => import("@/components/BaseAuto/Field/FieldOperator"),
	},

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			return rules;
		},
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">
			<v-avatar rounded size="40">
				<v-img src="/assets/no-image.png" />
			</v-avatar>
		</div>
		<div v-else>
			<v-avatar
				:style="{ cursor: 'zoom-in' }"
				size="40"
				rounded
				@click="showLightbox(data[field.key])"
			>
				<v-img :src="data[field.key]" />
			</v-avatar>
		</div>
	</div>

	<div v-else class="input-field">
		<v-text-field
			v-if="!isListOperator"
			v-model="input.value"
			:rules="fieldRules"
			:readonly="fieldReadonly"
			:label="fieldLabel"
			:counter="field.max"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			persistent-hint
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
		</v-text-field>

		<v-combobox
			v-else
			class="input-field"
			v-model="input.value"
			:rules="fieldRules"
			:readonly="fieldReadonly"
			:label="fieldLabel"
			:placeholder="fieldPlaceholder"
			:persistent-placeholder="fieldPlaceholder !== null"
			:hint="fieldHint"
			persistent-hint
			clearable
			multiple
			small-chips
			deletable-chips
		>
			<template #prepend>
				<FieldOperator v-model="input.operator" :field="field" />
			</template>

			<template #append>
				<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
			</template>
		</v-combobox>
	</div>
</template>
