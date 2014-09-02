using BackOffice.Models.General;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using Message.Client.Models;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    [BackOffice.Web.Controllers.Filters.LogginActionFilter]
    public partial class BaseController : Controller
    {
        #region public static properties

        /// <summary>
        /// Current Area Name
        /// </summary>
        public static string AreaName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings[BackOffice.Models.General.Constants.C_AppSetting_AreaName].ToString().Trim();
            }
        }

        public static string CurrentControllerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentActionName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }

        #endregion

        #region Session methods

        public void LogOut()
        {
            SessionController.SessionManager.Logout();
            Response.Redirect(Url.Action(MVC.Home.ActionNames.Index, MVC.Home.Name));
        }

        public void ChangeCurrentProfile(string ProfilePublicId)
        {
            SessionModel.UserAutorization.All(x =>
            {
                if (string.IsNullOrEmpty(ProfilePublicId) && x.Role ==
                    SessionController.Models.Profile.enumRole.SystemAdministrator)
                {
                    x.Selected = true;
                }
                else if (x.ProfilePublicId == ProfilePublicId)
                {
                    x.Selected = true;
                }
                else
                {
                    x.Selected = false;
                }

                return true;
            });
        }

        #endregion

        #region Menu Methods

        public static List<MenuPrincipalModel> GetPrincipalMenu()
        {
            List<MenuPrincipalModel> oRetorno = new List<MenuPrincipalModel>();

            enumMenuPrincipal CurrentSelected = SelectedPrincipalMenu();

            //add default dasahboard
            oRetorno.Add(new MenuPrincipalModel()
            {
                Menu = enumMenuPrincipal.Dashboard,
                EditPermision = enumEditPermision.Read,
                IsSelected = (enumMenuPrincipal.Dashboard == CurrentSelected),
                RelatedMenu = GetSecundaryMenu(enumMenuPrincipal.Dashboard, enumEditPermision.Read),
            });

            //get all permited modules
            BackOffice.Models.General.InternalSettings.Instance[
                BackOffice.Models.General.Constants.C_Settings_PrincipalMenu.
                Replace("{{RoleId}}", ((int)SessionModel.CurrentUserAutorization.Role).ToString())].Value.
                Split(';').
                All(pm =>
                {
                    string[] pmDesc = pm.Replace(" ", "").Split(',');
                    if (pmDesc.Length == 2)
                    {
                        enumMenuPrincipal oCurrent = (enumMenuPrincipal)Enum.Parse(typeof(enumMenuPrincipal), pmDesc[0]);
                        enumEditPermision oPermision = (enumEditPermision)Enum.Parse(typeof(enumEditPermision), pmDesc[1]);

                        oRetorno.Add(new MenuPrincipalModel()
                        {
                            Menu = oCurrent,
                            EditPermision = oPermision,
                            IsSelected = (CurrentSelected == oCurrent),
                            RelatedMenu = GetSecundaryMenu(oCurrent, oPermision),
                        });
                    }
                    return true;
                });

            return oRetorno;
        }

        public static List<MenuProfileModel> GetProfileMenu()
        {
            List<MenuProfileModel> oRetorno = new List<MenuProfileModel>();

            enumMenuProfile CurrentSelected = SelectedProfileMenu();

            enumEditPermision CurrentPermision = GetPrincipalMenu().Where(x => x.IsSelected == true).Select(x => x.EditPermision).DefaultIfEmpty(enumEditPermision.Read).FirstOrDefault();

            foreach (enumMenuProfile CurrentMenu in (enumMenuProfile[])Enum.GetValues(typeof(enumMenuProfile)))
            {
                oRetorno.Add(new MenuProfileModel()
                {
                    Menu = CurrentMenu,
                    EditPermision = CurrentPermision,
                    IsSelected = (CurrentMenu == CurrentSelected),
                });
            }

            return oRetorno;
        }

        public static List<MenuOfficeModel> GetOfficeMenu()
        {
            List<MenuOfficeModel> oRetorno = new List<MenuOfficeModel>();

            enumMenuOffice CurrentSelected = SelectedOfficeMenu();

            enumEditPermision CurrentPermision = GetPrincipalMenu().Where(x => x.IsSelected == true).Select(x => x.EditPermision).DefaultIfEmpty(enumEditPermision.Read).FirstOrDefault();

            foreach (enumMenuOffice CurrentMenu in (enumMenuOffice[])Enum.GetValues(typeof(enumMenuOffice)))
            {
                oRetorno.Add(new MenuOfficeModel()
                {
                    Menu = CurrentMenu,
                    EditPermision = CurrentPermision,
                    IsSelected = (CurrentMenu == CurrentSelected),
                });
            }

            return oRetorno;
        }

        public static List<MenuPatientModel> GetPatientMenu()
        {
            List<MenuPatientModel> oRetorno = new List<MenuPatientModel>();

            enumMenuPatient CurrentSelected = SelectedPatientMenu();

            enumEditPermision CurrentPermision = GetPrincipalMenu().Where(x => x.IsSelected == true).Select(x => x.EditPermision).DefaultIfEmpty(enumEditPermision.Read).FirstOrDefault();

            foreach (enumMenuPatient CurrentMenu in (enumMenuPatient[])Enum.GetValues(typeof(enumMenuPatient)))
            {
                oRetorno.Add(new MenuPatientModel()
                {
                    Menu = CurrentMenu,
                    EditPermision = CurrentPermision,
                    IsSelected = (CurrentMenu == CurrentSelected),
                });
            }

            return oRetorno;
        }

        private static List<MenuSecundaryModel> GetSecundaryMenu(enumMenuPrincipal MenuToEval, enumEditPermision EditPermisions)
        {
            List<MenuSecundaryModel> oReturn = new List<MenuSecundaryModel>();

            enumMenuSecundary CurrentSelected = SelectedSecundaryMenu();

            BackOffice.Models.General.InternalSettings.Instance[
                BackOffice.Models.General.Constants.C_Settings_SecundaryMenu.
                Replace("{{MenuPrincipal}}", MenuToEval.ToString())].Value.
                Split(',').
                Where(x => !string.IsNullOrEmpty(x)).
                All(pm =>
                {
                    enumMenuSecundary oCurrent = (enumMenuSecundary)Enum.Parse(typeof(enumMenuSecundary), pm.Replace(" ", ""));

                    oReturn.Add(new MenuSecundaryModel()
                    {
                        Menu = oCurrent,
                        EditPermision = EditPermisions,
                        IsSelected = (CurrentSelected == oCurrent)
                    });

                    return true;
                });
            return oReturn;
        }

        private static enumMenuPrincipal SelectedPrincipalMenu()
        {
            enumMenuPrincipal oReturn = enumMenuPrincipal.Dashboard;

            if (MVC.Home.ActionNames.Dashboard == CurrentActionName &&
                MVC.Home.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Dashboard;
            }
            else if (MVC.Insurance.Name == CurrentControllerName ||
                        (MVC.Profile.Name == CurrentControllerName &&
                        MVC.Profile.ActionNames.ProfileSearch == CurrentActionName) ||
                    MVC.Specialty.Name == CurrentControllerName ||
                    MVC.Treatment.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Administrator;
            }
            else if (MVC.Profile.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Profile;
            }
            else if (MVC.Appointment.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Appointment;
            }
            else if (MVC.Patient.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Patient;
            }

            return oReturn;
        }

        private static enumMenuSecundary SelectedSecundaryMenu()
        {
            enumMenuSecundary oReturn = enumMenuSecundary.None;

            if (MVC.Insurance.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Insurance;
            }
            else if (MVC.Profile.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Profile;
            }
            else if (MVC.Specialty.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Specialty;
            }
            else if (MVC.Treatment.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Treatment;
            }


            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Day == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Day;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Week == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Week;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Month == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Month;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.List == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_List;
            }

            return oReturn;
        }

        private static enumMenuProfile SelectedProfileMenu()
        {
            enumMenuProfile oReturn = enumMenuProfile.ProfileInfo;

            if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileEdit == CurrentActionName)
            {
                oReturn = enumMenuProfile.ProfileInfo;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileEditImage == CurrentActionName)
            {
                oReturn = enumMenuProfile.ProfileImages;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.AutorizationProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Autorization;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                (MVC.Profile.ActionNames.OfficeList == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeUpsert == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeTreatmentList == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeTreatmentUpsert == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeScheduleAvailableList == CurrentActionName))
            {
                oReturn = enumMenuProfile.Office;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.SpecialtyProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Specialty;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.InsuranceProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Insurance;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.TreatmentProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Treatment;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileMessangerUpsert == CurrentActionName)
            {
                oReturn = enumMenuProfile.Comunicactions;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
               MVC.Profile.ActionNames.RelatedProfileSearch == CurrentActionName)
            {
                oReturn = enumMenuProfile.RelatedProfiles;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
              MVC.Profile.ActionNames.ProfileReminderUpsert == CurrentActionName)
            {
                oReturn = enumMenuProfile.Comunicactions;
            }
            return oReturn;
        }

        private static enumMenuOffice SelectedOfficeMenu()
        {
            enumMenuOffice oReturn = enumMenuOffice.OfficeInfo;

            if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.OfficeUpsert == CurrentActionName)
            {
                oReturn = enumMenuOffice.OfficeInfo;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.OfficeScheduleAvailableList == CurrentActionName)
            {
                oReturn = enumMenuOffice.ScheduleAvalilable;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                (MVC.Profile.ActionNames.OfficeTreatmentList == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeTreatmentUpsert == CurrentActionName))
            {
                oReturn = enumMenuOffice.Treatments;
            }
            return oReturn;
        }

        private static enumMenuPatient SelectedPatientMenu()
        {
            enumMenuPatient oReturn = enumMenuPatient.PatientInfo;

            if (MVC.Patient.Name == CurrentControllerName &&
                MVC.Patient.ActionNames.PatientUpsert == CurrentActionName)
            {
                oReturn = enumMenuPatient.PatientInfo;
            }
            else if (MVC.Patient.Name == CurrentControllerName &&
                MVC.Patient.ActionNames.AppointmentList == CurrentActionName)
            {
                oReturn = enumMenuPatient.Appointment;
            }
            else if (MVC.Patient.Name == CurrentControllerName &&
                MVC.Patient.ActionNames.PatientNotes == CurrentActionName)
            {
                oReturn = enumMenuPatient.PatientNotes;
            }
            return oReturn;
        }

        public static List<MenuComunicationModel> GetComunicationMenu()
        {
            List<MenuComunicationModel> oRetorno = new List<MenuComunicationModel>();

            enumMenuComunications CurrentSelected = SelectedComunicationMenu();

            enumEditPermision CurrentPermision = GetPrincipalMenu().Where(x => x.IsSelected == true).Select(x => x.EditPermision).DefaultIfEmpty(enumEditPermision.Read).FirstOrDefault();

            foreach (enumMenuComunications CurrentMenu in (enumMenuComunications[])Enum.GetValues(typeof(enumMenuComunications)))
            {
                oRetorno.Add(new MenuComunicationModel()
                {
                    Menu = CurrentMenu,
                    EditPermision = CurrentPermision,
                    IsSelected = (CurrentMenu == CurrentSelected),
                });
            }

            return oRetorno;
        }

        private static enumMenuComunications SelectedComunicationMenu()
        {
            enumMenuComunications oReturn = enumMenuComunications.Messaging;

            if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileMessangerUpsert == CurrentActionName)
            {
                oReturn = enumMenuComunications.Messaging;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileReminderUpsert == CurrentActionName)
            {
                oReturn = enumMenuComunications.Reminders;
            }

            return oReturn;
        }

        #endregion

        #region Messenger

        public static bool SendMessage(ProfileModel Profile, enumProfileInfoType MessageType, List<PatientModel> PatientList, AppointmentModel AppointmentInfo, bool isMp)
        {
            #region Varibles Locales
            CreateMessageResponse result = new CreateMessageResponse();
            CreateMessageRequest oMessage = new CreateMessageRequest();
            OfficeModel CurrentOffice = new OfficeModel();

            #endregion
            oMessage.NewMessage = new ClientMessageModel();
            oMessage.NewMessage.CreateDate = DateTime.Now;
            //Load the message type to send 
            List<enumMessageType> messageTypeList = new List<enumMessageType>();
            messageTypeList = Profile.ProfileInfo.Where(x => x.ProfileInfoType == MessageType).Select(x => (enumMessageType)Convert.ToInt32(x.Value)).ToList();

            //Load the office info
            CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(AppointmentInfo.OfficePublicId);

            //valid the type message with the enumeration             
            foreach (enumMessageType mType in messageTypeList)
            {
                foreach (PatientModel item in PatientList)
                {
                    bool isPatientEmail = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.SendEmail).Select(x => Convert.ToBoolean(x.Value)).FirstOrDefault();
                    bool isPatientSms = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.SendSMS).Select(x => Convert.ToBoolean(x.Value)).FirstOrDefault();
                    if (mType == enumMessageType.Email && isPatientEmail)
                    {
                        oMessage.NewMessage.RelatedParameter = new List<ClientMessageParameter>();
                        oMessage.NewMessage.UserAction = BackOffice.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                        switch (MessageType)
                        {
                            case enumProfileInfoType.AsignedAppointment:
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "BeforeCare", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.BeforeCare).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                break;
                            case enumProfileInfoType.CancelAppointment:
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Reason", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.CancelAppointementReason).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ReescheduleLink", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "SendToProcessRelatedMsj", Value = "true" });
                                break;
                            case enumProfileInfoType.ModifyAppointment:
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                break;
                            case enumProfileInfoType.ReminderAppointment:
                                int ProgrameTime = Profile.ProfileInfo.Where(x => x.ProfileInfoType == MessageType).Select(x => x.LargeValue != "0" ? Convert.ToInt32(x.LargeValue) : 0).FirstOrDefault();
                                DateTime ApointmentDate = AppointmentInfo.StartDate.AddHours(ProgrameTime * -1);

                                oMessage.NewMessage.ProgramTime = ApointmentDate;
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentPublicId", Value = AppointmentInfo.AppointmentPublicId });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "isReminder", Value = "true" });
                                break;
                            case enumProfileInfoType.ReminderNextAppointment:

                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.ProgramTime = AppointmentInfo.StartDate;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "isReminder", Value = "true" });
                                break;
                        }
                        if (mType == enumMessageType.Email)
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Email).Select(x => x.Value).FirstOrDefault() });
                        if (mType == enumMessageType.Sms)
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Mobile).Select(x => x.Value).FirstOrDefault() });

                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = item.Name });
                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfilePublicId", Value = Profile.ProfilePublicId });
                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientPublicId", Value = item.PatientPublicId});
                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppPublicId", Value = AppointmentInfo.AppointmentPublicId });

                        //Valid the key "To"
                        string keyTO = oMessage.NewMessage.RelatedParameter.Where(x => x.Key == "TO").Select(x => x.Value).FirstOrDefault();
                        if (keyTO != null)
                            result = Message.Client.Client.Instance.CreateMessage(oMessage);
                        else
                            result.IsSuccess = false;
                    }
                    //Valid the conditions
                    if (mType == enumMessageType.Sms && isPatientSms)
                    {
                        oMessage.NewMessage.RelatedParameter = new List<ClientMessageParameter>();
                        oMessage.NewMessage.UserAction = BackOffice.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                        switch (MessageType)
                        {
                            case enumProfileInfoType.AsignedAppointment:
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "BeforeCare", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.BeforeCare).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                break;
                            case enumProfileInfoType.CancelAppointment:
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Reason", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.CancelAppointementReason).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ReescheduleLink", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                break;
                            case enumProfileInfoType.ModifyAppointment:
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.ProgramTime = DateTime.Now;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                break;
                            case enumProfileInfoType.ReminderAppointment:
                                int ProgrameTime = Profile.ProfileInfo.Where(x => x.ProfileInfoType == MessageType).Select(x => x.LargeValue != "0" ? Convert.ToInt32(x.LargeValue) : 0).FirstOrDefault();
                                DateTime ApointmentDate = AppointmentInfo.StartDate.AddHours(ProgrameTime * -1);

                                oMessage.NewMessage.ProgramTime = ApointmentDate;
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ConfirmCancelLink", Value = AppointmentInfo.AppointmentPublicId });
                                break;


                            case enumProfileInfoType.ReminderNextAppointment:

                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                oMessage.NewMessage.ProgramTime = AppointmentInfo.StartDate;
                                oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileUrl", Value = BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Profile_MarketPlaceUrl].Value.Replace("{ProfileName}", RemoveAccent(Profile.Name + " " + Profile.LastName)).Replace("{ProfilePublicId}", Profile.ProfilePublicId).Replace("{SpecialtyName}", !string.IsNullOrEmpty(Profile.DefaultSpecialty.Name) ? Profile.DefaultSpecialty.Name : string.Empty) });
                                break;
                        }
                        if (mType == enumMessageType.Email)
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Email).Select(x => x.Value).FirstOrDefault() });
                        if (mType == enumMessageType.Sms)
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Mobile).Select(x => x.Value).FirstOrDefault() });

                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = item.Name });
                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "LastName", Value = item.LastName });
                        oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientPublicId", Value = item.PatientPublicId });
                        //Valid the key "To"
                        string keyTO = oMessage.NewMessage.RelatedParameter.Where(x => x.Key == "TO").Select(x => x.Value).FirstOrDefault();
                        if (keyTO != null)
                            result = Message.Client.Client.Instance.CreateMessage(oMessage);
                        else
                            result.IsSuccess = false;
                    }
                }
            }
            return result.IsSuccess;
        }

        #endregion

        #region Url Methods

        private static Dictionary<char, char> oReplaceChar;
        public static Dictionary<char, char> ReplaceChar
        {
            get
            {
                if (oReplaceChar == null)
                {
                    oReplaceChar = new Dictionary<char, char>();
                    BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_Url_Invalid_Char].Value.
                        Split(';').All(ch =>
                        {
                            char okey = char.Parse(ch.Split(',')[0]);
                            char oval = char.Parse(ch.Split(',')[1].Replace("\\0", "\0"));
                            oReplaceChar[okey] = oval;
                            return true;
                        });
                }

                return oReplaceChar;
            }
        }

        public static string RemoveAccent(string strToEval)
        {
            string oReturn = strToEval.Trim().ToLower();

            ReplaceChar.All(rc =>
            {
                oReturn = oReturn.Replace(rc.Key, rc.Value);
                return true;
            });

            oReturn = string.Join("+", oReturn.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            return oReturn;
        }

        #endregion
    }
}