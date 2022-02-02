#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Dto
{
    public interface IDto
        : ICloneable
    {
        public void Validate();

        public IDictionary<string, ICollection<ValidationResult>> TryValidate();
    }
}
