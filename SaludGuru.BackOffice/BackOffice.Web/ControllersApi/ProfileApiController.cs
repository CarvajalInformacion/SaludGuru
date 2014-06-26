using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class ProfileApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Profile.ProfileSearchModel> ProfileSearch
            (string SearchCriteria, int PageNumber, int RowCount)
        {
            int oTotalCount;
            List<SaludGuruProfile.Manager.Models.Profile.ProfileModel> SearchProfile =
                SaludGuruProfile.Manager.Controller.Profile.ProfileSearch
                    (SearchCriteria == null ? string.Empty : SearchCriteria,
                    PageNumber,
                    RowCount,
                    out oTotalCount);

            if (SearchProfile != null && SearchProfile.Count > 0)
            {
                List<BackOffice.Models.Profile.ProfileSearchModel> oReturn = SearchProfile.
                    Select(x => new BackOffice.Models.Profile.ProfileSearchModel()
                    {
                        SearchProfileCount = oTotalCount,
                        ProfilePublicId = x.ProfilePublicId,
                        Name = x.Name + " " + x.LastName,
                        ProfileStatus = x.ProfileStatus.ToString(),
                        Certified = x.ProfileInfo.
                            Where(y => y.ProfileInfoType == SaludGuruProfile.Manager.Models.enumProfileInfoType.IsCertified).
                            Select(y => !string.IsNullOrEmpty(y.Value) && y.Value.Trim().ToLower() == "true" ? "true" : "false").
                            DefaultIfEmpty("false").FirstOrDefault(),
                        Email = x.ProfileInfo.
                            Where(y => y.ProfileInfoType == SaludGuruProfile.Manager.Models.enumProfileInfoType.Email).
                            Select(y => y.Value).
                            DefaultIfEmpty(string.Empty).FirstOrDefault(),
                    }).ToList();

                return oReturn;
            }
            else
            {
                return new List<Models.Profile.ProfileSearchModel>();
            }
        }
    }
}
