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

            //Se generan los objetos necesarios para realizar la prueba unitaria de autenticación.
            
            CarvajalLog.LogController logAuth = new CarvajalLog.LogController();

            /*Se declara y se definen los datos que va a llevar el modelo para realizar la prueba;
                Estos parametros se los puede modificar por razones de prueba. */
            CarvajalLog.Models.AuthLogModel authModel = new CarvajalLog.Models.AuthLogModel();
            authModel.IdUsuario = "1";
            authModel.IsSuccessfull = 1;
            authModel.LogAction = "Registro test auth 10 Junio";
            authModel.UserId = 1;
            authModel.ErrorMessage = "Registro test auth 10 Junio";
            authModel.IdLog = 1;

            //Se ejecuta el Método encargado de almacenar el nuevo registro de log.
            logAuth.SaveLog(authModel);
        }
    }
}
