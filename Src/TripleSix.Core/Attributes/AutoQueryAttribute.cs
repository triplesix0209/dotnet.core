using System;

namespace TripleSix.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoQueryAttribute : Attribute
    {
        public bool Enable { get; set; } = true;

        public string FieldName { get; set; } = null;
    }
}