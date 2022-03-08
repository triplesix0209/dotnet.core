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

const formatTypes = {
	clearVietnameseSign: (value) => {
		for (let i = 0; i < AccentsMap.length; i++) {
			let re = new RegExp(`[${AccentsMap[i].substring(1)}]`, "g");
			let char = AccentsMap[i][0];
			value = value.replace(re, char);
		}

		return value;
	},

	capitalize: (value) => value[0].toUpperCase() + value.substr(1),

	upperCase: (value) => value.toUpperCase(),

	lowerCase: (value) => value.toLowerCase(),

	titleCase: (value) =>
		value
			.split(" ")
			.map((x) => x[0].toUpperCase() + x.substr(1))
			.join(" "),

	kebabCase: (value) =>
		value
			.match(
				/[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+/g,
			)
			.map((x) => x.toLowerCase())
			.join("-"),

	template: (value, data) => stringTemplate(value, data),
};

Vue.filter("strFormat", function (value, type, data) {
	if (value === undefined || value === null) return value;

	let formatType = Object.getOwnPropertyNames(formatTypes).find(
		(x) => x.toLowerCase() === type.trim().toLowerCase(),
	);
	if (!formatType) throw Error(`format type '${type}' is invalid`);

	return formatTypes[formatType](value, data);
});
