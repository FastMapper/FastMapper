using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastMapper.Adapters;

namespace FastMapper.Tests
{
    [TestClass]
    public class PrimitiveTests
    {
        [TestMethod]
        public void TestPrimitiveTypes()
        {
            byte b = PrimitiveAdapter<int, byte>.Adapt(5);

            Assert.IsTrue(b == 5);
        }
    }
}
