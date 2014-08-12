using MedicalCalendar.Manager.DAL.Controller;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.General;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Controller
{
    public class Patient
    {
        public static PatientModel PatientGetAllByPublicPatientId(string PatientPublicId)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.PatientGetAllByPublicPatientId(PatientPublicId);
        }

        public static List<PatientModel> PatientSearch(string ProfilePublicId, string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            return MedicalCalendarDataController.Instance.PatientSearch(ProfilePublicId, SearchCriteria, PageNumber, RowCount, out TotalRows);
        }

        public static string UpsertPatientInfo(PatientModel PatientToUpsert, string ProfilePublicId, string UserPublicId)
        {
            string oPublicPatientId = PatientToUpsert.PatientPublicId;
            if (string.IsNullOrEmpty(oPublicPatientId))
            {
                oPublicPatientId = DAL.Controller.MedicalCalendarDataController.Instance.PatientCreate
                (PatientToUpsert.Name,
                PatientToUpsert.LastName,
                ProfilePublicId,
                UserPublicId);
            }
            else
            {
                DAL.Controller.MedicalCalendarDataController.Instance.PatientModify
               (oPublicPatientId,
               PatientToUpsert.Name,
               PatientToUpsert.LastName);
            }

            PatientToUpsert.PatientInfo.All(pri =>
            {
                if (pri.PatientInfoId <= 0)
                {
                    //create info
                    DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoCreate
                        (oPublicPatientId,
                        pri.PatientInfoType,
                        pri.Value,
                        pri.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoModify
                        (pri.PatientInfoId,
                        pri.Value,
                        pri.LargeValue);
                }

                return true;
            });
            return oPublicPatientId;
        }

        public static void UpsertPatientInfoItem(PatientInfoModel PatientInfoToUpsert, string PatientPublicId)
        {
            if (PatientInfoToUpsert.PatientInfoId <= 0)
            {
                //create info
                DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoCreate
                    (PatientPublicId,
                    PatientInfoToUpsert.PatientInfoType,
                    PatientInfoToUpsert.Value,
                    PatientInfoToUpsert.LargeValue);
            }
            else
            {
                //update info
                DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoModify
                    (PatientInfoToUpsert.PatientInfoId,
                    PatientInfoToUpsert.Value,
                    PatientInfoToUpsert.LargeValue);
            }
        }

        public static void DeletePatientInfoItem(PatientInfoModel PatientInfoToDelete)
        {
            //create info
            DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoDelete
                (PatientInfoToDelete.PatientInfoId);
        }

        public static List<ItemModel> GetPatientOptions()
        {
            return MedicalCalendarDataController.Instance.PatientGetOptions();
        }

        #region Market Place

        public static List<PatientModel> MPPatientGetByUserPublicId(string vUserPublicId)
        {
            return MedicalCalendarDataController.Instance.MPPatientGetByUserPublicId(vUserPublicId);
        }

        public static string MPUpsertPatientInfo(PatientModel PatientToUpsert, string ProfilePublicId, string UserPublicId)
        {
            string oPublicPatientId = PatientToUpsert.PatientPublicId;
            if (string.IsNullOrEmpty(oPublicPatientId))
            {
                oPublicPatientId = DAL.Controller.MedicalCalendarDataController.Instance.PatientCreate
                (PatientToUpsert.Name,
                PatientToUpsert.LastName,
                ProfilePublicId,
                UserPublicId);
            }
            else
            {
                DAL.Controller.MedicalCalendarDataController.Instance.PatientModify
               (oPublicPatientId,
               PatientToUpsert.Name,
               PatientToUpsert.LastName);
            }

            PatientToUpsert.PatientInfo.All(pri =>
            {
                if (pri.PatientInfoId <= 0)
                {
                    //create info
                    DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoCreate
                        (oPublicPatientId,
                        pri.PatientInfoType,
                        pri.Value,
                        pri.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.MedicalCalendarDataController.Instance.PatientInfoModify
                        (pri.PatientInfoId,
                        pri.Value,
                        pri.LargeValue);
                }

                return true;
            });
            return oPublicPatientId;
        }

        public static bool MPPatientTemporalUpsert(string PublicPatientId, string PublicProfileId, enumPatientState Status, string PublicPatientIdBO)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.MPPatientTemporalUpsert(PublicPatientId, PublicProfileId, Status, PublicPatientIdBO);
        }
        #endregion
    }
}
