using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Extensions;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.HelperExtensions
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void ToEpochTimestamp()
        {
            var datetime = new DateTime(1993, 9, 2, 4, 0, 0);
            var timestamp = 746942400000;

            Assert.AreEqual(datetime.ToEpochTimestamp(), timestamp);
        }

        [TestMethod]
        public void ParseEpochTimestamp()
        {
            var datetime = new DateTime(1993, 9, 2, 4, 0, 0);
            var timestamp = 746942400000;

            Assert.AreEqual(DateTimeHelper.ParseEpochTimestamp(timestamp), datetime);
        }
    }
}
