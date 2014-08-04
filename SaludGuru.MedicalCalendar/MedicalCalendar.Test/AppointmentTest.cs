using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MedicalCalendar.Test
{
    [TestClass]
    public class AppointmentTest
    {
        [TestMethod]
        public void GetSpecialDays()
        {
            List<MedicalCalendar.Manager.Models.General.SpecialDayModel> result =
                MedicalCalendar.Manager.Controller.Appointment.GetSpecialDays
                (1,
                "2C1D2510",
                new DateTime(2014, 1, 1),
                new DateTime(2014, 12, 31));

            Assert.AreEqual(true, result.Count > 0);
        }

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
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById("3724568H");

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

        [TestMethod]
        public void AppointmentGetByPatient()
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> result =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByPatient("1A7E3690");

            Assert.AreEqual(true, result.Count > 0);
        }

        [TestMethod]
        public void MPAppointmentGetByOfficeId()
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> result =
                MedicalCalendar.Manager.Controller.Appointment.MPAppointmentGetByOfficeId
                ("91917194",
                new DateTime(2014, 1, 1),
                new DateTime(2014, 12, 31));

            Assert.AreEqual(true, result.Count > 0);
        }
    }
}
