using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Treatment
{
    public class TreatmentUpsertModel
    {
        public List<TreatmentModel> CurrentTreatment { get; set; }
        public TreatmentModel TreatmentInfo { get; set; }
    }
}
