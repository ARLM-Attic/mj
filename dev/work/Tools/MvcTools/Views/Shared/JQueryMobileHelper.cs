﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.296
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/JQueryMobileHelper.cshtml")]
    public class _Page_Views_Shared_JQueryMobileHelper_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public _Page_Views_Shared_JQueryMobileHelper_cshtml()
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
WriteLiteral("<script type=\"text/javascript\">\r\n\r\n    // Enthält allg. Hilfsfunktionen für JQuer" +
"y Mobile (JQM) Anwendungen\r\n\r\n    // Blendet den JQM-BusyIndicator ein\r\n    func" +
"tion ShowBusyIndicator() {\r\n        $.mobile.loading(\'show\');\r\n    }\r\n\r\n    // B" +
"lendet den JQM-BusyIndicator wieder aus\r\n    function HideBusyIndicator() {\r\n   " +
"     $.mobile.loading(\'hide\');\r\n    }\r\n\r\n    // UI-Refresh für eine Listenelemen" +
"t, aufzurufen nach clientseitiger ListItem-Änderung\r\n    function RefreshListUI(" +
"listid) {\r\n        var lst = $(\"#\" + listid);\r\n        lst.listview(\'refresh\');\r" +
"\n    }\r\n\r\n    // UI-Refresh für eine Listenelement, aufzurufen nach clientseitig" +
"er ListItem-Änderung\r\n    function RefreshTableUI(tableid) {\r\n        var tbl = " +
"$(\"#\" + tableid);\r\n        tbl.table(\'refresh\');\r\n    }\r\n\r\n    // UI-Refresh für" +
" eine Select-Auswahlliste, aufzurufen nach clientseitiger Options-Änderung\r\n    " +
"function RefreshSelectUI(selectid) {\r\n        var sel = $(\"#\" + selectid);\r\n    " +
"    sel.selectmenu(\'refresh\');\r\n    }\r\n\r\n    // UI-Refresh für eine Checkbox, au" +
"fzurufen nach clientseitiger Auswahl-Änderung\r\n    function RefreshCheckboxUI(ch" +
"eckid) {\r\n        $(\"#\" + checkid).checkboxradio(\'refresh\');\r\n    }\r\n\r\n    // UI" +
"-Refresh für eine Radiobutton-Gruppe, aufzurufen nach clientseitiger Auswahl-Änd" +
"erung\r\n    function RefreshRadiobuttonUI(radioname) {\r\n        $(\"input[name=\" +" +
" radioname + \"]\").checkboxradio(\'refresh\');\r\n    }\r\n\r\n    // UI-Refresh für die " +
"im angegebenen Container/DIV enthaltenen Controls (z.B. Radiobuttons in einer Ta" +
"ble-Zelle)\r\n    function RefreshNestedControlsUI(containerid) {\r\n        $(\"#\" +" +
" containerid).trigger(\'create\');\r\n    }\r\n\r\n    // Auswahl der angegebenen Select" +
"-Option (ggf. inkl. UI-Refresh)\r\n    function SetDropdownValue(dropdownid, wert," +
" withUIRefresh) {\r\n        var ddl = $(\"#\" + dropdownid);\r\n        ddl.val(wert)" +
".attr(\'selected\', true).siblings(\'option\').removeAttr(\'selected\');\r\n        if (" +
"withUIRefresh == true) {\r\n            RefreshSelectUI(dropdownid);\r\n        }\r\n " +
"   }\r\n\r\n    // Ermitteln der aktuell ausgwählten Select-Option\r\n    function Get" +
"DropdownValue(dropdownid) {\r\n        return $(\"#\" + dropdownid + \" option:select" +
"ed\").val();\r\n    }\r\n\r\n    // Setzen des angegebenen Wertes der Checkbox (ggf. in" +
"kl. UI-Refresh)\r\n    function SetCheckboxValue(checkid, checked, withUIRefresh) " +
"{\r\n        $(\"#\" + checkid).attr(\'checked\', checked);\r\n        if (withUIRefresh" +
" == true) {\r\n            RefreshCheckboxUI(checkid);\r\n        }\r\n    }\r\n\r\n    //" +
" Setzen des angegebenen Wertes in der Radiobutton-Gruppe (ggf. inkl. UI-Refresh)" +
"\r\n    function SetRadiobuttonValue(radioid, wert, withUIRefresh) {\r\n        if (" +
"wert == \"\") {\r\n            $(\"input[name=\" + radioid + \"]:checked\").attr(\'checke" +
"d\', false);\r\n        } else {\r\n            $(\"input[name=\" + radioid + \"][value=" +
"\" + wert + \"]\").attr(\'checked\', true);\r\n        }\r\n        if (withUIRefresh == " +
"true) {\r\n            RefreshRadiobuttonUI(radioid);\r\n        }\r\n    }\r\n\r\n    // " +
"Ermitteln des gewählten Wertes der Checkbox\r\n    function GetCheckboxValue(check" +
"id) {\r\n        return ($(\"#\" + checkid).attr(\'checked\') == \"checked\");\r\n    }\r\n\r" +
"\n    // Ermitteln des gewählten Wertes der Radiobutton-Gruppe\r\n    function GetR" +
"adiobuttonValue(radioid) {\r\n        return $(\"input[name=\" + radioid + \"]:checke" +
"d\").val();\r\n    }\r\n\r\n    // Input vom Typ \"button\" enablen (inkl. UI-Refresh)\r\n " +
"   function EnableButton(buttonid) {\r\n        var btn = $(\"input[type=button][id" +
"=\" + buttonid + \"]\");\r\n        if (btn.hasClass(\"ui-btn-hidden\")) {\r\n           " +
" btn.button(\'enable\');\r\n            btn.button(\'refresh\');\r\n        }\r\n    }\r\n\r\n" +
"    // Input vom Typ \"button\" disablen (inkl. UI-Refresh)\r\n    function DisableB" +
"utton(buttonid) {\r\n        var btn = $(\"input[type=button][id=\" + buttonid + \"]\"" +
");\r\n        if (btn.hasClass(\"ui-btn-hidden\")) {\r\n            btn.button(\'disabl" +
"e\');\r\n            btn.button(\'refresh\');\r\n        }\r\n    }\r\n\r\n </script>");


        }
    }
}
