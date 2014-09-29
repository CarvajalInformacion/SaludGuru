using MarketPlace.Models.General;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using Message.Client.Models;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    [MarketPlace.Web.Controllers.Filters.MobileActionFilter]
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
                return System.Configuration.ConfigurationManager.AppSettings[MarketPlace.Models.General.Constants.C_AppSetting_AreaName].ToString().Trim();
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

        private static Dictionary<int, string> oEnabledCities;
        public static Dictionary<int, string> EnabledCities
        {
            get
            {
                if (oEnabledCities == null)
                {
                    oEnabledCities = MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_City_Cities].
                        Value.
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        ToDictionary
                            (k => Convert.ToInt32(k.Split(',')[0].Trim()),
                            v => v.Split(',')[1].Trim());
                }
                return oEnabledCities;
            }
        }

        private static Dictionary<int, int> oCityIndicative;
        public static Dictionary<int, int> CityIndicative
        {
            get
            {
                if (oCityIndicative == null)
                {
                    oCityIndicative = MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_City_Indicative].Value.
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        ToDictionary
                            (k => Convert.ToInt32(k.Split(',')[0].Trim()),
                            v => Convert.ToInt32(v.Split(',')[1].Trim()));
                }
                return oCityIndicative;


            }
        }

        public static int DefaultCityId
        {
            get { return Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_City_Default].Value); }
        }

        public static string DefaultCityName
        {
            get { return EnabledCities[Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_City_Default].Value)]; }
        }

        private CookieModel oCurrentCookie;
        public CookieModel CurrentCookie
        {
            get
            {
                if (oCurrentCookie == null)
                {
                    oCurrentCookie = GetCookie();
                }
                return oCurrentCookie;
            }
        }

        public string CurrentDomainUrl
        {
            get
            {
                if (Request.Url.PathAndQuery == "/")
                    return Request.Url.ToString().TrimEnd('/');
                else
                    return Request.Url.ToString().Replace(Request.Url.PathAndQuery, "").TrimEnd('/');
            }
        }

        #endregion

        #region Cookie methods

        //save cookie over request
        public void SetCookie(CookieModel CookieToUpdate)
        {
            try
            {
                string strCookieKey = MarketPlace.Models.General.Constants.C_Cookie_CookieKey;
                string strCookieValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(CookieToUpdate);

                if (Request.Cookies.AllKeys.Any(x => x == strCookieKey))
                {
                    Request.Cookies.Remove(strCookieKey);
                }
                this.ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie(strCookieKey, strCookieValue));

                oCurrentCookie = CookieToUpdate;
            }
            catch { }
        }

        public CookieModel GetCookie()
        {
            CookieModel oReturn;

            string strCookieKey = MarketPlace.Models.General.Constants.C_Cookie_CookieKey;

            if (!Request.Cookies.AllKeys.Any(x => x == strCookieKey))
            {
                oReturn = new CookieModel()
                {
                    CurrentCity = EnabledCities.FirstOrDefault().Key,
                };

                SetCookie(oReturn);
            }
            else
            {
                oReturn = (CookieModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                                    Deserialize(Request.Cookies[strCookieKey].Value, typeof(CookieModel));
            }

            return oReturn;
        }

        #endregion

        #region Session methods

        public void LogOut()
        {
            SessionController.SessionManager.Logout();
            Response.Redirect(Url.Action(MVC.Home.ActionNames.Index, MVC.Home.Name));
        }

        #endregion

        #region Messenger

        public static bool SendMessage(ProfileModel Profile, enumProfileInfoType? MessageType, List<PatientModel> PatientList, AppointmentModel AppointmentInfo, bool isNewPatient)
        {
            #region Varibles Locales
            CreateMessageResponse result = new CreateMessageResponse();
            CreateMessageRequest oMessage = new CreateMessageRequest();
            OfficeModel CurrentOffice = new OfficeModel();
            PatientModel CurrentPatient = new PatientModel();
            List<enumMessageType> messageTypeList = new List<enumMessageType>();
            #endregion
            oMessage.NewMessage = new ClientMessageModel();
            oMessage.NewMessage.CreateDate = DateTime.Now;
            if (!isNewPatient)
            {
                //Get info params to send de msj            
                CurrentPatient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserPublicId.ToString());

                //Load the message type to send 
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
                            oMessage.NewMessage.UserAction = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                            switch (MessageType)
                            {
                                case enumProfileInfoType.AsignedAppointment:
                                    oMessage.NewMessage.ProgramTime = DateTime.Now;
                                    oMessage.NewMessage.MessageType = mType + "_" + "MP" + MessageType.ToString();
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "BeforeCare", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.BeforeCare).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentPublicId", Value = AppointmentInfo.AppointmentPublicId });
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
                            }
                            if (mType == enumMessageType.Email)
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Email).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : "No Email").FirstOrDefault() });
                            if (mType == enumMessageType.Sms)
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Mobile).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : "0").FirstOrDefault() });

                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = item.Name });
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "LastName", Value = item.LastName });
                            result = Message.Client.Client.Instance.CreateMessage(oMessage);
                        }
                        if (mType == enumMessageType.Sms && isPatientSms)
                        {
                            oMessage.NewMessage.RelatedParameter = new List<ClientMessageParameter>();
                            oMessage.NewMessage.UserAction = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                            switch (MessageType)
                            {
                                case enumProfileInfoType.AsignedAppointment:
                                    oMessage.NewMessage.ProgramTime = DateTime.Now;
                                    oMessage.NewMessage.MessageType = mType + "_" + "MP" + MessageType.ToString();
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "BeforeCare", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.BeforeCare).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentPublicId", Value = AppointmentInfo.AppointmentPublicId });
                                    break;
                                case enumProfileInfoType.ReminderAppointment:
                                    int ProgrameTime = Profile.ProfileInfo.Where(x => x.ProfileInfoType == MessageType).Select(x => x.LargeValue != "0" ? Convert.ToInt32(x.LargeValue) : 0).FirstOrDefault();
                                    DateTime ApointmentDate = AppointmentInfo.StartDate.AddHours(ProgrameTime * -1);

                                    oMessage.NewMessage.ProgramTime = ApointmentDate;
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                    oMessage.NewMessage.MessageType = mType + "_" + MessageType.ToString();
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.Name + " " + Profile.LastName });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileLastName", Value = Profile.LastName });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ConfirmCancelLink", Value = AppointmentInfo.AppointmentPublicId });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentPublicId", Value = AppointmentInfo.AppointmentPublicId });
                                    break;
                            }
                            if (mType == enumMessageType.Email)
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Email).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : "No Email").FirstOrDefault() });
                            if (mType == enumMessageType.Sms)
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = item.PatientInfo.Where(x => x.PatientInfoType == enumPatientInfoType.Mobile).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : "0").FirstOrDefault() });

                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = item.Name });
                            oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "LastName", Value = item.LastName });
                            result = Message.Client.Client.Instance.CreateMessage(oMessage);
                        }
                        if (mType == enumMessageType.GuruNotification)
                        {
                            oMessage.NewMessage.RelatedParameter = new List<ClientMessageParameter>();
                            oMessage.NewMessage.UserAction = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                            switch (MessageType)
                            {
                                case enumProfileInfoType.AsignedAppointment:
                                    oMessage.NewMessage.ProgramTime = DateTime.Now;
                                    oMessage.NewMessage.MessageType = mType + "_" + "MP" + MessageType.ToString();
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = item.Name });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentDate", Value = RemoveAccent(AppointmentInfo.StartDate.ToString("ddd d MMM", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co"))).Replace("+", " ") });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Hour", Value = AppointmentInfo.StartDate.ToString("hh:mm tt", System.Globalization.CultureInfo.CreateSpecificCulture("ES-co")) });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficePhone", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Telephone).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "OfficeAddress", Value = CurrentOffice.OfficeInfo.Where(x => x.OfficeInfoType == enumOfficeInfoType.Address).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "BeforeCare", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.BeforeCare).Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "AppointmentPublicId", Value = AppointmentInfo.AppointmentPublicId });
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
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Reason", Value = AppointmentInfo.AppointmentInfo.Where(x => x.AppointmentInfoType == enumAppointmentInfoType.CancelAppointementReason).Select(x => x.Value).FirstOrDefault() });
                                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "SendToProcessRelatedMsj", Value = "true" });
                                    break;
                            }

                            List<string> emailRelatedList = new List<string>();
                            List<ProfileAutorizationModel> ProfileAutorizationModelList = new List<ProfileAutorizationModel>();
                            ProfileAutorizationModelList = SaludGuruProfile.Manager.Controller.Profile.GetProfileAutorization(Profile.ProfilePublicId);

                            if (ProfileAutorizationModelList != null)
                            {
                                //Find the autorized 
                                emailRelatedList = ProfileAutorizationModelList.Where(x => x.UserEmail != null).Select(x => x.UserEmail).ToList();
                                string autorizedEmail = string.Join(",", emailRelatedList);
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = string.Join(",", Auth.Client.Controller.Client.GetUserListByEmailList(autorizedEmail).Where(x => x.UserPublicId != null).Select(x => x.UserPublicId)) });

                                //crear el to separad por coma con los usuarios autorizados por ese perfil
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "From", Value = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserPublicId.ToString() });

                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = item.Name });
                                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "LastName", Value = item.LastName });
                                result = Message.Client.Client.Instance.CreateMessage(oMessage);
                            }
                        }
                    }
                }
            }
            else
            {
                oMessage.NewMessage.RelatedParameter = new List<ClientMessageParameter>();
                oMessage.NewMessage.UserAction = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserId.ToString();
                oMessage.NewMessage.ProgramTime = DateTime.Now;
                oMessage.NewMessage.MessageType = "GuruNotification_NewPatient";

                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "PatientName", Value = PatientList.FirstOrDefault().Name + PatientList.FirstOrDefault().LastName });
                oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "ProfileName", Value = Profile.LastName });

                List<string> emailRelatedList = new List<string>();
                List<ProfileAutorizationModel> ProfileAutorizationModelList = new List<ProfileAutorizationModel>();
                ProfileAutorizationModelList = SaludGuruProfile.Manager.Controller.Profile.GetProfileAutorization(Profile.ProfilePublicId);

                if (ProfileAutorizationModelList != null)
                {
                    //Find the autorized 
                    emailRelatedList = ProfileAutorizationModelList.Where(x => x.UserEmail != null).Select(x => x.UserEmail).ToList();
                    string autorizedEmail = string.Join(",", emailRelatedList);
                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "TO", Value = string.Join(",", Auth.Client.Controller.Client.GetUserListByEmailList(autorizedEmail).Where(x => x.UserPublicId != null).Select(x => x.UserPublicId)) });
                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "From", Value = MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserPublicId.ToString() });
                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "Name", Value = PatientList.FirstOrDefault().Name });
                    oMessage.NewMessage.RelatedParameter.Add(new ClientMessageParameter() { Key = "LastName", Value = PatientList.FirstOrDefault().LastName });
                    result = Message.Client.Client.Instance.CreateMessage(oMessage);
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
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_Invalid_Char].Value.
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