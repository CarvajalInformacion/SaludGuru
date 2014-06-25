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
                Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(" ");

            return View(Model);
        }

        public virtual ActionResult SpecialtyUpsert(string specialtyId)
        {
            SpecialtyUpsertModel Model = new SpecialtyUpsertModel()
            {
                CurrentSpecialty = string.IsNullOrEmpty(specialtyId) ? null : SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(specialtyId),                
            };

            if (!string.IsNullOrEmpty(specialtyId))
                Model.SpecialtyInfo = Model.CurrentSpecialty.FirstOrDefault();

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                SpecialtyModel modelList = GetSpecialtyInfoRequestModel();

                //upsert specialty
                int oSpecialtyId = SaludGuruProfile.Manager.Controller.Specialty.Upsert
                    (modelList);

                //modelList.CategoryId = Convert.ToInt32(specialtyId);
                //int idReturn = SaludGuruProfile.Manager.Controller.Specialty.Upsert(Model);

                //redirect to upgrade page
                if (string.IsNullOrEmpty(modelList.CategoryId.ToString()))
                {
                    return RedirectToAction(MVC.Specialty.ActionNames.SpecialtyUpsert, MVC.Specialty.Name);
                }
                else
                {
                    Model.CurrentSpecialty = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(oSpecialtyId.ToString());
                }
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
                    SpecialtyInfo = new List<CategoryInfoModel>
                    {
                        new CategoryInfoModel()
                        {
                            CategoryInfoId = string.IsNullOrEmpty(Request["CatId_Keyword"])?0:int.Parse(Request["CatId_Keyword"].ToString().Trim()),
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