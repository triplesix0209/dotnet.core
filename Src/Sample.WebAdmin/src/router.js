import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

const routes = [];
routes.push({
	path: "/",
	name: "home",
	component: () => import("@/views/pages/Home"),
});

const router = new Router({
	base: process.env.BASE_URL,
	mode: "history",
	routes,
});

export default router;
