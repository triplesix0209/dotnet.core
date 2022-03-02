const { defineConfig } = require("@vue/cli-service");

module.exports = defineConfig({
	configureWebpack: { devtool: "source-map" },
	css: { sourceMap: process.env.NODE_ENV === "development" },
	transpileDependencies: ["vuetify"],
});
