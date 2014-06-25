using SaludGuruProfile.Manager.DAL.Controller;
using SaludGuruProfile.Manager.Interfaces;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Controller
{
    public class Specialty
    {
        public static List<SpecialtyModel> GetAllAdmin(string Parameter)
        {
            List<SpecialtyModel> oReturn = new List<SpecialtyModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin
                (enumCategoryType.Specialty, Parameter);

            imodelList.All(im =>
            {
                oReturn.Add((SpecialtyModel)im);
                return true;
            });

            return oReturn;
        }

        public static int Upsert(SpecialtyModel SpecialtyToUpsert)
        {
            int oSpecialtyId = SpecialtyToUpsert.CategoryId;

            if (oSpecialtyId <= 0)
            {
                //create specialty
                oSpecialtyId = DAL.Controller.ProfileDataController.Instance.CategoryCreate(SpecialtyToUpsert.CategoryType, SpecialtyToUpsert.Name);
            }
            else
            {
                //update specialty
                DAL.Controller.ProfileDataController.Instance.CategoryModify(oSpecialtyId, SpecialtyToUpsert.Name);
            }
            return oSpecialtyId;
        }
    }
}
