using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using SessionController.Models.Profile.Autorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.DAL.Controller
{
    internal class ProfileDataController : SaludGuruProfile.Manager.Interfaces.IProfileData
    {
        #region singleton instance
        private static SaludGuruProfile.Manager.Interfaces.IProfileData oInstance;
        internal static SaludGuruProfile.Manager.Interfaces.IProfileData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ProfileDataController();
                return oInstance;
            }
        }

        private SaludGuruProfile.Manager.Interfaces.IProfileData DataFactory;
        #endregion

        #region constructor
        ProfileDataController()
        {
            ProfileDataFactory factory = new ProfileDataFactory();
            DataFactory = factory.GetProfileDataInstance();
        }
        #endregion

        #region Category

        public int CategoryCreate(enumCategoryType CategoryType, string Name)
        {
            return DataFactory.CategoryCreate(CategoryType, Name);
        }

        public void CategoryModify(int CategoryId, string Name)
        {
            DataFactory.CategoryModify(CategoryId, Name);
        }

        public void CategoryDelete(int CategoryId)
        {
            DataFactory.CategoryDelete(CategoryId);
        }

        public void RelatedCategoryCreate(int CategoryParent, int CategoryChild)
        {
            DataFactory.RelatedCategoryCreate(CategoryParent, CategoryChild);
        }

        public void RelatedCategoryDelete(int CategoryParent, int CategoryChild)
        {
            DataFactory.RelatedCategoryDelete(CategoryParent, CategoryChild);
        }

        public int CategoryInfoCreate(int CategoryId, enumCategoryInfoType CategoryInfoType, string Value, string LargeValue)
        {
            return DataFactory.CategoryInfoCreate(CategoryId, CategoryInfoType, Value, LargeValue);
        }

        public void CategoryInfoModify(int CategoryInfoTypeId, string Value, string LargeValue)
        {
            DataFactory.CategoryInfoModify(CategoryInfoTypeId, Value, LargeValue);
        }

        public void CategoryInfoDelete(int CategoryInfoTypeId)
        {
            DataFactory.CategoryInfoDelete(CategoryInfoTypeId);
        }

        public List<Interfaces.ICategoryModel> CategoryGetAllAdmin(enumCategoryType categoryType, string Parameter)
        {
            return DataFactory.CategoryGetAllAdmin(categoryType, Parameter);
        }

        public List<Interfaces.ICategoryModel> MPCategoryGetAvailableCategory(string InsuranceName, string SpecialtyName, string TreatmentName)
        {
            return DataFactory.MPCategoryGetAvailableCategory(InsuranceName, SpecialtyName, TreatmentName);
        }

        #endregion

        #region Profile

        public string ProfileCreate(string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus)
        {
            return DataFactory.ProfileCreate(Name, LastName, ProfileType, ProfileStatus);
        }

        public void ProfileUpdate(string ProfilePublicId, string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus)
        {
            DataFactory.ProfileUpdate(ProfilePublicId, Name, LastName, ProfileType, ProfileStatus);
        }

        public int ProfileInfoCreate(string ProfilePublicId, enumProfileInfoType ProfileInfoType, string Value, string LargeValue)
        {
            return DataFactory.ProfileInfoCreate(ProfilePublicId, ProfileInfoType, Value, LargeValue);
        }

        public void ProfileInfoModify(int ProfileInfoId, string Value, string LargeValue)
        {
            DataFactory.ProfileInfoModify(ProfileInfoId, Value, LargeValue);
        }

        public void ProfileInfoDelete(int ProfileInfoId)
        {
            DataFactory.ProfileInfoDelete(ProfileInfoId);
        }

        public List<RelatedProfileModel> RelatedProfileGetAllByParentId(string ProfileParent)
        {
            return DataFactory.RelatedProfileGetAllByParentId(ProfileParent);
        }

        public void RelatedProfileCreate(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            DataFactory.RelatedProfileCreate(ProfilePublicIdParent, ProfilePublicIdChild);
        }

        public void RelatedProfileDelete(string ProfilePublicIdParent, string ProfilePublicIdChild)
        {
            DataFactory.RelatedProfileDelete(ProfilePublicIdParent, ProfilePublicIdChild);
        }

        public List<ProfileModel> ProfileSearchToRelate(string SearchCriteria, string vProfilePublicToExclude, int PageNumber, int RowCount)
        {
            return DataFactory.ProfileSearchToRelate(SearchCriteria, vProfilePublicToExclude, PageNumber, RowCount);
        }

        public void ProfileCategoryUpsert(string ProfilePublicId, int CategoryId, bool IsDefault)
        {
            DataFactory.ProfileCategoryUpsert(ProfilePublicId, CategoryId, IsDefault);
        }

        public void ProfileCategoryDelete(string ProfilePublicId, int CategoryId)
        {
            DataFactory.ProfileCategoryDelete(ProfilePublicId, CategoryId);
        }

        public List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ProfileSearch(SearchCriteria, PageNumber, RowCount, out TotalRows);
        }

        public List<ItemModel> ProfileGetOptions()
        {
            return DataFactory.ProfileGetOptions();
        }

        public ProfileModel ProfileGetFullAdminBasicInfo(string ProfilePublicId)
        {
            return DataFactory.ProfileGetFullAdminBasicInfo(ProfilePublicId);
        }

        public ProfileModel ProfileGetFullAdminCategory(string ProfilePublicId)
        {
            return DataFactory.ProfileGetFullAdminCategory(ProfilePublicId);
        }

        public ProfileModel ProfileGetFullAdminOffice(string ProfilePublicId)
        {
            return DataFactory.ProfileGetFullAdminOffice(ProfilePublicId);
        }

        public ProfileModel ProfileGetFullAdminRelatedProfile(string ProfilePublicId)
        {
            return DataFactory.ProfileGetFullAdminRelatedProfile(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullBasicInfo(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullBasicInfo(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullCategory(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullCategory(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullOfficeBasicInfo(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullOfficeBasicInfo(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullOfficeCategory(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullOfficeCategory(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullOfficeScheduleAvailable(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullOfficeScheduleAvailable(ProfilePublicId);
        }

        public ProfileModel MPProfileGetFullRelatedProfile(string ProfilePublicId)
        {
            return DataFactory.MPProfileGetFullRelatedProfile(ProfilePublicId);
        }

        public ProfileModel MPProfileGetProfilePublicIdFromOldId(string OldProfileId)
        {
            return DataFactory.MPProfileGetProfilePublicIdFromOldId(OldProfileId);
        }

        public List<AutocompleteModel> MPProfileSearchAC(int CityId, string Query)
        {
            return DataFactory.MPProfileSearchAC(CityId, Query);
        }

        public List<ProfileModel> MPProfileSearchBasicInfo(bool IsQuery, int CityId, string Query, int? InsuranceId, int? SpecialtyId, int? TreatmentId, int RowCount, int PageNumber, out int TotalRows)
        {
            return DataFactory.MPProfileSearchBasicInfo(IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber, out TotalRows);
        }

        public List<ProfileModel> MPProfileSearchCategory(bool IsQuery, int CityId, string Query, int? InsuranceId, int? SpecialtyId, int? TreatmentId, int RowCount, int PageNumber)
        {
            return DataFactory.MPProfileSearchCategory(IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber);
        }

        public List<ProfileModel> MPProfileSearchOfficeBasicInfo(bool IsQuery, int CityId, string Query, int? InsuranceId, int? SpecialtyId, int? TreatmentId, int RowCount, int PageNumber)
        {
            return DataFactory.MPProfileSearchOfficeBasicInfo(IsQuery, CityId, Query, InsuranceId, SpecialtyId, TreatmentId, RowCount, PageNumber);
        }

        public ProfileModel GetFeaturedProfile(int Quantity)
        {
            return DataFactory.GetFeaturedProfile(Quantity);
        }

        #endregion

        #region Office

        public string OfficeCreate(string ProfilePublicId, int CityId, string Name, bool IsDefault)
        {
            return DataFactory.OfficeCreate(ProfilePublicId, CityId, Name, IsDefault);
        }

        public void OfficeUpdate(string OfficePublicId, int CityId, string Name, bool IsDefault)
        {
            DataFactory.OfficeUpdate(OfficePublicId, CityId, Name, IsDefault);
        }

        public void OfficeDelete(string OfficePublicId)
        {
            DataFactory.OfficeDelete(OfficePublicId);
        }

        public int OfficeInfoCreate(string OfficePublicId, enumOfficeInfoType OfficeInfoType, string Value, string LargeValue)
        {
            return DataFactory.OfficeInfoCreate(OfficePublicId, OfficeInfoType, Value, LargeValue);
        }

        public void OfficeInfoModify(int OfficeInfoId, string Value, string LargeValue)
        {
            DataFactory.OfficeInfoModify(OfficeInfoId, Value, LargeValue);
        }

        public void OfficeInfoDelete(int OfficeInfoId)
        {
            DataFactory.OfficeInfoDelete(OfficeInfoId);
        }

        public int ScheduleAvailableCreate(string OfficePublicId, DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime)
        {
            return DataFactory.ScheduleAvailableCreate(OfficePublicId, Day, StartTime, EndTime);
        }

        public void ScheduleAvailableDelete(int ScheduleAvailableId)
        {
            DataFactory.ScheduleAvailableDelete(ScheduleAvailableId);
        }

        public int OfficeCategoryInfoCreate(string OfficePublicId, int CategoryId, enumOfficeCategoryInfoType CategoryInfoType, string Value, string LargeValue)
        {
            return DataFactory.OfficeCategoryInfoCreate(OfficePublicId, CategoryId, CategoryInfoType, Value, LargeValue);
        }

        public void OfficeCategoryInfoModify(int OfficeCategoryInfoId, string Value, string LargeValue)
        {
            DataFactory.OfficeCategoryInfoModify(OfficeCategoryInfoId, Value, LargeValue);
        }

        public void OfficeCategoryInfoDelete(int OfficeCategoryInfoId)
        {
            DataFactory.OfficeCategoryInfoDelete(OfficeCategoryInfoId);
        }

        public List<ProfileModel> ProfileSearchInfo(string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.ProfileSearch(SearchCriteria, PageNumber, RowCount, out TotalRows);
        }

        public OfficeModel OfficeGetFullAdminBasicInfo(string OfficePublicId)
        {
            return DataFactory.OfficeGetFullAdminBasicInfo(OfficePublicId);
        }

        public OfficeModel OfficeGetFullAdminCategory(string OfficePublicId)
        {
            return DataFactory.OfficeGetFullAdminCategory(OfficePublicId);
        }

        public OfficeModel OfficeGetFullAdminScheduleAvailable(string OfficePublicId)
        {
            return DataFactory.OfficeGetFullAdminScheduleAvailable(OfficePublicId);
        }

        public ProfileModel OfficeGetScheduleSettingsBasicInfo(string ProfilePublicId)
        {
            return DataFactory.OfficeGetScheduleSettingsBasicInfo(ProfilePublicId);
        }

        public ProfileModel OfficeGetScheduleSettingsCategory(string ProfilePublicId)
        {
            return DataFactory.OfficeGetScheduleSettingsCategory(ProfilePublicId);
        }

        public ProfileModel OfficeGetScheduleSettingsScheduleAvailable(string ProfilePublicId)
        {
            return DataFactory.OfficeGetScheduleSettingsScheduleAvailable(ProfilePublicId);
        }

        #endregion

        #region Autorization

        public int ProfileRoleCreate(string ProfilePublicId, SessionController.Models.Profile.enumRole RoleId, string UserEmail)
        {
            return DataFactory.ProfileRoleCreate(ProfilePublicId, RoleId, UserEmail);
        }

        public void ProfileRoleDelete(int ProfileRoleId)
        {
            DataFactory.ProfileRoleDelete(ProfileRoleId);
        }

        public List<SessionController.Models.Profile.Autorization.AutorizationModel> GetAutorization(string UserEmail)
        {
            return DataFactory.GetAutorization(UserEmail);
        }

        public List<ProfileAutorizationModel> GetProfileAutorization(string ProfilePublicId)
        {
            return DataFactory.GetProfileAutorization(ProfilePublicId);
        }

        #endregion

        #region City
        public List<CityModel> CityGetAll()
        {
            return DataFactory.CityGetAll();
        }
        #endregion
    }
}
