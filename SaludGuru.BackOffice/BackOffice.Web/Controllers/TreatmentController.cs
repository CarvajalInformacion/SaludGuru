using BackOffice.Models.Treatment;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class TreatmentController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }

        #region Treatment

        public virtual ActionResult TreatmentList(string ProfilePublicId)
        {
            List<TreatmentModel> Model = new List<TreatmentModel>();
            if (!string.IsNullOrEmpty(ProfilePublicId))
                Model = SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(ProfilePublicId);
            else
                Model = SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(string.Empty);

            return View(Model);
        }

        public virtual ActionResult TreatmentUpsert(string treatmentId)
        {
            TreatmentUpsertModel Model = new TreatmentUpsertModel()
            {
                TreatmentInfo = string.IsNullOrEmpty(treatmentId) ? null :
                    SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(treatmentId).
                        Where(x => x.CategoryId.ToString() == treatmentId).FirstOrDefault(),
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                TreatmentModel modelList = GetTreatmentInfoRequestModel();

                //upsert treatment
                int oTreatmentId = SaludGuruProfile.Manager.Controller.Treatment.Upsert
                    (modelList);

                //redirect to upgrade page
                if (string.IsNullOrEmpty(modelList.CategoryId.ToString()))
                {
                    //new 
                    return RedirectToAction(MVC.Treatment.ActionNames.TreatmentUpsert, MVC.Treatment.Name, new { treatmentId = oTreatmentId });
                }
                else
                {
                    Model.TreatmentInfo = string.IsNullOrEmpty(treatmentId) ? null :
                        SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(treatmentId).
                            Where(x => x.CategoryId.ToString() == treatmentId).FirstOrDefault();
                }
            }
            return View(Model);
        }

        #endregion

        #region private methods

        private TreatmentModel GetTreatmentInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                TreatmentModel oReturn = new TreatmentModel()
                {
                    Name = Request["Name"].ToString(),
                    CategoryId = Convert.ToInt32(Request["TreatmentID"]),
                    TreatmentInfo = new List<CategoryInfoModel> 
                    {
                        new CategoryInfoModel() 
                        {
                            CategoryInfoId = Convert.ToInt32(Request["CatId_Keyword"]),
                            CategoryInfoType = enumCategoryInfoType.Keyword,
                            LargeValue = Request["Keyword"].ToString(),
                        },

                        new CategoryInfoModel()
                        {
                            CategoryInfoId = Convert.ToInt32(Request["DurationTime"]),
                            CategoryInfoType = enumCategoryInfoType.DurationTime,
                            LargeValue = Request["DurationTime"].ToString(),
                        },
                    }
                };
                return oReturn;
            }
            return null;
        }

        #endregion
    }
}