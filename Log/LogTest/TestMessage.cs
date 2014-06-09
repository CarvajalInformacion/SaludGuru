using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogTest
{
    [TestClass]
    public class TestMessage
    {
        [TestMethod]
        public void TestMethod1()
        {
            //CarvajalLog.DAL.Controller.LogDataController logMessage = new CarvajalLog.DAL.Controller.LogDataController();
            //logMessage.SaveLogMessage(1, "registro de prueba", 1, "Prueba desde visual", "1440", "1020", 154);

            CarvajalLog.LogController logMessage = new CarvajalLog.LogController();
            CarvajalLog.Models.MessageLogModel messageModel = new CarvajalLog.Models.MessageLogModel();
            messageModel.IdMessage = 1;
            messageModel.IsSuccessfull = 1;
            messageModel.LogAction = "Registro test";
            messageModel.MessageFrom = "1540";
            messageModel.MessageTo = "1540";
            messageModel.UserId = 100;
            messageModel.ErrorMessage = "Registro test";
            messageModel.IdLog = 1;
            logMessage.SaveLog(messageModel);
        }
    }
}
