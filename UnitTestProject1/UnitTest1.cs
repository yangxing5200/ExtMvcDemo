using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yx.ServiceProxy.Basic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            string sql = "select * from abc";
            string sum = " sum(a.ass) ";
            string result = ToSum(sql, sum);
            var s = result;
        }
        public string ToSum(string sql, string sum)
        {
            var select = string.Format(" select {0} ", sum);
            return select + sql.Substring(sql.ToLower().IndexOf(" from "));
        }
        [TestMethod]
        public void TestWcfMethod()
        {
            BaServiceProxy proxy=new BaServiceProxy();
            proxy.GetData(8888);
        }
    }
}
