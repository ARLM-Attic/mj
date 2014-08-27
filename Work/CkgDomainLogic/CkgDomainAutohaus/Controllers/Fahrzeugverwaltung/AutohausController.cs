﻿using System.Web.Mvc;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Autohaus.ViewModels;
using DocumentTools.Services;
using Telerik.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Autohaus-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausController
    {
        public FahrzeugverwaltungViewModel FahrzeugverwaltungViewModel { get { return GetViewModel<FahrzeugverwaltungViewModel>(); } }

        [CkgApplication]
        public ActionResult Fahrzeugverwaltung()
        {
            FahrzeugverwaltungViewModel.DataInit();

            return View(FahrzeugverwaltungViewModel);
        }

        [GridAction]
        public ActionResult FahrzeugeAjaxBinding()
        {
            return View(new GridModel(FahrzeugverwaltungViewModel.FahrzeugeFiltered));
        }

        [HttpPost]
        public ActionResult FilterFahrzeugeGrid(string filterValue, string filterColumns)
        {
            FahrzeugverwaltungViewModel.FilterFahrzeuge(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FahrzeugCreate()
        {
            ModelState.Clear();

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/FahrzeugCreate", FahrzeugverwaltungViewModel.FahrzeugCreate());
        }

        [HttpPost]
        public ActionResult FahrzeugDelete(int id)
        {
            FahrzeugverwaltungViewModel.FahrzeugDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FahrzeugCreateSave(Fahrzeug model)
        {
            if (ModelState.IsValid)
                model = FahrzeugverwaltungViewModel.FahrzeugAdd(model, ModelState.AddModelError);

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/FahrzeugCreate", model);
        }

        [HttpPost]
        public ActionResult FahrzeugEditSave(Fahrzeug model)
        {
            if (ModelState.IsValid)
                model = FahrzeugverwaltungViewModel.FahrzeugSave(model, ModelState.AddModelError);

            return PartialView("Fahrzeugverwaltung/Fahrzeugakte/Partial/Uebersicht/FahrzeugDetails", model);
        }

        #region Export

        public ActionResult ExportFahrzeugeFilteredExcel(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugverwaltungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAndSendAsResponse("Fahrzeuge", dt);

            return new EmptyResult();
        }

        public ActionResult ExportFahrzeugeFilteredPdf(int page, string orderBy, string filterBy)
        {
            var dt = FahrzeugverwaltungViewModel.FahrzeugeFiltered.GetGridFilteredDataTable(orderBy, filterBy, LogonContext.CurrentGridColumns);
            new ExcelDocumentFactory().CreateExcelDocumentAsPDFAndSendAsResponse("Fahrzeuge", dt, landscapeOrientation: true);

            return new EmptyResult();
        }

        #endregion
    }
}