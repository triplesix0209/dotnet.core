namespace TripleSix.Core.Constants
{
    /// <summary>
    /// Loại Signing Key.
    /// </summary>
    public enum IdentitySigningKeyModes
    {
        /// <summary>
        /// Signing Key cố định từ Appsetting.
        /// </summary>
        Static = 0,

        /// <summary>
        /// Signing Key lấy động.
        /// </summary>
        Dynamic = 1,
    }
}
