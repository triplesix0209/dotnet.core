﻿using Microsoft.Extensions.Configuration;

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
        }

        /// <summary>
        /// Signing Key.
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        /// Danh sách Issuer hợp lệ.
        /// </summary>
        public string[] Issuer { get; set; } = new[] { "Identity" };

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
    }
}
