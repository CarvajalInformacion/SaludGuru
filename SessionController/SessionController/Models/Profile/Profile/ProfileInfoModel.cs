using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController.Models.Profile.Profile
{
    public class ProfileInfoModel
    {
        public int ProfileInfoId { get; set; }
        public enumProfileInfoType? ProfileInfoType { get; set; }
        public string Value { get; set; }
        public string LargeValue { get; set; }
    }
}
