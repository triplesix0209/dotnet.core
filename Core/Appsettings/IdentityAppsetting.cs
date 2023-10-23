using Microsoft.Extensions.Configuration;

namespace TripleSix.Core.Appsettings
{
    public class IdentityAppsetting : BaseAppsetting
    {
        public IdentityAppsetting(IConfiguration configuration)
            : base(configuration, "Identity")
        {
        }

        /// <summary>
        /// Signing Key.
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        /// Bật/tắt kiểm tra Issuer.
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// Issuer hợp lệ.
        /// </summary>
        public string ValidIssuer { get; set; } = "Identity";
    }
}
