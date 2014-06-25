using BackOffice.Models.General;
using BackOffice.Models.Specialty;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class SpecialtyController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult SpecialtyList(string ProfilePublicId)
        {
            List<SpecialtyModel> Model = new List<SpecialtyModel>();

            Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(ProfilePublicId);
           
            return View(Model);
        }

        public virtual ActionResult SpecialtyUpsert(string SpecialtyPublicId, string ProfilePublicId)
        {
            //SpecialtyUpsertModel Model = new SpecialtyUpsertModel()
            //{
            //    Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            //    CurrentSpecialty = string.IsNullOrEmpty(SpecialtyPublicId) ? null : SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(SpecialtyPublicId),
            //};

            return View();
        }


    }
}