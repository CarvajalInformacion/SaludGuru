using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Models.Office
{
    public class OfficeInfoModel
    {
        public int OfficeInfoId { get; set; }
        public enumOfficeInfoType OfficeInfoType { get; set; }
        public string Value { get; set; }
        public string LargeValue { get; set; }
    }
}
