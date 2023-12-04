using System;

namespace TripleSix.CoreOld.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoQueryAttribute : Attribute
    {
        public bool Enable { get; set; } = true;

        public string FieldName { get; set; } = null;
    }
}