namespace TripleSix.Core.Quartz
{
    public class ScopedDependency : IScopedDependency
    {
        public ScopedDependency(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        /// <inheritdoc/>
        public string Scope { get; }
    }
}
