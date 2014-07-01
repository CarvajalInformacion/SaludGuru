using SaludGuruProfile.Manager.Models.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Controller
{
    public class Office
    {
        #region Office

        public static OfficeModel OfficeGetFullAdmin(string OfficePublicId)
        {
            return DAL.Controller.ProfileDataController.Instance.OfficeGetFullAdmin(OfficePublicId);
        }

        public static string UpsertOfficeInfo(string ProfilePublicId, OfficeModel OfficeToUpsert)
        {
            //upsert office
            string oOfficePublicId = OfficeToUpsert.OfficePublicId;
            if (string.IsNullOrEmpty(oOfficePublicId))
            {
                oOfficePublicId = DAL.Controller.ProfileDataController.Instance.OfficeCreate
                    (ProfilePublicId,
                    OfficeToUpsert.City.CityId,
                    OfficeToUpsert.Name,
                    OfficeToUpsert.IsDefault);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.OfficeUpdate
                    (OfficeToUpsert.OfficePublicId,
                    OfficeToUpsert.City.CityId,
                    OfficeToUpsert.Name,
                    OfficeToUpsert.IsDefault);
            }

            //upsert profile info
            OfficeToUpsert.OfficeInfo.All(ofi =>
            {
                if (ofi.OfficeInfoId <= 0)
                {
                    //create info
                    DAL.Controller.ProfileDataController.Instance.OfficeInfoCreate
                        (oOfficePublicId,
                        ofi.OfficeInfoType,
                        ofi.Value,
                        ofi.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.ProfileDataController.Instance.OfficeInfoModify
                        (ofi.OfficeInfoId,
                        ofi.Value,
                        ofi.LargeValue);
                }
                return true;
            });

            return oOfficePublicId;
        }

        #endregion

        #region Office Treatment

        public static void UpsertTreatmentOffice(string OfficePublicId, TreatmentOfficeModel TreatmentOfficeToUpsert)
        {
            //upsert office category info
            TreatmentOfficeToUpsert.TreatmentOfficeInfo.All(toi =>
            {
                if (toi.CategoryInfoId <= 0)
                {
                    //create info
                    DAL.Controller.ProfileDataController.Instance.OfficeCategoryInfoCreate
                        (OfficePublicId,
                        TreatmentOfficeToUpsert.CategoryId,
                        toi.OfficeCategoryInfoType,
                        toi.Value,
                        toi.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.ProfileDataController.Instance.OfficeCategoryInfoModify
                        (toi.CategoryInfoId,
                        toi.Value,
                        toi.LargeValue);
                }
                return true;
            });
        }

        #endregion

        #region Schedule Available

        public static void ScheduleAvailableCreate(string OfficePublicId, ScheduleAvailableModel ScheduleToCreate)
        {
            DAL.Controller.ProfileDataController.Instance.ScheduleAvailableCreate
                (OfficePublicId,
                ScheduleToCreate.Day,
                ScheduleToCreate.StartTime,
                ScheduleToCreate.EndTime);
        }

        public static void ScheduleAvailableRemove(int ScheduleAvailableId)
        {
            DAL.Controller.ProfileDataController.Instance.ScheduleAvailableDelete(ScheduleAvailableId);
        }

        #endregion
    }
}
