export default {
	namespaced: true,

	state: {
		mediaList: [],
		mediaIndex: null,
	},

	mutations: {
		setMedia(state, payload) {
			state.mediaList = payload;
		},

		setIndex(state, payload) {
			state.mediaIndex = payload;
		},
	},

	getters: {
		mediaList: (state) => state.mediaList,

		mediaIndex: (state) => state.mediaIndex,
	},

	actions: {
		showLightbox(context, medias) {
			if (medias === undefined || medias === null || medias === []) return;
			if (!Array.isArray(medias)) medias = [medias];
			context.commit("setMedia", medias);
			context.commit("setIndex", 0);
		},

		closeLightbox(context) {
			context.commit("setIndex", null);
		},
	},
};
