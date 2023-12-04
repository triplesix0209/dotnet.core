using System;
using TripleSix.CoreOld.WebApi.Results;

namespace TripleSix.CoreOld.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerApiAttribute : Attribute
    {
        public SwaggerApiAttribute(string summary)
        {
            Summary = summary;
            ResponseType = typeof(SuccessResult);
        }

        public SwaggerApiAttribute(Type responseType)
        {
            ResponseType = responseType;
        }

        public SwaggerApiAttribute(string summary, Type responseType)
        {
            Summary = summary;
            ResponseType = responseType;
        }

        public SwaggerApiAttribute(string summary, string description, Type responseType)
        {
            Summary = summary;
            Description = description;
            ResponseType = responseType;
        }

        public string Summary { get; set; }

        public string Description { get; set; }

        public Type ResponseType { get; set; }
    }
}