using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình tài liệu hệ thống.
    /// </summary>
    public class DocumentAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình tài liệu hệ thống.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public DocumentAppsetting(IConfiguration configuration)
            : base(configuration, "Document")
        {
        }

        /// <summary>
        /// Route đến tài liệu hệ thống.
        /// </summary>
        public string Route { get; set; } = "_document";

        /// <summary>
        /// Cấu hình Document Page.
        /// </summary>
        public DocumentPageAppsetting Page { get; set; } = new();

        /// <summary>
        /// Cấu hình Swagger.
        /// </summary>
        public DocumentSwaggerAppsetting Swagger { get; set; } = new();
    }

    /// <summary>
    /// Cấu hình Document Page.
    /// </summary>
    public class DocumentPageAppsetting
    {
        /// <summary>
        /// Bật/tắt Document Page.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Đường dẫn thư mục tài liệu.
        /// </summary>
        public string FolderPath { get; set; } = "Document";

        /// <summary>
        /// File tài liệu mặc định.
        /// </summary>
        public string DefaultFile { get; set; } = "index.html";
    }

    /// <summary>
    /// Cấu hình Swagger.
    /// </summary>
    public class DocumentSwaggerAppsetting
    {
        /// <summary>
        /// Bật/tắt swagger.
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Tiêu đề của API Document.
        /// </summary>
        public string Title { get; set; } = "API Document";

        /// <summary>
        /// Version của API Document.
        /// </summary>
        public string Version { get; set; } = string.Empty;
    }
}
