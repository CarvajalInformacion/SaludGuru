using SaludGuruProfile.Manager.DAL.Controller;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Controller
{
    public class Profile
    {
        public List<ProfileModel> ProfileSearch(string SearchCriteria, int PageNumber, int RowCount)
        {
            List<ProfileModel> oReturn = ProfileDataController.Instance.ProfileSearch(SearchCriteria, PageNumber, RowCount);

            return oReturn;
        }

        public static List<ItemModel> GetProfileOptions()
        {
            return ProfileDataController.Instance.ProfileGetOptions();
        }

        public static void InsuranceProfileUpsert(string ProfilePublicId, int CategoryId, bool IsDefault)
        {
            ProfileDataController.Instance.ProfileCategoryUpsert(ProfilePublicId, CategoryId, IsDefault);
        }

        public static void InsuranceProfileDelete(string ProfilePublicId, int CategoryId)
        {
            ProfileDataController.Instance.ProfileCategoryDelete(ProfilePublicId, CategoryId);
        }

        public static void SpecialtyProfileUpsert(string ProfilePublicId, int CategoryId, bool IsDefault)
        {
            ProfileDataController.Instance.ProfileCategoryUpsert(ProfilePublicId, CategoryId, IsDefault);
        }

        public static void SpecialtyProfileDelete(string ProfilePublicId, int CategoryId)
        {
            ProfileDataController.Instance.ProfileCategoryDelete(ProfilePublicId, CategoryId);
        }
    }
}
