import Vue from "vue";
import stringTemplate from "string-template";

Vue.filter("strUppercase", function (value) {
	if (!value) return value;
	return value.toUpperCase();
});

Vue.filter("strLowercase", function (value) {
	if (!value) return value;
	return value.toLowerCase();
});

Vue.filter("strCapitalize", function (value) {
	if (!value) return value;
	return value[0].toUpperCase() + value.substr(1);
});

Vue.filter("strTitlecase", function (value) {
	if (!value) return value;
	return value
		.split(" ")
		.map((x) => x[0].toUpperCase() + x.substr(1))
		.join(" ");
});

Vue.filter("strTemplate", function (template, payload) {
	if (!template) return template;
	return stringTemplate(template, payload);
});
