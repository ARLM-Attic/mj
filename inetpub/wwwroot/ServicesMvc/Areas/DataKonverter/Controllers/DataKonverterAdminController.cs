﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DataKonverter.Contracts;
using CkgDomainLogic.DataKonverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.DataKonverter.Controllers
{
    public class DataKonverterAdminController : CkgDomainController 
    {

        public override string DataContextKey { get { return GetDataContextKey<KroschkeDataKonverterViewModel>(); } }

        public KroschkeDataKonverterViewModel ViewModel
        {
            get { return GetViewModel<KroschkeDataKonverterViewModel>(); }
            set { SetViewModel(value); }
        }

        public DataKonverterAdminController(IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
            : base(appSettings, logonContext)
        {
            if (IsInitialRequestOf("Index"))
                ViewModel = null;

            InitViewModelExpicit(ViewModel, appSettings, logonContext, dataKonverterDataService);
        }

        private void InitViewModelExpicit(KroschkeDataKonverterViewModel vm, IAppSettings appSettings, ILogonContextDataService logonContext, IDataKonverterDataService dataKonverterDataService)
        {
            InitViewModel(vm, appSettings, logonContext, dataKonverterDataService);
            InitModelStatics();
        }

        void InitModelStatics()
        {
            //CkgDomainLogic.Autohaus.Models.Zulassungsdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
            //CkgDomainLogic.Autohaus.Models.Fahrzeugdaten.GetZulassungViewModel = GetViewModel<KroschkeZulassungViewModel>;
        }

        [CkgApplication]
        public ActionResult Index()
        {
            // return View("Partial/Konfiguration");
            // return View("Test");
            return View(ViewModel);
        }

        [CkgApplication]
        public ActionResult Prozessauswahl()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Konfiguration()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Testimport()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult Abschluss()
        {
            return View();
        }


        #region Ajax

        [HttpPost]
        public JsonResult LiveTransform(string input, string func)
        {
            var output = "";
            output = string.Format("#{0}", input);

            return Json(new { Output = output });
        }

        #endregion

    }
}