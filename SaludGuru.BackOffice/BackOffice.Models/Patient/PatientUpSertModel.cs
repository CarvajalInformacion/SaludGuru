﻿using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Patient
{
    public class PatientUpSertModel
    {
        public PatientModel Patient { get; set; }

        public List<ItemModel> PatientOptions { get; set; }

        public List<AppointmentModel> RelatedAppointment { get; set; }

        public List<SaludGuruProfile.Manager.Models.General.InsuranceModel> Insurance { get; set; }

        public List<PatientInfoModel> Notes
        {
            get
            {
                return Patient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.DoctorNotes).
                    ToList();
            }
        }
    }
}
