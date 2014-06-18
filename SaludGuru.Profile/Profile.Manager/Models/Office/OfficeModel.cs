using Profile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models.Office
{
    public class OfficeModel
    {
        public string OfficePublicId { get; set; }
        public CityModel City { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public List<OfficeInfoModel> OfficeInfo { get; set; }
        public List<ScheduleAvailableModel> ScheduleAvailable { get; set; }

        public List<TreatmentModel> RelatedTreatment { get; set; }
        public TreatmentModel DefaultTreatment { get; set; }
    }
}
