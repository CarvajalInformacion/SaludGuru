using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Message.Test
{
    [TestClass]
    public class MessageProcessTest : ClientTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Assert.AreEqual(true,false);
            Message.Manager.MessageProcess mgPr = new Message.Manager.MessageProcess();
            mgPr.StartProcess();
        }
    }
}
