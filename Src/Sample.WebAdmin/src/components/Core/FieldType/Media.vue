<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-media",
	mixins: [FieldMixin],

	components: {
		FieldLink: () => import("./FieldComponent/FieldLink"),
	},

	computed: {
		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			return rules;
		},
	},

	methods: {
		fileChanged(e) {
			this.input.file = e.target.files[0];
			this.input.value = `${
				this.input.file.name
			} (${this.$options.filters.numeral(this.input.file.size, "0 b")})`;
			this.raiseChangeInput({ value: this.input.file });
		},

		clearFile() {
			this.$refs.file.value = null;
			this.input.file = null;
			this.input.value = null;
		},

		inputMediaLink() {
			if (this.input.file) return URL.createObjectURL(this.input.file);
			if (this.input.value) return this.input.value;
			return "/assets/no-image.png";
		},

		undo() {
			this.input.value = this.input.prevValue;
			this.input.operator = this.input.prevOperator;
			this.input.file = null;
		},
	},
};
</script>

<template>
	<div v-if="mode === 'list'">
		<div v-if="isEmptyFieldData">
			<v-avatar rounded size="40">
				<v-img src="/assets/no-image.png" contain />
			</v-avatar>
		</div>
		<div v-else>
			<v-avatar
				:style="{ cursor: 'zoom-in' }"
				size="40"
				rounded
				@click="showLightbox(data[field.key])"
			>
				<v-img :src="data[field.key]" contain />
			</v-avatar>
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
			<template #prepend>
				<v-avatar
					v-if="data[field.key]"
					:style="{ cursor: 'zoom-in' }"
					size="30"
					rounded
					@click="showLightbox(data[field.key])"
				>
					<v-img :src="data[field.key]" contain />
				</v-avatar>

				<v-avatar v-else rounded size="30">
					<v-img src="/assets/no-image.png" contain />
				</v-avatar>
			</template>

			<template #append>
				<FieldLink :field="fieldBase" :data="data" />
			</template>
		</v-text-field>
	</div>

	<v-text-field
		v-else
		v-model="input.value"
		:rules="fieldRules"
		:readonly="fieldReadonly || !!input.file"
		:label="fieldLabel"
		:counter="field.max"
		:placeholder="fieldPlaceholder"
		:persistent-placeholder="fieldPlaceholder !== null"
		:hint="fieldHint"
		persistent-hint
		clearable
		@click:clear="clearFile"
		@change="raiseChangeInput({ value: input.value })"
	>
		<template #prepend>
			<v-avatar
				size="30"
				rounded
				:style="{ cursor: 'pointer' }"
				@click="$refs[`file`].click()"
			>
				<v-img :src="inputMediaLink()" />
			</v-avatar>

			<input
				ref="file"
				class="d-none"
				type="file"
				accept="image/*"
				@change="fileChanged"
			/>
		</template>

		<template #append>
			<v-icon v-if="canUndo" @click="undo">mdi-arrow-u-left-top</v-icon>
		</template>
	</v-text-field>
</template>
