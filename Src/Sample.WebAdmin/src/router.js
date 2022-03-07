import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

const routes = {
	base: [
		{
			name: "login",
			path: "/login",
			component: () => import("@/views/pages/Login"),
		},
		{
			name: "admin",
			path: "/",
			redirect: "/home",
			component: () => import("@/views/layouts/Admin/Layout"),
			children: [
				{
					path: "/home",
					name: "home",
					component: () => import("@/views/pages/Home"),
					meta: { title: "trang chính", sortOrder: -1 },
				},
			],
		},
	],
};

// #region [init router]

const adminRoute = {
	path: "/",
	component: () => import("@/views/layouts/Admin/Layout"),
	children: [],
};

adminRoute.children.push({
	path: "/:controller",
	name: "auto-list",
	component: () => import("@/views/pages/Auto/List"),
});

adminRoute.children.push({
	path: "/:controller/create",
	name: "auto-create",
	component: () => import("@/views/pages/Auto/Create"),
});

adminRoute.children.push({
	path: "/:controller/:id/update",
	name: "auto-update",
	component: () => import("@/views/pages/Auto/Update"),
});

adminRoute.children.push({
	path: "/:controller/:id",
	name: "auto-detail",
	component: () => import("@/views/pages/Auto/Detail"),
});

adminRoute.children.push({
	path: "/:controller/:id/changelog",
	name: "auto-changelog",
	component: () => import("@/views/pages/Auto/ChangeLog"),
});

let routeItems = [];
if (routes.base) for (let route of routes.base) routeItems.push(route);
routeItems = routeItems.concat(adminRoute, [
	{
		name: "error",
		path: "/error/:code",
		component: () => import("@/views/pages/Error"),
	},
	{
		path: "*",
		redirect: { name: "error", params: { code: "404" } },
	},
]);

const router = new Router({
	base: process.env.BASE_URL,
	mode: "history",
	routes: routeItems,
});

// #endregion

router.beforeEach(async (to, from, next) => {
	setTimeout(() => window.scrollTo(0, 0), 100);
	next();
});

export default router;
