<script>
import PageMixin from "@/mixins/page";

export default {
	name: "account-detail",
	mixins: [PageMixin],

	components: {
		BaseDetailPage: () => import("@/components/Core/Page/Detail"),
	},

	data: () => ({ data: null }),

	methods: {
		async sendVerifyMail() {
			await this.doSubmit({
				form: null,
				successMessage: `Đã gửi mail thành công`,
				handler: async () => {
					await this.requestApi({
						method: "post",
						url: `/admin/account/${this.data.id}/Verify/Email`,
					});
				},
			});
		},
	},
};
</script>

<template>
	<BaseDetailPage :data.sync="data">
		<template #external-button>
			<v-btn
				class="mr-2"
				color="info"
				:loading="loading"
				small
				@click="sendVerifyMail"
			>
				Gửi E-mail xác thực
				<v-icon dark right> mdi-email </v-icon>
			</v-btn>
		</template>
	</BaseDetailPage>
</template>
