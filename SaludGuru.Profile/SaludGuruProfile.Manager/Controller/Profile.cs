using SaludGuruProfile.Manager.DAL.Controller;
using SaludGuruProfile.Manager.Models;
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

        public static List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            List<ProfileModel> oReturn = ProfileDataController.Instance.ProfileSearch(SearchCriteria, PageNumber, RowCount, out TotalRows);

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

        /// <summary>
        /// update profile info specific detail
        /// </summary>
        /// <param name="ProfileToUpSert"></param>
        public static void UpsertProfileDetailInfo(ProfileModel ProfileToUpSert)
        {
            //upsert profile info
            ProfileToUpSert.ProfileInfo.All(pri =>
            {
                if (pri.ProfileInfoId <= 0)
                {
                    //create info
                    DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                        (ProfileToUpSert.ProfilePublicId,
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
        }

        /// <summary>
        /// delete profile info specific detail
        /// </summary>
        /// <param name="ProfileInfoToDelete"></param>
        public static void DeleteProfileDetailInfo(List<ProfileInfoModel> ProfileInfoToDelete)
        {
            //upsert profile info
            ProfileInfoToDelete.All(pri =>
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoDelete
                    (pri.ProfileInfoId);

                return true;
            });
        }

        /// <summary>
        /// insert or update profile small image
        /// </summary>
        /// <param name="ProfileToUpsert"></param>
        /// <param name="ImagePath"></param>
        public static void UpsertProfileSmallImage(ProfileModel ProfileToUpsert, string ImagePath)
        {
            //process image
            string oPublicImage = SaludGuruProfile.Manager.Image.ImagePreprocesing.ProcessImage(ImagePath, enumImageType.ProfileSmall);

            //upload images
            SaludGuruProfile.Manager.Image.ImageLoader oLoader = new Image.ImageLoader()
            {
                FilesToUpload = new List<string>() { ImagePath, oPublicImage },
                RemoteFolder = SaludGuruProfile.Manager.Models.General.InternalSettings.Instance[SaludGuruProfile.Manager.Models.Constants.C_Settings_Image_ProfileImage_RemoteFolder].Value,
            };

            oLoader.StartUpload();

            //save urls to database model

            //get profile info id
            int ProfileInfoId = ProfileToUpsert.ProfileInfo.
                Where(x => x.ProfileInfoType == enumProfileInfoType.ImageProfileSmall).
                Select(x => x.ProfileInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            //upsert public image
            if (ProfileInfoId <= 0)
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageProfileSmall,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem.IndexOf(oPublicImage) > 0).Select(x => x.FilePathRemoteSystem).DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem.IndexOf(oPublicImage) > 0).Select(x => x.FilePathRemoteSystem).DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    null);
            }

            //upsert original image
            ProfileInfoId = ProfileToUpsert.ProfileInfo.
                Where(x => x.ProfileInfoType == enumProfileInfoType.ImageProfileSmallOriginal).
                Select(x => x.ProfileInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            if (ProfileInfoId <= 0)
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageProfileSmallOriginal,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem.IndexOf(ImagePath) > 0).Select(x => x.FilePathRemoteSystem).DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem.IndexOf(ImagePath) > 0).Select(x => x.FilePathRemoteSystem).DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    null);
            }

        }

        /// <summary>
        /// insert or update profile large image
        /// </summary>
        /// <param name="ProfileToUpsert"></param>
        /// <param name="ImagePath"></param>
        public static void UpsertProfileLargeImage(ProfileModel ProfileToUpsert, string ImagePath)
        {
            string oImgPrep = SaludGuruProfile.Manager.Image.ImagePreprocesing.ProcessImage(ImagePath, enumImageType.ProfileLarge);

        }

        /// <summary>
        /// insert or update profile general images
        /// </summary>
        /// <param name="ProfileToUpsert"></param>
        /// <param name="ImagePath"></param>
        public static void UpsertProfileGeneralImage(ProfileModel ProfileToUpsert, Dictionary<string, int?> ImagePath)
        {
            string oImgPrep = SaludGuruProfile.Manager.Image.ImagePreprocesing.ProcessImage(ImagePath.FirstOrDefault().Key, enumImageType.ProfileGeneral);

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
