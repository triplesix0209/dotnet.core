<script>
import PageMixin from "@/mixins/page";

export default {
	name: "profile",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/Core/FieldPanel"),
	},

	data: () => ({
		mode: "read",
		data: null,
		inputs: null,

		fields: [
			{
				key: "firstName",
				name: "tên gọi",
				type: "string",
				max: 100,
			},
			{
				key: "lastName",
				name: "tên họ",
				type: "string",
				max: 100,
			},
			{
				key: "avatarLink",
				name: "ảnh đại diện",
				type: "media",
			},
		],
	}),

	methods: {
		async _loaded() {
			await this.loadData();
		},

		async loadData() {
			let { data } = await this.requestApi({
				method: "get",
				url: "/identity",
				toggleLoading: true,
			});

			this.data = data;
		},

		switchMode(mode) {
			this.mode = mode;
		},

		async submit() {
			let data = await this.prepareInput({
				fields: this.fields,
				inputs: this.inputs,
			});

			await this.doSubmit({
				successMessage: `đã cập nhật thông tin cá nhân`,
				handler: async () => {
					await this.requestApi({
						method: "put",
						url: "/identity",
						data,
					});

					await this.loadData();
					this.switchMode("read");
				},
			});
		},
	},
};
</script>

<template>
	<v-container v-if="initialing || !data">
		<v-row>
			<v-col>
				<v-skeleton-loader type="article,actions" />
			</v-col>
		</v-row>
	</v-container>

	<div v-else>
		<v-fade-transition hide-on-leave>
			<v-container v-if="mode === 'read'">
				<v-row class="pt-5" align="end">
					<v-col cols="12" sm="8">
						ID: <b>{{ data.accountId }}</b>
					</v-col>

					<v-col class="d-flex pt-0 pl-0 justify-end" cols="12" sm="4">
						<v-btn
							v-permission="'profile.update'"
							class="ml-2"
							color="info"
							:disabled="loading"
							small
							:to="{ name: 'change-password' }"
						>
							Đổi mật khẩu
							<v-icon dark right> mdi-key </v-icon>
						</v-btn>

						<v-btn
							v-permission="'profile.update'"
							class="ml-2"
							color="success"
							:disabled="loading"
							small
							@click="switchMode('update')"
						>
							Sửa
							<v-icon dark right> mdi-pencil </v-icon>
						</v-btn>
					</v-col>
				</v-row>

				<v-row>
					<v-col>
						<FieldPanel :data="data" :fields="fields" mode="detail" />
					</v-col>
				</v-row>

				<v-row align="end">
					<v-col class="d-flex justify-end" cols="12">
						<v-btn
							v-permission="'profile.update'"
							class="ml-2"
							color="info"
							:disabled="loading"
							small
							:to="{ name: 'change-password' }"
						>
							Đổi mật khẩu
							<v-icon dark right> mdi-key </v-icon>
						</v-btn>

						<v-btn
							v-permission="'profile.update'"
							class="ml-2"
							color="success"
							:disabled="loading"
							small
							@click="switchMode('update')"
						>
							Sửa
							<v-icon dark right> mdi-pencil </v-icon>
						</v-btn>
					</v-col>
				</v-row>
			</v-container>
		</v-fade-transition>

		<v-fade-transition hide-on-leave>
			<v-form
				ref="form"
				v-if="mode === 'update'"
				:disabled="loading"
				@submit.stop.prevent="submit"
			>
				<v-container>
					<v-row class="pt-5" align="end">
						<v-col cols="12" sm="8">
							ID: <b>{{ data.accountId }}</b>
						</v-col>

						<v-col class="d-flex pt-0 pl-0 justify-end" cols="12" sm="4">
							<v-btn
								v-permission="'profile.update'"
								v-if="mode === 'update'"
								class="ml-2"
								:disabled="loading"
								small
								@click="switchMode('read')"
							>
								Hủy bỏ
							</v-btn>

							<v-btn
								v-permission="'profile.update'"
								v-if="mode === 'update'"
								type="submit"
								class="ml-2"
								color="success"
								:disabled="loading"
								small
							>
								Ghi nhận
								<v-icon dark right> mdi-pencil </v-icon>
							</v-btn>
						</v-col>
					</v-row>

					<v-row>
						<v-col>
							<FieldPanel
								v-model="inputs"
								:data="data"
								:fields="fields"
								mode="input"
							/>
						</v-col>
					</v-row>

					<v-row align="end">
						<v-col class="d-flex justify-end" cols="12">
							<v-btn
								v-permission="'profile.update'"
								v-if="mode === 'update'"
								class="ml-2"
								:disabled="loading"
								small
								@click="switchMode('read')"
							>
								Hủy bỏ
							</v-btn>

							<v-btn
								v-permission="'profile.update'"
								v-if="mode === 'update'"
								type="submit"
								class="ml-2"
								color="success"
								:disabled="loading"
								small
							>
								Ghi nhận
								<v-icon dark right> mdi-pencil </v-icon>
							</v-btn>
						</v-col>
					</v-row>
				</v-container>
			</v-form>
		</v-fade-transition>
	</div>
</template>
