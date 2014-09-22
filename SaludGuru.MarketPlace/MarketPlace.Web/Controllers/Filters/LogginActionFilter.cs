using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlace.Web.Controllers.Filters
{
    public class LogginActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["controller"] == "Appointment" &&
                filterContext.RouteData.Values["action"] == "Index" &&
                !MarketPlace.Models.General.SessionModel.UserIsLoggedIn)
            {
                filterContext.HttpContext.Response.Redirect("/Profile/Index?ProfilePublicId=" + filterContext.HttpContext.Request["ProfilePublicId"]);
            }
        }

        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

        }
    }
}