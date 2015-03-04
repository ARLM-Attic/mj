
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 1.0
//     Created at 14.05.2008
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

namespace SAPProxy_VW
{

  [RfcStructure(AbapName ="ZDAD_WEB_IMP_ZUL_001" , Length = 667, Length2 = 1334)]
  public class ZDAD_WEB_IMP_ZUL_001 : SAPStructure
  {
    
    [RfcField(AbapName = "REFERENZ2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 0, Offset2 = 0)]
    [XmlElement("REFERENZ2")]
    public string Referenz2
    { 
       get
       {
          return _Referenz2;
       }
       set
       {
          _Referenz2 = value;
       }
    }
    private string _Referenz2;

    [RfcField(AbapName = "VHB_TEIL", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 5, Length2 = 10, Offset = 20, Offset2 = 40)]
    [XmlElement("VHB_TEIL")]
    public string Vhb_Teil
    { 
       get
       {
          return _Vhb_Teil;
       }
       set
       {
          _Vhb_Teil = value;
       }
    }
    private string _Vhb_Teil;

    [RfcField(AbapName = "REFERENZ1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 25, Offset2 = 50)]
    [XmlElement("REFERENZ1")]
    public string Referenz1
    { 
       get
       {
          return _Referenz1;
       }
       set
       {
          _Referenz1 = value;
       }
    }
    private string _Referenz1;

    [RfcField(AbapName = "ANREDE_ESO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 40, Length2 = 80, Offset = 45, Offset2 = 90)]
    [XmlElement("ANREDE_ESO")]
    public string Anrede_Eso
    { 
       get
       {
          return _Anrede_Eso;
       }
       set
       {
          _Anrede_Eso = value;
       }
    }
    private string _Anrede_Eso;

    [RfcField(AbapName = "STRASSE_ESO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 60, Length2 = 120, Offset = 85, Offset2 = 170)]
    [XmlElement("STRASSE_ESO")]
    public string Strasse_Eso
    { 
       get
       {
          return _Strasse_Eso;
       }
       set
       {
          _Strasse_Eso = value;
       }
    }
    private string _Strasse_Eso;

    [RfcField(AbapName = "PSTLZ_ESO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 145, Offset2 = 290)]
    [XmlElement("PSTLZ_ESO")]
    public string Pstlz_Eso
    { 
       get
       {
          return _Pstlz_Eso;
       }
       set
       {
          _Pstlz_Eso = value;
       }
    }
    private string _Pstlz_Eso;

    [RfcField(AbapName = "ORT_ESO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 40, Length2 = 80, Offset = 155, Offset2 = 310)]
    [XmlElement("ORT_ESO")]
    public string Ort_Eso
    { 
       get
       {
          return _Ort_Eso;
       }
       set
       {
          _Ort_Eso = value;
       }
    }
    private string _Ort_Eso;

    [RfcField(AbapName = "NUMMER_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 25, Length2 = 50, Offset = 195, Offset2 = 390)]
    [XmlElement("NUMMER_SUS")]
    public string Nummer_Sus
    { 
       get
       {
          return _Nummer_Sus;
       }
       set
       {
          _Nummer_Sus = value;
       }
    }
    private string _Nummer_Sus;

    [RfcField(AbapName = "NAME1_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 40, Length2 = 80, Offset = 220, Offset2 = 440)]
    [XmlElement("NAME1_SUS")]
    public string Name1_Sus
    { 
       get
       {
          return _Name1_Sus;
       }
       set
       {
          _Name1_Sus = value;
       }
    }
    private string _Name1_Sus;

    [RfcField(AbapName = "NAME2_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 40, Length2 = 80, Offset = 260, Offset2 = 520)]
    [XmlElement("NAME2_SUS")]
    public string Name2_Sus
    { 
       get
       {
          return _Name2_Sus;
       }
       set
       {
          _Name2_Sus = value;
       }
    }
    private string _Name2_Sus;

    [RfcField(AbapName = "STRASSE_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 60, Length2 = 120, Offset = 300, Offset2 = 600)]
    [XmlElement("STRASSE_SUS")]
    public string Strasse_Sus
    { 
       get
       {
          return _Strasse_Sus;
       }
       set
       {
          _Strasse_Sus = value;
       }
    }
    private string _Strasse_Sus;

    [RfcField(AbapName = "HAUSNR_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 360, Offset2 = 720)]
    [XmlElement("HAUSNR_SUS")]
    public string Hausnr_Sus
    { 
       get
       {
          return _Hausnr_Sus;
       }
       set
       {
          _Hausnr_Sus = value;
       }
    }
    private string _Hausnr_Sus;

    [RfcField(AbapName = "PSTLZ_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 370, Offset2 = 740)]
    [XmlElement("PSTLZ_SUS")]
    public string Pstlz_Sus
    { 
       get
       {
          return _Pstlz_Sus;
       }
       set
       {
          _Pstlz_Sus = value;
       }
    }
    private string _Pstlz_Sus;

    [RfcField(AbapName = "ORT_SUS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 40, Length2 = 80, Offset = 380, Offset2 = 760)]
    [XmlElement("ORT_SUS")]
    public string Ort_Sus
    { 
       get
       {
          return _Ort_Sus;
       }
       set
       {
          _Ort_Sus = value;
       }
    }
    private string _Ort_Sus;

    [RfcField(AbapName = "ZIELBAHNHOF", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 5, Length2 = 10, Offset = 420, Offset2 = 840)]
    [XmlElement("ZIELBAHNHOF")]
    public string Zielbahnhof
    { 
       get
       {
          return _Zielbahnhof;
       }
       set
       {
          _Zielbahnhof = value;
       }
    }
    private string _Zielbahnhof;

    [RfcField(AbapName = "BEM_SUS_BEARB", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 100, Length2 = 200, Offset = 425, Offset2 = 850)]
    [XmlElement("BEM_SUS_BEARB")]
    public string Bem_Sus_Bearb
    { 
       get
       {
          return _Bem_Sus_Bearb;
       }
       set
       {
          _Bem_Sus_Bearb = value;
       }
    }
    private string _Bem_Sus_Bearb;

    [RfcField(AbapName = "STAT_SUS_BEARB", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 525, Offset2 = 1050)]
    [XmlElement("STAT_SUS_BEARB")]
    public string Stat_Sus_Bearb
    { 
       get
       {
          return _Stat_Sus_Bearb;
       }
       set
       {
          _Stat_Sus_Bearb = value;
       }
    }
    private string _Stat_Sus_Bearb;

    [RfcField(AbapName = "HERST", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 527, Offset2 = 1054)]
    [XmlElement("HERST")]
    public string Herst
    { 
       get
       {
          return _Herst;
       }
       set
       {
          _Herst = value;
       }
    }
    private string _Herst;

    [RfcField(AbapName = "FZGTYP", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 547, Offset2 = 1094)]
    [XmlElement("FZGTYP")]
    public string Fzgtyp
    { 
       get
       {
          return _Fzgtyp;
       }
       set
       {
          _Fzgtyp = value;
       }
    }
    private string _Fzgtyp;

    [RfcField(AbapName = "VARIANTE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 100, Length2 = 200, Offset = 567, Offset2 = 1134)]
    [XmlElement("VARIANTE")]
    public string Variante
    { 
       get
       {
          return _Variante;
       }
       set
       {
          _Variante = value;
       }
    }
    private string _Variante;

  }

}