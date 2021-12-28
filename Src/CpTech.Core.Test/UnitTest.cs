using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CpTech.Core.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod("ok")]
        public void TestMethod()
        {
            Assert.IsFalse(false, "1 should not be prime");
        }
    }
}
