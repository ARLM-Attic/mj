﻿using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugSperrenVerschiebenDataService : ICkgGeneralDataService
    {
        List<Domaenenfestwert> GetFarben();

        List<FahrzeuguebersichtPDI> GetPDIStandorte();

        List<Fahrzeuguebersicht> GetFahrzeuge();

        int FahrzeugeSperren(bool sperren, string sperrText, ref List<Fahrzeuguebersicht> fahrzeuge);

        int FahrzeugeVerschieben(string zielPdi, ref List<Fahrzeuguebersicht> fahrzeuge);

        int FahrzeugeTexteErfassen(string bemerkungIntern, string bemerkungExtern, ref List<Fahrzeuguebersicht> fahrzeuge);
    }
}
