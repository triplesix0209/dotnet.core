<script>
import FieldMixin from "@/mixins/field";

export default {
	name: "field-type-list",
	mixins: [FieldMixin],

	components: {
		DataTable: () => import("../DataTable"),
		FieldPanel: () => import("../FieldPanel"),
	},

	data: () => ({
		editorDialog: {
			open: false,
			loading: false,
			mode: null,
			data: null,
			inputs: null,
			index: null,
		},
	}),

	computed: {
		dialogTitle() {
			if (this.editorDialog.mode === "create")
				return `Thêm mục cho ${this.field.name}`;
			return this.field.name;
		},

		fieldRules() {
			let rules = [];

			if (this.field.isRequired)
				rules.push((v) => !!v || "Không được phép bỏ trống");

			if (this.field.min !== undefined && this.field.min !== null)
				rules.push(
					(v) =>
						!v ||
						v.length >= this.field.min ||
						`Phải >= ${this.$numeral(this.field.min)} ký tự`,
				);

			if (this.field.max !== undefined && this.field.max !== null)
				rules.push(
					(v) =>
						!v ||
						v.length <= this.field.max ||
						`Phải <= ${this.$numeral(this.field.max)} ký tự`,
				);

			return rules;
		},
	},

	methods: {
		onCreate() {
			this.editorDialog.inputs = {};
			this.editorDialog.mode = "create";
			this.editorDialog.open = true;
			this.$nextTick(() => this.$refs.form.reset());
		},

		onEdit(item, index) {
			this.editorDialog.index = index;
			this.editorDialog.data = item;
			this.editorDialog.inputs = {};
			this.editorDialog.mode = "edit";
			this.editorDialog.open = true;
		},

		onDelete(item, index) {
			this.input.value.splice(index, 1);
		},

		async submit() {
			await this.doSubmit({
				handler: async () => {
					if (this.input.value === undefined || this.input.value === null)
						this.input.value = [];

					if (this.editorDialog.mode === "create") {
						this.input.value.push(
							await this.prepareInput({
								inputs: this.editorDialog.inputs,
								fields: this.field.listItemFields,
							}),
						);
					} else if (this.editorDialog.mode === "edit") {
						let item = this.input.value[this.editorDialog.index];
						let newValue = await this.prepareInput({
							inputs: this.editorDialog.inputs,
							fields: this.field.listItemFields,
						});

						for (let field of this.field.listItemFields)
							item[field.key] = newValue[field.key];
					}

					this.editorDialog.open = false;
				},
			});
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
		<v-label>{{ fieldLabel }}</v-label>
		<DataTable
			:fields="field.listItemFields"
			:items="data[field.key] ? data[field.key] : []"
			:total="data[field.key] ? data[field.key].length : 0"
			hide-action
		/>
	</div>

	<div class="field-list-type" v-else>
		<v-toolbar flat dense>
			<v-label>{{ fieldLabel }}</v-label>
			<v-spacer />
			<v-btn color="primary" small @click.stop="onCreate">thêm</v-btn>
		</v-toolbar>

		<DataTable
			:fields="field.listItemFields"
			:items="input.value ? input.value : []"
			:total="input.value ? input.value.length : 0"
			hide-action
		>
			<template #external-action="{ loading, item, index }">
				<v-btn
					class="mr-2"
					color="red"
					:loading="loading"
					small
					dark
					@click="onDelete(item, index)"
				>
					<v-icon dark> mdi-delete </v-icon>
				</v-btn>

				<v-btn
					color="info"
					:loading="loading"
					small
					@click="onEdit(item, index)"
				>
					<v-icon dark> mdi-pencil </v-icon>
				</v-btn>
			</template>
		</DataTable>

		<v-dialog v-model="editorDialog.open" persistent>
			<v-form
				ref="form"
				:disabled="editorDialog.loading"
				@submit.stop.prevent="submit"
			>
				<v-card>
					<v-card-title> {{ dialogTitle }} </v-card-title>

					<v-card-text class="text--primary">
						<FieldPanel
							v-model="editorDialog.inputs"
							:data="editorDialog.data"
							:fields="field.listItemFields"
							hide-group
						/>
					</v-card-text>

					<v-card-actions>
						<v-spacer />
						<v-btn small @click="editorDialog.open = false">Hủy bỏ</v-btn>
						<v-btn small type="submit" color="primary">Ghi nhận</v-btn>
					</v-card-actions>
				</v-card>
			</v-form>
		</v-dialog>
	</div>
</template>

<style lang="scss" scoped>
.field-list-type ::v-deep {
	.v-toolbar__content {
		padding: 0;
	}
}
</style>
