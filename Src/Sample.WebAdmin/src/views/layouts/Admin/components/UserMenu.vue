<script>
import BaseMixin from "@/mixins/base";
import { mapActions } from "vuex";

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
	<v-menu v-if="currentUser" transition="slide-y-transition" bottom offset-y>
		<template #activator="{ on, attrs }">
			<slot v-bind:on="on" v-bind:attrs="attrs"></slot>
		</template>

		<v-card max-width="300">
			<v-img
				:src="currentUser.avatarLink"
				gradient="to top right, rgba(0,0,0,.3), rgba(0,0,0,.7)"
				width="300"
				height="300"
				dark
			>
				<router-link :to="{ name: 'profile' }">
					<v-card-title class="fill-height pa-2 white--text text-h4 align-end">
						{{ currentUser.name }}
					</v-card-title>
				</router-link>
			</v-img>

			<v-list>
				<v-list-item :to="{ name: 'profile' }">
					<v-list-item-icon> <v-icon>mdi-account</v-icon> </v-list-item-icon>
					<v-list-item-title> Thông tin cá nhân </v-list-item-title>
				</v-list-item>

				<v-list-item @click="logout">
					<v-list-item-icon> <v-icon>mdi-logout</v-icon> </v-list-item-icon>
					<v-list-item-title> Đăng xuất </v-list-item-title>
				</v-list-item>
			</v-list>
		</v-card>
	</v-menu>
</template>
