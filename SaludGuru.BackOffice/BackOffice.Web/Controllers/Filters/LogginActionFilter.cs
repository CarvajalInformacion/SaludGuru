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
            if (filterContext.RouteData.Values["controller"] != "Home" &&
                filterContext.RouteData.Values["action"] != "Index" &&
                !BackOffice.Models.General.SessionModel.UserIsLoggedIn)
            {
                filterContext.HttpContext.Response.Redirect("/");
            }
        }

        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

        }
    }
}