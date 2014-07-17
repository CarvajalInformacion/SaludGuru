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

        public ProfileModel ProfileGetFullAdmin(string ProfilePublicId)
        {
            return DataFactory.ProfileGetFullAdmin(ProfilePublicId);
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

        public OfficeModel OfficeGetFullAdmin(string OfficePublicId)
        {
            return DataFactory.OfficeGetFullAdmin(OfficePublicId);
        }

        public ProfileModel OfficeGetScheduleSettings(string ProfilePublicId)
        {
            return DataFactory.OfficeGetScheduleSettings(ProfilePublicId);
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
