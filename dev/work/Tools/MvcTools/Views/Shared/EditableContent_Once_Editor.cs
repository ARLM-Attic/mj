﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcTools.Views.Shared
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditableContent_Once_Editor.cshtml")]
    public class _Page_Views_Shared_EditableContent_Once_Editor_cshtml : System.Web.Mvc.WebViewPage<MvcTools.Models.ContentEntity >
    {
#line hidden

        public _Page_Views_Shared_EditableContent_Once_Editor_cshtml()
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


WriteLiteral("\r\n\r\n");


Write(Html.Telerik().Editor().Name("EditableContent_Editor").Tools(tools => tools.ViewHTML()).Value(@Model.Text).HtmlAttributes(new { style = "height:600px;" } ));

WriteLiteral("\r\n<p>\r\n    <button type=\"submit\" class=\"t-button t-state-default\" onclick=\"SaveEd" +
"itableContentText(\'");


                                                                                        Write(Model.ID);

WriteLiteral("\')\">Speichern</button>\r\n    <button type=\"submit\" class=\"t-button t-state-default" +
"\" onclick=\"CancelEditableContentText()\">Abbrechen</button>\r\n</p>\r\n");


        }
    }
}
