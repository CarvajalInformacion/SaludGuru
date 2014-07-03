using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MedicalCalendar.Test
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        public void PatientGetAllByPublicPatientId()
        {
            int TotalRows;
            List<MedicalCalendar.Manager.Models.Patient.PatientModel> result = MedicalCalendar.Manager.Controller.Patient.PatientSearch("205ECBD0", string.Empty, 0, 3, out  TotalRows);

            Assert.AreEqual(true, result.Count > 0);
        }
    }
}
