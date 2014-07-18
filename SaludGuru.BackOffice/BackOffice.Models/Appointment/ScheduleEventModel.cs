using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ScheduleEventModel
    {
        #region FullCalendar Properties

        public string id { get { return CurrentAppointment.AppointmentPublicId; } }

        public string title
        {
            get
            {
                //get appointment title
                string AppointmentTitle = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_Appointment_TitleTemplate.Replace("{{StatusId}}", ((int)CurrentAppointment.Status).ToString())].Value;

                if (CurrentAppointment.RelatedPatient == null || CurrentAppointment.RelatedPatient.Count == 0)
                {
                    //no patients over appointment
                    AppointmentTitle = AppointmentTitle.
                        Replace("{AppointmentName}", "Esta cita no tiene pacientes.").
                        Replace("{AppointmentImage}",
                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Appointment_ImageEmpty].Value);
                }
                else if (CurrentAppointment.RelatedPatient.Count == 1)
                {
                    //one patient over appointment
                    AppointmentTitle = AppointmentTitle.
                        Replace("{AppointmentName}", CurrentAppointment.RelatedPatient.Select(x => x.Name + " " + x.LastName).FirstOrDefault()).
                        Replace("{AppointmentImage}",
                            CurrentAppointment.RelatedPatient.
                                Select(x => x.PatientInfo.
                                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.ProfileImage).
                                    Select(y => !string.IsNullOrEmpty(y.Value) ? y.Value :
                                                    BackOffice.Models.General.InternalSettings.Instance
                                                        [BackOffice.Models.General.Constants.C_Settings_PatientImage_Man].Value).
                                    DefaultIfEmpty(BackOffice.Models.General.InternalSettings.Instance
                                                        [BackOffice.Models.General.Constants.C_Settings_PatientImage_Man].Value).
                                    FirstOrDefault()).FirstOrDefault());
                }
                else if (CurrentAppointment.RelatedPatient.Count > 1)
                {
                    //more than one patient
                    AppointmentTitle = AppointmentTitle.
                        Replace("{AppointmentName}", "Esta cita tiene varios pacientes asociados.").
                        Replace("{AppointmentImage}",
                        BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Appointment_ImageGroup].Value);
                }
                return AppointmentTitle;
            }
        }

        public DateTime start { get { return CurrentAppointment.StartDate; } }

        public DateTime end { get { return CurrentAppointment.EndDate; } }

        public string className { get { return "AppointmentStatus_" + ((int)CurrentAppointment.Status).ToString(); } }

        public bool allDay { get { return false; } }

        #endregion

        #region Form Properties

        public string AppointmentPublicId { get { return CurrentAppointment.AppointmentPublicId; } }

        public string OfficePublicId { get { return CurrentAppointment.OfficePublicId; } }

        public int Duration { get { return (CurrentAppointment.EndDate - CurrentAppointment.StartDate).Minutes; } }

        public string StartDate { get { return CurrentAppointment.StartDate.ToString("dd/MM/yyyy"); } }

        public string StartTime { get { return CurrentAppointment.StartDate.ToString("hh:mm tt").Replace(".", "").ToUpper(); } }

        public string EndTime { get { return CurrentAppointment.EndDate.ToString("hh:mm tt").Replace(".", "").ToUpper(); } }

        public int TreatmentId
        {
            get
            {
                return CurrentAppointment.AppointmentInfo.
                    Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category).
                    Select(x => Convert.ToInt32(x.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public int AppointmentStatus { get { return (int)CurrentAppointment.Status; } }

        public List<PatientSearchModel> CurrentPatientInfo { get { return CurrentAppointment.RelatedPatient.Select(x => new PatientSearchModel(x)).ToList(); } }

        #endregion

        #region Related Appointment

        private AppointmentModel CurrentAppointment { get; set; }

        #endregion

        public ScheduleEventModel(AppointmentModel vCurrentAppointment)
        {
            CurrentAppointment = vCurrentAppointment;
        }

    }
}
