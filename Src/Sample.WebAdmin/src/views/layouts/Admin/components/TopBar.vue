<script>
import BaseMixin from "@/mixins/base";

export default {
	name: "AdminLayout-TopBar",
	mixins: [BaseMixin],

	props: {
		toggleLeftBar: { type: Boolean, default: null },
	},

	computed: {
		_toggleLeftBar: {
			get() {
				return this.toggleLeftBar;
			},
			set(value) {
				this.$emit("update:toggle-left-bar", value);
			},
		},
	},
};
</script>

<template>
	<v-app-bar elevation="0" app>
		<v-toolbar-title> Application </v-toolbar-title>

		<v-spacer />

		<v-btn class="d-md-none" @click="_toggleLeftBar = !_toggleLeftBar" icon>
			<v-icon v-if="!_toggleLeftBar" dark> mdi-menu </v-icon>
			<v-icon v-else dark> mdi-close </v-icon>
		</v-btn>
	</v-app-bar>
</template>
