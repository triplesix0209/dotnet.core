using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Test.Extensions
{
    [TestClass]
    public class DateTimeExtensionTest
    {
        [TestMethod]
        public void ToEpochTimestamp()
        {
            var datetime = new DateTime(1993, 9, 2, 4, 0, 0);
            var timestamp = 746942400000;

            Assert.AreEqual(datetime.ToEpochTimestamp(), timestamp);
        }
    }
}
