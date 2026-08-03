using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình ApiKey xác thực.
    /// </summary>
    public class ApiKeyAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Khởi tạo <see cref="ApiKeyAppsetting"/>.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public ApiKeyAppsetting(IConfiguration configuration)
            : base(configuration, "ApiKey")
        {
            if (AppCode.IsNullOrEmpty())
                throw new ArgumentException("Must not be null or empty", nameof(AppCode));

            if (PrivateKey.IsNullOrEmpty())
                throw new ArgumentException("Must not be null or empty", nameof(PrivateKey));

            if (PublicKeyEndpoint.IsNullOrEmpty())
                throw new ArgumentException("Must not be null or empty", nameof(PublicKeyEndpoint));

            if (SignKeyLifetimeSeconds < 0)
                throw new ArgumentException("Must be >= 0", nameof(SignKeyLifetimeSeconds));

            if (CacheLifetimeSeconds < 0)
                throw new ArgumentException("Must be >= 0", nameof(CacheLifetimeSeconds));
        }

        /// <summary>
        /// Mã ứng dụng (AppCode / Issuer), được dùng tạo kid dạng api-key-&lt;AppCode&gt;.
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// Private Key (PEM format) dùng để ký request đi.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Endpoint lấy Public Key của Identity hoặc Service phát hành.
        /// </summary>
        public string PublicKeyEndpoint { get; set; }

        /// <summary>
        /// Thời gian sống của SignKey (giây), mặc định là 300 giây (5 phút).
        /// </summary>
        public int SignKeyLifetimeSeconds { get; set; } = 300;

        /// <summary>
        /// Thời gian cache Public Key (giây), mặc định là 900 giây (15 phút).
        /// </summary>
        public int CacheLifetimeSeconds { get; set; } = 900;
    }
}
