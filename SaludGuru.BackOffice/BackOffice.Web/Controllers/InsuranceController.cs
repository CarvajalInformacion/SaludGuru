using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class InsuranceController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult InsuranceList(string param)
        {
            List<InsuranceModel> Model = new List<InsuranceModel>();
            if (!string.IsNullOrWhiteSpace(param))
                Model = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(param);
            else
                Model = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(" ");

            return View(Model);
        }

        public virtual ActionResult InsuranceUpsert(string insuranceId)
        {
            InsuranceModel model = new InsuranceModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                //int idReturn = SaludGuruProfile.Manager.Controller.Insurance.
            }
            return null;
        }
    }
}