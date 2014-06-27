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
    public partial class PatientController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PatientController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected PatientController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult Upsert()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Upsert);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult PatientGetAllByPublicPatientId()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PatientGetAllByPublicPatientId);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PatientController Actions { get { return MVC.Patient; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Patient";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Patient";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string List = "List";
            public readonly string Upsert = "Upsert";
            public readonly string PatientGetAllByPublicPatientId = "PatientGetAllByPublicPatientId";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string List = "List";
            public const string Upsert = "Upsert";
            public const string PatientGetAllByPublicPatientId = "PatientGetAllByPublicPatientId";
        }


        static readonly ActionParamsClass_Upsert s_params_Upsert = new ActionParamsClass_Upsert();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Upsert UpsertParams { get { return s_params_Upsert; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Upsert
        {
            public readonly string PatientPublicId = "PatientPublicId";
        }
        static readonly ActionParamsClass_PatientGetAllByPublicPatientId s_params_PatientGetAllByPublicPatientId = new ActionParamsClass_PatientGetAllByPublicPatientId();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_PatientGetAllByPublicPatientId PatientGetAllByPublicPatientIdParams { get { return s_params_PatientGetAllByPublicPatientId; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_PatientGetAllByPublicPatientId
        {
            public readonly string PublicPatientId = "PublicPatientId";
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
    public partial class T4MVC_PatientController : BackOffice.Web.Controllers.PatientController
    {
        public T4MVC_PatientController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult List()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
            ListOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void UpsertOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string PatientPublicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Upsert(string PatientPublicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Upsert);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PatientPublicId", PatientPublicId);
            UpsertOverride(callInfo, PatientPublicId);
            return callInfo;
        }

        [NonAction]
        partial void PatientGetAllByPublicPatientIdOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string PublicPatientId);

        [NonAction]
        public override System.Web.Mvc.ActionResult PatientGetAllByPublicPatientId(string PublicPatientId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PatientGetAllByPublicPatientId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "PublicPatientId", PublicPatientId);
            PatientGetAllByPublicPatientIdOverride(callInfo, PublicPatientId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
