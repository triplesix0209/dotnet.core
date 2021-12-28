using System;

namespace CpTech.Core.Attributes
{
    public class EnumDataAttribute : Attribute
    {
        public EnumDataAttribute(string data)
        {
            Data = data;
        }

        public string Data { get; protected set; }
    }
}