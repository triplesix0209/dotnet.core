import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

const routes = [
	{
		path: "/",
		redirect: "/home",
		component: () => import("@/views/layouts/Admin/Layout"),
		children: [
			{
				path: "/home",
				name: "home",
				component: () => import("@/views/pages/Home"),
			},
		],
	},

	{
		name: "login",
		path: "/login",
		component: () => import("@/views/pages/Login"),
	},

	{
		name: "error",
		path: "/error/:code",
		component: () => import("@/views/pages/Error"),
	},

	{
		path: "*",
		redirect: { name: "error", params: { code: "404" } },
	},
];

const router = new Router({
	base: process.env.BASE_URL,
	mode: "history",
	routes,
});

router.beforeEach(async (to, from, next) => {
	setTimeout(() => window.scrollTo(0, 0), 100);
	next();
});

export default router;
