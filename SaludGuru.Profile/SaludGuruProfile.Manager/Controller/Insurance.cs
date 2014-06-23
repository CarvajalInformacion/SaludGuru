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
        public static List<InsuranceModel> GetAllAdmin(string Parameter)
        {
            List<InsuranceModel> oReturn = new List<InsuranceModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin
                (enumCategoryType.Insurance, Parameter);

            imodelList.All(im =>
            {
                oReturn.Add((InsuranceModel)im);
                return true;
            });

            return oReturn;
        }
    }
}
