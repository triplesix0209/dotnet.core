namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Adds Tag Group metadata for a given controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SwaggerTagGroupAttribute : Attribute
    {
        /// <summary>
        /// Adds Tag Group metadata for a given controller.
        /// </summary>
        /// <param name="name">name for the tag group.</param>
        /// <param name="orderIndex">A index use for sort tag groups.</param>
        /// <param name="description">A short description for the tag group.</param>
        public SwaggerTagGroupAttribute(string name, int orderIndex = 0, string? description = null)
        {
            Name = name;
            Description = description;
            OrderIndex = orderIndex;
        }

        /// <summary>
        /// A name for the tag group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A index use for sort tag groups.
        /// </summary>
        public int OrderIndex { get; }

        /// <summary>
        /// A short description for the tag group.
        /// </summary>
        public string? Description { get; }
    }
}
