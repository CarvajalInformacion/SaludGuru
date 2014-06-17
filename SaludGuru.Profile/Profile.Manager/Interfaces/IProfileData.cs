using SessionController.Models.Profile;
using SessionController.Models.Profile.Autorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Interfaces
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

        #endregion

        #region Profile
        string ProfileCreate(string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus);
        void ProfileUpdate(string ProfilePublicId, string Name, string LastName, enumProfileType ProfileType, enumProfileStatus ProfileStatus);

        int ProfileInfoCreate(string ProfilePublicId, enumProfileInfoType ProfileInfoType, string Value, string LargeValue);
        void ProfileInfoModify(int ProfileInfoTypeId, string Value, string LargeValue);
        void ProfileInfoDelete(int ProfileInfoTypeId);

        void RelatedProfileCreate(string ProfilePublicIdParent, string ProfilePublicIdChild);
        void RelatedProfileDelete(string ProfilePublicIdParent, string ProfilePublicIdChild);

        void ProfileCategoryUpsert(string ProfilePublicId, int CategoryId, bool IsDefault);
        void ProfileCategoryDelete(string ProfilePublicId, int CategoryId);

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

        #endregion

        #region Autorization

        int ProfileRoleCreate(string ProfilePublicId, enumRole RoleId, string UserEmail);
        void ProfileRoleDelete(int ProfileRoleId);

        List<AutorizationModel> GetAutorization(string UserEmail);

        #endregion
    }
}
