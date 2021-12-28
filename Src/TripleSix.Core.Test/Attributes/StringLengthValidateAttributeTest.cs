using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TripleSix.Core.Test.Attributes
{
    [TestClass]
    public class StringLengthValidateAttributeTest
    {
        [TestMethod]
        public void ValidMax()
        {
            var attr = new StringLengthValidateAttribute(10);
            Assert.IsTrue(attr.IsValid("123456789"));
            Assert.IsTrue(attr.IsValid("1234567890"));
        }

        [TestMethod]
        public void ValidRange()
        {
            var attr = new StringLengthValidateAttribute(1, 10);
            Assert.IsTrue(attr.IsValid("1"));
            Assert.IsTrue(attr.IsValid("12"));
            Assert.IsTrue(attr.IsValid("123456789"));
            Assert.IsTrue(attr.IsValid("1234567890"));
        }

        [TestMethod]
        public void Null()
        {
            var attrMax = new StringLengthValidateAttribute(10);
            Assert.IsTrue(attrMax.IsValid(null));

            var attrRange = new StringLengthValidateAttribute(1, 10);
            Assert.IsTrue(attrRange.IsValid(null));
        }

        [TestMethod]
        public void WrongValueMax()
        {
            var max = 10;
            var attr = new StringLengthValidateAttribute(max);
            var name = "test";
            var errorTemplate = $"{name} không được vượt quá {max} ký tự";
            var value = "12345678901";

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

        [TestMethod]
        public void WrongValueRange()
        {
            var min = 1;
            var max = 10;
            var attr = new StringLengthValidateAttribute(min, max);
            var name = "test";
            var errorTemplate = $"{name} phải có độ dài trong khoảng {min} - {max} ký tự";
            var value = string.Empty;

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
