﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.296
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcTools.Views.Error
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Telerik.Web.Mvc.UI;
    using MvcTools.Web;
    using MvcTools.Data;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Error/Index.cshtml")]
    public class _Page_Views_Error_Index_cshtml : System.Web.Mvc.WebViewPage<Models.ErrorModel>
    {
#line hidden

        public _Page_Views_Error_Index_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {


WriteLiteral(@"
<style type=""text/css"">
    #errorMessage {
        color: red;
        font-weight: normal;
        padding-top: 20px;
    }
</style>

<h2>Fehler</h2>
<br />
<div>
    Wir bitten um Entschuldigung, etwas ist schief gelaufen ...    
</div>
<div id=""errorMessage"">
");


      
        string message = null;
        switch (Model.HttpStatusCode)
        {
            case 404:
                message = "Die angeforderte Seite wurde nicht gefunden."; 
                break;
            case 500:
                message = string.Concat("Folgender Server-Fehler ist aufgetreten:<br /> ", Model.Exception.Message, "<br /><br />", Model.Exception.InnerException == null ? "" : Model.Exception.InnerException.Message);
                break;
        }
    

WriteLiteral("    \r\n    <br/>\r\n    ");


Write(Html.Raw(message));

WriteLiteral("\r\n</div>\r\n\r\n\r\n");


 if (!string.IsNullOrEmpty(Model.OriginControllerName))
{

WriteLiteral("    <br/>");



WriteLiteral("<br/>\r\n");



WriteLiteral("    <div style=\"font-size: 1.2em;\">");


                              Write(Html.ActionLink("Zurück zur Anwendung ...", "Index", Model.OriginControllerName));

WriteLiteral("</div>\r\n");


}


        }
    }
}