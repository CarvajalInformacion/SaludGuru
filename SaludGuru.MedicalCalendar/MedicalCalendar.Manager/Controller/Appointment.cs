using MedicalCalendar.Manager.DAL.Controller;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Controller
{
    public class Appointment
    {
        public static List<SpecialDayModel> GetSpecialDays
            (int CountryId, string ProfilePublicId, DateTime StartDate)
        {
            List<SpecialDayModel> oReturn = new List<SpecialDayModel>();

            //get special holiday
            oReturn.AddRange(Holiday.GetByCountry(CountryId).Select(x =>
                new SpecialDayModel()
                {
                    SpecialDayType = Models.enumSpecialDayType.Holiday,
                    SpecialDay = x.Day,
                }).ToList());

            //get not available days for profile

            List<SpecialDayModel> bm = MedicalCalendarDataController.Instance.GetScheduleBusy
                (ProfilePublicId, null, StartDate, null, null).
                Where(x => x.MaxFreeTime.Seconds == 0).
                GroupBy(x => new
                {
                    SpecialDayType = Models.enumSpecialDayType.NotAvailable,
                    SpecialDay = x.EvalDate
                }).
                Select(x => new SpecialDayModel()
                {
                    SpecialDay = x.Key.SpecialDay,
                    SpecialDayType = x.Key.SpecialDayType
                }).ToList();

            oReturn.AddRange(bm);

            return oReturn;
        }

        public static List<ScheduleBusyModel> GetScheduleBusy
            (string ProfilePublicId, string OfficePublicId, DateTime? StartDate, DateTime? EndDate, int? CategoryId)
        {
            return MedicalCalendarDataController.Instance.GetScheduleBusy
                (ProfilePublicId, OfficePublicId, StartDate, EndDate, CategoryId);
        }

        public static List<AppointmentModel> AppointmentGetByPatient(string PatientPublicId)
        {
            PatientModel CurrentPatient = Patient.PatientGetAllByPublicPatientId(PatientPublicId);

            List<AppointmentModel> oReturn = MedicalCalendarDataController.Instance.AppointmentGetByPatient(PatientPublicId);

            oReturn.All(x =>
            {
                x.RelatedPatient = new List<PatientModel>() { CurrentPatient };
                return true;
            });

            return oReturn;
        }

        public static string UpsertAppointmentInfo(AppointmentModel AppointmentToUpSert, List<PatientModel> PatientToRemove)
        {
            //upsert appointment
            string oAppointmentPublicId = AppointmentToUpSert.AppointmentPublicId;
            if (string.IsNullOrEmpty(oAppointmentPublicId))
            {
                oAppointmentPublicId = DAL.Controller.MedicalCalendarDataController.Instance.AppointmentCreate
                    (AppointmentToUpSert.OfficePublicId,
                    AppointmentToUpSert.Status,
                    AppointmentToUpSert.StartDate,
                    AppointmentToUpSert.EndDate);
            }
            else
            {
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentModify
                    (AppointmentToUpSert.AppointmentPublicId,
                    AppointmentToUpSert.OfficePublicId,
                    AppointmentToUpSert.Status,
                    AppointmentToUpSert.StartDate,
                    AppointmentToUpSert.EndDate);
            }

            //upsert appointment info
            AppointmentToUpSert.AppointmentInfo.All(api =>
            {
                if (api.AppointmentInfoId <= 0)
                {
                    //create info
                    DAL.Controller.MedicalCalendarDataController.Instance.AppointmentInfoCreate
                        (oAppointmentPublicId,
                        api.AppointmentInfoType,
                        api.Value,
                        api.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.MedicalCalendarDataController.Instance.AppointmentInfoModify
                        (api.AppointmentInfoId,
                        api.Value,
                        api.LargeValue);
                }

                return true;
            });

            //delete patient
            PatientToRemove.All(ptr =>
            {
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentPatientDelete
                    (oAppointmentPublicId,
                    ptr.PatientPublicId);

                return true;
            });

            //upsert patient
            AppointmentToUpSert.RelatedPatient.All(ptu =>
            {
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentPatientCreate
                    (oAppointmentPublicId,
                    ptu.PatientPublicId);

                return true;
            });

            return oAppointmentPublicId;
        }

        public static void UpdateAppointmentStatus(AppointmentModel AppointmentToUpSert)
        {
            DAL.Controller.MedicalCalendarDataController.Instance.AppointmentModifyStatus
                (AppointmentToUpSert.AppointmentPublicId,
                AppointmentToUpSert.Status);
        }

        public static void UpdateAppointmentInfoItem(AppointmentInfoModel AppointmentInfoToUpSert, string AppointmentPublicId)
        {
            if (AppointmentInfoToUpSert.AppointmentInfoId <= 0)
            {
                //create info
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentInfoCreate
                    (AppointmentPublicId,
                    AppointmentInfoToUpSert.AppointmentInfoType,
                    AppointmentInfoToUpSert.Value,
                    AppointmentInfoToUpSert.LargeValue);
            }
            else
            {
                //update info
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentInfoModify
                    (AppointmentInfoToUpSert.AppointmentInfoId,
                    AppointmentInfoToUpSert.Value,
                    AppointmentInfoToUpSert.LargeValue);
            }
        }

        public static List<AppointmentModel> AppointmentGetByOfficeId
            (string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            List<AppointmentModel> oReturn = DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByOfficeIdBasicInfo
                (OfficePublicId, StartDateTime, EndDateTime);

            List<AppointmentModel> oAux = DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByOfficeIdPatientInfo
                (OfficePublicId, StartDateTime, EndDateTime);

            if (oReturn != null)
            {
                oReturn.All(x =>
                {
                    x.RelatedPatient = oAux.Where(y => x.AppointmentPublicId == y.AppointmentPublicId).Select(y => y.RelatedPatient).FirstOrDefault();
                    return true;
                });
            }

            return oReturn;
        }

        public static AppointmentModel AppointmentGetById(string AppointmentPublicId)
        {

            AppointmentModel oReturn = DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByIdBasicInfo(AppointmentPublicId);

            AppointmentModel oAux = DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByIdPatientInfo(AppointmentPublicId);

            if (oReturn != null)
            {
                oReturn.RelatedPatient = oAux.RelatedPatient;
            }

            return oReturn;
        }

        public static List<AppointmentMonthModel> AppointmentGetByOfficeIdMonth(string OfficePublicId, DateTime StartDateTime)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByOfficeIdMonth(OfficePublicId, StartDateTime);
        }

        public static string GetAppointmentByOldId(string OldAppointmentId)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.GetAppointmentByOldId(OldAppointmentId);
        }

        public static bool AppointmentValidateDuplicate(string OfficePublicId, string AppointmentPublicId, DateTime StartDate, DateTime EndDate)
        {
            return MedicalCalendarDataController.Instance.AppointmentValidateDuplicate(OfficePublicId, AppointmentPublicId, StartDate, EndDate);
        }

        #region MarketPlace

        public static List<AppointmentModel> MPAppointmentGetByOfficeId
            (string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            List<AppointmentModel> oReturn = DAL.Controller.MedicalCalendarDataController.Instance.MPAppointmentGetByOfficeIdBasicInfo
                (OfficePublicId, StartDateTime, EndDateTime);

            return oReturn;
        }

        #endregion
    }
}
