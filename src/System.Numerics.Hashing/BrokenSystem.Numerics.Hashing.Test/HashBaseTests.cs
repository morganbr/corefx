using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Numerics.Hashing.Test
{
    [TestClass]
    public class HashBaseTests
    {
        [TestMethod]
        public void TestNullBytes()
        {
            AppendHash hash = new AppendHash();
            Assert.ThrowsException<ArgumentNullException>(() => hash.ComputeHash((byte[])null));
        }
    }
}
