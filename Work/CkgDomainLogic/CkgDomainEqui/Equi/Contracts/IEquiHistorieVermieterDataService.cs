﻿using System.Collections.Generic;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiHistorieVermieterDataService
    {
        EquiHistorieSuchparameter Suchparameter { get; set; }

        List<EquiHistorieInfoVermieter> HistorieInfos { get; }

        void MarkForRefreshHistorieInfos();

        EquiHistorieVermieter GetEquiHistorie(string equiNr, string meldungsNr);

        byte[] GetHistorieAsPdf(string equiNr, string meldungsNr);

        
        #region Fahrzeug Anforderungen
        
        IEnumerable<FahrzeugAnforderung> FahrzeugAnforderungenLoad(string fahrgestellnummer);
        void FahrzeugAnforderungSave(FahrzeugAnforderung item);
        IEnumerable<SelectItem> FahrzeugAnforderungenLoadDocTypes();

        #endregion
    }
}
