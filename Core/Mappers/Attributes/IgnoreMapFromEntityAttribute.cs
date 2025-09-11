namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cờ đánh dấu ignore property khi thực hiện MapFromEntity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreMapFromEntityAttribute : Attribute
    {
    }
}
