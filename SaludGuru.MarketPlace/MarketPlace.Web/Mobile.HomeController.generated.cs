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
namespace MarketPlace.Web.Areas.Mobile.Controllers
{
    public partial class HomeController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected HomeController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult ChangeCity()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeCity);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public HomeController Actions { get { return MVC.Mobile.Home; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Mobile";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Home";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Home";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string ChangeCity = "ChangeCity";
            public readonly string LegalTerms = "LegalTerms";
            public readonly string ConditionsAndRestrictions = "ConditionsAndRestrictions";
            public readonly string FAQ = "FAQ";
            public readonly string Contact = "Contact";
            public readonly string LogOutUser = "LogOutUser";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string ChangeCity = "ChangeCity";
            public const string LegalTerms = "LegalTerms";
            public const string ConditionsAndRestrictions = "ConditionsAndRestrictions";
            public const string FAQ = "FAQ";
            public const string Contact = "Contact";
            public const string LogOutUser = "LogOutUser";
        }


        static readonly ActionParamsClass_ChangeCity s_params_ChangeCity = new ActionParamsClass_ChangeCity();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChangeCity ChangeCityParams { get { return s_params_ChangeCity; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChangeCity
        {
            public readonly string NewCityId = "NewCityId";
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
                public readonly string ConditionsAndRestrictions = "ConditionsAndRestrictions";
                public readonly string Contact = "Contact";
                public readonly string FAQ = "FAQ";
                public readonly string Index = "Index";
                public readonly string LegalTerms = "LegalTerms";
            }
            public readonly string ConditionsAndRestrictions = "~/Areas/Mobile/Views/Home/ConditionsAndRestrictions.cshtml";
            public readonly string Contact = "~/Areas/Mobile/Views/Home/Contact.cshtml";
            public readonly string FAQ = "~/Areas/Mobile/Views/Home/FAQ.cshtml";
            public readonly string Index = "~/Areas/Mobile/Views/Home/Index.cshtml";
            public readonly string LegalTerms = "~/Areas/Mobile/Views/Home/LegalTerms.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_HomeController : MarketPlace.Web.Areas.Mobile.Controllers.HomeController
    {
        public T4MVC_HomeController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ChangeCityOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int NewCityId);

        [NonAction]
        public override System.Web.Mvc.ActionResult ChangeCity(int NewCityId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeCity);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "NewCityId", NewCityId);
            ChangeCityOverride(callInfo, NewCityId);
            return callInfo;
        }

        [NonAction]
        partial void LegalTermsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LegalTerms()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LegalTerms);
            LegalTermsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ConditionsAndRestrictionsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ConditionsAndRestrictions()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ConditionsAndRestrictions);
            ConditionsAndRestrictionsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void FAQOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult FAQ()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.FAQ);
            FAQOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ContactOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Contact()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Contact);
            ContactOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void LogOutUserOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LogOutUser()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOutUser);
            LogOutUserOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
