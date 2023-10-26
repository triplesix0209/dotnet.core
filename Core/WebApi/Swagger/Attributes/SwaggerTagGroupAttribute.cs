﻿namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Adds Tag Group metadata for a given controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SwaggerTagGroupAttribute : Attribute
    {
        public SwaggerTagGroupAttribute(string description, int orderIndex = 0)
        {
            Description = description;
            OrderIndex = orderIndex;
        }

        /// <summary>
        /// A short description for the tag group.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// A index use for sort tag groups.
        /// </summary>
        public int OrderIndex { get; }
    }
}
