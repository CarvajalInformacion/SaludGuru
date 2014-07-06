using MedicalCalendar.Manager.DAL.Controller;
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
            (int CountryId, string ProfilePublicId, DateTime StartDate, DateTime EndDate)
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

            return oReturn;
        }

        public static List<AppointmentModel> AppointmentList(string ProfilePublicId)
        {
            List<AppointmentModel> oReturn = MedicalCalendarDataController.Instance.AppointmentList(ProfilePublicId);

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
                DAL.Controller.MedicalCalendarDataController.Instance.AppointmentPatientDelete
                    (oAppointmentPublicId,
                    ptu.PatientPublicId);

                return true;
            });

            return oAppointmentPublicId;
        }

        public static List<AppointmentModel> AppointmentGetByProfileId
            (string ProfilePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.AppointmentGetByProfileId
                (ProfilePublicId, StartDateTime, EndDateTime);
        }
    }
}
