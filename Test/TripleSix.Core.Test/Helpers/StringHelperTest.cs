using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void RemoveVietnameseSign()
        {
            var input = "Tạ Hồng Quang Lực";
            var expected = "Ta Hong Quang Luc";

            Assert.AreEqual(expected, input.ClearVietnameseCase());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace()
        {
            string? data;

            data = null;
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = string.Empty;
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = "  ";
            Assert.IsTrue(data.IsNullOrWhiteSpace());

            data = "a";
            Assert.IsFalse(data.IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void ToCamelCase()
        {
            var input = "Ta  hong-QuangLuc";
            var expected = "taHongQuangLuc";

            Assert.AreEqual(expected, input.ToCamelCase());
        }

        [TestMethod]
        public void ToSnakeCase()
        {
            var input = "Ta  hong-QuangLuc";
            var expected = "ta_hong_quang_luc";

            Assert.AreEqual(expected, input.ToSnakeCase());
        }

        [TestMethod]
        public void ToKebabCase()
        {
            var input = "Ta  hong-QuangLuc";
            var expected = "ta-hong-quang-luc";

            Assert.AreEqual(expected, input.ToKebabCase());
        }
    }
}
