using TripleSix.Core.Helpers;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public partial class EnumHelperTest
    {
        [TestMethod]
        public void GetDescription()
        {
            AccountTypeEnum input;
            string expected;

            input = AccountTypeEnum.Root;
            expected = "Root";
            Assert.AreEqual(expected, input.GetDescription());

            input = AccountTypeEnum.Admin;
            expected = "Quản trị";
            Assert.AreEqual(expected, input.GetDescription());

            input = AccountTypeEnum.Common;
            expected = "Thông thường";
            Assert.AreEqual(expected, input.GetDescription());
        }
    }

    public partial class EnumHelperTest
    {
        protected enum AccountTypeEnum
        {
            Root = 0,

            [System.ComponentModel.Description("Quản trị")]
            Admin = 1,

            [System.ComponentModel.Description("Thông thường")]
            Common = 2,
        }
    }
}
