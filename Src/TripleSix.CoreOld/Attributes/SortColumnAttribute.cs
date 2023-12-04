using System;

namespace TripleSix.CoreOld.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortColumnAttribute : Attribute
    {
        public SortColumnAttribute(params string[] externalColumns)
        {
            ExternalColumns = externalColumns;
        }

        public string EntityName { get; set; }

        public string[] ExternalColumns { get; set; }
    }
}
