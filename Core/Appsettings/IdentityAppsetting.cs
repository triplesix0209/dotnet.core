using Microsoft.Extensions.Configuration;
using TripleSix.Core.Constants;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    /// <summary>
    /// Cấu hình identity.
    /// </summary>
    public class IdentityAppsetting : BaseAppsetting
    {
        /// <summary>
        /// Cấu hình identity.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public IdentityAppsetting(IConfiguration configuration)
            : base(configuration, "Identity")
        {
            if (SigningKeyMode == IdentitySigningKeyModes.Static && SigningKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(SigningKey));
            if (SigningKeyCacheTimelife.HasValue && SigningKeyCacheTimelife < 0)
                throw new ArgumentException(nameof(SigningKeyCacheTimelife));
        }

        /// <summary>
        /// Phương pháp lấy Signing Key.
        /// </summary>
        public IdentitySigningKeyModes SigningKeyMode { get; set; } = IdentitySigningKeyModes.Static;

        /// <summary>
        /// Thời gian cache của Signing Key.
        /// </summary>
        public long? SigningKeyCacheTimelife { get; set; }

        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Signing Key.
        /// </summary>
        public string? SigningKey { get; set; } = null;

        /// <summary>
        /// Issuer hợp lệ.
        /// </summary>
        public string Issuer { get; set; } = "Identity";

        /// <summary>
        /// Danh sách Audience hợp lệ.
        /// </summary>
        public string[] Audience { get; set; } = new[] { "Identity" };

        /// <summary>
        /// Bật/tắt kiểm tra Issuer.
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// Bật/tắt kiểm tra Audience.
        /// </summary>
        public bool ValidateAudience { get; set; } = false;

        /// <summary>
        /// Danh sách các User ID được phép bypass kiểm tra thời gian.
        /// </summary>
        public string[]? BypassUserIds { get; set; } = null;

        /// <summary>
        /// Token sử dụng để request internal.
        /// </summary>
        public string? InternalToken { get; set; } = null;

        /// <summary>
        /// Url sử dụng để request internal.
        /// </summary>
        public string? InternalUrl { get; set; } = null;
    }
}
