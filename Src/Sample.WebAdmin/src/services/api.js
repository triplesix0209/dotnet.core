import * as Jose from "jose";
import TokenService from "@/services/token";
import axios from "axios";
import moment from "moment-timezone";
import stringTemplate from "string-template";

const service = {
	identity: {
		baseUrl: process.env.VUE_APP_API_IDENTITY,
	},
	static: {
		baseUrl: process.env.VUE_APP_API_STATIC,
	},
};

// #region [helpers]

function init(context) {
	if (!context.client) {
		context.client = axios.create({
			baseURL: context.baseUrl,
		});
	}

	let httpMethods = ["get", "post", "put", "delete"];
	for (let method of httpMethods) {
		context[method] = async (options) =>
			await request({
				...options,
				client: context.client,
				method,
			});
	}
}

async function refreshAccessToken({ accessToken, refreshToken } = {}) {
	if (!accessToken) throw Error("missing accessToken");
	if (!refreshToken) throw Error("missing refreshToken");

	if (
		moment
			.duration(moment.unix(Jose.decodeJwt(accessToken).exp).diff(moment.utc()))
			.asSeconds() > 5
	)
		return accessToken;

	let { data } = await service.identity.put({
		url: "/identity/refreshToken",
		data: { refreshToken },
		accessToken: null,
		refreshToken: null,
	});

	return data.accessToken;
}

async function request({
	client,
	method,
	url,
	path,
	params,
	data,
	form,
	responseType,
	accessTokenField,
	accessToken,
	refreshToken,
} = {}) {
	if (accessTokenField === undefined) accessTokenField = "Authorization";
	if (accessToken === undefined) accessToken = TokenService.getAccessToken();
	if (refreshToken === undefined) refreshToken = TokenService.getRefreshToken();
	if (accessToken && refreshToken)
		accessToken = await refreshAccessToken({ accessToken, refreshToken });

	let headers = {};
	if (accessToken) headers[accessTokenField] = accessToken;

	if (form) {
		headers["Content-Type"] = "multipart/form-data";
		data = form;
	}

	url = stringTemplate(url, path);

	try {
		let response = await client({
			method,
			url,
			params,
			data,
			responseType,
			headers,
		});
		return response.data;
	} catch ({ response }) {
		let { status, statusText, data } = response;
		if (responseType === "arraybuffer")
			data = JSON.parse(new TextDecoder("utf-8").decode(data));
		if (typeof data === "object") {
			let error = new Error(data.error.message);
			error.code = data.error.code;
			error.statusCode = status;
			error.message = data.error.message;
			error.data = data.error.data;
			throw error;
		} else if (typeof data === "string" && data.length > 0) {
			let error = new Error(data);
			error.statusCode = status;
			error.message = data;
			throw error;
		} else {
			let error = new Error(statusText);
			error.statusCode = status;
			error.message = statusText;
			throw error;
		}
	}
}

// #endregion

// #region [setup]

if (service.identity) init(service.identity);

if (service.static) {
	init(service.static);

	service.static.upload = async function (files) {
		let form = new FormData();
		form.append("files", files);

		let { data } = await service.static.post({
			url: "/upload",
			form,
		});

		let result = data;
		if (Array.isArray(files)) return result;
		return result[0];
	};
}

service.admin = {};
let baseUrls = JSON.parse(process.env.VUE_APP_API_ADMINS);
for (let key in baseUrls) {
	let api = { baseUrl: baseUrls[key] };
	init(api);
	service.admin[key] = api;
}

// #endregion

export default service;
