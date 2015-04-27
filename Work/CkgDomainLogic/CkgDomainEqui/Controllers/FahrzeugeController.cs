﻿using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        private string _dataContextKey = "";

        public override string DataContextKey
        {
            get { return _dataContextKey; }
        }

        public FahrzeugeController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugeDataService fahrzeugeDataService,
            IFehlteilEtikettenDataService fehlteilEtikettenDataService,
            IUploadFahrzeugeinsteuerungDataService uploadFahrzeugeinsteuerungDataService,
            ITreuhandDataService tempTreuhandDataService,
            IFahrzeugvoravisierungDataService tempfahrzeugvoravisierungDataService,
            IDispositionslisteDataService dispositionslisteDataService,
            IZulaufEinsteuerungDataService zulaufEinsteuerungDataService,
            IFahrzeuguebersichtDataService fahrzeuguebersichtDataService
            )
            : base(appSettings, logonContext)
        {
            InitViewModel(AbgemeldeteFahrzeugeViewModel, appSettings, logonContext, fahrzeugeDataService);         
            InitViewModel(FehlteilEtikettenViewModel, appSettings, logonContext, fehlteilEtikettenDataService);
            InitViewModel(UploadFahrzeugeinsteuerungViewModel, appSettings, logonContext,
                uploadFahrzeugeinsteuerungDataService);
            InitViewModel(Zb2BestandSecurityFleetViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitModelStatics();
            InitViewModel(TreuhandbestandViewModel, appSettings, logonContext, fahrzeugeDataService);            
            InitViewModel(UnfallmeldungenViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitViewModel(TreuhandverwaltungViewModel, appSettings, logonContext, tempTreuhandDataService);
            InitViewModel(TreuhandverwaltungViewModel, appSettings, logonContext, tempTreuhandDataService);
            InitViewModel(FahrzeugvoravisierungViewModel, appSettings, logonContext, tempfahrzeugvoravisierungDataService);
            InitViewModel(DispositionslisteViewModel, appSettings, logonContext, dispositionslisteDataService);
            InitViewModel(ZulaufEinsteuerungViewModel, appSettings, logonContext, zulaufEinsteuerungDataService);
            InitViewModel(FahrzeuguebersichtViewModel, appSettings, logonContext, fahrzeuguebersichtDataService);
            InitViewModel(FahrzeuguebersichtViewModel, appSettings, logonContext, fahrzeugeDataService);           
        }

        private void InitModelStatics()
        {
            Zb2BestandSecurityFleetSelektor.GetViewModel =
                GetViewModel<CkgDomainLogic.Fahrzeuge.ViewModels.Zb2BestandSecurityFleetViewModel>;
            TreuhandverwaltungSelektor.GetViewModel =
                GetViewModel<CkgDomainLogic.Fahrzeuge.ViewModels.TreuhandverwaltungViewModel>;
            FahrzeuguebersichtSelektor.GetViewModel =
                GetViewModel<CkgDomainLogic.Fahrzeuge.ViewModels.FahrzeuguebersichtViewModel>;
        }
    }
}
