using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình elasticsearch.
    /// </summary>
    public class ElasticsearchAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình elasticsearch.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public ElasticsearchAppsetting(IConfiguration configuration)
            : base(configuration, "Elasticsearch")
        {
            if (Host.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Host));

            if (Template != null)
            {
                if (Template.Pattern.IsNullOrEmpty()) throw new ArgumentNullException($"{nameof(Template)}.{nameof(Template.Pattern)}");
                if (Template.NumberOfReplicas < 0) throw new ArgumentException("Must be >= 0", $"{nameof(Template)}.{nameof(Template.NumberOfReplicas)}");
            }
        }

        /// <summary>
        /// Host kết nối.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Username kết nối.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Password kết nối.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Số cấp property cho phép, để hạn chế trường hợp liên kết vòng.
        /// </summary>
        public int MaxDepthPropertyAllowed { get; set; } = 500;

        /// <summary>
        /// Cấu hình template.
        /// </summary>
        public ElasticsearchTemplateAppsetting? Template { get; set; }
    }

    /// <summary>
    /// Cấu hình template elasticsearch.
    /// </summary>
    public class ElasticsearchTemplateAppsetting
    {
        /// <summary>
        /// Template pattern, không cần thiết phải có * ở cuối.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Số lượng replicas.
        /// </summary>
        public int? NumberOfReplicas { get; set; }
    }
}
