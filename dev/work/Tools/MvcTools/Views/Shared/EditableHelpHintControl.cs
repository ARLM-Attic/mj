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
    using MvcTools;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditableHelpHintControl.cshtml")]
    public class _Page_Views_Shared_EditableHelpHintControl_cshtml : System.Web.Mvc.WebViewPage<MvcTools.Models.ContentEntity >
    {
#line hidden

        public _Page_Views_Shared_EditableHelpHintControl_cshtml()
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




WriteLiteral("\r\n");


Write(Html.Once("EditableContent_Once",item => new System.Web.WebPages.HelperResult(__razor_template_writer => {


WriteLiteralTo(@__razor_template_writer, " ");


         WriteTo(@__razor_template_writer, Html.Partial("EditableContent_Once"));


                                                                                    })));

WriteLiteral("\r\n\r\n<img id=\"editableContentImage_");


                          Write(Model.ID);

WriteLiteral("\" src=\"/Shared/Images/help.png\" alt=\"\" class=\"show-tooltip\" \r\n");


      if (MvcSettings.CurrentUserIsEditor)
     {

WriteLiteral("         ");

WriteLiteral("onclick = \"ShowEditableContentEditor(\'");


                                                 Write(Model.ID);

WriteLiteral("\')\" style = \"cursor: pointer\"");


                                                                                                   ;
     }

WriteLiteral("     />\r\n<span id=\"editableContentText_");


                          Write(Model.ID);

WriteLiteral("\" class=\"show-tooltip-text\">");


                                                                Write(Html.Raw(@Model.Text));

WriteLiteral("</span>\r\n");


        }
    }
}
