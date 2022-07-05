using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class CollectionHelperTest
    {
        [TestMethod]
        public void IsNullOrEmpty()
        {
            string[]? array;
            List<string>? list;

            array = null;
            Assert.IsTrue(array.IsNullOrEmpty());
            list = null;
            Assert.IsTrue(list.IsNullOrEmpty());

            array = Array.Empty<string>();
            Assert.IsTrue(array.IsNullOrEmpty());
            list = new List<string>();
            Assert.IsTrue(list.IsNullOrEmpty());

            array = new[] { "a", "b" };
            Assert.IsFalse(array.IsNullOrEmpty());
            list = new List<string> { "a", "b" };
            Assert.IsFalse(list.IsNullOrEmpty());
        }

        [TestMethod]
        public void ToStringTest()
        {
            var input = new[] { 1, 2, 3, 4 };
            var separator = ", ";
            var expected = "1, 2, 3, 4";

            Assert.AreEqual(expected, input.ToString(separator));
        }
    }
}
