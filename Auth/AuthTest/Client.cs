using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AuthTest
{
    [TestClass]
    public class Client
    {
        [TestMethod]
        public void GetUserList()
        {
            List<SessionController.Models.Auth.User> userList = new List<SessionController.Models.Auth.User>();
            userList = Auth.Client.Controller.Client.GetUserList("9049CF97,6092272D,82E18904,1F2FA79B");

        }
    }
}
