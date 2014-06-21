using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Office
{
    public class OfficeModel
    {
        public string OfficePublicId { get; set; }
        public CityModel City { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }

        public List<OfficeInfoModel> OfficeInfo { get; set; }
        public List<ScheduleAvailableModel> ScheduleAvailable { get; set; }

        public List<TreatmentOfficeModel> RelatedTreatment { get; set; }
        public TreatmentModel DefaultTreatment { get; set; }
    }
}
