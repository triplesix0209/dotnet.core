namespace TripleSix.Core.Mappers
{
    /// <summary>
    /// Cờ đánh dấu ignore property khi thực hiện MapToEntity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreMapToEntityAttribute : Attribute
    {
    }
}
