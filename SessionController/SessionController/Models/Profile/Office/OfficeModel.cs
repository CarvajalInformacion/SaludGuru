using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController.Models.Profile.Office
{
    public class OfficeModel
    {
        public string OfficePublicId { get; set; }
        public SessionController.Models.Profile.General.CityModel City { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public List<OfficeInfoModel> OfficeInfo { get; set; }
        public List<ScheduleAvailableModel> ScheduleAvailable { get; set; }

        public List<SessionController.Models.Profile.General.TreatmentModel> RelatedTreatment { get; set; }
        public SessionController.Models.Profile.General.TreatmentModel DefaultTreatment { get; set; }
    }
}
