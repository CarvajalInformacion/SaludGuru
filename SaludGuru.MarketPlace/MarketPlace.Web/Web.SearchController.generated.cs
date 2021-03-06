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
namespace MarketPlace.Web.Areas.Web.Controllers
{
    public partial class SearchController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SearchController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected SearchController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SearchController Actions { get { return MVC.Web.Search; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Web";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Search";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Search";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string SpecialtyName = "SpecialtyName";
            public readonly string TreatmentName = "TreatmentName";
            public readonly string InsuranceName = "InsuranceName";
            public readonly string CityName = "CityName";
            public readonly string Query = "Query";
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
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Areas/Web/Views/Search/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_SearchController : MarketPlace.Web.Areas.Web.Controllers.SearchController
    {
        public T4MVC_SearchController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string SpecialtyName, string TreatmentName, string InsuranceName, string CityName, string Query);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(string SpecialtyName, string TreatmentName, string InsuranceName, string CityName, string Query)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "SpecialtyName", SpecialtyName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "TreatmentName", TreatmentName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "InsuranceName", InsuranceName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "CityName", CityName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "Query", Query);
            IndexOverride(callInfo, SpecialtyName, TreatmentName, InsuranceName, CityName, Query);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
