using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FastMapper.Adapters;

namespace FastMapper.Tests
{
    #region TestObject

    public class A
    { 
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal GetTotal()
        {
            return X + Y;
        }
    }

    public class B
    {
        public decimal Total { get; set; }
    }

    public class C
    {
        public B BClass { get; set; }
    }

    public class D
    {
        public decimal BClassTotal { get; set; }
    }

    #endregion

    [TestClass]
    public class FlatteningTests
    {
        [TestMethod]
        public void GetMethodTest()
        {
            var b = ClassAdapter<A, B>.Adapt(new A { X = 100, Y = 50 });
            
            Assert.IsNotNull(b);
            Assert.IsTrue(b.Total == 150);
        }

        [TestMethod]
        public void PropertyTest()
        {
            var d = ClassAdapter<C, D>.Adapt(new C { BClass = new B { Total = 250 } });

            Assert.IsNotNull(d);
            Assert.IsTrue(d.BClassTotal == 250);
        }
    }
}
