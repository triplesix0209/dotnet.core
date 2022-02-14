#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using TripleSix.Core.Dto;

namespace Sample.Middle.Mappers
{
    public class GlobalMapper : TripleSix.Core.Mappers.GlobalMapper
    {
        protected override IEnumerable<Type> SelectDtoType(string objectName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => typeof(IDataDto).IsAssignableFrom(t))
                .Where(t =>
                {
                    return new[]
                    {
                        objectName + "Dto",
                        objectName + "DataDto",
                    }.Contains(t.Name);
                }));
        }
    }
}
