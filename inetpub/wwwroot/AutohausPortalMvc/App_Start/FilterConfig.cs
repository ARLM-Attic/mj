﻿using System.Web.Mvc;
using CkgDomainLogic.General.Controllers;
using MvcTools.Web;

namespace AutohausPortalMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute(null));

            filters.Add(new ValidateInputAttribute(false));

            filters.Add(new CkgAuthorizeAttribute());
        }
    }
}