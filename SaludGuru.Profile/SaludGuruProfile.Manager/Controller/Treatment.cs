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

        public static int Upsert(TreatmentModel TreatmentToUpsert)
        {
            int oTreatmentId = TreatmentToUpsert.CategoryId;

            if(oTreatmentId <= 0)
            {
                //Create Treatment
                oTreatmentId = DAL.Controller.ProfileDataController.Instance.CategoryCreate(TreatmentToUpsert.CategoryType, TreatmentToUpsert.Name);
            }
            else
            {
                //Update Treatment
                DAL.Controller.ProfileDataController.Instance.CategoryModify(TreatmentToUpsert.CategoryId, TreatmentToUpsert.Name);
            }

            TreatmentToUpsert.TreatmentInfo.All(info =>
               {
                   if (info.CategoryInfoId <= 0)
                   {
                       //create info
                       DAL.Controller.ProfileDataController.Instance.CategoryInfoCreate
                           (TreatmentToUpsert.CategoryId,
                           info.CategoryInfoType,
                           info.Value,
                           info.LargeValue);
                   }
                   else
                   {
                       //update info
                       DAL.Controller.ProfileDataController.Instance.CategoryInfoModify
                           (info.CategoryInfoId,
                           info.Value,
                           info.LargeValue);
                   }
                   return true;
            });
            return oTreatmentId;
        }
    }
}
