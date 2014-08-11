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

namespace SaludGuruProfile.Manager.Interfaces
{
    internal interface IProfileData
    {
        #region Category
        int CategoryCreate(enumCategoryType CategoryType, string Name);
        void CategoryModify(int CategoryId, string Name);
        void CategoryDelete(int CategoryId);

        void RelatedCategoryCreate(int CategoryParent, int CategoryChild);
        void RelatedCategoryDelete(int CategoryParent, int CategoryChild);

        int CategoryInfoCreate(int CategoryId, enumCategoryInfoType CategoryInfoType, string Value, string LargeValue);
        void CategoryInfoModify(int CategoryInfoTypeId, string Value, string LargeValue);
        void CategoryInfoDelete(int CategoryInfoTypeId);

        List<ICategoryModel> CategoryGetAllAdmin(enumCategoryType categoryType, string Parameter);
        #endregion

        #region Profile
        string ProfileCreate(string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus);
        void ProfileUpdate(string ProfilePublicId, string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus);

        int ProfileInfoCreate(string ProfilePublicId, enumProfileInfoType ProfileInfoType, string Value, string LargeValue);
        void ProfileInfoModify(int ProfileInfoId, string Value, string LargeValue);
        void ProfileInfoDelete(int ProfileInfoId);

        List<RelatedProfileModel> RelatedProfileGetAllByParentId(string ProfileParent);
        void RelatedProfileCreate(string ProfilePublicIdParent, string ProfilePublicIdChild);
        void RelatedProfileDelete(string ProfilePublicIdParent, string ProfilePublicIdChild);
        List<ProfileModel> ProfileSearchToRelate(string SearchCriteria, string vProfilePublicToExclude, int PageNumber, int RowCount);

        void ProfileCategoryUpsert(string ProfilePublicId, int CategoryId, bool IsDefault);
        void ProfileCategoryDelete(string ProfilePublicId, int CategoryId);

        List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount, out int TotalRows);
        List<ItemModel> ProfileGetOptions();

        ProfileModel ProfileGetFullAdminBasicInfo(string ProfilePublicId);
        ProfileModel ProfileGetFullAdminCategory(string ProfilePublicId);
        ProfileModel ProfileGetFullAdminOffice(string ProfilePublicId);
        ProfileModel ProfileGetFullAdminRelatedProfile(string ProfilePublicId);

        ProfileModel MPProfileGetFullBasicInfo(string ProfilePublicId);
        ProfileModel MPProfileGetFullCategory(string ProfilePublicId);
        ProfileModel MPProfileGetFullOfficeBasicInfo(string ProfilePublicId);
        ProfileModel MPProfileGetFullOfficeCategory(string ProfilePublicId);
        ProfileModel MPProfileGetFullOfficeScheduleAvailable(string ProfilePublicId);
        ProfileModel MPProfileGetFullRelatedProfile(string ProfilePublicId);
        ProfileModel MPProfileGetProfilePublicIdFromOldId(string OldProfileId);

        #endregion

        #region Office
        string OfficeCreate(string ProfilePublicId, int CityId, string Name, bool IsDefault);
        void OfficeUpdate(string OfficePublicId, int CityId, string Name, bool IsDefault);
        void OfficeDelete(string OfficePublicId);

        int OfficeInfoCreate(string OfficePublicId, enumOfficeInfoType OfficeInfoType, string Value, string LargeValue);
        void OfficeInfoModify(int OfficeInfoId, string Value, string LargeValue);
        void OfficeInfoDelete(int OfficeInfoId);

        int ScheduleAvailableCreate(string OfficePublicId, DayOfWeek Day, TimeSpan StartTime, TimeSpan EndTime);
        void ScheduleAvailableDelete(int ScheduleAvailableId);

        int OfficeCategoryInfoCreate(string OfficePublicId, int CategoryId, enumOfficeCategoryInfoType CategoryInfoType, string Value, string LargeValue);
        void OfficeCategoryInfoModify(int OfficeCategoryInfoId, string Value, string LargeValue);
        void OfficeCategoryInfoDelete(int OfficeCategoryInfoId);

        OfficeModel OfficeGetFullAdminBasicInfo(string OfficePublicId);
        OfficeModel OfficeGetFullAdminCategory(string OfficePublicId);
        OfficeModel OfficeGetFullAdminScheduleAvailable(string OfficePublicId);

        ProfileModel OfficeGetScheduleSettingsBasicInfo(string ProfilePublicId);
        ProfileModel OfficeGetScheduleSettingsCategory(string ProfilePublicId);
        ProfileModel OfficeGetScheduleSettingsScheduleAvailable(string ProfilePublicId);

        #endregion

        #region Autorization

        int ProfileRoleCreate(string ProfilePublicId, SessionController.Models.Profile.enumRole RoleId, string UserEmail);
        void ProfileRoleDelete(int ProfileRoleId);

        List<AutorizationModel> GetAutorization(string UserEmail);

        List<ProfileAutorizationModel> GetProfileAutorization(string ProfilePublicId);

        #endregion

        #region City
        List<CityModel> CityGetAll();
        #endregion
    }
}
