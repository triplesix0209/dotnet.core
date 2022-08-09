namespace TripleSix.Core.AutoAdmin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminModelAttribute : Attribute
    {
        /// <summary>
        /// Entity sử dụng.
        /// </summary>
        public Type EntityType { get; set; }
    }
}
