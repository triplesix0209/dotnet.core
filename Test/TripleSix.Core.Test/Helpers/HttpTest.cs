using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class HttpTest
    {
        [TestMethod]
        public void GetValue()
        {
            var header = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "id", "666" },
                { "name", "Tạ Hồng Quang Lực" },
                { "isAdmin", "1" },
            });

            Assert.AreEqual(header.GetValue<int>("id"), int.Parse(header["id"]));
            Assert.AreEqual(header.GetValue("name"), header["name"].ToString());
            Assert.AreEqual(header.GetValue("isAdmin", converter: x => x == "1"), header["isAdmin"].ToString() == "1");
        }
    }
}
