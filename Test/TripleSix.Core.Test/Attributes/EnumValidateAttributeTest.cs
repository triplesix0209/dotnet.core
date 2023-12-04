using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.CoreOld.Attributes;

namespace TripleSix.CoreOld.Test.Attributes
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
        public void Valid()
        {
            var attr = new EnumValidateAttribute();
            Assert.IsTrue(attr.IsValid(EnumOne.One));
        }

        [TestMethod]
        public void Null()
        {
            var attr = new EnumValidateAttribute();
            Assert.IsTrue(attr.IsValid(null));
        }

        [TestMethod]
        public void DiffType()
        {
            var attr = new EnumValidateAttribute(typeof(EnumTwo));
            Assert.IsFalse(attr.IsValid(EnumOne.Three));
        }

        [TestMethod]
        public void WrongType()
        {
            var attr = new EnumValidateAttribute();
            Assert.IsFalse(attr.IsValid(string.Empty));
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
