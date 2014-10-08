﻿using System;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Dokumentenanforderung der Zulassungstellen.
    /// </summary>
    public class Report99 : CKG.Base.Business.DatenimportBase
    {
        #region "Declarations"
        String strKennzeichen;
        #endregion

        #region "Properties"
        /// <summary>
        /// Selektierte StVa zur Übergabe an das Bapi.
        /// </summary>
        public String PKennzeichen
        {
            get { return strKennzeichen; }
            set { strKennzeichen = value; }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">User-Object</param>
        /// <param name="objApp">Applikations-Object</param>
        /// <param name="strFilename">Empty</param>
        public Report99(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        {}
        /// <summary>
        ///  Datenselektion für die Ausgabe der Dokumentenanforderung. Bapi: Z_M_ZGBS_BEN_ZULASSUNGSUNT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Dokumentenanforderung.aspx.cs</param>
        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Report99ZLD.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ZGBS_BEN_ZULASSUNGSUNT", ref m_objApp, ref m_objUser, ref page);
               
                    myProxy.setImportParameter("I_ZKBA1", "");
                    myProxy.setImportParameter("I_ZKBA2", "");
                    myProxy.setImportParameter("I_ZKFZKZ", strKennzeichen);
                    myProxy.setImportParameter("I_AUSWAHL", "");

                    myProxy.callBapi();

                    DataTable m_tblResultRaw = new DataTable();
                    m_tblResult = myProxy.getExportTable("GT_WEB");
                    WriteLogEntry(true, "I_ZKFZKZ=" + strKennzeichen , ref m_tblResult, false);
                }

                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            WriteLogEntry(false, "I_ZKFZKZ=" + strKennzeichen + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref m_tblResult, false);
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            WriteLogEntry(false, "I_ZKFZKZ=" + strKennzeichen + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref m_tblResult, false);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }
        /// <summary>
        /// Pflegen der Dokumentenanforderung der Zulassungstellen. Bapi: Z_ZLD_IMPORT_ZULUNT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Change99ZLD.aspx</param>
        /// <param name="tblForm">Importtabelle</param>
        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblForm)
        {

            m_strClassAndMethod = "ZLD_Suche.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMPORT_ZULUNT", ref m_objApp, ref m_objUser, ref page);

                    DataTable tblSap = new DataTable();
                    tblSap = myProxy.getImportTable("GS_WEB");


                    DataRow SapRow = tblSap.NewRow();

                    foreach (DataRow FormRow in tblForm.Rows)
                    {

                        SapRow["MANDT"] = FormRow["MANDT"].ToString();
                        SapRow["ZKBA1"] = FormRow["ZKBA1"].ToString();
                        SapRow["ZKBA2"] = FormRow["ZKBA2"].ToString();
                        SapRow["AENAM"] = FormRow["AENAM"].ToString();
                        SapRow["AEDAT"] = FormRow["AEDAT"].ToString();

                        //Privat Zulassung

                        SapRow["PZUL_BRIEF"] = FormRow["PZUL_BRIEF"].ToString();
                        SapRow["PZUL_SCHEIN"] = FormRow["PZUL_SCHEIN"].ToString();
                        SapRow["PZUL_COC"] = FormRow["PZUL_COC"].ToString();
                        SapRow["PZUL_DECK"] = FormRow["PZUL_DECK"].ToString();
                        SapRow["PZUL_VOLLM"] = FormRow["PZUL_VOLLM"].ToString();
                        SapRow["PZUL_AUSW"] = FormRow["PZUL_AUSW"].ToString();
                        SapRow["PZUL_GEWERB"] = FormRow["PZUL_GEWERB"].ToString();
                        SapRow["PZUL_HANDEL"] = FormRow["PZUL_HANDEL"].ToString();
                        SapRow["PZUL_LAST"] = FormRow["PZUL_LAST"].ToString();
                        SapRow["PZUL_BEM"] = FormRow["PZUL_BEM"].ToString();

                        //Privat Umschreibung

                        SapRow["PUMSCHR_SCHEIN"] = FormRow["PUMSCHR_SCHEIN"].ToString();
                        SapRow["PUMSCHR_BRIEF"] = FormRow["PUMSCHR_BRIEF"].ToString();
                        SapRow["PUMSCHR_COC"] = FormRow["PUMSCHR_COC"].ToString();
                        SapRow["PUMSCHR_DECK"] = FormRow["PUMSCHR_DECK"].ToString();
                        SapRow["PUMSCHR_VOLLM"] = FormRow["PUMSCHR_VOLLM"].ToString();
                        SapRow["PUMSCHR_AUSW"] = FormRow["PUMSCHR_AUSW"].ToString();
                        SapRow["PUMSCHR_GEWERB"] = FormRow["PUMSCHR_GEWERB"].ToString();
                        SapRow["PUMSCHR_HANDEL"] = FormRow["PUMSCHR_HANDEL"].ToString();
                        SapRow["PUMSCHR_LAST"] = FormRow["PUMSCHR_LAST"].ToString();
                        SapRow["PUMSCHR_BEM"] = FormRow["PUMSCHR_BEM"].ToString();

                        //Privat Umkennzeichnung
                        SapRow["PUMK_BRIEF"] = FormRow["PUMK_BRIEF"].ToString();
                        SapRow["PUMK_SCHEIN"] = FormRow["PUMK_SCHEIN"].ToString();
                        SapRow["PUMK_COC"] = FormRow["PUMK_COC"].ToString();
                        SapRow["PUMK_DECK"] = FormRow["PUMK_DECK"].ToString();
                        SapRow["PUMK_VOLLM"] = FormRow["PUMK_VOLLM"].ToString();
                        SapRow["PUMK_AUSW"] = FormRow["PUMK_AUSW"].ToString();
                        SapRow["PUMK_GEWERB"] = FormRow["PUMK_GEWERB"].ToString();
                        SapRow["PUMK_HANDEL"] = FormRow["PUMK_HANDEL"].ToString();
                        SapRow["PUMK_LAST"] = FormRow["PUMK_LAST"].ToString();
                        SapRow["PUMK_BEM"] = FormRow["PUMK_BEM"].ToString();

                        //Privat Ersatzfahrzeugschein
                        SapRow["PERS_BRIEF"] = FormRow["PERS_BRIEF"].ToString();
                        SapRow["PERS_SCHEIN"] = FormRow["PERS_SCHEIN"].ToString();
                        SapRow["PERS_COC"] = FormRow["PERS_COC"].ToString();
                        SapRow["PERS_DECK"] = FormRow["PERS_DECK"].ToString();
                        SapRow["PERS_VOLLM"] = FormRow["PERS_VOLLM"].ToString();
                        SapRow["PERS_AUSW"] = FormRow["PERS_AUSW"].ToString();
                        SapRow["PERS_GEWERB"] = FormRow["PERS_GEWERB"].ToString();
                        SapRow["PERS_HANDEL"] = FormRow["PERS_HANDEL"].ToString();
                        SapRow["PERS_LAST"] = FormRow["PERS_LAST"].ToString();
                        SapRow["PERS_BEM"] = FormRow["PERS_BEM"].ToString();

                        //Unternehmen Umschreibung
                        SapRow["UZUL_BRIEF"] = FormRow["UZUL_BRIEF"].ToString();
                        SapRow["UZUL_SCHEIN"] = FormRow["UZUL_SCHEIN"].ToString();
                        SapRow["UZUL_COC"] = FormRow["UZUL_COC"].ToString();
                        SapRow["UZUL_DECK"] = FormRow["UZUL_DECK"].ToString();
                        SapRow["UZUL_VOLLM"] = FormRow["UZUL_VOLLM"].ToString();
                        SapRow["UZUL_AUSW"] = FormRow["UZUL_AUSW"].ToString();
                        SapRow["UZUL_GEWERB"] = FormRow["UZUL_GEWERB"].ToString();
                        SapRow["UZUL_HANDEL"] = FormRow["UZUL_HANDEL"].ToString();
                        SapRow["UZUL_LAST"] = FormRow["UZUL_LAST"].ToString();
                        SapRow["UZUL_BEM"] = FormRow["UZUL_BEM"].ToString();

                        //Unternehmen Umschreibung
                        SapRow["UUMSCHR_BRIEF"] = FormRow["UUMSCHR_BRIEF"].ToString();
                        SapRow["UUMSCHR_SCHEIN"] = FormRow["UUMSCHR_SCHEIN"].ToString();
                        SapRow["UUMSCHR_COC"] = FormRow["UUMSCHR_COC"].ToString();
                        SapRow["UUMSCHR_DECK"] = FormRow["UUMSCHR_DECK"].ToString();
                        SapRow["UUMSCHR_VOLLM"] = FormRow["UUMSCHR_VOLLM"].ToString();
                        SapRow["UUMSCHR_AUSW"] = FormRow["UUMSCHR_AUSW"].ToString();
                        SapRow["UUMSCHR_GEWERB"] = FormRow["UUMSCHR_GEWERB"].ToString();
                        SapRow["UUMSCHR_HANDEL"] = FormRow["UUMSCHR_HANDEL"].ToString();
                        SapRow["UUMSCHR_LAST"] = FormRow["UUMSCHR_LAST"].ToString();
                        SapRow["UUMSCHR_BEM"] = FormRow["UUMSCHR_BEM"].ToString();

                        //Unternehmen Umkennzeichnung
                        SapRow["UUMK_BRIEF"] = FormRow["UUMK_BRIEF"].ToString();
                        SapRow["UUMK_SCHEIN"] = FormRow["UUMK_SCHEIN"].ToString();
                        SapRow["UUMK_COC"] = FormRow["UUMK_COC"].ToString();
                        SapRow["UUMK_DECK"] = FormRow["UUMK_DECK"].ToString();
                        SapRow["UUMK_VOLLM"] = FormRow["UUMK_VOLLM"].ToString();
                        SapRow["UUMK_AUSW"] = FormRow["UUMK_AUSW"].ToString();
                        SapRow["UUMK_GEWERB"] = FormRow["UUMK_GEWERB"].ToString();
                        SapRow["UUMK_HANDEL"] = FormRow["UUMK_HANDEL"].ToString();
                        SapRow["UUMK_LAST"] = FormRow["UUMK_LAST"].ToString();
                        SapRow["UUMK_BEM"] = FormRow["UUMK_BEM"].ToString();

                        //Unternehmen Ersatzfahrzeugschein
                        SapRow["UERS_BRIEF"] = FormRow["UERS_BRIEF"].ToString();
                        SapRow["UERS_SCHEIN"] = FormRow["UERS_SCHEIN"].ToString();
                        SapRow["UERS_COC"] = FormRow["UERS_COC"].ToString();
                        SapRow["UERS_DECK"] = FormRow["UERS_DECK"].ToString();
                        SapRow["UERS_VOLLM"] = FormRow["UERS_VOLLM"].ToString();
                        SapRow["UERS_AUSW"] = FormRow["UERS_AUSW"].ToString();
                        SapRow["UERS_GEWERB"] = FormRow["UERS_GEWERB"].ToString();
                        SapRow["UERS_HANDEL"] = FormRow["UERS_HANDEL"].ToString();
                        SapRow["UERS_LAST"] = FormRow["UERS_LAST"].ToString();
                        SapRow["UERS_BEM"] = FormRow["UERS_BEM"].ToString();


                        SapRow["STVALN"] = FormRow["STVALN"].ToString();
                        SapRow["URL"] = FormRow["URL"].ToString();
                        SapRow["STVALNFORM"] = FormRow["STVALNFORM"].ToString();
                        SapRow["STVALNGEB"] = FormRow["STVALNGEB"].ToString();

                        tblSap.Rows.Add(SapRow);
                    }
                    myProxy.callBapi();

                    Int32 subrc = 0;

                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);

                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_intStatus = subrc;
                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }
    #endregion
    }
}