namespace TripleSix.Core.Quartz
{
    /// <summary>
    /// Scoped dependency.
    /// </summary>
    public interface IScopedDependency
    {
        /// <summary>
        /// Scope.
        /// </summary>
        string Scope { get; }
    }
}
