namespace TripleSix.Core.Constants
{
    /// <summary>
    /// Loại Signing Key.
    /// </summary>
    public enum IdentitySigningKeyModes
    {
        /// <summary>
        /// Signing Key lấy động.
        /// </summary>
        Dynamic = 1,

        /// <summary>
        /// Signing Key cố định từ Appsetting.
        /// </summary>
        Static = 2,

        /// <summary>
        /// Public key từ JWKS endpoint.
        /// </summary>
        Jwks = 3,
    }
}
