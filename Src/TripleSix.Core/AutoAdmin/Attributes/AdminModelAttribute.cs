using System;

namespace TripleSix.Core.AutoAdmin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminModelAttribute : Attribute
    {
        /// <summary>
        /// tên entity sử dụng.
        /// </summary>
        public string EntityName { get; set; }
    }
}
