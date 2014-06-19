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
    public class Insurance
    {
        //#region static crud methods

        //public static int Create(InsuranceModel NewInsurance)
        //{
        //    return ProfileDataController.Instance.CategoryCreate
        //        (NewInsurance.CategoryType.Value, NewInsurance.Name);
        //}

        //public static void Modify(InsuranceModel AlterInsurance)
        //{
        //    ProfileDataController.Instance.CategoryModify
        //        (AlterInsurance.InsuranceId, AlterInsurance.Name);
        //}

        //public static void Delete(int InsuranceId)
        //{
        //    ProfileDataController.Instance.CategoryDelete
        //        (InsuranceId);
        //}

        //#endregion

        //#region static get methods

        //public static List<Insurance> GetAll()
        //{
        //    return null;
        //}

        //#endregion

        public static List<InsuranceModel> CategoryGetAllAdmin(string Parameter)
        {
            List<InsuranceModel> oReturn = new List<InsuranceModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin(enumCategoryType.Insurance, Parameter);

            imodelList.All(im => 
            {
                oReturn.Add((InsuranceModel)im);
                return true; 
            });

            return oReturn;
        }
    }
}
