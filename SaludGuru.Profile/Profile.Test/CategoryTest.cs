using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;

namespace Profile.Test
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void CategoryGetAllAdmin()
        {
            SaludGuruProfile.Manager.Controller.Treatment.CategoryGetAllAdmin("Em");           
        }
    }
}
