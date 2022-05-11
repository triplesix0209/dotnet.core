<script>
import "@ckeditor/ckeditor5-build-classic/build/translations/vi";
import FieldMixin from "@/mixins/field";
import CKEditorClassic from "@ckeditor/ckeditor5-build-classic";
import Uploader from "@/plugins/ckeditor.uploader";

export default {
	name: "field-type-html",
	mixins: [FieldMixin],

	components: {
		FieldLink: () => import("./FieldComponent/FieldLink"),
		FieldOperator: () => import("./FieldComponent/FieldOperator"),
	},

	data: () => ({
		editor: CKEditorClassic,
		config: {
			language: "vi",
			extraPlugins: [
				(editor) => {
					editor.plugins.get("FileRepository").createUploadAdapter = (loader) =>
						new Uploader(loader);
				},
			],
		},
	}),

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
		<div v-if="isEmptyFieldData">-</div>
		<div v-else>{{ data[field.key] }}</div>
	</div>

	<div v-else-if="mode === 'detail'">
		<v-label>
			{{ fieldLabel }}

			<v-tooltip
				v-if="fieldHint"
				open-delay="100"
				color="rgba(0, 0, 0, 1)"
				bottom
			>
				<template #activator="{ on }">
					<v-icon v-on="on" right> mdi-help-circle-outline </v-icon>
				</template>
				{{ fieldHint }}
			</v-tooltip>

			<FieldLink :field="fieldBase" :data="data" />
		</v-label>

		<ckeditor
			v-model="fieldDisplayValue"
			:editor="editor"
			:config="config"
			disabled
		/>
	</div>

	<div v-else-if="!isListOperator" class="html-editor">
		<v-label>
			{{ fieldLabel }}

			<v-tooltip
				v-if="fieldHint"
				open-delay="100"
				color="rgba(0, 0, 0, 1)"
				bottom
			>
				<template #activator="{ on }">
					<v-icon v-on="on" right> mdi-help-circle-outline </v-icon>
				</template>
				{{ fieldHint }}
			</v-tooltip>
		</v-label>

		<ckeditor v-model="input.value" :editor="editor" :config="config" />
	</div>

	<v-combobox
		v-else
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
	</v-combobox>
</template>

<style lang="scss" scoped>
.html-editor {
	label {
		.v-icon {
			margin: 0;
			font-size: 17px;
			vertical-align: baseline;
		}
	}
}
</style>
