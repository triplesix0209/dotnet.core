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
        /// Issuer hợp lệ.
        /// </summary>
        public string Issuer { get; set; } = "Identity";

        /// <summary>
        /// Bật/tắt kiểm tra Issuer.
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;
    }
}
