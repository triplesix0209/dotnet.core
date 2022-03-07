import Vue from "vue";
import stringTemplate from "string-template";

const AccentsMap = [
	"aàảãáạăằẳẵắặâầẩẫấậ",
	"AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
	"dđ",
	"DĐ",
	"eèẻẽéẹêềểễếệ",
	"EÈẺẼÉẸÊỀỂỄẾỆ",
	"iìỉĩíị",
	"IÌỈĨÍỊ",
	"oòỏõóọôồổỗốộơờởỡớợ",
	"OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
	"uùủũúụưừửữứự",
	"UÙỦŨÚỤƯỪỬỮỨỰ",
	"yỳỷỹýỵ",
	"YỲỶỸÝỴ",
];

Vue.filter("strClearVietnameseSign", function (value) {
	if (!value) return value;

	for (let i = 0; i < AccentsMap.length; i++) {
		let re = new RegExp(`[${AccentsMap[i].substr(1)}]`, "g");
		let char = AccentsMap[i][0];
		value = value.replace(re, char);
	}

	return value;
});

Vue.filter("strCapitalize", function (value) {
	if (!value) return value;
	return value[0].toUpperCase() + value.substr(1);
});

Vue.filter("strUpperCase", function (value) {
	if (!value) return value;
	return value.toUpperCase();
});

Vue.filter("strLowerCase", function (value) {
	if (!value) return value;
	return value.toLowerCase();
});

Vue.filter("strTitleCase", function (value) {
	if (!value) return value;
	return value
		.split(" ")
		.map((x) => x[0].toUpperCase() + x.substr(1))
		.join(" ");
});

Vue.filter("strKebabCase", function (value) {
	if (!value) return value;
	return value
		.match(/[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+/g)
		.map((x) => x.toLowerCase())
		.join("-");
});

Vue.filter("strTemplate", function (template, payload) {
	if (!template) return template;
	return stringTemplate(template, payload);
});
