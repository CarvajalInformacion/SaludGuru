using System.Web.Mvc;

namespace MarketPlace.Web.Areas.Mobile
{
    public class MobileAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mobile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
        }
    }
}