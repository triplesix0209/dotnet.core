namespace TripleSix.Core.Quartz
{
    /// <summary>
    /// Scoped dependency.
    /// </summary>
    public class ScopedDependency : IScopedDependency
    {
        /// <summary>
        /// Scoped dependency.
        /// </summary>
        /// <param name="scope">Scope.</param>
        public ScopedDependency(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <inheritdoc/>
        public string Scope { get; }
    }
}
