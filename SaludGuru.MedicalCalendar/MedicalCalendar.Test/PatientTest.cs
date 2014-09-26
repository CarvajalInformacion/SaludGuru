using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MedicalCalendar.Manager.Models;

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

        [TestMethod]
        public void PatientGetByUserPublicId()
        {
            List<MedicalCalendar.Manager.Models.Patient.PatientModel> result = MedicalCalendar.Manager.Controller.Patient.MPPatientGetByUserPublicId("17B1EF7E");

            Assert.AreEqual(true, result.Count > 0);
        }

        [TestMethod]
        public void PatientGetFromIdentificationNumber()
        {
            MedicalCalendar.Manager.Models.Patient.PatientModel result =
                MedicalCalendar.Manager.Controller.Patient.PatientGetFromIdentificationNumber("B8C3B7F4", "1032461140");

            Assert.IsNotNull(result);
        }
    }
}
