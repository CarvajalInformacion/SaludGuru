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

            ////Se generan los objetos necesarios para realizar la prueba unitaria de mensajería.

            CarvajalLog.LogController logMessage = new CarvajalLog.LogController();

            /*Se declara y se definen los datos que va a llevar el modelo para realizar la prueba;
                Estos parametros se los puede modificar por razones de prueba. */
            CarvajalLog.Models.MessageLogModel messageModel = new CarvajalLog.Models.MessageLogModel();
            messageModel.IdMessage = 1;
            messageModel.IsSuccessfull = 1;
            messageModel.LogAction = "Registro test";
            messageModel.MessageFrom = "1540";
            messageModel.MessageTo = "1540";
            messageModel.UserId = 100;
            messageModel.ErrorMessage = "Registro test";
            messageModel.IdLog = 1;

            //Se ejecuta el Método encargado de almacenar el nuevo registro de log.
            logMessage.SaveLog(messageModel);
        }
    }
}
