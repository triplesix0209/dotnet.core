<script>
import BaseMixin from "@/mixins/base";

export default {
	name: "field-panel",
	mixins: [BaseMixin],

	components: {
		FieldItem: () => import("@/components/Core/FieldItem"),
	},

	props: {
		value: { type: Object },
		data: { type: Object },
		objectId: { type: String },
		fields: { type: Array, required: true },
		groups: { type: Array, default: () => [] },
		fieldSm: [Number, String],
		fieldMd: [Number, String],
		fieldLg: [Number, String],
		hideGroup: { type: Boolean },
		mode: {
			type: String,
			default: "input",
			validator(value) {
				return ["detail", "list", "input"].includes(value);
			},
		},
	},

	data() {
		return { inputs: this._inputFromValue() };
	},

	computed: {
		renderGroups() {
			let result = this.groups;
			if (!result || result.length === 0) return [];

			if (!result.some((x) => x.code === null))
				result.unshift({ code: null, name: "thông tin", icon: null });

			return result.filter(
				(x) => this._renderFields({ group: x.code }).length > 0,
			);
		},
	},

	watch: {
		value() {
			let inputs = this._inputFromValue();
			if (JSON.stringify(inputs) !== JSON.stringify(this.inputs))
				this.inputs = inputs;
		},

		inputs: {
			deep: true,
			handler() {
				this.$emit("input", this.inputs);
			},
		},
	},

	methods: {
		_inputFromValue() {
			let result = {};

			for (let field of this.fields) {
				result[field.key] = { value: null, operator: null };

				if (!!this.data && this.data[field.key] !== undefined) {
					let value = this.data[field.key];

					if (field.type === "enum") value = value?.toString();

					result[field.key].value = value;
					result[field.key].prevValue = value;
				}

				if (!!this.value && !!this.value[field.key])
					result[field.key] = this.value[field.key];
			}

			return result;
		},

		_renderFields({ group } = {}) {
			let result = this.fields.filter(
				(x) => x.render === undefined || x.render === true,
			);
			if (group !== undefined) result = result.filter((x) => x.group === group);
			return result;
		},

		inputChange(payload) {
			this.$emit("change", payload);
		},
	},
};
</script>

<template>
	<v-container v-if="hideGroup" fluid>
		<v-row class="pb-3">
			<v-col
				v-for="field in _renderFields()"
				class="pb-0"
				:key="field.key"
				:sm="fieldSm ? fieldSm : field.gridCol"
				:md="fieldMd ? fieldMd : field.gridCol"
				:lg="fieldLg ? fieldLg : field.gridCol"
				cols="12"
			>
				<slot
					:name="`field-${field.key}`"
					:input="inputs[field.key]"
					:data="data"
					:field="field"
					:field-list="fields"
					:mode="mode"
				>
					<FieldItem
						v-model="inputs[field.key]"
						:data="data"
						:field="field"
						:field-list="fields"
						:object-id="objectId"
						:mode="mode"
						@change="inputChange"
					/>
				</slot>
			</v-col>
		</v-row>
	</v-container>

	<div v-else-if="renderGroups.length === 0">
		<v-card>
			<v-container>
				<v-row class="pb-3">
					<v-col
						v-for="field in _renderFields()"
						class="pb-0"
						:key="field.key"
						:sm="fieldSm ? fieldSm : field.gridCol"
						:md="fieldMd ? fieldMd : field.gridCol"
						:lg="fieldLg ? fieldLg : field.gridCol"
						cols="12"
					>
						<slot
							:name="`field-${field.key}`"
							:input="inputs[field.key]"
							:data="data"
							:field="field"
							:field-list="fields"
							:mode="mode"
						>
							<FieldItem
								:data="data"
								:field="field"
								:field-list="fields"
								:object-id="objectId"
								:mode="mode"
								v-model="inputs[field.key]"
								@change="inputChange"
							/>
						</slot>
					</v-col>
				</v-row>
			</v-container>
		</v-card>
	</div>

	<div v-else class="group-wrapper">
		<div v-for="group in renderGroups" :key="group.code" class="group-item">
			<div class="group-item-name elevation-2">
				{{ group.name | strFormat("capitalize") }}
			</div>

			<v-card class="group-item-content">
				<v-container>
					<v-row class="pb-3">
						<v-col
							v-for="field in _renderFields({ group: group.code })"
							class="pb-0"
							:key="field.key"
							:sm="fieldSm ? fieldSm : field.gridCol"
							:md="fieldMd ? fieldMd : field.gridCol"
							:lg="fieldLg ? fieldLg : field.gridCol"
							cols="12"
						>
							<slot
								:name="`field-${field.key}`"
								:input="inputs[field.key]"
								:data="data"
								:field="field"
								:field-list="fields"
								:mode="mode"
							>
								<FieldItem
									v-model="inputs[field.key]"
									:data="data"
									:field="field"
									:field-list="fields"
									:object-id="objectId"
									:mode="mode"
									@change="inputChange"
								/>
							</slot>
						</v-col>
					</v-row>
				</v-container>
			</v-card>
		</div>
	</div>
</template>

<style lang="scss" scoped>
.group-wrapper ::v-deep {
	margin: -12px 0;

	.group-item {
		padding: 12px 0;

		.group-item-name {
			display: inline-block;
			padding: 8px 12px;
			background-color: white;
			border-radius: 4px 4px 0 0;
			font-weight: bold;
		}

		.group-item-content {
			border-top-left-radius: 0;
		}
	}
}
</style>
