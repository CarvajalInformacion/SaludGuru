using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }
        public enumUserInfoType Infotype { get; set; }
        public string Value { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime Create { get; set; }
    }
}
