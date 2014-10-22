
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 1.0
//     Created at 17.04.2008
//     Created from Windows 2000
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using SAP.Connector;

namespace SAPProxy_SixtService
{

  [RfcStructure(AbapName ="ZDAD_ZUL_AUFTR_1" , Length = 744, Length2 = 1488)]
  public class ZDAD_ZUL_AUFTR_1 : SAPStructure
  {
    
    [RfcField(AbapName = "MANDT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 0, Offset2 = 0)]
    [XmlElement("MANDT")]
    public string Mandt
    { 
       get
       {
          return _Mandt;
       }
       set
       {
          _Mandt = value;
       }
    }
    private string _Mandt;

    [RfcField(AbapName = "KUNNR_AG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 3, Offset2 = 6)]
    [XmlElement("KUNNR_AG")]
    public string Kunnr_Ag
    { 
       get
       {
          return _Kunnr_Ag;
       }
       set
       {
          _Kunnr_Ag = value;
       }
    }
    private string _Kunnr_Ag;

    [RfcField(AbapName = "FAHRZEUGIDENT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 17, Length2 = 34, Offset = 13, Offset2 = 26)]
    [XmlElement("FAHRZEUGIDENT")]
    public string Fahrzeugident
    { 
       get
       {
          return _Fahrzeugident;
       }
       set
       {
          _Fahrzeugident = value;
       }
    }
    private string _Fahrzeugident;

    [RfcField(AbapName = "DAT_IMP", RfcType = RFCTYPE.RFCTYPE_DATE, Length = 8, Length2 = 16, Offset = 30, Offset2 = 60)]
    [XmlElement("DAT_IMP")]
    public string Dat_Imp
    { 
       get
       {
          return _Dat_Imp;
       }
       set
       {
          _Dat_Imp = value;
       }
    }
    private string _Dat_Imp;

    [RfcField(AbapName = "ZEIT_IMP", RfcType = RFCTYPE.RFCTYPE_TIME, Length = 6, Length2 = 12, Offset = 38, Offset2 = 76)]
    [XmlElement("ZEIT_IMP")]
    public string Zeit_Imp
    { 
       get
       {
          return _Zeit_Imp;
       }
       set
       {
          _Zeit_Imp = value;
       }
    }
    private string _Zeit_Imp;

    [RfcField(AbapName = "DOFAHRZEUGEINGA", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 44, Offset2 = 88)]
    [XmlElement("DOFAHRZEUGEINGA")]
    public string Dofahrzeugeinga
    { 
       get
       {
          return _Dofahrzeugeinga;
       }
       set
       {
          _Dofahrzeugeinga = value;
       }
    }
    private string _Dofahrzeugeinga;

    [RfcField(AbapName = "DATUMEINGANGNOW", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 45, Offset2 = 90)]
    [XmlElement("DATUMEINGANGNOW")]
    public string Datumeingangnow
    { 
       get
       {
          return _Datumeingangnow;
       }
       set
       {
          _Datumeingangnow = value;
       }
    }
    private string _Datumeingangnow;

    [RfcField(AbapName = "DATUMZULASSUNG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 46, Offset2 = 92)]
    [XmlElement("DATUMZULASSUNG")]
    public string Datumzulassung
    { 
       get
       {
          return _Datumzulassung;
       }
       set
       {
          _Datumzulassung = value;
       }
    }
    private string _Datumzulassung;

    [RfcField(AbapName = "ZULORTVORGABE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 50, Length2 = 100, Offset = 56, Offset2 = 112)]
    [XmlElement("ZULORTVORGABE")]
    public string Zulortvorgabe
    { 
       get
       {
          return _Zulortvorgabe;
       }
       set
       {
          _Zulortvorgabe = value;
       }
    }
    private string _Zulortvorgabe;

    [RfcField(AbapName = "HALTERCODE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 50, Length2 = 100, Offset = 106, Offset2 = 212)]
    [XmlElement("HALTERCODE")]
    public string Haltercode
    { 
       get
       {
          return _Haltercode;
       }
       set
       {
          _Haltercode = value;
       }
    }
    private string _Haltercode;

    [RfcField(AbapName = "HALTERNAME1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 156, Offset2 = 312)]
    [XmlElement("HALTERNAME1")]
    public string Haltername1
    { 
       get
       {
          return _Haltername1;
       }
       set
       {
          _Haltername1 = value;
       }
    }
    private string _Haltername1;

    [RfcField(AbapName = "HALTERNAME2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 186, Offset2 = 372)]
    [XmlElement("HALTERNAME2")]
    public string Haltername2
    { 
       get
       {
          return _Haltername2;
       }
       set
       {
          _Haltername2 = value;
       }
    }
    private string _Haltername2;

    [RfcField(AbapName = "HALTERSTR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 25, Length2 = 50, Offset = 216, Offset2 = 432)]
    [XmlElement("HALTERSTR")]
    public string Halterstr
    { 
       get
       {
          return _Halterstr;
       }
       set
       {
          _Halterstr = value;
       }
    }
    private string _Halterstr;

    [RfcField(AbapName = "HALTERHAUSNUMME", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 7, Length2 = 14, Offset = 241, Offset2 = 482)]
    [XmlElement("HALTERHAUSNUMME")]
    public string Halterhausnumme
    { 
       get
       {
          return _Halterhausnumme;
       }
       set
       {
          _Halterhausnumme = value;
       }
    }
    private string _Halterhausnumme;

    [RfcField(AbapName = "HALTERPLZ", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 248, Offset2 = 496)]
    [XmlElement("HALTERPLZ")]
    public string Halterplz
    { 
       get
       {
          return _Halterplz;
       }
       set
       {
          _Halterplz = value;
       }
    }
    private string _Halterplz;

    [RfcField(AbapName = "HALTERORT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 258, Offset2 = 516)]
    [XmlElement("HALTERORT")]
    public string Halterort
    { 
       get
       {
          return _Halterort;
       }
       set
       {
          _Halterort = value;
       }
    }
    private string _Halterort;

    [RfcField(AbapName = "HALTERLAND", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 288, Offset2 = 576)]
    [XmlElement("HALTERLAND")]
    public string Halterland
    { 
       get
       {
          return _Halterland;
       }
       set
       {
          _Halterland = value;
       }
    }
    private string _Halterland;

    [RfcField(AbapName = "VERSICHERUNGCOD", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 5, Length2 = 10, Offset = 291, Offset2 = 582)]
    [XmlElement("VERSICHERUNGCOD")]
    public string Versicherungcod
    { 
       get
       {
          return _Versicherungcod;
       }
       set
       {
          _Versicherungcod = value;
       }
    }
    private string _Versicherungcod;

    [RfcField(AbapName = "NUTZUNGARTCODE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 296, Offset2 = 592)]
    [XmlElement("NUTZUNGARTCODE")]
    public string Nutzungartcode
    { 
       get
       {
          return _Nutzungartcode;
       }
       set
       {
          _Nutzungartcode = value;
       }
    }
    private string _Nutzungartcode;

    [RfcField(AbapName = "VNEHMERCODE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 298, Offset2 = 596)]
    [XmlElement("VNEHMERCODE")]
    public string Vnehmercode
    { 
       get
       {
          return _Vnehmercode;
       }
       set
       {
          _Vnehmercode = value;
       }
    }
    private string _Vnehmercode;

    [RfcField(AbapName = "VNEHMERNAME1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 308, Offset2 = 616)]
    [XmlElement("VNEHMERNAME1")]
    public string Vnehmername1
    { 
       get
       {
          return _Vnehmername1;
       }
       set
       {
          _Vnehmername1 = value;
       }
    }
    private string _Vnehmername1;

    [RfcField(AbapName = "VNEHMERNAME2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 338, Offset2 = 676)]
    [XmlElement("VNEHMERNAME2")]
    public string Vnehmername2
    { 
       get
       {
          return _Vnehmername2;
       }
       set
       {
          _Vnehmername2 = value;
       }
    }
    private string _Vnehmername2;

    [RfcField(AbapName = "VNEHMERSTR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 25, Length2 = 50, Offset = 368, Offset2 = 736)]
    [XmlElement("VNEHMERSTR")]
    public string Vnehmerstr
    { 
       get
       {
          return _Vnehmerstr;
       }
       set
       {
          _Vnehmerstr = value;
       }
    }
    private string _Vnehmerstr;

    [RfcField(AbapName = "VNEHMERHAUSNUMM", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 7, Length2 = 14, Offset = 393, Offset2 = 786)]
    [XmlElement("VNEHMERHAUSNUMM")]
    public string Vnehmerhausnumm
    { 
       get
       {
          return _Vnehmerhausnumm;
       }
       set
       {
          _Vnehmerhausnumm = value;
       }
    }
    private string _Vnehmerhausnumm;

    [RfcField(AbapName = "VNEHMERPLZ", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 400, Offset2 = 800)]
    [XmlElement("VNEHMERPLZ")]
    public string Vnehmerplz
    { 
       get
       {
          return _Vnehmerplz;
       }
       set
       {
          _Vnehmerplz = value;
       }
    }
    private string _Vnehmerplz;

    [RfcField(AbapName = "VNEHMERORT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 410, Offset2 = 820)]
    [XmlElement("VNEHMERORT")]
    public string Vnehmerort
    { 
       get
       {
          return _Vnehmerort;
       }
       set
       {
          _Vnehmerort = value;
       }
    }
    private string _Vnehmerort;

    [RfcField(AbapName = "VNEHMERLAND", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 440, Offset2 = 880)]
    [XmlElement("VNEHMERLAND")]
    public string Vnehmerland
    { 
       get
       {
          return _Vnehmerland;
       }
       set
       {
          _Vnehmerland = value;
       }
    }
    private string _Vnehmerland;

    [RfcField(AbapName = "KENNZEICHENVORG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 443, Offset2 = 886)]
    [XmlElement("KENNZEICHENVORG")]
    public string Kennzeichenvorg
    { 
       get
       {
          return _Kennzeichenvorg;
       }
       set
       {
          _Kennzeichenvorg = value;
       }
    }
    private string _Kennzeichenvorg;

    [RfcField(AbapName = "ZULASSUNGARTCO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 447, Offset2 = 894)]
    [XmlElement("ZULASSUNGARTCO")]
    public string Zulassungartco
    { 
       get
       {
          return _Zulassungartco;
       }
       set
       {
          _Zulassungartco = value;
       }
    }
    private string _Zulassungartco;

    [RfcField(AbapName = "STATION", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 6, Length2 = 12, Offset = 448, Offset2 = 896)]
    [XmlElement("STATION")]
    public string Station
    { 
       get
       {
          return _Station;
       }
       set
       {
          _Station = value;
       }
    }
    private string _Station;

    [RfcField(AbapName = "STATIONNAME1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 454, Offset2 = 908)]
    [XmlElement("STATIONNAME1")]
    public string Stationname1
    { 
       get
       {
          return _Stationname1;
       }
       set
       {
          _Stationname1 = value;
       }
    }
    private string _Stationname1;

    [RfcField(AbapName = "STATIONNAME2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 484, Offset2 = 968)]
    [XmlElement("STATIONNAME2")]
    public string Stationname2
    { 
       get
       {
          return _Stationname2;
       }
       set
       {
          _Stationname2 = value;
       }
    }
    private string _Stationname2;

    [RfcField(AbapName = "STATIONSTR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 514, Offset2 = 1028)]
    [XmlElement("STATIONSTR")]
    public string Stationstr
    { 
       get
       {
          return _Stationstr;
       }
       set
       {
          _Stationstr = value;
       }
    }
    private string _Stationstr;

    [RfcField(AbapName = "STATIONHAUSNUMM", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 544, Offset2 = 1088)]
    [XmlElement("STATIONHAUSNUMM")]
    public string Stationhausnumm
    { 
       get
       {
          return _Stationhausnumm;
       }
       set
       {
          _Stationhausnumm = value;
       }
    }
    private string _Stationhausnumm;

    [RfcField(AbapName = "STATIONPLZ", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 554, Offset2 = 1108)]
    [XmlElement("STATIONPLZ")]
    public string Stationplz
    { 
       get
       {
          return _Stationplz;
       }
       set
       {
          _Stationplz = value;
       }
    }
    private string _Stationplz;

    [RfcField(AbapName = "STATIONORT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 564, Offset2 = 1128)]
    [XmlElement("STATIONORT")]
    public string Stationort
    { 
       get
       {
          return _Stationort;
       }
       set
       {
          _Stationort = value;
       }
    }
    private string _Stationort;

    [RfcField(AbapName = "STATIONLAND", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 594, Offset2 = 1188)]
    [XmlElement("STATIONLAND")]
    public string Stationland
    { 
       get
       {
          return _Stationland;
       }
       set
       {
          _Stationland = value;
       }
    }
    private string _Stationland;

    [RfcField(AbapName = "STATIONTELEFON", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 597, Offset2 = 1194)]
    [XmlElement("STATIONTELEFON")]
    public string Stationtelefon
    { 
       get
       {
          return _Stationtelefon;
       }
       set
       {
          _Stationtelefon = value;
       }
    }
    private string _Stationtelefon;

    [RfcField(AbapName = "STATIONEMAIL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 617, Offset2 = 1234)]
    [XmlElement("STATIONEMAIL")]
    public string Stationemail
    { 
       get
       {
          return _Stationemail;
       }
       set
       {
          _Stationemail = value;
       }
    }
    private string _Stationemail;

    [RfcField(AbapName = "ANTRIEBKUERZEL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 637, Offset2 = 1274)]
    [XmlElement("ANTRIEBKUERZEL")]
    public string Antriebkuerzel
    { 
       get
       {
          return _Antriebkuerzel;
       }
       set
       {
          _Antriebkuerzel = value;
       }
    }
    private string _Antriebkuerzel;

    [RfcField(AbapName = "GETRIEBEKUERZEL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 638, Offset2 = 1276)]
    [XmlElement("GETRIEBEKUERZEL")]
    public string Getriebekuerzel
    { 
       get
       {
          return _Getriebekuerzel;
       }
       set
       {
          _Getriebekuerzel = value;
       }
    }
    private string _Getriebekuerzel;

    [RfcField(AbapName = "REIFENARTKUERZE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 639, Offset2 = 1278)]
    [XmlElement("REIFENARTKUERZE")]
    public string Reifenartkuerze
    { 
       get
       {
          return _Reifenartkuerze;
       }
       set
       {
          _Reifenartkuerze = value;
       }
    }
    private string _Reifenartkuerze;

    [RfcField(AbapName = "NAVIKUERZEL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 641, Offset2 = 1282)]
    [XmlElement("NAVIKUERZEL")]
    public string Navikuerzel
    { 
       get
       {
          return _Navikuerzel;
       }
       set
       {
          _Navikuerzel = value;
       }
    }
    private string _Navikuerzel;

    [RfcField(AbapName = "AUSFUERUNGKUERZ", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 642, Offset2 = 1284)]
    [XmlElement("AUSFUERUNGKUERZ")]
    public string Ausfuerungkuerz
    { 
       get
       {
          return _Ausfuerungkuerz;
       }
       set
       {
          _Ausfuerungkuerz = value;
       }
    }
    private string _Ausfuerungkuerz;

    [RfcField(AbapName = "MODELBEZEICHNUNG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 645, Offset2 = 1290)]
    [XmlElement("MODELBEZEICHNUNG")]
    public string Modelbezeichnung
    { 
       get
       {
          return _Modelbezeichnung;
       }
       set
       {
          _Modelbezeichnung = value;
       }
    }
    private string _Modelbezeichnung;

    [RfcField(AbapName = "PDI_DAD", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 675, Offset2 = 1350)]
    [XmlElement("PDI_DAD")]
    public string Pdi_Dad
    { 
       get
       {
          return _Pdi_Dad;
       }
       set
       {
          _Pdi_Dad = value;
       }
    }
    private string _Pdi_Dad;

    [RfcField(AbapName = "EQUNR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 18, Length2 = 36, Offset = 685, Offset2 = 1370)]
    [XmlElement("EQUNR")]
    public string Equnr
    { 
       get
       {
          return _Equnr;
       }
       set
       {
          _Equnr = value;
       }
    }
    private string _Equnr;

    [RfcField(AbapName = "QMNUM_Z1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 12, Length2 = 24, Offset = 703, Offset2 = 1406)]
    [XmlElement("QMNUM_Z1")]
    public string Qmnum_Z1
    { 
       get
       {
          return _Qmnum_Z1;
       }
       set
       {
          _Qmnum_Z1 = value;
       }
    }
    private string _Qmnum_Z1;

    [RfcField(AbapName = "VBELN_ZUL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 715, Offset2 = 1430)]
    [XmlElement("VBELN_ZUL")]
    public string Vbeln_Zul
    { 
       get
       {
          return _Vbeln_Zul;
       }
       set
       {
          _Vbeln_Zul = value;
       }
    }
    private string _Vbeln_Zul;

    [RfcField(AbapName = "FLAG_ERL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 725, Offset2 = 1450)]
    [XmlElement("FLAG_ERL")]
    public string Flag_Erl
    { 
       get
       {
          return _Flag_Erl;
       }
       set
       {
          _Flag_Erl = value;
       }
    }
    private string _Flag_Erl;

    [RfcField(AbapName = "ZFCODE", RfcType = RFCTYPE.RFCTYPE_NUM, Length = 3, Length2 = 6, Offset = 726, Offset2 = 1452)]
    [XmlElement("ZFCODE")]
    public string Zfcode
    { 
       get
       {
          return _Zfcode;
       }
       set
       {
          _Zfcode = value;
       }
    }
    private string _Zfcode;

    [RfcField(AbapName = "ZWKENNZ", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 15, Length2 = 30, Offset = 729, Offset2 = 1458)]
    [XmlElement("ZWKENNZ")]
    public string Zwkennz
    { 
       get
       {
          return _Zwkennz;
       }
       set
       {
          _Zwkennz = value;
       }
    }
    private string _Zwkennz;

  }

}