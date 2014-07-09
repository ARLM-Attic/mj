﻿using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IBriefVersandDataService : ICkgGeneralDataService
    {
        List<VersandGrund> GetVersandgruende(bool endgVersand);

        string SaveVersandBeauftragung(IEnumerable<VersandAuftragsAnlage> versandAuftraege);


        #region not used yet
        
        Fahrzeug GetFahrzeugBriefForVin(string vin);
        IEnumerable<Fahrzeug> GetFahrzeugBriefe(Fahrzeug fahrzeugBriefParameter);
        
        #endregion
    }
}
