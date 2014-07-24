using BackOffice.Models.Appointment;
using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class PatientApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<PatientSearchModel> PatientSearch
            (string SearchCriteria, int PageNumber, int RowCount)
        {
            int oTotalCount;
            List<MedicalCalendar.Manager.Models.Patient.PatientModel> SearchPatient =
                MedicalCalendar.Manager.Controller.Patient.PatientSearch
                (BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId,
                SearchCriteria == null ? string.Empty : SearchCriteria,
                PageNumber,
                RowCount,
                out oTotalCount);

            if (SearchPatient != null && SearchPatient.Count > 0)
            {
                List<BackOffice.Models.Patient.PatientSearchModel> oReturn = SearchPatient.
                    Select(x => new BackOffice.Models.Patient.PatientSearchModel(x)
                    {
                        SearchPatientCount = oTotalCount,
                    }).ToList();

                return oReturn;
            }
            else
            {
                return new List<Models.Patient.PatientSearchModel>();
            }
        }

        [HttpPost]
        [HttpGet]
        public PatientSearchModel AddPatientDoctorNote
            (string PatientPublicId)
        {
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DoctorNote"]))
            {
                MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfoItem
                    (new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = MedicalCalendar.Manager.Models.enumPatientInfoType.DoctorNotes,
                            LargeValue = System.Web.HttpContext.Current.Request["DoctorNote"].Trim(),
                        },
                    PatientPublicId);
            }
            return new Models.Patient.PatientSearchModel(
                MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId));
        }

        [HttpPost]
        [HttpGet]
        public PatientSearchModel DeletePatientDoctorNote(string PatientPublicId, string PatientInfoId)
        {
            MedicalCalendar.Manager.Controller.Patient.DeletePatientInfoItem
                (new PatientInfoModel()
                {
                    PatientInfoId = Convert.ToInt32(PatientInfoId),
                });

            return new Models.Patient.PatientSearchModel(
                            MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId));
        }

        [HttpPost]
        [HttpGet]
        public List<ScheduleEventModel> GetAppoinmentByPatient(string PatientPublicId, int Quantity)
        {
            List<AppointmentModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByPatient
                (PatientPublicId);
            if (lstAppointment != null)
            {
                if (Quantity > 0)
                {
                    lstAppointment = lstAppointment.OrderByDescending(x => x.StartDate).Take(Quantity).ToList();
                }

                lstAppointment.All(x =>
                {
                    x.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, x.StartDate.Hour, x.StartDate.Minute, 0);
                    x.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, x.EndDate.Hour, x.EndDate.Minute, 0);
                    return true;
                });

                return lstAppointment.Select(x => new ScheduleEventModel(x)).ToList();
            }
            else
                return new List<ScheduleEventModel>();
        }
    }
}