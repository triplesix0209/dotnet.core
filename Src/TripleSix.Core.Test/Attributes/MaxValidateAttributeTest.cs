using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripleSix.Core.Test.Attributes
{
    [TestClass]
    public class MaxValidateAttributeTest
    {
        [TestMethod]
        public void Valid()
        {
            var attr = new MaxValidateAttribute(10);
            Assert.IsTrue(attr.IsValid(9));
            Assert.IsTrue(attr.IsValid(10));
        }

        [TestMethod]
        public void Null()
        {
            var attr = new MaxValidateAttribute(0);
            Assert.IsTrue(attr.IsValid(null));
        }

        [TestMethod]
        public void WrongValue()
        {
            var max = 10;
            var attr = new MaxValidateAttribute(max);
            var name = "test";
            var errorTemplate = $"giá trị của {name} không được lớn hơn {max}";
            var value = 11;

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
