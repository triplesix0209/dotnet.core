import Vue from "vue";
import Router from "vue-router";

Vue.use(Router);

const routes = {
	page: [
		{
			path: "/",
			redirect: "/account",
		},
		{
			path: "/profile",
			name: "profile",
			component: () => import("@/views/pages/Profile/ProfileInfo"),
			meta: {
				title: "thông tin cá nhân",
				menu: { hide: true },
			},
		},
		{
			path: "/changePassword",
			name: "change-password",
			component: () => import("@/views/pages/Profile/ChangePassword"),
			meta: {
				title: "thay đổi mật khẩu",
				menu: { hide: true },
			},
		},
	],

	detail: [
		{
			controller: "account",
			component: () => import("@/views/pages/Account/Detail"),
		},
		{
			controller: "permission-group",
			component: () => import("@/views/pages/PermissionGroup/Detail"),
		},
	],

	create: [
		{
			controller: "permission-group",
			component: () => import("@/views/pages/PermissionGroup/Create"),
		},
	],

	update: [
		{
			controller: "permission-group",
			component: () => import("@/views/pages/PermissionGroup/Update"),
		},
	],

	changelog: [
		{
			controller: "permission-group",
			component: () => import("@/views/pages/PermissionGroup/ChangeLog"),
		},
	],
};

// #region [init router]

const adminRoute = {
	path: "",
	component: () => import("@/views/layouts/Admin/Layout"),
	meta: { root: true },
	children: [],
};

if (routes.page) {
	for (let route of routes.page) adminRoute.children.push(route);
}

if (routes.list) {
	for (let route of routes.list) {
		adminRoute.children.push({
			...route,
			...{
				name: `${route.controller}-list`,
				path: `/${route.controller}`,
				meta: { controller: route.controller },
			},
		});
	}
}

adminRoute.children.push({
	path: "/:controller",
	name: "auto-list",
	component: () => import("@/views/pages/Auto/List"),
});

if (routes.create) {
	for (let route of routes.create) {
		adminRoute.children.push({
			...route,
			...{
				name: `${route.controller}-create`,
				path: `/${route.controller}/create`,
				meta: { controller: route.controller },
			},
		});
	}
}

adminRoute.children.push({
	path: "/:controller/create",
	name: "auto-create",
	component: () => import("@/views/pages/Auto/Create"),
});

if (routes.update) {
	for (let route of routes.update) {
		adminRoute.children.push({
			...route,
			...{
				name: `${route.controller}-update`,
				path: `/${route.controller}/:id/update`,
				meta: { controller: route.controller },
			},
		});
	}
}

adminRoute.children.push({
	path: "/:controller/:id/update",
	name: "auto-update",
	component: () => import("@/views/pages/Auto/Update"),
});

if (routes.changelog) {
	for (let route of routes.changelog) {
		adminRoute.children.push({
			...route,
			...{
				name: `${route.controller}-changelog`,
				path: `/${route.controller}/:id/changelog`,
				meta: { controller: route.controller },
			},
		});
	}
}

adminRoute.children.push({
	path: "/:controller/:id/changelog",
	name: "auto-changelog",
	component: () => import("@/views/pages/Auto/ChangeLog"),
});

if (routes.detail) {
	for (let route of routes.detail) {
		adminRoute.children.push({
			...route,
			...{
				name: `${route.controller}-detail`,
				path: `/${route.controller}/:id`,
				meta: { controller: route.controller },
			},
		});
	}
}

adminRoute.children.push({
	path: "/:controller/:id",
	name: "auto-detail",
	component: () => import("@/views/pages/Auto/Detail"),
});

let routeItems = [];
routeItems = routeItems.concat(adminRoute, [
	{
		name: "login",
		path: "/login",
		component: () => import("@/views/pages/System/Login"),
	},
	{
		name: "error",
		path: "/error/:code",
		component: () => import("@/views/pages/System/Error"),
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

export default router;
