using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Profile.Manager.Controller;
using Profile.Manager.Models;

namespace Profile.Test
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void CategoryGetAllAdmin()
        {
            Profile.Manager.Controller.Treatment.CategoryGetAllAdmin("Em");           
        }
    }
}
