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
            ProfileModel oReturn = DAL.Controller.ProfileDataController.Instance.ProfileGetFullAdminBasicInfo(ProfilePublicId);

            ProfileModel oAux = DAL.Controller.ProfileDataController.Instance.ProfileGetFullAdminCategory(ProfilePublicId);

            if (oAux != null)
            {
                oReturn.RelatedSpecialty = oAux.RelatedSpecialty;
                oReturn.DefaultSpecialty = oAux.DefaultSpecialty;
                oReturn.RelatedInsurance = oAux.RelatedInsurance;
                oReturn.RelatedTreatment = oAux.RelatedTreatment;
            }

            oAux = DAL.Controller.ProfileDataController.Instance.ProfileGetFullAdminOffice(ProfilePublicId);

            if (oAux != null && oAux.RelatedOffice != null)
            {
                oReturn.RelatedOffice = oAux.RelatedOffice;
            }

            oAux = DAL.Controller.ProfileDataController.Instance.ProfileGetFullAdminRelatedProfile(ProfilePublicId);

            if (oAux != null && oAux.ChildProfile != null)
            {
                oReturn.ChildProfile = oAux.ChildProfile;
            }

            return oReturn;
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
                RemoteFolder = SaludGuruProfile.Manager.Models.General.InternalSettings.Instance
                    [SaludGuruProfile.Manager.Models.Constants.C_Settings_Image_ProfileImage_RemoteFolder].
                    Value.Replace("{{ProfilePublicId}}", ProfileToUpsert.ProfilePublicId),
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
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == oPublicImage).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == oPublicImage).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
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
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ImagePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ImagePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }

            //delete temporal images
            System.IO.File.Delete(oPublicImage);
            System.IO.File.Delete(ImagePath);
        }

        /// <summary>
        /// insert or update profile large image
        /// </summary>
        /// <param name="ProfileToUpsert"></param>
        /// <param name="ImagePath"></param>
        public static void UpsertProfileLargeImage(ProfileModel ProfileToUpsert, string ImagePath)
        {
            //process image
            string oPublicImage = SaludGuruProfile.Manager.Image.ImagePreprocesing.ProcessImage(ImagePath, enumImageType.ProfileLarge);

            //upload images
            SaludGuruProfile.Manager.Image.ImageLoader oLoader = new Image.ImageLoader()
            {
                FilesToUpload = new List<string>() { ImagePath, oPublicImage },
                RemoteFolder = SaludGuruProfile.Manager.Models.General.InternalSettings.Instance
                    [SaludGuruProfile.Manager.Models.Constants.C_Settings_Image_ProfileImage_RemoteFolder].
                    Value.Replace("{{ProfilePublicId}}", ProfileToUpsert.ProfilePublicId),
            };

            oLoader.StartUpload();

            //save urls to database model

            //get profile info id
            int ProfileInfoId = ProfileToUpsert.ProfileInfo.
                Where(x => x.ProfileInfoType == enumProfileInfoType.ImageProfileLarge).
                Select(x => x.ProfileInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            //upsert public image
            if (ProfileInfoId <= 0)
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageProfileLarge,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == oPublicImage).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == oPublicImage).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }

            //upsert original image
            ProfileInfoId = ProfileToUpsert.ProfileInfo.
                Where(x => x.ProfileInfoType == enumProfileInfoType.ImageProfileLargeOriginal).
                Select(x => x.ProfileInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();

            if (ProfileInfoId <= 0)
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageProfileLargeOriginal,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ImagePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }
            else
            {
                DAL.Controller.ProfileDataController.Instance.ProfileInfoModify
                    (ProfileInfoId,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ImagePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);
            }

            //delete temporal images
            System.IO.File.Delete(oPublicImage);
            System.IO.File.Delete(ImagePath);

        }

        /// <summary>
        /// insert or update profile general images
        /// </summary>
        /// <param name="ProfileToUpsert"></param>
        /// <param name="ImagePath"></param>
        public static void InsertProfileGeneralImage(ProfileModel ProfileToUpsert, List<string> lstImagePath)
        {
            lstImagePath.All(ImagePath =>
            {
                //process image
                string oPublicImage = SaludGuruProfile.Manager.Image.ImagePreprocesing.ProcessImage(ImagePath, enumImageType.ProfileGeneral);

                //upload images
                SaludGuruProfile.Manager.Image.ImageLoader oLoader = new Image.ImageLoader()
                {
                    FilesToUpload = new List<string>() { ImagePath, oPublicImage },
                    RemoteFolder = SaludGuruProfile.Manager.Models.General.InternalSettings.Instance
                        [SaludGuruProfile.Manager.Models.Constants.C_Settings_Image_ProfileImage_RemoteFolder].
                        Value.Replace("{{ProfilePublicId}}", ProfileToUpsert.ProfilePublicId),
                };

                oLoader.StartUpload();

                //save urls to database model

                //insert public image
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageGeneral,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == oPublicImage).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);


                //insert original image
                DAL.Controller.ProfileDataController.Instance.ProfileInfoCreate
                    (ProfileToUpsert.ProfilePublicId,
                    enumProfileInfoType.ImageGeneralOriginal,
                    oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ImagePath).
                        Select(x => x.PublishFile.ToString()).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault(),
                    null);

                //delete temporal images
                System.IO.File.Delete(oPublicImage);
                System.IO.File.Delete(ImagePath);

                return true;
            });
        }

        public static ProfileModel GetRelatedProfileAll(string PublicParentId)
        {
            List<ProfileModel> childs = new List<ProfileModel>();
            ProfileModel oLoad = new ProfileModel();
            List<RelatedProfileModel> relatedList = new List<RelatedProfileModel>();

            oLoad = ProfileGetFullAdmin(PublicParentId);

            if (oLoad.ChildProfile != null)
            {
                foreach (var item in oLoad.ChildProfile)
                {
                    childs.Add(ProfileGetFullAdmin(item.ProfilePublicId));
                }
                oLoad.ChildProfile = new List<ProfileModel>();
                oLoad.ChildProfile.AddRange(childs);
            }
            return oLoad;
        }

        public static List<ProfileModel> ProfileSearchToRelate(string SearchCriteria, string vProfilePublicToExclude, int PageNumber, int RowCount)
        {
            List<ProfileModel> oReturn = ProfileDataController.Instance.ProfileSearchToRelate(SearchCriteria, vProfilePublicToExclude, PageNumber, RowCount);

            return oReturn;
        }

        public static void RelatedProfileCreate(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            ProfileDataController.Instance.RelatedProfileCreate(ProfilePublicIdParent, ProfilePublicIdChild);
        }

        public static void RelatedProfileDelete(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            ProfileDataController.Instance.RelatedProfileDelete(ProfilePublicIdParent, ProfilePublicIdChild);
        }

        public static ProfileModel GetFeaturedProfile(int Quantity)
        {
            ProfileModel oReturn = DAL.Controller.ProfileDataController.Instance.GetFeaturedProfile(Quantity);

            return oReturn;
        }

        #region MarketPlace

        public static ProfileModel MPProfileGetFull(string ProfilePublicId)
        {
            ProfileModel oReturn = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullBasicInfo(ProfilePublicId);

            if (oReturn != null && !string.IsNullOrEmpty(oReturn.ProfilePublicId))
            {
                ProfileModel oAux = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullCategory(ProfilePublicId);

                if (oAux != null)
                {
                    oReturn.RelatedSpecialty = oAux.RelatedSpecialty;
                    oReturn.DefaultSpecialty = oAux.DefaultSpecialty;
                    oReturn.RelatedInsurance = oAux.RelatedInsurance;
                    oReturn.RelatedTreatment = oAux.RelatedTreatment;
                }

                oAux = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullOfficeBasicInfo(ProfilePublicId);

                if (oAux != null && oAux.RelatedOffice != null)
                {
                    oReturn.RelatedOffice = oAux.RelatedOffice;

                    ProfileModel oAuxOffice1 = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullOfficeCategory(ProfilePublicId);
                    ProfileModel oAuxOffice2 = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullOfficeScheduleAvailable(ProfilePublicId);

                    oReturn.RelatedOffice.All(of =>
                    {
                        if (oAuxOffice1 != null)
                        {
                            of.RelatedTreatment = oAuxOffice1.
                                            RelatedOffice.
                                            Where(x => x.OfficePublicId == of.OfficePublicId).
                                            Select(x => x.RelatedTreatment).
                                            FirstOrDefault();
                        }

                        if (oAuxOffice2 != null)
                        {
                            of.ScheduleAvailable = oAuxOffice2.
                                            RelatedOffice.
                                            Where(x => x.OfficePublicId == of.OfficePublicId).
                                            Select(x => x.ScheduleAvailable).
                                            FirstOrDefault();
                        }

                        return true;
                    });
                }

                oAux = DAL.Controller.ProfileDataController.Instance.MPProfileGetFullRelatedProfile(ProfilePublicId);

                if (oAux != null && oAux.ChildProfile != null)
                {
                    oReturn.ChildProfile = oAux.ChildProfile;
                }
            }

            return oReturn;
        }

        public static ProfileModel MPProfileGetProfilePublicIdFromOldId(string OldProfileId)
        {
            return DAL.Controller.ProfileDataController.Instance.MPProfileGetProfilePublicIdFromOldId(OldProfileId);
        }

        public static List<AutocompleteModel> MPProfileSearchAC(int CityId, string Query)
        {
            return DAL.Controller.ProfileDataController.Instance.MPProfileSearchAC(CityId, Query);
        }

        public static List<ProfileModel> MPProfileSearch
            (bool IsQuery,
            int CityId,
            string Query,
            int? InsuranceId,
            int? SpecialtyId,
            int? TreatmentId,
            int RowCount,
            int PageNumber,
            out int TotalRows)
        {
            List<ProfileModel> oReturn = DAL.Controller.ProfileDataController.Instance.MPProfileSearchBasicInfo
                (IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber, out TotalRows);

            if (oReturn == null)
                oReturn = new List<ProfileModel>();

            List<ProfileModel> oAux = DAL.Controller.ProfileDataController.Instance.MPProfileSearchCategory
                (IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber);

            if (oAux == null)
                oAux = new List<ProfileModel>();

            List<ProfileModel> oAuxOf = DAL.Controller.ProfileDataController.Instance.MPProfileSearchOfficeBasicInfo
                (IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber);

            if (oAuxOf == null)
                oAuxOf = new List<ProfileModel>();


            oReturn.All(p =>
            {
                p.RelatedSpecialty = oAux.Where(x => x.ProfilePublicId == p.ProfilePublicId).Select(x => x.RelatedSpecialty).FirstOrDefault();
                p.DefaultSpecialty = oAux.Where(x => x.ProfilePublicId == p.ProfilePublicId).Select(x => x.DefaultSpecialty).FirstOrDefault();
                p.RelatedInsurance = oAux.Where(x => x.ProfilePublicId == p.ProfilePublicId).Select(x => x.RelatedInsurance).FirstOrDefault();
                p.RelatedTreatment = oAux.Where(x => x.ProfilePublicId == p.ProfilePublicId).Select(x => x.RelatedTreatment).FirstOrDefault();

                p.RelatedOffice = oAuxOf.Where(x => x.ProfilePublicId == p.ProfilePublicId).Select(x => x.RelatedOffice).FirstOrDefault();

                return true;
            });

            return oReturn;
        }

        #endregion

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

        #region Category

        public static void RelatedCategoryCreate(int CategoryParent, int CategoryChild)
        {
            DAL.Controller.ProfileDataController.Instance.RelatedCategoryCreate(CategoryParent, CategoryChild);
        }

        #region MarketPlace

        public static List<Interfaces.ICategoryModel> MPCategoryGetAvailableCategory(string InsuranceName, string SpecialtyName, string TreatmentName)
        {
            return DAL.Controller.ProfileDataController.Instance.MPCategoryGetAvailableCategory(InsuranceName, SpecialtyName, TreatmentName);
        }

        #endregion

        #endregion
    }
}
