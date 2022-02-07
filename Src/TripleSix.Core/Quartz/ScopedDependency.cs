#pragma warning disable SA1649 // File name should match first type name

using System;

namespace TripleSix.Core.Quartz
{
    public interface IScopedDependency
    {
        string Scope { get; }
    }

    public class ScopedDependency : IScopedDependency
    {
        public ScopedDependency(string scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public string Scope { get; }
    }
}
