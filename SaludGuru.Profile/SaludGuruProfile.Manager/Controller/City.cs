using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Controller
{
    public class City
    {
        public static List<CityModel> CityGetAll()
        {
            return DAL.Controller.ProfileDataController.Instance.CityGetAll();
        }
    }
}
