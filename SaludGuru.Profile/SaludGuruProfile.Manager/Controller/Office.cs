using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
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
            OfficeModel oReturn = DAL.Controller.ProfileDataController.Instance.OfficeGetFullAdminBasicInfo(OfficePublicId);

            OfficeModel oAux = DAL.Controller.ProfileDataController.Instance.OfficeGetFullAdminCategory(OfficePublicId);

            if (oAux != null && oAux.RelatedTreatment != null)
            {
                oReturn.RelatedTreatment = oAux.RelatedTreatment;
            }

            oAux = DAL.Controller.ProfileDataController.Instance.OfficeGetFullAdminScheduleAvailable(OfficePublicId);

            if (oAux != null && oAux.ScheduleAvailable != null)
            {
                oReturn.ScheduleAvailable = oAux.ScheduleAvailable;
            }

            return oReturn;
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

        public static ProfileModel OfficeGetScheduleSettings(string ProfilePublicId)
        {
            ProfileModel oReturn = DAL.Controller.ProfileDataController.Instance.OfficeGetScheduleSettingsBasicInfo(ProfilePublicId);

            ProfileModel oAux = DAL.Controller.ProfileDataController.Instance.OfficeGetScheduleSettingsCategory(ProfilePublicId);

            if (oAux != null && oAux.RelatedOffice != null)
            {
                oReturn.RelatedOffice.All(x =>
                {
                    x.RelatedTreatment = oAux.RelatedOffice.Where(y => x.OfficePublicId == y.OfficePublicId).FirstOrDefault().RelatedTreatment;
                    return true;
                });
            }

            oAux = DAL.Controller.ProfileDataController.Instance.OfficeGetScheduleSettingsScheduleAvailable(ProfilePublicId);

            if (oAux != null && oAux.RelatedOffice != null)
            {
                oReturn.RelatedOffice.All(x =>
                {
                    x.ScheduleAvailable = oAux.RelatedOffice.Where(y => x.OfficePublicId == y.OfficePublicId).FirstOrDefault().ScheduleAvailable;
                    return true;
                });
            }

            return oReturn;
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
