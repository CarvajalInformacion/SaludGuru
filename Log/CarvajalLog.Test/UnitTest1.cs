using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarvajalLog.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CarvajalLog.Interfaces.ILogModel DataToSave1 = new CarvajalLog.Models.AuthLogModel()
            {
                IdLog = 0,
                UserName = ""
            };

            CarvajalLog.LogController.Instance.SaveLog(DataToSave1);

            CarvajalLog.Interfaces.ILogModel DataToSave2 = new CarvajalLog.Models.MessageLogModel()
            {
                IdLog = 0,
                otracosa = 0
            };

            CarvajalLog.LogController.Instance.SaveLog(DataToSave2);
        }
    }
}
