import Vue from "vue";
import App from "@/App.vue";
import Router from "@/router";
import Store from "@/stores/_index";
import Vuetify from "@/plugins/vuetify";

// filter
import "@/filters/string";

// directive
import "@/directives/permission";

Vue.config.productionTip = false;
new Vue({
	router: Router,
	store: Store,
	vuetify: Vuetify,
	render: (h) => h(App),
}).$mount("#app");
