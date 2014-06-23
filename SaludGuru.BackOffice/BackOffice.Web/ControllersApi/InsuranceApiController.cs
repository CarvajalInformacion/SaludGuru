using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class InsuranceApiController : BaseApiController
    {
        [HttpGet]
        [HttpPost]
        public List<SaludGuruProfile.Manager.Models.General.InsuranceModel> Read(string param)
        {
            List<InsuranceModel> resultList = new List<SaludGuruProfile.Manager.Models.General.InsuranceModel>();
            //if (true)
            //{
                
            //}
            resultList = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin("o");
           
            return resultList;
        }
    }
}
