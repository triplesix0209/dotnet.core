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
			<v-btn icon v-bind="attrs" v-on="on">
				<v-avatar> <v-img :src="currentUser.avatarLink" /> </v-avatar>
			</v-btn>
		</template>

		<v-list>
			<v-list-item>
				<v-list-item-title class="text-right">
					<strong>{{ currentUser.name | strTitleCase }}</strong>
				</v-list-item-title>
			</v-list-item>
		</v-list>

		<v-divider />

		<v-list>
			<v-list-item-group>
				<v-list-item @click="logout">
					<v-list-item-icon> <v-icon>mdi-logout</v-icon> </v-list-item-icon>
					<v-list-item-title> Đăng xuất </v-list-item-title>
				</v-list-item>
			</v-list-item-group>
		</v-list>
	</v-menu>
</template>
