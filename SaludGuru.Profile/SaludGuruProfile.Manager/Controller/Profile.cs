using SaludGuruProfile.Manager.DAL.Controller;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using SessionController.Models.Profile.Autorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Controller
{
    public class Profile
    {
        #region Profile

        public static List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount)
        {
            List<ProfileModel> oReturn = ProfileDataController.Instance.ProfileSearch(SearchCriteria, PageNumber, RowCount);

            return oReturn;
        }

        /// <summary>
        /// Get full profile information for admin platform
        /// </summary>
        /// <param name="ProfilePublicId"></param>
        /// <returns></returns>
        public static ProfileModel ProfileGetFullAdmin(string ProfilePublicId)
        {
            return DAL.Controller.ProfileDataController.Instance.ProfileGetFullAdmin(ProfilePublicId);
        }

        /// <summary>
        /// get all options for a profile
        /// </summary>
        /// <returns>list posible options</returns>
        public static List<ItemModel> GetProfileOptions()
        {
            return ProfileDataController.Instance.ProfileGetOptions();
        }

        /// <summary>
        /// insert or update profile
        /// </summary>
        /// <param name="ProfileToUpSert">profile info to upsert</param>
        /// <returns>PublicProfileId</returns>
        public static string UpsertProfileInfo(ProfileModel ProfileToUpSert)
        {
            //upsert profile
            string oPublicProfileId = ProfileToUpSert.ProfilePublicId;
            if (string.IsNullOrEmpty(oPublicProfileId))
            {
                oPublicProfileId = DAL.Controller.ProfileDataController.Instance.ProfileCreate
                    (ProfileToUpSert.Name,
                    ProfileToUpSert.LastName,
                    ProfileToUpSert.ProfileType,
                    ProfileToUpSert.ProfileStatus);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileUpdate
                    (oPublicProfileId,
                    ProfileToUpSert.Name,
                    ProfileToUpSert.LastName,
                    ProfileToUpSert.ProfileType,
                    ProfileToUpSert.ProfileStatus);
            }

            //upsert profile info
            ProfileToUpSert.ProfileInfo.All(pri =>
            {
                if (pri.ProfileInfoId <= 0)
                {
                    //create info
                    DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                        (oPublicProfileId,
                        pri.ProfileInfoType,
                        pri.Value,
                        pri.LargeValue);
                }
                else
                {
                    //update info
                    DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                        (pri.ProfileInfoId,
                        pri.Value,
                        pri.LargeValue);
                }

                return true;
            });

            return oPublicProfileId;
        }

        #endregion

        #region Insurance

        public static void InsuranceProfileUpsert(ProfileModel ProfileToUpsert)
        {
            ProfileToUpsert.RelatedInsurance.All(sp =>
            {
                ProfileDataController.Instance.ProfileCategoryUpsert
                    (ProfileToUpsert.ProfilePublicId,
                    sp.CategoryId,
                    false);

                return true;
            });
        }

        public static void InsuranceProfileDelete(string ProfilePublicId, int InsuranceId)
        {
            ProfileDataController.Instance.ProfileCategoryDelete(ProfilePublicId, InsuranceId);
        }

        #endregion

        #region Specialty

        public static void SpecialtyProfileUpsert(ProfileModel ProfileToUpsert)
        {
            ProfileToUpsert.RelatedSpecialty.All(sp =>
            {
                ProfileDataController.Instance.ProfileCategoryUpsert
                    (ProfileToUpsert.ProfilePublicId,
                    sp.CategoryId,
                    (ProfileToUpsert.DefaultSpecialty != null && sp.CategoryId == ProfileToUpsert.DefaultSpecialty.CategoryId));

                return true;
            });
        }

        public static void SpecialtyProfileDelete(string ProfilePublicId, int SpecialtyId)
        {
            ProfileDataController.Instance.ProfileCategoryDelete(ProfilePublicId, SpecialtyId);
        }

        #endregion

        #region Treatment

        public static void TreatmentProfileUpsert(ProfileModel ProfileToUpsert)
        {
            ProfileToUpsert.RelatedTreatment.All(sp =>
            {
                ProfileDataController.Instance.ProfileCategoryUpsert
                    (ProfileToUpsert.ProfilePublicId,
                    sp.CategoryId,
                    false);

                return true;
            });
        }

        public static void TreatmentProfileDelete(string ProfilePublicId, int TreatmentId)
        {
            ProfileDataController.Instance.ProfileCategoryDelete(ProfilePublicId, TreatmentId);
        }

        #endregion

        #region Autorization

        public static List<ProfileAutorizationModel> GetProfileAutorization(string ProfilePublicId)
        {
            return DAL.Controller.ProfileDataController.Instance.GetProfileAutorization(ProfilePublicId);
        }

        public static int ProfileAutorizationUpsert(ProfileAutorizationModel ProfileAutorizationToUpsert)
        {
            return DAL.Controller.ProfileDataController.Instance.ProfileRoleCreate(ProfileAutorizationToUpsert.ProfilePublicId, ProfileAutorizationToUpsert.Role, ProfileAutorizationToUpsert.UserEmail);
        }

        public static void DeleteProfileAutorization(int ProfileRoleId)
        {
            DAL.Controller.ProfileDataController.Instance.ProfileRoleDelete(ProfileRoleId);
        }

        #endregion
    }
}
