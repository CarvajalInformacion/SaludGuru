﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string PublicUserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public bool? Gender { get; set; }
        public List<UserInfo> ExtraData { get; set; }
        public List<UserProvider> UserLogins { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime Create { get; set; }
    }
}
