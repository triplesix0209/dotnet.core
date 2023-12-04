using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.Test.Helpers
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void MD5Hash()
        {
            var input = "triplesix";
            var expected = "E31387C31860020ADE43BB189091CE92";

            Assert.AreEqual(expected, HashHelper.MD5Hash(input));
        }

        [TestMethod]
        public void SHA1Hash()
        {
            var input = "triplesix";
            var expected = "59CA996D67000A2D30FA284380F296DA2DA85FC0";

            Assert.AreEqual(expected, HashHelper.SHA1Hash(input));
        }
    }
}
