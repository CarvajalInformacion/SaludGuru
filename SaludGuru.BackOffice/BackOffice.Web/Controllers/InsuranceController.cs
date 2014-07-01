using SaludGuruProfile.Manager.Models;
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

        /// <summary>
        /// Función que obtiene una lista de seguros de acuerdo al parametro
        /// </summary>
        /// <param name="param">Parametro para buscar</param>
        /// <returns>Modelo a la vista</returns>
        public virtual ActionResult InsuranceList(string param)
        {
            List<InsuranceModel> Model = new List<InsuranceModel>();
            if (!string.IsNullOrWhiteSpace(param))
                Model = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(param);
            else
                Model = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(" ");

            return View(Model);
        }

        /// <summary>
        /// Función que se encarga de actualizar el seguro de acuerdo al Id o de crearlo si no existe
        /// </summary>
        /// <param name="insuranceId">Id ?</param>
        /// <returns>La vista con el modelo actuelizado</returns>
        public virtual ActionResult InsuranceUpsert(string insuranceId)
        {
            InsuranceModel model = new InsuranceModel();
            if (!string.IsNullOrEmpty(insuranceId))
            {
                List<InsuranceModel> modelList = new List<InsuranceModel>();
                modelList = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(insuranceId.ToString());
                model = modelList.FirstOrDefault();
            }           

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                model = GetInsuranceInfoRequestModel();
                model.CategoryId = Convert.ToInt32(insuranceId);
                int idReturn = SaludGuruProfile.Manager.Controller.Insurance.Upsert(model);

                //redirect to update page
                if (string.IsNullOrEmpty(model.CategoryId.ToString()))
                    return RedirectToAction(MVC.Insurance.ActionNames.InsuranceUpsert, MVC.Insurance.Name);
                else
                    return RedirectToAction(MVC.Insurance.ActionNames.InsuranceList, MVC.Insurance.Name);
            }
            
            return View(model);
        }

        /// <summary>
        /// Obtiene la información desde el request 
        /// </summary>
        /// <returns>Model de seguro</returns>
        private InsuranceModel GetInsuranceInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                InsuranceModel oReturn = new InsuranceModel()
                {
                    Name = Request["Name"].ToString(),
                    LastModify = DateTime.Now,
                };
                return oReturn;
            }
            return null;
        }
    }
}