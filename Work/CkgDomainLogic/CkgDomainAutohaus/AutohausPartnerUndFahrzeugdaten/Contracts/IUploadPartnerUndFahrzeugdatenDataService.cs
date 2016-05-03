﻿using System.Collections.Generic;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Contracts
{
    public interface IUploadPartnerUndFahrzeugdatenDataService
    {
        List<IUploadItem> UploadItems { get; set; }

        void LoadTypdaten(IEnumerable<Fahrzeugdaten> fahrzeuge);

        string SaveUploadItems();
    }
}