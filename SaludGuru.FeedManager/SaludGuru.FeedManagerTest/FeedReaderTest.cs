using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SaludGuru.FeedManagerTest
{
    [TestClass]
    public class FeedReaderTest
    {
        [TestMethod]
        public void GetFeed()
        {
            List<SaludGuru.FeedManager.Models.FeedReaderModel> oReturn = SaludGuru.FeedManager.FeedController.GetFeed(2);
            Assert.AreEqual(oReturn.Count > 0, true);
        }
    }
}
