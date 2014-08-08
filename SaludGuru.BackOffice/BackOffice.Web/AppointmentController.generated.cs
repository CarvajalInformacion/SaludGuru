// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace BackOffice.Web.Controllers
{
    public partial class AppointmentController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AppointmentController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AppointmentController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Day()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Day);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Week()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Week);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult List()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Month()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Month);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Detail()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Detail);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AppointmentController Actions { get { return MVC.Appointment; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Appointment";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Appointment";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Day = "Day";
            public readonly string Week = "Week";
            public readonly string List = "List";
            public readonly string Month = "Month";
            public readonly string Detail = "Detail";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Day = "Day";
            public const string Week = "Week";
            public const string List = "List";
            public const string Month = "Month";
            public const string Detail = "Detail";
        }


        static readonly ActionParamsClass_Day s_params_Day = new ActionParamsClass_Day();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Day DayParams { get { return s_params_Day; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Day
        {
            public readonly string Date = "Date";
            public readonly string SelectedOffice = "SelectedOffice";
        }
        static readonly ActionParamsClass_Week s_params_Week = new ActionParamsClass_Week();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Week WeekParams { get { return s_params_Week; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Week
        {
            public readonly string Date = "Date";
            public readonly string SelectedOffice = "SelectedOffice";
        }
        static readonly ActionParamsClass_List s_params_List = new ActionParamsClass_List();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_List ListParams { get { return s_params_List; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_List
        {
            public readonly string Date = "Date";
            public readonly string SelectedOffice = "SelectedOffice";
        }
        static readonly ActionParamsClass_Month s_params_Month = new ActionParamsClass_Month();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Month MonthParams { get { return s_params_Month; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Month
        {
            public readonly string Date = "Date";
            public readonly string SelectedOffice = "SelectedOffice";
        }
        static readonly ActionParamsClass_Detail s_params_Detail = new ActionParamsClass_Detail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Detail DetailParams { get { return s_params_Detail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Detail
        {
            public readonly string UpsertAction = "UpsertAction";
            public readonly string Date = "Date";
            public readonly string AppointmentPublicId = "AppointmentPublicId";
            public readonly string ReturnUrl = "ReturnUrl";
            public readonly string AppointmentPublicIdToDuplicate = "AppointmentPublicIdToDuplicate";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AppointmentController : BackOffice.Web.Controllers.AppointmentController
    {
        public T4MVC_AppointmentController() : base(Dummy.Instance) { }

        [NonAction]
        partial void DayOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string Date, string SelectedOffice);

        [NonAction]
        public override System.Web.Mvc.ActionResult Day(string Date, string SelectedOffice)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Day);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Date", Date);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SelectedOffice", SelectedOffice);
            DayOverride(callInfo, Date, SelectedOffice);
            return callInfo;
        }

        [NonAction]
        partial void WeekOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string Date, string SelectedOffice);

        [NonAction]
        public override System.Web.Mvc.ActionResult Week(string Date, string SelectedOffice)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Week);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Date", Date);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SelectedOffice", SelectedOffice);
            WeekOverride(callInfo, Date, SelectedOffice);
            return callInfo;
        }

        [NonAction]
        partial void ListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string Date, string SelectedOffice);

        [NonAction]
        public override System.Web.Mvc.ActionResult List(string Date, string SelectedOffice)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Date", Date);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SelectedOffice", SelectedOffice);
            ListOverride(callInfo, Date, SelectedOffice);
            return callInfo;
        }

        [NonAction]
        partial void MonthOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string Date, string SelectedOffice);

        [NonAction]
        public override System.Web.Mvc.ActionResult Month(string Date, string SelectedOffice)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Month);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Date", Date);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SelectedOffice", SelectedOffice);
            MonthOverride(callInfo, Date, SelectedOffice);
            return callInfo;
        }

        [NonAction]
        partial void DetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string UpsertAction, string Date, string AppointmentPublicId, string ReturnUrl, string AppointmentPublicIdToDuplicate);

        [NonAction]
        public override System.Web.Mvc.ActionResult Detail(string UpsertAction, string Date, string AppointmentPublicId, string ReturnUrl, string AppointmentPublicIdToDuplicate)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Detail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "UpsertAction", UpsertAction);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Date", Date);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "AppointmentPublicId", AppointmentPublicId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "ReturnUrl", ReturnUrl);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "AppointmentPublicIdToDuplicate", AppointmentPublicIdToDuplicate);
            DetailOverride(callInfo, UpsertAction, Date, AppointmentPublicId, ReturnUrl, AppointmentPublicIdToDuplicate);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
