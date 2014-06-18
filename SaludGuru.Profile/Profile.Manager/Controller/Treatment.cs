using Profile.Manager.DAL.Controller;
using Profile.Manager.Interfaces;
using Profile.Manager.Models;
using Profile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Controller
{
    public class Treatment
    {
        public static List<TreatmentModel> CategoryGetAllAdmin(string Parameter)
        {
            List<TreatmentModel> oReturn = new List<TreatmentModel>();
            List<ICategoryModel> imodelList = ProfileDataController.Instance.CategoryGetAllAdmin(enumCategoryType.Treatment, Parameter);

            imodelList.All(im =>
            {
                oReturn.Add((TreatmentModel)im);
                return true;
            });

            return oReturn;
        }
    }
}
