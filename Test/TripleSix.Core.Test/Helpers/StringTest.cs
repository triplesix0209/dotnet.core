using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void IsNullOrWhiteSpace()
        {
            string data;

            data = null;
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = string.Empty;
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = " ";
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = "a";
            Assert.IsFalse(data.IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void IsNotNullOrWhiteSpace()
        {
            string data;

            data = null;
            Assert.IsFalse(data.IsNotNullOrWhiteSpace());

            data = string.Empty;
            Assert.IsFalse(data.IsNotNullOrWhiteSpace());

            data = " ";
            Assert.IsFalse(data.IsNotNullOrWhiteSpace());

            data = "a";
            Assert.IsTrue(data.IsNotNullOrWhiteSpace());
        }

        [TestMethod]
        public void RemoveVietnameseSign()
        {
            string input = "Tạ Hồng Quang Lực";
            string expected = "Ta Hong Quang Luc";

            Assert.AreEqual(expected, input.RemoveVietnameseSign());
        }

        [TestMethod]
        public void ToCamelCase()
        {
            string input = "Ta  hong-QuangLuc";
            string expected = "taHongQuangLuc";

            Assert.AreEqual(expected, input.ToCamelCase());
        }

        [TestMethod]
        public void ToSnakeCase()
        {
            string input = "Ta  hong-QuangLuc";
            string expected = "ta_hong_quang_luc";

            Assert.AreEqual(expected, input.ToSnakeCase());
        }

        [TestMethod]
        public void ToKebabCase()
        {
            string input = "Ta  hong-QuangLuc";
            string expected = "ta-hong-quang-luc";

            Assert.AreEqual(expected, input.ToKebabCase());
        }
    }
}
