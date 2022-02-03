using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        public void RandomString()
        {
            var generateCount = 100;
            var length = 10;
            var chars = "ABC";

            var values = new List<string>();
            for (var i = 1; i <= generateCount; i++)
            {
                var value = RandomHelper.RandomString(length, chars);
                values.Add(value);

                Assert.AreEqual(value.Length, length);
                Assert.IsTrue(value.All(x => chars.Contains(x)));
            }

            Assert.IsTrue(values.Distinct().Count() > 1);
        }

        [TestMethod]
        public void RandomNumber()
        {
            var generateCount = 100;
            var min = 1;
            var max = 10;

            var values = new List<int>();
            for (var i = 1; i <= generateCount; i++)
            {
                var value = RandomHelper.RandomNumber(min, max);
                values.Add(value);

                Assert.IsTrue(min <= value && value <= max);
            }

            Assert.IsTrue(values.Distinct().Count() > 1);
        }
    }
}
