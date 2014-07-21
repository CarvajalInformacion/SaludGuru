using BackOffice.Models.Specialty;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class SpecialtyController : BaseController
    {
        #region Specialty

        public virtual ActionResult SpecialtyList(string ProfilePublicId)
        {
            List<SpecialtyModel> Model = new List<SpecialtyModel>();
            if (!string.IsNullOrWhiteSpace(ProfilePublicId))
                Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(ProfilePublicId);
            else
                Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(string.Empty);

            return View(Model);
        }

        public virtual ActionResult SpecialtyUpsert(string specialtyId)
        {
            SpecialtyUpsertModel Model = new SpecialtyUpsertModel()
            {
                SpecialtyInfo = string.IsNullOrEmpty(specialtyId) ? null :
                    SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(specialtyId).
                        Where(x => x.CategoryId.ToString() == specialtyId).FirstOrDefault(),
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                SpecialtyModel modelList = GetSpecialtyInfoRequestModel();

                //upsert specialty
                int oSpecialtyId = SaludGuruProfile.Manager.Controller.Specialty.Upsert
                    (modelList);

                //redirect to upgrade page
                if (string.IsNullOrEmpty(modelList.CategoryId.ToString()))
                {
                    //new
                    return RedirectToAction(MVC.Specialty.ActionNames.SpecialtyUpsert, MVC.Specialty.Name, new { specialtyId = oSpecialtyId });
                }
                else
                {
                    //update
                    Model.SpecialtyInfo = string.IsNullOrEmpty(specialtyId) ? null :
                        SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(specialtyId).
                            Where(x => x.CategoryId.ToString() == specialtyId).FirstOrDefault();
                }
            }
            if(Model.SpecialtyInfo == null)
            {
                Model.SpecialtyInfo = new SpecialtyModel()
                {
                    SpecialtyInfo = new List<CategoryInfoModel>()
                };
            }
            return View(Model);
        }

        #endregion

        #region private methods

        private SpecialtyModel GetSpecialtyInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                SpecialtyModel oReturn = new SpecialtyModel()
                {
                    Name = Request["Name"].ToString(),
                    CategoryId = Convert.ToInt32(Request["SpecialtyId"]),
                    SpecialtyInfo = new List<CategoryInfoModel>
                    {
                        new CategoryInfoModel()
                        {
                            CategoryInfoId = Convert.ToInt32(Request["CatId_Keyword"]),
                            CategoryInfoType = enumCategoryInfoType.Keyword,
                            LargeValue = Request["Keyword"].ToString(),
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