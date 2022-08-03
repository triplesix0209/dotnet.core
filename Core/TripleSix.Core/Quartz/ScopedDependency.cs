namespace TripleSix.Core.Quartz
{
    public class ScopedDependency : IScopedDependency
    {
        public ScopedDependency(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public string Scope { get; }
    }
}
