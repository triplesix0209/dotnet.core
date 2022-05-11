import ApiService from "@/services/api";

export default class UploadAdapter {
	constructor(loader) {
		this.loader = loader;
	}

	upload() {
		return this.loader.file.then(
			(file) =>
				new Promise((resolve, reject) => {
					if (!ApiService.static) {
						reject(Error("Static API is not configured"));
						return;
					}

					ApiService.static
						.upload(file)
						.then((data) => {
							resolve({ default: data.url });
						})
						.catch((e) => reject(e));
				}),
		);
	}
}
