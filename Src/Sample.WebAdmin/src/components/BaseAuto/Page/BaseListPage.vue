<script>
import PageMixin from "@/mixins/page";

export default {
	name: "base-list-page",
	mixins: [PageMixin],

	components: {},

	props: {},

	data: () => ({}),

	computed: {},

	watch: {},

	methods: {
		loaded() {
			// console.log("ok");
		},
	},
};
</script>

<template>
	<v-container>
		<v-form :disabled="loading">
			<v-card>
				<v-container>
					<v-row>
						<v-col> a </v-col>
						<v-col> b </v-col>
						<v-col> c </v-col>
						<v-col> d </v-col>
						<v-col> e </v-col>
						<v-col> f </v-col>
					</v-row>
				</v-container>

				<v-card-actions>
					<v-btn block type="submit" color="primary" :loading="loading">
						Lấy dữ liệu
					</v-btn>
				</v-card-actions>
			</v-card>
		</v-form>
	</v-container>
</template>

<style lang="scss" scoped>
</style>
