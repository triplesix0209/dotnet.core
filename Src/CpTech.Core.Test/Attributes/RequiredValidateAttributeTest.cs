using System.ComponentModel.DataAnnotations;
using CpTech.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CpTech.Core.Test.Attributes
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
            var errorTemplate = $"{name} là thông tin bắt buộc";

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
