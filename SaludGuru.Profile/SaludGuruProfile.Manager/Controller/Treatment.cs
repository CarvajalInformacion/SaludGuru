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
    public class Treatment
    {
        public static List<TreatmentModel> GetAllAdmin(string Parameter)
        {
            List<TreatmentModel> oReturn = new List<TreatmentModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin
                (enumCategoryType.Treatment, Parameter);

            imodelList.All(im =>
            {
                oReturn.Add((TreatmentModel)im);
                return true;
            });

            return oReturn;
        }
    }
}
