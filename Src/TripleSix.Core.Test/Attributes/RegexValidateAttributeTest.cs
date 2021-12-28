using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripleSix.Core.Test.Attributes
{
    [TestClass]
    public class RegexValidateAttributeTest
    {
        private const string _regexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        private const string _name = "email";

        [TestMethod]
        public void Valid()
        {
            var attr = new RegexValidateAttribute(_regexPattern, _name);
            Assert.IsTrue(attr.IsValid("triplesix0209@gmail.com"));
        }

        [TestMethod]
        public void Null()
        {
            var attr = new RegexValidateAttribute(_regexPattern, _name);
            Assert.IsTrue(attr.IsValid(null));
        }

        [TestMethod]
        public void WrongValue()
        {
            var attr = new RegexValidateAttribute(_regexPattern, _name);
            var name = "test";
            var errorTemplate = $"{name} phải có dạng {_name}";
            var value = "triplesix0209";

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
