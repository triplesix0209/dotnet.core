import "material-design-icons-iconfont/dist/material-design-icons.css";
import "vuetify/dist/vuetify.min.css";
import Vue from "vue";
import Vuetify from "vuetify/lib/framework";
import VuetifyConfirm from "vuetify-confirm";
import vi from "vuetify/lib/locale/vi";

Vue.use(Vuetify);

const vuetify = new Vuetify({
	theme: {
		options: {
			customProperties: true,
		},
		themes: {
			light: {
				primary: "#5867dd",
				secondary: "#e8ecfa",
				accent: "#5d78ff",
				error: "#fd397a",
				info: "#5578eb",
				success: "#0abb87",
				warning: "#ffb822",
			},
		},
	},
	icons: {
		iconfont: "md",
	},
	lang: {
		locales: { vi },
		current: "vi",
	},
});

Vue.use(VuetifyConfirm, {
	vuetify,
	icon: "info",
	color: "info",
	title: "Xác nhận",
	buttonTrueText: "Có",
	buttonFalseText: "Không",
});

export default vuetify;
