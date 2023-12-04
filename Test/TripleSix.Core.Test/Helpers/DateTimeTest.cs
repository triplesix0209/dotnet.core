using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.Test.Helpers
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
