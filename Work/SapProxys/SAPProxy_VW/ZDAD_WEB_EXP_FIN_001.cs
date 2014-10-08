
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

  [RfcStructure(AbapName ="ZDAD_WEB_EXP_FIN_001" , Length = 98, Length2 = 196)]
  public class ZDAD_WEB_EXP_FIN_001 : SAPStructure
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

    [RfcField(AbapName = "FZGTYP", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 25, Offset2 = 50)]
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

    [RfcField(AbapName = "REFERENZ1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 45, Offset2 = 90)]
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

    [RfcField(AbapName = "CHASSIS_NUM", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 65, Offset2 = 130)]
    [XmlElement("CHASSIS_NUM")]
    public string Chassis_Num
    { 
       get
       {
          return _Chassis_Num;
       }
       set
       {
          _Chassis_Num = value;
       }
    }
    private string _Chassis_Num;

    [RfcField(AbapName = "FCODE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 95, Offset2 = 190)]
    [XmlElement("FCODE")]
    public string Fcode
    { 
       get
       {
          return _Fcode;
       }
       set
       {
          _Fcode = value;
       }
    }
    private string _Fcode;

  }

}