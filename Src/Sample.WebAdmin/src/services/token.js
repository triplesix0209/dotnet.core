const ACCESS_TOKEN_KEY = "accessToken";
const REFRESH_TOKEN_KEY = "refreshToken";
const service = { ACCESS_TOKEN_KEY, REFRESH_TOKEN_KEY };

service.getAccessToken = function () {
	return window.localStorage[ACCESS_TOKEN_KEY];
};

service.getRefreshToken = function () {
	return window.localStorage[REFRESH_TOKEN_KEY];
};

service.setToken = function (accessToken, refreshToken) {
	window.localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
	window.localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
};

service.clearToken = function () {
	window.localStorage.removeItem(ACCESS_TOKEN_KEY);
	window.localStorage.removeItem(REFRESH_TOKEN_KEY);
};

export default service;
