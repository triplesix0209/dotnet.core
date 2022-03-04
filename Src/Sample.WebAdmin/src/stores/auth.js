import * as Jose from "jose";
import Moment from "moment-timezone";
import ApiService from "@/services/api";
import TokenService from "@/services/token";

function generateUser({ profile, accessToken }) {
	let result = { ...profile };
	if (!profile.avatarLink || profile.avatarLink === "")
		result.avatarLink = "/assets/default-avatar.png";

	if (accessToken) {
		let tokenData = Jose.decodeJwt(accessToken);
		result.accessLevel = Number(tokenData.accessLevel);

		if (tokenData.accessLevel === "root") {
			result.permissions = null;
		} else if (
			!tokenData.permission ||
			(typeof tokenData.permission === "string" &&
				tokenData.permission.trim() === "")
		) {
			result.permissions = null;
		} else if (typeof tokenData.permission === "string") {
			result.permissions = tokenData.permission.trim().split(" ");
		}
	}

	return result;
}

export default {
	namespaced: true,

	state: {
		isAuthenticated: false,
		user: null,
	},

	mutations: {
		clearAuth(state) {
			state.isAuthenticated = false;
			state.user = null;
			TokenService.clearToken();
		},

		setAuth(state, data) {
			state.isAuthenticated = true;
			state.user = generateUser(data);
			TokenService.setToken(data.accessToken, data.refreshToken);
		},

		setUser(state, data) {
			state.isAuthenticated = true;
			state.user = generateUser(data);
		},

		updateUser(state, data) {
			state.user = { ...state.user, ...data };
		},
	},

	getters: {
		isAuthenticated: (state) => state.isAuthenticated,

		currentUser: (state) => (state.isAuthenticated ? state.user : null),
	},

	actions: {
		async load(context) {
			let accessToken = TokenService.getAccessToken();
			if (!accessToken) {
				context.commit("clearAuth");
				return;
			}

			let remainTimelife = Moment.duration(
				Moment.unix(Jose.decodeJwt(accessToken).exp).diff(Moment.utc()),
			).asSeconds();

			if (remainTimelife <= 3) {
				try {
					let refreshToken = TokenService.getRefreshToken();
					let { data } = await ApiService.identity.put({
						url: "/identity/refreshToken",
						data: { refreshToken },
					});

					context.commit("setAuth", data);
					return;
				} catch {
					context.commit("clearAuth");
					return;
				}
			}

			try {
				let { data } = await ApiService.identity.get({
					url: "/identity",
					accessToken,
				});
				context.commit("setUser", { accessToken, profile: data });
			} catch {
				context.commit("clearAuth");
			}
		},

		async login(context, payload) {
			let { data } = await ApiService.identity.post({
				url: "/identity",
				data: payload,
				accessToken: null,
			});

			context.commit("setAuth", data);
			return data;
		},

		async logout(context) {
			await ApiService.identity.delete({
				url: "/identity",
			});

			context.commit("clearAuth");
		},

		updateUser(context, data) {
			context.commit("updateUser", data);
		},
	},
};
