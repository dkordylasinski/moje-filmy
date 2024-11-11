const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({
	transpileDependencies: true,
	devServer: {
		proxy: {
			'/api': {
				target: 'https://filmy.programdemo.pl',
				changeOrigin: true,
				pathRewrite: {
					'^/api': '',
				},
				onProxyReq(proxyReq, req, res) {
					console.log('Przekierowane zapytanie: ', proxyReq.path);
				},
			},
		},
	},
});
