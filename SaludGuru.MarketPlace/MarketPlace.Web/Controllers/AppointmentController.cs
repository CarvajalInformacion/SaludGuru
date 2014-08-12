using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class AppointmentController : BaseController
    {
        public virtual ActionResult Index(string ProfilePublicId, string OfficePublicId, string Date)
        {
            AppointmentViewModel oModel = new AppointmentViewModel()
            {
                CurrentDate = string.IsNullOrEmpty(Date) ? DateTime.Now : DateTime.ParseExact(Date.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture),
                CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
                PatientGroup = MedicalCalendar.Manager.Controller.Patient.MPPatientGetByUserPublicId("17B1EF7E"), //TODO: Ajustar el usuario no quemarlo
                CurrentOffice = OfficePublicId,
                StartDate = Date
            };
            return View(oModel);
        }

        public virtual ActionResult ConfirmationAppointment(string ProfilePublicId)
        {
            ProfileModel oModel = new ProfileModel();
            string NewAppointmentPublicId = string.Empty;
            bool SendNotifications = false;
            List<PatientModel> patientToRemove = new List<PatientModel>();
            oModel = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId);

            AppointmentModel appToCreate = new AppointmentModel();
            appToCreate = this.GetAppointmetRequest(oModel);
            NewAppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo(appToCreate, patientToRemove);

            if (SendNotifications)
            {
                //TODO: send message new patient PatientNew

                //TODO: send message removed patient
            }

            OfficeModel office = new OfficeModel();
            //office = SaludGuruProfile.Manager.Controller.Office.

            return View(appToCreate);
        }

        #region Private Functions
        private AppointmentModel GetAppointmetRequest(ProfileModel CurrentProfileModel)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
            && bool.Parse(Request["UpsertAction"]))
            {
                OfficeModel SelectedOfficeModel = CurrentProfileModel.RelatedOffice.Where(x => x.OfficePublicId == Request["SelectedOffice"].ToString()).Select(x => x).FirstOrDefault();
                TreatmentOfficeModel TreatmentSelected = SelectedOfficeModel.RelatedTreatment.Where(x => x.CategoryId == Convert.ToInt32(Request["SelectedTreatment"])).FirstOrDefault();
                string DurationDate = TreatmentSelected.TreatmentOfficeInfo.Where(x => x.OfficeCategoryInfoType == enumOfficeCategoryInfoType.DurationTime).Select(x => x.Value).FirstOrDefault();

                AppointmentModel oReturn = new AppointmentModel()
                {
                    OfficePublicId = Request["SelectedOffice"].ToString(),
                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New,
                    StartDate = Convert.ToDateTime(Request["StartDate"]),
                    EndDate = Convert.ToDateTime(Request["StartDate"]).AddMinutes(Convert.ToInt32(DurationDate)),
                    AppointmentInfo = new List<AppointmentInfoModel>(),
                    RelatedPatient = new List<PatientModel>(),
                };
                //treatment
                int oTreatmentId = Convert.ToInt32(Request["SelectedTreatment"].Replace(" ", ""));
                oReturn.AppointmentInfo.Add
                    (new AppointmentInfoModel()
                    {
                        AppointmentInfoId = 0,
                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category,
                        Value = oTreatmentId.ToString(),
                    });

                //after care
                oReturn.AppointmentInfo.Add
                    (new AppointmentInfoModel()
                    {
                        AppointmentInfoId = 0,
                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
                        LargeValue = TreatmentSelected.TreatmentOfficeInfo.Where(x => x.OfficeCategoryInfoType == enumOfficeCategoryInfoType.AfterCare).Select(x => x.Value).FirstOrDefault(),
                    });

                //before care
                oReturn.AppointmentInfo.Add
                    (new AppointmentInfoModel()
                    {
                        AppointmentInfoId = 0,
                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
                        LargeValue = TreatmentSelected.TreatmentOfficeInfo.Where(x => x.OfficeCategoryInfoType == enumOfficeCategoryInfoType.BeforeCare).Select(x => x.Value).FirstOrDefault(),
                    });

                //get patient to add
                oReturn.RelatedPatient = Request["SelectedPatientItem"].Split(',').
                    Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length == 8).
                    Select(x => new PatientModel()
                    {
                        PatientPublicId = x.Replace(" ", "")
                    }).ToList();

                return oReturn;
            }
            return null;
        }
        #endregion
    }
}