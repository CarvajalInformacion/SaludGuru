using BackOffice.Models.General;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class PatientController : BaseController
    {
        public virtual ActionResult Search(string PatientPublicId, string SearchParam)
        {
            return View();
        }
        
        public virtual ActionResult Upsert(string PatientPublicId)
        {
            return View();
        }

        public virtual ActionResult PatientGetAllByPublicPatientId(string PublicPatientId)
        {
            PatientModel modelToReturn = new PatientModel();
            modelToReturn = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PublicPatientId);
            return View(modelToReturn);
        }
    }
}