using System.ComponentModel.DataAnnotations;
using CpTech.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CpTech.Core.Test.Attributes
{
    [TestClass]
    public class EnumValidateAttributeTest
    {
        private enum EnumOne
        {
            One = 1,
            Two = 2,
            Three = 3,
        }

        private enum EnumTwo
        {
            One = 1,
            Two = 2,
        }

        [TestMethod]
        public void Null()
        {
            var attr = new EnumValidateAttribute();
            EnumOne? value = null;
            bool result = attr.IsValid(value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WrongType()
        {
            var attr = new EnumValidateAttribute();
            var value = string.Empty;
            bool result = attr.IsValid(value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DiffType()
        {
            var attr = new EnumValidateAttribute(typeof(EnumTwo));
            var value = EnumOne.Three;
            bool result = attr.IsValid(value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RightValue()
        {
            var attr = new EnumValidateAttribute();
            var value = EnumOne.One;
            bool result = attr.IsValid(value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WrongValue()
        {
            var attr = new EnumValidateAttribute(typeof(EnumOne));
            var name = "test";
            var errorTemplate = $"giá trị của {name} không nằm trong tập giá trị cho phép";
            var value = 4;

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
