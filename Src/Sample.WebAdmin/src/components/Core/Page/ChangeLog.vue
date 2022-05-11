<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-changelog-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/Core/FieldPanel"),
	},

	props: {
		api: { type: String },
	},

	data: () => ({
		page: 1,
		total: null,
		pageCount: null,
		timeline: null,
		current: null,
		mode: null,
	}),

	computed: {
		id() {
			return this.$route.params.id;
		},

		changelogMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_LIST_CHANGELOG,
				firstOrDefault: true,
			});
		},

		detailMethod() {
			return this.getMethod({
				api: this.api,
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_DETAIL,
				firstOrDefault: true,
			});
		},

		detailUrl() {
			return CONST.generateMethodUrl(CONST.METHOD_TYPE_DETAIL, {
				controller: this.controller.code,
				id: this.id,
			});
		},

		canLoadMore() {
			return this.page < this.pageCount;
		},

		beforeData() {
			let data = JSON.parse(this.current.beforeData);
			if (data === undefined || data === null) return null;
			data.id = this.id;
			return data;
		},

		afterData() {
			let data = JSON.parse(this.current.afterData);
			data.id = this.id;
			return data;
		},

		displayFields() {
			let mode = this.mode === 0 ? "beforeData" : "afterData";
			if (mode === "beforeData" && !this.beforeData) return [];

			return this.detailMethod.detailFields.filter(
				(field) => this[mode][field.key] !== undefined,
			);
		},

		hiddenFields() {
			let mode = this.mode === 0 ? "beforeData" : "afterData";
			if (mode === "beforeData" && !this.beforeData) return [];

			return this.detailMethod.detailFields.filter(
				(field) => this[mode][field.key] === undefined,
			);
		},
	},

	methods: {
		_validatePage() {
			if (!this.controller) throw Error("controller is invalid");
			if (!this.changelogMethod) throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.changelogMethod.permissionCodes,
					this.changelogMethod.permissionOperator,
				)
			)
				throw Error("access is denied");
		},

		_breadcrumbs() {
			return [
				{
					text: `Danh sách`,
					href: CONST.generateMethodUrl(CONST.METHOD_TYPE_LIST, {
						controller: this.controller.code,
					}),
				},
				{
					text: `Chi tiết`,
					href: this.detailUrl,
				},
				{
					text: `Thay đổi`,
				},
			];
		},

		async _loaded() {
			await this.loadTimeline();
		},

		async loadTimeline({ force, setPage } = {}) {
			if (!force && this.loading) return;
			if (setPage) this.page = setPage;

			let { meta, data } = await this.requestApi({
				controllerMethod: this.changelogMethod,
				path: { id: this.id },
				params: { page: this.page, size: 10 },
				toggleLoading: true,
			});

			this.total = meta.total;
			this.pageCount = meta.pageCount;
			if (!this.timeline) this.timeline = data;
			else this.timeline = this.timeline.concat(data);
			this.$emit("update:timeline", this.timeline);
		},

		selectItem(item) {
			this.current = item;
			this.$emit("update:data", item);
		},

		mediaLink(link) {
			return link ?? "/assets/default-avatar.png";
		},
	},
};
</script>

<template>
	<v-container v-if="initialing || !timeline">
		<v-row>
			<v-col>
				<v-skeleton-loader type="article,actions" />
			</v-col>
		</v-row>
	</v-container>

	<v-container v-else>
		<v-row v-if="timeline.length === 0">
			<v-col>
				<v-alert class="ma-0" type="info" text>
					Hiện đối tượng không có bất kỳ sự thay đổi nào để hiển thị
				</v-alert>
			</v-col>
		</v-row>

		<v-row v-else>
			<v-col>
				<v-card>
					<v-card-text class="changelog-body">
						<div class="left-side primary darken white--text">
							<v-timeline align-top dense dark>
								<v-timeline-item
									v-for="item in timeline"
									:key="item.id"
									:color="
										current && current.id === item.id ? 'secondary' : 'primary'
									"
									fill-dot
								>
									<template #icon>
										<v-btn icon large @click="selectItem(item)">
											<v-avatar>
												<v-img
													max-width="32"
													max-height="32"
													:src="
														mediaLink(item.actor ? item.actor.avatarLink : null)
													"
												/>
											</v-avatar>
										</v-btn>
									</template>

									<v-btn
										class="timeline-item"
										@click="selectItem(item)"
										:plain="!current || current.id !== item.id"
										tile
									>
										<div class="timeline-item-container">
											<div class="timeline-item-name font-weight-normal">
												<strong>
													{{ item.actor ? item.actor.name : null }}
												</strong>
											</div>
											<v-tooltip color="rgba(0,0,0,1)" bottom>
												<template #activator="{ on, attrs }">
													<div
														v-bind="attrs"
														v-on="on"
														class="timeline-item-datetime"
													>
														{{ item.datetime | moment("from") }}
													</div>
												</template>
												<span>
													{{ item.datetime | moment("DD/MM/YYYY HH:mm") }}
												</span>
											</v-tooltip>
										</div>
									</v-btn>
								</v-timeline-item>
							</v-timeline>

							<v-btn
								v-if="canLoadMore"
								class="timeline-load-more"
								:loading="loading"
								@click="loadTimeline({ setPage: page + 1 })"
							>
								Tải thêm
							</v-btn>
						</div>

						<div v-if="current" class="right-side pa-6">
							<v-container>
								<v-row>
									<v-col class="px-0 pt-0" cols="12" sm="6">
										<v-btn-toggle
											v-model="mode"
											color="primary accent-3"
											mandatory
											rounded
										>
											<v-btn small> Trước thay đổi </v-btn>
											<v-btn small> Sau thay đổi </v-btn>
										</v-btn-toggle>
									</v-col>

									<v-col cols="12" sm="6" align="end">
										<v-tooltip
											v-if="hiddenFields.length > 0"
											color="rgba(0,0,0,1)"
											bottom
										>
											<template #activator="{ on, attrs }">
												<span v-bind="attrs" v-on="on" class="red--text">
													<strong>
														{{ hiddenFields.length | numeral("0,0") }}
													</strong>
													trường dữ liệu bị ẩn do không còn dùng
												</span>
											</template>
											<div>
												Các trường bị ẩn: <br />
												<span
													v-for="(field, index) in hiddenFields"
													:key="field"
												>
													{{ index + 1 }}) {{ field.key }} <br />
												</span>
											</div>
										</v-tooltip>
									</v-col>

									<v-col class="p-0" cols="12">
										<v-alert
											v-if="mode === 0 && !beforeData"
											class="m-0"
											type="info"
											dense
											text
										>
											Thời điểm này không có dữ liệu này.
										</v-alert>

										<FieldPanel
											v-if="mode === 0 && beforeData"
											:data="beforeData"
											:fields="displayFields"
											:object-id="id"
											mode="detail"
											hide-group
										>
											<template
												v-for="field in displayFields"
												v-slot:[`field-${field.key}`]
											>
												<slot
													:name="`field-${field.key}`"
													:field-list="displayFields"
													:field="field"
													:mode="'beforeData'"
												/>
											</template>
										</FieldPanel>

										<FieldPanel
											v-if="mode === 1"
											:data="afterData"
											:fields="displayFields"
											:object-id="id"
											mode="detail"
											hide-group
										>
											<template
												v-for="field in displayFields"
												v-slot:[`field-${field.key}`]
											>
												<slot
													:name="`field-${field.key}`"
													:field-list="displayFields"
													:field="field"
													:mode="'afterData'"
												/>
											</template>
										</FieldPanel>
									</v-col>
								</v-row>
							</v-container>
						</div>
					</v-card-text>
				</v-card>
			</v-col>
		</v-row>
	</v-container>
</template>

<style lang="scss" scoped>
.changelog-body ::v-deep {
	display: flex;
	flex-direction: row;
	flex-wrap: wrap;
	padding: 0;

	.left-side {
		width: 170px;

		.v-timeline {
			&:before {
				left: calc(48px - 18px) !important;
			}

			.v-timeline-item {
				margin-left: -40px;

				.v-timeline-item__divider {
					min-width: 50px;
				}
			}
		}

		.timeline-item {
			height: 45px;
			width: 120px;
			margin: 1px 0;
			padding: 10px !important;
			text-align: left;
			text-transform: none;
			font-size: unset;
			letter-spacing: unset;
			border-top-left-radius: 10px;
			border-bottom-left-radius: 10px;

			.timeline-item-container {
				display: grid;
				width: 100%;
				grid-template-areas:
					"name"
					"datetime";

				.timeline-item-avatar {
					grid-area: avatar;
				}

				.timeline-item-name {
					grid-area: name;
					display: inline-block;
					width: 100px;
					white-space: nowrap;
					overflow: hidden;
					text-overflow: ellipsis;
				}

				.timeline-item-datetime {
					grid-area: datetime;
				}
			}
		}

		.timeline-load-more {
			width: calc(100% - 30px);
			margin: 15px;
			margin-top: 0;
		}
	}

	.right-side {
		width: calc(100% - 170px);
	}

	@media only screen and (max-width: 425px) {
		.left-side,
		.right-side {
			width: 100%;
		}
	}
}
</style>
