<script>
import PageMixin from "@/mixins/page";
import { CONST } from "@/stores/layout";

export default {
	name: "base-list-page",
	mixins: [PageMixin],

	components: {
		FieldPanel: () => import("@/components/BaseAuto/Field/FieldPanel"),
		DataTable: () => import("@/components/BaseAuto/DataTable"),
	},

	data: () => ({
		filter: { old: null, current: null, toggle: false },
		total: null,
		items: [],
		pagination: {},
	}),

	computed: {
		listMethod() {
			return this.getMethod({
				controller: this.controller.code,
				type: CONST.METHOD_TYPE_LIST,
				firstOrDefault: true,
			});
		},

		filterCount() {
			let count = 0;
			for (let key in this.filter.old) {
				let filter = this.filter.old[key];
				if (!filter) continue;
				if (
					!this._isNoNeedValueOperator(filter.operator) &&
					(filter.value === undefined || filter.value === null)
				)
					continue;

				count++;
			}

			return count;
		},
	},

	watch: {
		async "pagination"() {
			if (this.pagination.sortBy.length > 0) await this.loadData();
		},

		async "pagination.page"(val, old) {
			if (old === undefined) return;
			await this.loadData();
		},
	},

	methods: {
		_validatePage() {
			if (!this.controller) throw Error("controller is invalid");
			if (!this.listMethod) throw Error("method is invalid");
			if (
				!this.checkPermission(
					this.listMethod.permissionCodes,
					this.listMethod.permissionOperator,
				)
			)
				throw Error("access is denied");
		},

		_breadcrumbs() {
			return [{ text: `Danh sách` }];
		},

		async _loaded() {
			this.loadFilterFromQuery();
			await this.loadData();
		},

		_isNoNeedValueOperator(operator) {
			return [
				CONST.FIELD_OPERATOR_NULL,
				CONST.FIELD_OPERATOR_NOT_NULL,
			].includes(operator);
		},

		async loadData({ force, setPage } = {}) {
			if (!force && this.loading) return;
			if (setPage) this.pagination.page = setPage;

			let params = {
				page: this.pagination.page,
				size: this.pagination.itemsPerPage,
			};

			for (let key in this.filter.current) {
				let filter = this.filter.current[key];
				if (!filter) continue;

				if (this._isNoNeedValueOperator(filter.operator)) {
					params[`${key}.operator`] = filter.operator;
					continue;
				}

				if (filter.value === undefined || filter.value === null) continue;
				params[`${key}.operator`] = filter.operator;
				if (!Array.isArray(filter.value)) params[`${key}.value`] = filter.value;
				else {
					for (let i = 0; i < filter.value.length; i++)
						params[`${key}.value[${i}]`] = filter.value[i];
				}
			}

			if (this.pagination.sortBy) {
				for (let i = 0; i < this.pagination.sortBy.length; i++) {
					let field = this.listMethod.itemFields[this.pagination.sortBy[i]];
					params[`sortColumn[${i}].name`] = field.sortColumn;
					params[`sortColumn[${i}].order`] = this.pagination.sortDesc[i]
						? "desc"
						: "asc";
				}
			}

			let { meta, data } = await this.requestApi({
				controllerMethod: this.listMethod,
				params,
			});

			this.saveFilterToQuery();
			this.filter.old = this.filter.current;

			this.total = meta.total;
			this.items = data;
		},

		async clearFilter() {
			this.filter.current = {};
			await this.loadData({ setPage: 1 });
		},

		loadFilterFromQuery() {
			let query = this.$route.query;

			if (query.page) this.pagination.page = Number(query.page);

			this.filter.current = {};
			for (let queryKey in query) {
				let key = queryKey.split(".")[0];
				let type = queryKey.split(".")[1];
				let data = query[queryKey];

				if (
					this.filter.current[key] === undefined ||
					this.filter.current[key] === null
				)
					this.filter.current[key] = {};

				if (/\[(\d)\]/.test(type)) {
					let index = /\[(\d)\]/.exec(type)[1];
					type = type.split("[")[0];

					if (
						this.filter.current[key][type] === undefined ||
						this.filter.current[key][type] === null
					)
						this.filter.current[key][type] = [];

					this.filter.current[key][type].push({ index, data });
				} else this.filter.current[key][type] = data;
			}

			for (let key in this.filter.current) {
				let value = this.filter.current[key].value;
				if (!Array.isArray(value)) continue;

				this.filter.current[key].value = value
					.sort((a, b) => a.index - b.index)
					.map((x) => x.data);
			}
		},

		saveFilterToQuery() {
			let query = {};

			if (this.pagination.page && this.pagination.page > 1)
				query.page = this.pagination.page;

			if (this.filter.current) {
				for (let key in this.filter.current) {
					let filter = this.filter.current[key];
					if (!filter) continue;

					if (this._isNoNeedValueOperator(filter.operator)) {
						query[`${key}.operator`] = filter.operator;
						continue;
					}

					if (filter.value === undefined || filter.value === null) continue;
					query[`${key}.operator`] = filter.operator;
					if (!Array.isArray(filter.value))
						query[`${key}.value`] = filter.value;
					else {
						for (let i = 0; i < filter.value.length; i++)
							query[`${key}.value[${i}]`] = filter.value[i];
					}
				}
			}

			this.$router.replace({ query }).catch(() => {});
		},
	},
};
</script>

<template>
	<v-container v-if="!initialing">
		<v-scroll-x-transition leave-absolute>
			<v-form
				v-if="filter.toggle"
				:disabled="loading"
				@submit.stop.prevent="loadData({ setPage: 1 })"
			>
				<v-row>
					<v-col>
						<v-card>
							<FieldPanel
								v-model="filter.current"
								:fields="this.listMethod.filterFields"
								field-lg="3"
								mode="input"
							/>

							<v-card-actions>
								<v-row>
									<v-col>
										<v-tooltip
											open-delay="100"
											color="rgba(0, 0, 0, 1)"
											nudge-right="35"
											bottom
										>
											<template v-slot:activator="{ on, attrs }">
												<v-btn
													v-bind="attrs"
													v-on="on"
													class="mr-2"
													:disabled="loading"
													icon
													@click="filter.toggle = false"
												>
													<v-icon>mdi-chevron-double-left</v-icon>
												</v-btn>
											</template>
											<span>Ẩn bảng lọc</span>
										</v-tooltip>

										<v-tooltip
											open-delay="100"
											color="rgba(0, 0, 0, 1)"
											nudge-right="35"
											bottom
										>
											<template v-slot:activator="{ on, attrs }">
												<v-btn
													v-bind="attrs"
													v-on="on"
													class="mr-2"
													:disabled="loading || filterCount === 0"
													icon
													@click="clearFilter"
												>
													<v-icon color="red">mdi-filter-off</v-icon>
												</v-btn>
											</template>
											<span>Loại bỏ các tiêu chí lọc</span>
										</v-tooltip>

										<v-btn
											type="submit"
											color="primary"
											:style="{ width: 'calc(100% - 88px)' }"
											:loading="loading"
										>
											Lọc dữ liệu
										</v-btn>
									</v-col>
								</v-row>
							</v-card-actions>
						</v-card>
					</v-col>
				</v-row>
			</v-form>
		</v-scroll-x-transition>

		<v-row>
			<v-col>
				<v-tooltip
					v-if="!filter.toggle"
					open-delay="100"
					color="rgba(0, 0, 0, 1)"
					nudge-right="35"
					bottom
				>
					<template v-slot:activator="{ on, attrs }">
						<v-btn
							v-bind="attrs"
							v-on="on"
							class="mr-2"
							color="primary"
							dark
							@click="
								() => {
									filter.current = filter.old;
									filter.toggle = true;
								}
							"
						>
							<v-icon>mdi-filter</v-icon>
							<v-badge v-if="filterCount" color="red" :content="filterCount" />
						</v-btn>
					</template>
					<span>Mở bảng dữ liệu</span>
				</v-tooltip>

				<span v-if="total">
					Tổng có <strong>{{ total | numeral("0,0") }}</strong> mục
				</span>
			</v-col>
		</v-row>

		<v-row v-if="total === 0">
			<v-col>
				<v-alert v-if="filterCount > 0" text type="error">
					Không có dữ liệu nào phù hợp với tiêu chí lọc của bạn
				</v-alert>

				<v-alert v-else text type="info">
					Hiện không có dữ liệu để hiển thị
				</v-alert>
			</v-col>
		</v-row>

		<v-row v-if="total > 0">
			<v-col>
				<DataTable
					:fields="listMethod.itemFields"
					:loading="loading"
					:items="items"
					:total="total"
					:pagination.sync="pagination"
					:controller="controller.code"
				/>
			</v-col>
		</v-row>
	</v-container>
</template>
