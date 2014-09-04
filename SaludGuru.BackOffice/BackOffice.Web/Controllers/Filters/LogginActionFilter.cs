using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackOffice.Web.Controllers.Filters
{
    public class LogginActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if (!BackOffice.Models.General.SessionModel.UserIsLoggedIn)
            {
                bool DoRedirect = false;

                DoRedirect = !((filterContext.RouteData.Values["controller"].ToString() == "Home" &&
                             filterContext.RouteData.Values["action"].ToString() == "Index") ||
                             (filterContext.RouteData.Values["controller"].ToString() == "ExternalAppointment"));

                if (DoRedirect)
                {
                    filterContext.HttpContext.Response.Redirect("/");
                }
            }
        }

        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

        }
    }
}