import Vue from "vue";
import { Datetime } from "vue-datetime";
import App from "@/App.vue";
import Router from "@/router";
import Store from "@/stores/_index";
import Vuetify from "@/plugins/vuetify";

// directive
import "@/directives/permission";

// filter
import "@/filters/strFormat";

// plugins
import "@/plugins/ckeditor";
import "@/plugins/vue-datetime";
import "@/plugins/vue-lightbox";
import "@/plugins/vue-moment";
import "@/plugins/vue-numeral";
import "@/plugins/vue-toast";

// components
Vue.component("datetime", Datetime);

Vue.config.productionTip = false;
new Vue({
	router: Router,
	store: Store,
	vuetify: Vuetify,
	render: (h) => h(App),
}).$mount("#app");
