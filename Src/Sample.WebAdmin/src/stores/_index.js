import Vue from "vue";
import Vuex from "vuex";
import Auth from "./auth";
import Layout from "./layout";
import Lightbox from "./lightbox";

Vue.use(Vuex);

export default new Vuex.Store({
	modules: {
		auth: Auth,
		layout: Layout,
		lightbox: Lightbox,
	},
});
