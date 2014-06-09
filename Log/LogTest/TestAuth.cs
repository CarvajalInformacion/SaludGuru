using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogTest
{
    [TestClass]
    public class TestAuth
    {
        [TestMethod]
        public void TestMethod1()
        {
            //CarvajalLog.DAL.Controller.LogDataController logAuth = new CarvajalLog.DAL.Controller.LogDataController();
            //logAuth.SaveLogAuth(1, "aasdas", 1, "asdasd", 1);

            CarvajalLog.LogController logAuth = new CarvajalLog.LogController();
            CarvajalLog.Models.AuthLogModel authModel = new CarvajalLog.Models.AuthLogModel();
            authModel.IdUsuario = "1";
            authModel.IsSuccessfull = 1;
            authModel.LogAction = "Registro test auth";
            authModel.UserId = 1;
            authModel.ErrorMessage = "Registro test auth";
            authModel.IdLog = 1;
            logAuth.SaveLog(authModel);
        }
    }
}
