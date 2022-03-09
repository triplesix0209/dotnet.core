<script>
import BaseMixin from "@/mixins/base";
import { CONST } from "@/stores/layout";

export default {
	name: "field-link",
	mixins: [BaseMixin],

	props: {
		field: { type: Object, required: true },
		data: { type: Object },
	},

	computed: {
		detailUrl() {
			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller: this.field.modelController.code,
				id: this.data[this.field.key],
			});
		},
	},
};
</script>

<template>
	<router-link v-if="field.type === 'id' && detailUrl" :to="detailUrl">
		<v-icon color="primary">mdi-open-in-new</v-icon>
	</router-link>
</template>
