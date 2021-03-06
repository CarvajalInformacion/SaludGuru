﻿using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models.Appointment;
using SaludGuruProfile.Manager.Models.Office;
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

                AppointmentTitle = AppointmentTitle.
                                        Replace("{AppointmentName}", AppointmentText).
                                        Replace("{AppointmentImage}", AppointmentImage);

                return AppointmentTitle;
            }
        }

        public DateTime start { get { return CurrentAppointment.StartDate; } }

        public DateTime end { get { return CurrentAppointment.EndDate; } }

        public string className { get { return "AppointmentStatus_" + ((int)CurrentAppointment.Status).ToString(); } }

        public bool allDay { get { return false; } }

        #endregion

        #region Appointment patient

        public string AppointmentImage
        {
            get
            {
                if (CurrentAppointment.RelatedPatient == null || CurrentAppointment.RelatedPatient.Count == 0)
                {
                    //no patients over appointment
                    return BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Appointment_ImageEmpty].Value;
                }
                else if (CurrentAppointment.RelatedPatient.Count == 1)
                {
                    //one patient over appointment
                    return CurrentAppointment.RelatedPatient.
                                Select(x => x.PatientInfo.
                                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.ProfileImage).
                                    Select(y => !string.IsNullOrEmpty(y.Value) ? y.Value :
                                                    BackOffice.Models.General.InternalSettings.Instance
                                                        [BackOffice.Models.General.Constants.C_Settings_PatientImage_Man].Value).
                                    DefaultIfEmpty(BackOffice.Models.General.InternalSettings.Instance
                                                        [BackOffice.Models.General.Constants.C_Settings_PatientImage_Man].Value).
                                    FirstOrDefault()).FirstOrDefault();
                }
                else if (CurrentAppointment.RelatedPatient.Count > 1)
                {
                    //more than one patient
                    return BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Appointment_ImageGroup].Value;
                }
                return string.Empty;
            }
        }

        public string AppointmentText
        {
            get
            {
                if (CurrentAppointment.RelatedPatient == null || CurrentAppointment.RelatedPatient.Count == 0)
                {
                    //no patients over appointment
                    return "Esta cita no tiene pacientes - " + TreatmentName + ".";
                }
                else if (CurrentAppointment.RelatedPatient.Count == 1)
                {
                    //one patient over appointment
                    return CurrentAppointment.RelatedPatient.Select(x => x.Name + " " + x.LastName + " - " + TreatmentName + ".").FirstOrDefault();
                }
                else if (CurrentAppointment.RelatedPatient.Count > 1)
                {
                    //more than one patient
                    return "Esta cita tiene varios pacientes asociados - " + TreatmentName + ".";
                }
                return string.Empty;
            }
        }

        public string AppointmentDateText
        {
            get
            {
                return CurrentAppointment.StartDate.ToString("dd \\de MMMM hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) +
                    CurrentAppointment.EndDate.ToString(" - hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"));
            }
        }

        public string OfficeName { get { return CurrentAppointment.OfficeName; } }

        public string StatusName
        {
            get
            {
                if (string.IsNullOrEmpty(AppointmentPublicId))
                {
                    return BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_Appointment_StatusName_New].Value;
                }
                else
                {
                    return BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_Appointment_StatusName.
                        Replace("{{StatusId}}", ((int)CurrentAppointment.Status).ToString())].Value;
                }
            }
        }

        #endregion

        #region Form Properties

        public string AppointmentPublicId { get { return CurrentAppointment.AppointmentPublicId; } }

        public string OfficePublicId { get { return CurrentAppointment.OfficePublicId; } }

        public int Duration { get { return (int)(CurrentAppointment.EndDate - CurrentAppointment.StartDate).TotalMinutes; } }

        public string StartDate { get { return CurrentAppointment.StartDate.ToString("dd/MM/yyyy"); } }

        public string StartTime { get { return CurrentAppointment.StartDate.ToString("hh:mm tt").Replace(".", "").ToUpper(); } }

        public string EndTime { get { return CurrentAppointment.EndDate.ToString("hh:mm tt").Replace(".", "").ToUpper(); } }

        public int TreatmentId
        {
            get
            {
                return CurrentAppointment.AppointmentInfo.
                    Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category &&
                            !string.IsNullOrEmpty(x.Value)).
                    Select(x => Convert.ToInt32(x.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
            }
        }

        public string TreatmentName
        {
            get
            {
                string strTreatmentName = string.Empty;
                if (CurrentOffice != null)
                {
                    strTreatmentName = CurrentOffice.RelatedTreatment.
                        Where(x => x.CategoryId == TreatmentId).
                        Select(x => string.IsNullOrEmpty(x.Name) ? string.Empty : x.Name).
                        DefaultIfEmpty("").
                        FirstOrDefault();
                }
                return strTreatmentName;
            }
        }

        public int AppointmentStatus { get { return (int)CurrentAppointment.Status; } }

        public List<PatientSearchModel> CurrentPatientInfo { get { return CurrentAppointment.RelatedPatient != null ? CurrentAppointment.RelatedPatient.Select(x => new PatientSearchModel(x)).ToList() : new List<PatientSearchModel>(); } }

        public string AfterCare { get { return CurrentAppointment.AppointmentInfo.Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare).Select(x => x.LargeValue).DefaultIfEmpty(string.Empty).FirstOrDefault(); } }

        public string BeforeCare { get { return CurrentAppointment.AppointmentInfo.Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare).Select(x => x.LargeValue).DefaultIfEmpty(string.Empty).FirstOrDefault(); } }

        public string AppointmentNote { get { return CurrentAppointment.AppointmentInfo.Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.AppointmentNote).Select(x => x.LargeValue).DefaultIfEmpty(string.Empty).FirstOrDefault(); } }

        public AppointmentInfoModel AppointmentCancelReason { get { return CurrentAppointment.AppointmentInfo.Any(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.CancelAppointementReason) ? CurrentAppointment.AppointmentInfo.Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.CancelAppointementReason).FirstOrDefault() : null; } }

        #endregion

        #region Related Appointment

        private AppointmentModel CurrentAppointment { get; set; }

        private OfficeModel CurrentOffice { get; set; }

        #endregion

        public ScheduleEventModel(AppointmentModel vCurrentAppointment, OfficeModel vCurrentOffice)
        {
            CurrentAppointment = vCurrentAppointment;
            CurrentOffice = vCurrentOffice;
        }

    }
}
