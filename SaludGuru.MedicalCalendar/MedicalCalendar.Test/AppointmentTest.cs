using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MedicalCalendar.Test
{
    [TestClass]
    public class AppointmentTest
    {
        [TestMethod]
        public void AppointmentGetByOfficeId()
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> result =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                ("91917194",
                new DateTime(2014, 1, 1),
                new DateTime(2014, 12, 31));

            Assert.AreEqual(true, result.Count > 0);
        }

        [TestMethod]
        public void AppointmentGetById()
        {
            MedicalCalendar.Manager.Models.Appointment.AppointmentModel result =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById("37245685");

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void AppointmentGetByOfficeIdMonth()
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentMonthModel> result =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeIdMonth("91917194", DateTime.Now);

            Assert.IsNotNull(result);

            Assert.AreEqual(true, result.Count > 0);
        }
    }
}
