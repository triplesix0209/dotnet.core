#pragma warning disable SA1649 // File name should match first type name

using System;

namespace CpTech.Core.Dto
{
    public abstract class BaseDto
        : IDto
    {
        public virtual object Clone()
        {
            var type = GetType();
            var result = Activator.CreateInstance(type);

            foreach (var p in type.GetProperties())
            {
                type.GetProperty(p.Name)?.SetValue(result, p.GetValue(this));
            }

            return result!;
        }
    }
}