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
        #region static crud methods

        #endregion

        public static List<SpecialtyModel> CategoryGetAllAdmin(string Parameter)
        {
            List<SpecialtyModel> oReturn = new List<SpecialtyModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin(enumCategoryType.Specialty, Parameter);

            imodelList.All(im =>
            {
                oReturn.Add((SpecialtyModel)im);
                return true;
            });

            return oReturn;
        }
    }
}
