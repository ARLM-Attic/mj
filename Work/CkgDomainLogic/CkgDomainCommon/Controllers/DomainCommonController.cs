﻿using System.Web.Mvc;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Contracts;
using GeneralTools.Services;
using GeneralTools.Models;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Standard Adressenpflege Dialog 
    /// </summary>
    public class DomainCommonController : CkgDomainController
    {
        private readonly int _portalType = 3;   // 1 = Portal, 2 = Services, 3 = ServicesMvc

        public override string DataContextKey { get { return GetDataContextKey<AdressenPflegeViewModel>(); } }

        public CkgCommonViewModel CkgCommonViewModel { get { return GetViewModel<CkgCommonViewModel>(); } }

        
        public DomainCommonController(IAppSettings appSettings, ILogonContextDataService logonContext, IAdressenDataService adressenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(AdressenPflegeViewModel, appSettings, logonContext, adressenDataService);
            InitViewModel(CkgCommonViewModel, appSettings, logonContext);
        }


        public ActionResult LogPageVisit(string appID)
        {
            if (appID.IsNullOrEmpty() || LogonContext == null || LogonContext.User == null || LogonContext.Customer == null)
                return new EmptyResult();

            var logService = new LogService();
            logService.LogPageVisit(appID.ToInt(), LogonContext.User.UserID, LogonContext.Customer.CustomerID, LogonContext.Customer.KUNNR.ToInt(), _portalType);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppFavoritesEditModeSwitch()
        {
            LogonContext.AppFavoritesEditMode = !LogonContext.AppFavoritesEditMode;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AppFavoritesEditSwitchOneFavorite(int appID)
        {
            return Json(new { isFavorite = LogonContext.AppFavoritesEditSwitchOneFavorite(appID) });
        }

        [HttpPost]
        public ActionResult AppFavoriteButtonsCoreRefresh()
        {
            return PartialView("AppFavorites/AppFavoriteButtonsCore", LogonContext);
        }

        [CkgApplication]
        public ActionResult Index()
        {
            if (LogonContext != null && LogonContext.Customer != null && LogonContext.Customer.MvcSelectionUrl.IsNotNullOrEmpty())
                return RedirectPermanent(LogonContext.Customer.MvcSelectionUrl);

            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }

        [CkgApplication]
        public ActionResult Kontakt()
        {
            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult UserMessages()
        {
            return View(CkgCommonViewModel);
        }

        [CkgApplication]
        public ActionResult Impressum()
        {
            return View();
        }

        [CkgApplication]
        public ActionResult AdressenPflege(string kennung, string kdnr)
        {
            AdressenPflegeViewModel.DataInit(kennung ?? "VERSANDADRESSE", kdnr ?? LogonContext.KundenNr);

            return View(AdressenPflegeViewModel);
        }
    }
}
