using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Test.Attributes
{
    [TestClass]
    public class RequiredValidateAttributeTest
    {
        [TestMethod]
        public void Valid()
        {
            var attr = new RequiredValidateAttribute();
            Assert.IsTrue(attr.IsValid("abc"));
        }

        [TestMethod]
        public void WrongValue()
        {
            var attr = new RequiredValidateAttribute();
            var name = "test";
            var errorTemplate = $"{name} không được bỏ trống";

            Assert.ThrowsException<ValidationException>(() =>
            {
                try
                {
                    attr.Validate(null, name);
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
