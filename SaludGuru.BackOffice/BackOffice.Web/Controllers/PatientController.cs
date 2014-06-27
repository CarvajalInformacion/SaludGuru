using BackOffice.Models.General;
using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models;
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

        public virtual ActionResult PatientCreate()
        {

            return View();
        }

        public virtual ActionResult PatientEdit()
        {
            PatientUpSertModel Model = new PatientUpSertModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                //PatientModel ProfileToCreate = GetProfileInfoRequestModel();

                ////create profile 
                //string oProfilePublicId = SaludGuruProfile.Manager.Controller.Profile.UpsertProfileInfo
                //    (ProfileToCreate);

                ////get updated profile info
                //Model.Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId);
            }
            return View(Model);
        }

        private PatientModel GetPatientInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                 && bool.Parse(Request["UpsertAction"]))
            {
                PatientModel oReturn = new PatientModel()
                {
                    PatientPublicId = Request["PatientPublicId"],
                    Name = Request["Name"].ToString(),
                    LastModify = DateTime.Now,
                    LastName = Request["LastName"].ToString(),

                    PatientInfo = new List<PatientInfoModel>() 
                    { 
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_IdentificationNumber"])?0:int.Parse(Request["CatId_IdentificationNumber"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.IdentificationNumber,
                            Value = Request["IdentificationNumber"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Email"])?0:int.Parse(Request["CatId_Email"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Email,
                            Value = Request["Email"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_IdentificationNumber"])?0:int.Parse(Request["CatId_IdentificationNumber"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.IdentificationNumber,
                            Value = Request["IdentificationNumber"].ToString(),
                        },
                    }
                };

                return oReturn;
            }
            return null;
        }
    }
}