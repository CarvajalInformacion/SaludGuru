using SessionController.Models.Profile.Autorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.Controller
{
    public class Autorization
    {
        /// <summary>
        /// get autorization for email
        /// </summary>
        /// <param name="UserEmail">email to eval autorize</param>
        /// <returns>list profiles and roles autorized</returns>
        public static List<AutorizationModel> GetEmailAutorization(string UserEmail)
        {
            return DAL.Controller.ProfileDataController.Instance.GetAutorization(UserEmail);
        }
    }
}
