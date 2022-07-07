using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class DateTimeHelperTest
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

            Assert.AreEqual(timestamp.ToEpochTimestamp(), datetime);
        }
    }
}
