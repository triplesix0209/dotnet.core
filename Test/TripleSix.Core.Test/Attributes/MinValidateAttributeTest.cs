using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.CoreOld.Attributes;

namespace TripleSix.CoreOld.Test.Attributes
{
    [TestClass]
    public class MinValidateAttributeTest
    {
        [TestMethod]
        public void Valid()
        {
            var attr = new MinValidateAttribute(10);
            Assert.IsTrue(attr.IsValid(10));
            Assert.IsTrue(attr.IsValid(11));
        }

        [TestMethod]
        public void Null()
        {
            var attr = new MinValidateAttribute(0);
            Assert.IsTrue(attr.IsValid(null));
        }

        [TestMethod]
        public void WrongValue()
        {
            var min = 10;
            var attr = new MinValidateAttribute(min);
            var name = "test";
            var errorTemplate = $"giá trị của {name} không được nhỏ hơn {min}";
            var value = 9;

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
