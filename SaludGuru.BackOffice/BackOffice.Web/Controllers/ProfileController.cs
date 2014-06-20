using BackOffice.Models.Profile;
using SaludGuruProfile.Manager.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BackOffice.Web.Controllers
{
    public partial class ProfileController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult Create()
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = null,
            };

            return View(Model);
        }


        public virtual ActionResult Specialty(string ProfilePublicId)
        {
            ProfileSpecialtyModel Model = new ProfileSpecialtyModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                SpecialtyToSelect = SaludGuruProfile.Manager.Controller.Specialty.CategoryGetAllAdmin(string.Empty),
            };
            return View(Model);
        }

        //public JsonResult AutoCompleteSpecialty(string term)
        //{
        //    List<SaludGuruProfile.Manager.Models.General.SpecialtyModel> Model = ;

        //    var result = (from s in Model
        //                  where s.Name.Contains(term)
        //                  select s).ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public virtual ActionResult Insurance(string ProfilePublicId)
        {
            ProfileInsuranceModel Model = new ProfileInsuranceModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                InsuranceToSelect = SaludGuruProfile.Manager.Controller.Insurance.CategoryGetAllAdmin(string.Empty),
            };
            return View();
        }
    }
}