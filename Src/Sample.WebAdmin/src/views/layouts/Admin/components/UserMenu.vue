<script>
import { mapActions } from "vuex";
import BaseMixin from "@/mixins/base";

export default {
	name: "AdminLayout-UserMenu",
	mixins: [BaseMixin],

	methods: {
		...mapActions(["auth/logout"]),

		async logout() {
			if (!(await this.confirm({ message: "bạn muốn đăng xuất?" }))) return;

			await this["auth/logout"]();
			this.$router.go();
		},
	},
};
</script>

<template>
	<v-menu
		v-if="currentUser"
		transition="slide-y-transition"
		bottom
		offset-y
		:close-on-content-click="false"
	>
		<template v-slot:activator="{ on, attrs }">
			<slot v-bind:on="on" v-bind:attrs="attrs"></slot>
		</template>

		<v-card max-width="200">
			<v-img
				:src="currentUser.avatarLink"
				gradient="to top right, rgba(0,0,0,.3), rgba(0,0,0,.7)"
				width="200"
				height="200"
				dark
			>
				<v-card-title class="fill-height pa-2 white--text text-h4 align-end">
					{{ currentUser.name }}
				</v-card-title>
			</v-img>

			<v-list>
				<v-list-item-group>
					<v-list-item @click="logout">
						<v-list-item-icon> <v-icon>mdi-logout</v-icon> </v-list-item-icon>
						<v-list-item-title> Đăng xuất </v-list-item-title>
					</v-list-item>
				</v-list-item-group>
			</v-list>
		</v-card>
	</v-menu>
</template>
