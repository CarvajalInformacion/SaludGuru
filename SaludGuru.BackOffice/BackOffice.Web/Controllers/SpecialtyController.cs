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
        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult SpecialtyList(string ProfilePublicId)
        {
            List<SpecialtyModel> Model = new List<SpecialtyModel>();
            if (!string.IsNullOrWhiteSpace(ProfilePublicId))
                Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(ProfilePublicId);
            else
                Model = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(" ");
                        
            return View(Model);
        }

        public virtual ActionResult SpecialtyUpsert(string SpecialtyPublicId)
        {
            SpecialtyModel Model = new SpecialtyModel();

            if(!string.IsNullOrEmpty(Request["UpsertAction"]) 
                && bool.Parse(Request["UpsertAction"]))
            {
                //int idReturn = SaludGuruProfile.Manager.Controller.Specialty.
                
            }

            return null;
        }


    }
}