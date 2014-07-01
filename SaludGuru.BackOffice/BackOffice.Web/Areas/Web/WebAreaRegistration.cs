using System.Web.Mvc;

namespace BackOffice.Web.Areas.Web
{
    public class WebAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Web";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}