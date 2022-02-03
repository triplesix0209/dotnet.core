using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TripleSix.Core.Extensions;

namespace TripleSix.Core.Test.Helpers
{
    [TestClass]
    public class ArrayListTest
    {
        [TestMethod]
        public void IsNullOrEmpty()
        {
            string[] array;
            List<string> list;

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
        public void IsNotNullOrEmpty()
        {
            string[] array;
            List<string> list;

            array = null;
            Assert.IsFalse(array.IsNotNullOrEmpty());
            list = null;
            Assert.IsFalse(list.IsNotNullOrEmpty());

            array = Array.Empty<string>();
            Assert.IsFalse(array.IsNotNullOrEmpty());
            list = new List<string>();
            Assert.IsFalse(list.IsNotNullOrEmpty());

            array = new[] { "a", "b" };
            Assert.IsTrue(array.IsNotNullOrEmpty());
            list = new List<string> { "a", "b" };
            Assert.IsTrue(list.IsNotNullOrEmpty());
        }
    }
}
