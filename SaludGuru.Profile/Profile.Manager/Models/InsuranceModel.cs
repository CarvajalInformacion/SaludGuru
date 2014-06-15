using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models
{
    public class InsuranceModel
    {
        public int InsuranceId { get; set; }
        public string Name { get; set; }
        public enumCategoryType CategoryType { get { return enumCategoryType.Insurance; } }
    }
}
