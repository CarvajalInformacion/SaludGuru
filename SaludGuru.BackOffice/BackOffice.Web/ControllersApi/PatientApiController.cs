using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{    
    public class PatientApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Patient.PatientSearchModel> PatientSearch
            (string PublicProfileId, string SearchCriteria, int PageNumber, int RowCount)
         {
            int oTotalCount;
            List<MedicalCalendar.Manager.Models.Patient.PatientModel> SearchPatient =
                MedicalCalendar.Manager.Controller.Patient.PatientSearch
                (PublicProfileId,
                SearchCriteria == null ? string.Empty : SearchCriteria,
                PageNumber,
                RowCount,
                out oTotalCount);

            if (SearchPatient != null && SearchPatient.Count > 0)
            {
                List<BackOffice.Models.Patient.PatientSearchModel> oReturn = SearchPatient.
                    Select(x => new BackOffice.Models.Patient.PatientSearchModel()
                    {
                        SearchPatientCount = oTotalCount,
                        PatientPublicId = x.PatientPublicId,
                        Name = x.Name + " " + x.LastName,
                        Identification = x.PatientInfo.
                            Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.IdentificationNumber).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).FirstOrDefault(),
                        Email = x.PatientInfo.
                            Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.Email).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).FirstOrDefault(),
                        Telephone = x.PatientInfo.
                        Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.Telephone).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    }).ToList();

                return oReturn;
            }
            else
            {
                return new List<Models.Patient.PatientSearchModel>();
            }
        }
    }
}