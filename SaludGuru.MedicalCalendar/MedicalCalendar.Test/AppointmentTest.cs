using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MedicalCalendar.Test
{
    [TestClass]
    public class AppointmentTest
    {
        [TestMethod]
        public void AppointmentGetByProfileId()
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> result =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                ("91917194", 
                new DateTime(2014, 1, 1), 
                new DateTime(2014, 12, 31));

            Assert.AreEqual(true, result.Count > 0);
        }
    }
}
