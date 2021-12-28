using System.ComponentModel.DataAnnotations;
using CpTech.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CpTech.Core.Test.Attributes
{
    [TestClass]
    public class RangeValidateAttributeTest
    {
        [TestMethod]
        public void Valid()
        {
            var attr = new RangeValidateAttribute(1, 10);
            Assert.IsTrue(attr.IsValid(1));
            Assert.IsTrue(attr.IsValid(2));
            Assert.IsTrue(attr.IsValid(9));
            Assert.IsTrue(attr.IsValid(10));
        }

        [TestMethod]
        public void Null()
        {
            var attr = new RangeValidateAttribute(1, 10);
            Assert.IsTrue(attr.IsValid(null));
        }

        [TestMethod]
        public void WrongValue()
        {
            var min = 1;
            var max = 10;
            var attr = new RangeValidateAttribute(min, max);
            var name = "test";
            var errorTemplate = $"giá trị của {name} phải nằm trong khoảng {min} - {max}";
            var value = 0;

            Assert.ThrowsException<ValidationException>(() =>
            {
                try
                {
                    attr.Validate(value, name);
                }
                catch (ValidationException e)
                {
                    Assert.AreEqual(errorTemplate, e.Message);
                    throw;
                }
            });
        }
    }
}
