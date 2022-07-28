using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Appsettings
{
    public class IdentityAppsetting : BaseAppsetting
    {
        public IdentityAppsetting(IConfiguration configuration)
            : base(configuration, "Identity")
        {
            if (Issuer.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(Issuer));
            if (SecretKey.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(SecretKey));
        }

        /// <summary>
        /// Host phát sinh token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Secret key để mã hóa token.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Thời gian sống của access token (giây).
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 300;

        /// <summary>
        /// Thời gian sống của session token (giây).
        /// </summary>
        public int SessionTokenLifetime { get; set; } = 86400;
    }
}
