﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Security;
using AutohausPortal.lib;
using CKG.Base.Kernel.Common;
using System.Data;
using GeneralTools.Models;
using Telerik.Web.UI;
namespace AutohausPortal.forms
{
    /// <summary>
    /// Auftragseingabe für Sonstige Dienstleistungen. Benutzte Klassen AHErfassung und objCommon.
    /// </summary>
    public partial class Sonstiges : Page
    {
        private User m_User;
        private App m_App;
        private AHErfassung objVorerf;
        private ZLDCommon objCommon;
        Boolean BackfromList;
        String IDKopf;
        String AppIDListe;
        Boolean IsInAsync;

        #region Events
        
        /// <summary>
        /// Page_Load Ereignis
        /// Überprüfen ob der Benutzer von der Auftragsliste einen Vorgang aufruft
        /// oder einen neuen Vorgang anlegen will
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (BackfromList)
                {
                    if (Request.QueryString["id"] != null)
                    { IDKopf = Request.QueryString["id"].ToString(); }
                    else
                    { lblError.Text = "Fehler beim Laden des Vorganges!"; }

                    objVorerf = (AHErfassung)Session["objVorerf"];
                    var id = IDKopf.ToLong(0);
                    if (id != 0)
                    {
                        objVorerf.SelectVorgang(id); // Vorgang laden
                        fillForm();
                        SelectValues();
                    }
                    else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
                }
                else
                {
                    objVorerf = new AHErfassung(ref m_User, m_App, "AS", "570");
                    fillForm();
                }
                Session["objVorerf"] = objVorerf;

            }
            else if (Request.Params.Get("__EVENTTARGET") == "RadWindow1")
            {
                if ((Session["RedirectToAuftragsliste"] != null) && ((bool)Session["RedirectToAuftragsliste"]))
                {
                    Session["RedirectToAuftragsliste"] = null;
                    Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe);
                }
                else
                {
                    //Schließen des Druckdialogs: PrintDialogKundenformular.aspx
                    RadWindow downloaddoc = RadWindowManager1.Windows[0];
                    downloaddoc.Visible = false;
                    downloaddoc.VisibleOnPageLoad = false;
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Form1",
            "<script type='text/javascript'>openform1();</script>", false);
        }

        /// <summary>
        /// Page_Init Ereignis
        /// Überprüfung ob dem User diese Applikation zugeordnet ist
        /// Laden der Stammdaten wenn noch nicht im Session-Object
        /// Aufruf von "fillDropDowns" - Füllen der DropDpwn-Controls mit den Stammdaten
        /// Aufruf von "TableToJSArray" - Füllen eines Javascript-Array mit Sonderkennzeichen
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            BackfromList = false;
            if (Request.QueryString["B"] != null) { BackfromList = true; }
            AppIDListe = "";
            if (BackfromList)
            {
                if (Request.QueryString["BackAppID"] != null)
                { AppIDListe = Request.QueryString["BackAppID"].ToString(); }
                else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
            }
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                if (!objCommon.Init(Session["AppID"].ToString(), Session.SessionID, this))
                {
                    lblError.Visible = true;
                    lblError.Text = objCommon.Message;
                    return;
                }
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

            }
            IsInAsync = ScriptManager.GetCurrent(this).IsInAsyncPostBack;
            fillDropDowns();

        }

        /// <summary>
        /// ddlKunnr1_ItemsRequested - Ereignis
        /// Suchanfrage in der Kundendropdown(Eingabe, Auswahl)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxItemsRequestedEventArgs</param>
        protected void ddlKunnr1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            ddlKunnr1.Items.Clear();

            string text = e.Text;
            DataRow[] rows = objCommon.tblKundenStamm.Select("NAME1 Like '" + text + "*'");
            int itemsPerRequest = 10;
            int itemOffset = e.NumberOfItems;
            int endOffset = itemOffset + itemsPerRequest;
            if (endOffset > rows.Length)
            {
                endOffset = rows.Length;
            }

            for (int i = itemOffset; i < endOffset; i++)
            {
                ddlKunnr1.Items.Add(new RadComboBoxItem(rows[i]["NAME1"].ToString(), rows[i]["KUNNR"].ToString()));
            }
        }

        /// <summary>
        /// ddlKunnr1_SelectedIndexChanged - Ereignis
        /// Änderung des Kunden in der DropDown ddlKunnr1
        /// Funktionsaufruf: setReferenz(), addAttributes(), enableDefaultValue()
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RadComboBoxSelectedIndexChangedEventArgs</param>
        protected void ddlKunnr1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            setReferenz();
        }

        /// <summary>
        /// Übernehmen der Eingabedaten in die Klasseneigenschaften
        /// Überprüfen ob Neuanlage(objVorerf.InsertDB_ZLD) oder vorhandener Vorgang editiert wurde(objVorerf.UpdateDB_ZLD)
        /// Speichern der Daten in der Datenbank
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {

            lblError.Text = "";
            lblMessage.Text = "";
            ClearError();
            objVorerf = (AHErfassung)Session["objVorerf"];
            ValidateData();

            String RemoveDefault = "";
            if (lblError.Text.Length == 0)
            {

                if (objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern der Filiale"; return; }

                objVorerf.Kunnr = ddlKunnr1.SelectedValue;
                objVorerf.Kundenname = (ddlKunnr1.SelectedItem != null ? ddlKunnr1.SelectedItem.Text : "");

                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'").Length == 0)
                { lblError.Text = "Fehler beim Speichern des zulassungskreises"; return; }
                objVorerf.KreisKennz = ddlStVa1.SelectedValue;
                objVorerf.Kreis = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa1.SelectedValue + "'")[0]["KREISTEXT"].ToString();

                objVorerf.EVB = "";
                objVorerf.StillDate = "";
                objVorerf.KurzZeitKennz = "";

                objVorerf.WunschKenn = false;
                objVorerf.Reserviert = false;
                objVorerf.ReserviertKennz = "";
                objVorerf.MussReserviert = false;
                objVorerf.KennzVorhanden = false;
                objVorerf.Feinstaub = false;
                objVorerf.Kennzeichen = ddlStVa1.SelectedValue + "-";
                objVorerf.Ref1 = "";
                objVorerf.Ref2 = "";
                objVorerf.Ref3 = "";
                objVorerf.Ref4 = "";
                bool istCpdKunde = false;
                DataRow[] rowKunde = objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'");
                if (rowKunde.Length > 0)
                {
                    if (txtReferenz1.Text != rowKunde[0]["REF_NAME_01"].ToString())
                    { objVorerf.Ref1 = txtReferenz1.Text.ToUpper(); }
                    if (txtReferenz2.Text != rowKunde[0]["REF_NAME_02"].ToString())
                    { objVorerf.Ref2 = txtReferenz2.Text.ToUpper(); }
                    if (txtReferenz3.Text != rowKunde[0]["REF_NAME_03"].ToString())
                    { objVorerf.Ref3 = txtReferenz3.Text.ToUpper(); }
                    if (txtReferenz4.Text != rowKunde[0]["REF_NAME_04"].ToString())
                    { objVorerf.Ref4 = txtReferenz4.Text.ToUpper(); }
                    if (rowKunde[0]["XCPDK"].ToString() == "X")
                    {
                        istCpdKunde = true;
                    }
                }

                objVorerf.ZulDate = txtDatum.Text;
                objVorerf.TuvAu = "";
                objVorerf.VorhKennzReserv = false;
                objVorerf.ZollVers = "";
                objVorerf.ZollVersDauer = "";
                objVorerf.Altkenn = "";
                objVorerf.Haltedauer = "";

                objVorerf.Serie = false;
                objVorerf.Saison = false;
                objVorerf.SaisonBeg = "";
                objVorerf.SaisonEnd = "";
                objVorerf.KennzForm = "";

                objVorerf.EinKennz = false;
                objVorerf.Fahrzeugart = ddlFahrzeugart.SelectedValue;
                objVorerf.LangText = txtService2.Text;

                if (!proofBank()) { return; }
                if (!proofBankAndAddressData(istCpdKunde)) { return; }

                objVorerf.Name1 = ucBankdatenAdresse.Name1;
                objVorerf.Name2 = ucBankdatenAdresse.Name2;
                objVorerf.Strasse = ucBankdatenAdresse.Strasse;
                objVorerf.PLZ = ucBankdatenAdresse.Plz;
                objVorerf.Ort = ucBankdatenAdresse.Ort;
                objVorerf.SWIFT = ucBankdatenAdresse.IsSWIFTInitial ? "" : ucBankdatenAdresse.SWIFT;
                objVorerf.IBAN = ucBankdatenAdresse.IBAN;
                objVorerf.Bankkey = ucBankdatenAdresse.Bankkey;
                objVorerf.Kontonr = ucBankdatenAdresse.Kontonr;
                objVorerf.Geldinstitut = ucBankdatenAdresse.IsGeldinstitutInitial ? "" : ucBankdatenAdresse.Geldinstitut;
                objVorerf.Inhaber = ucBankdatenAdresse.Kontoinhaber;
                objVorerf.EinzugErm = ucBankdatenAdresse.Einzug;
                objVorerf.Rechnung = ucBankdatenAdresse.Rechnung;
                objVorerf.Barzahlung = ucBankdatenAdresse.Bar;
                Session["objVorerf"] = objVorerf;

                objVorerf.Kennztyp = "";

                objVorerf.EinKennz = false;
                objVorerf.Bemerkung = ucBemerkungenNotizen.Bemerkung;
                objVorerf.Notiz = ucBemerkungenNotizen.Notiz;
                objVorerf.VkKurz = ucBemerkungenNotizen.VKKurz;
                RemoveDefault = "";
                if (ucBemerkungenNotizen.KunRef.Replace("Kundeninterne Referenz", "").Length > 0) { RemoveDefault = ucBemerkungenNotizen.KunRef; }
                objVorerf.InternRef = RemoveDefault;

                objVorerf.AppID = Session["AppID"].ToString();

                LongStringToSap LSTS = new LongStringToSap();

                if (!objVorerf.IsNewVorgang)
                {
                    if (objVorerf.NrLangText != "")
                    {
                        LSTS.UpdateString(m_User, m_App, this, objVorerf.LangText, objVorerf.NrLangText, "");
                    }
                    objVorerf.SaveVorgangToSap(Session["AppID"].ToString(), Session.SessionID, this, istCpdKunde, true);
                    ShowKundenformulare(true);
                    return;
                }

                
                if (objVorerf.LangText != "")
                {
                    objVorerf.NrLangText = LSTS.InsertString(m_User, m_App, this, objVorerf.LangText, "");
                }
                objVorerf.SaveVorgangToSap(Session["AppID"].ToString(), Session.SessionID, this, istCpdKunde, true);

                if (objVorerf.Status == 0)
                    getAuftraege();

                if (objVorerf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Datensatz unter ID " + objVorerf.id_sap + " gespeichert.";
                    ShowKundenformulare();
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen der Daten: " + objVorerf.Message;
                }

                ClearForm();
            }
            else { proofInserted(); }
        }

        private void ShowKundenformulare(bool redirect = false)
        {
            Session["objVorerf"] = objVorerf;
            Session["RedirectToAuftragsliste"] = redirect;
            //Öffnen des Druckdialogs: PrintDialogKundenformulare.aspx
            RadWindow downloaddoc = RadWindowManager1.Windows[0];
            downloaddoc.Visible = true;
            downloaddoc.VisibleOnPageLoad = true;
        }

        /// <summary>
        /// cmdCancel_Click Ereignis - zurück
        /// Zurück zur Auftragsliste(Auftraege.aspx) bzw. Startseite(Selection.aspx)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (BackfromList == false)
            { Response.Redirect("/AutohausPortal/(S(" + Session.SessionID + "))/Start/Selection.aspx"); }
            else { Response.Redirect("Auftraege.aspx?AppID=" + AppIDListe); }
        } 

        #endregion

        #region Methods
        
        /// <summary>
        /// Füllen der Dropdowns mit Stammdaten
        /// </summary>
        private void fillDropDowns()
        {

            try
            {
                DataView tmpDView = new DataView();
                if (objCommon.tblKundenStamm.Rows.Count > 1)
                {
                    tmpDView = objCommon.tblKundenStamm.DefaultView;
                    tmpDView.Sort = "NAME1";
                    ddlKunnr1.DataSource = tmpDView;
                    ddlKunnr1.DataValueField = "KUNNR";
                    ddlKunnr1.DataTextField = "NAME1";
                    ddlKunnr1.DataBind();
                }
                else if (objCommon.tblKundenStamm.Rows.Count == 1)
                {

                    if (ddlKunnr1.Items.Count == 0 && !IsInAsync && BackfromList == false)
                    {
                        String Kunnr = objCommon.tblKundenStamm.Rows[0]["KUNNR"].ToString();
                        String Kundenname = objCommon.tblKundenStamm.Rows[0]["NAME1"].ToString();
                        ddlKunnr1.Items.Add(new RadComboBoxItem(Kundenname, Kunnr));
                        ddlKunnr1.SelectedValue = Kunnr;
                        setReferenz();
                        disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input");
                    }
                }

                //Zulassungskreise 
                tmpDView = objCommon.tblStvaStamm.DefaultView;
                tmpDView.Sort = "KREISTEXT";
                tmpDView.RowFilter = "KREISKZ <> ''";
                ddlStVa1.DataSource = tmpDView;
                ddlStVa1.DataValueField = "KREISKZ";
                ddlStVa1.DataTextField = "KREISTEXT";
                ddlStVa1.DataBind();

                //Fahrzeugarten
                tmpDView = objCommon.tblFahrzeugarten.DefaultView;
                tmpDView.Sort = "DOMVALUE_L";
                ddlFahrzeugart.DataSource = tmpDView;
                ddlFahrzeugart.DataValueField = "DOMVALUE_L";
                ddlFahrzeugart.DataTextField = "DDTEXT";
                ddlFahrzeugart.DataBind();
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
            }

        }

        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// </summary>
        private void fillForm()
        {

            objVorerf.VKBUR = m_User.Reference.Substring(4, 4);
            objVorerf.VKORG = m_User.Reference.Substring(0, 4);

            if (objVorerf.IsNewVorgang)
            {
                ucBemerkungenNotizen.addAttributesKunRef();
            }
            ddlFahrzeugart.SelectedValue = "1";
        }

        /// <summary>
        /// Setzen der Hilfstexte(TextBox DefaultValue) für die Referenzfelder je nach Kundenauswahl.
        /// </summary>
        private void setReferenz()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                txtReferenz1.Text = drow[0]["REF_NAME_01"].ToString();
                txtReferenz2.Text = drow[0]["REF_NAME_02"].ToString();
                txtReferenz3.Text = drow[0]["REF_NAME_03"].ToString();
                txtReferenz4.Text = drow[0]["REF_NAME_04"].ToString();
                if (String.IsNullOrEmpty(txtReferenz1.Text)) { txtReferenz1.Enabled = false; } else { txtReferenz1.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz2.Text)) { txtReferenz2.Enabled = false; } else { txtReferenz2.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz3.Text)) { txtReferenz3.Enabled = false; } else { txtReferenz3.Enabled = true; }
                if (String.IsNullOrEmpty(txtReferenz4.Text)) { txtReferenz4.Enabled = false; } else { txtReferenz4.Enabled = true; }

                // CPD-Kunde mit Einzugsermächtigung?
                if (drow[0]["XCPDEIN"].ToString() == "X")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetEinzug",
                        "<script type='text/javascript'>setZahlartEinzug();</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetZahlartRadiobuttons",
                        "<script type='text/javascript'>resetZahlart();</script>", false);
                }
            }
            addAttributes(txtReferenz1); enableDefaultValue(txtReferenz1);
            addAttributes(txtReferenz2); enableDefaultValue(txtReferenz2);
            addAttributes(txtReferenz3); enableDefaultValue(txtReferenz3);
            addAttributes(txtReferenz4); enableDefaultValue(txtReferenz4);
        }

        /// <summary>
        /// Fügt Javascript-Funktionen einer Textbox hinzu
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void addAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Add("onblur", "if(this.value=='')this.value=this.defaultValue");
            txtBox.Attributes.Add("onfocus", "if(this.value==this.defaultValue)this.value=''");
        }

        /// <summary>
        /// Entfernt Javascript-Funktionen der Textbox
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void removeAttributes(TextBox txtBox)
        {
            txtBox.Attributes.Remove("onblur");
            txtBox.Attributes.Remove("onfocus");
            disableDefaultValue(txtBox);
        }

        /// <summary>
        /// entfernt den Vorschlagswert der Textbox, wenn Eingabe erfolgte
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void disableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
    "<script type='text/javascript'>disableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        ///  Entfernt den Vorschlagswert der Textbox der gerenderten DropDown, wenn Eingabe erfolgte 
        /// </summary>
        /// <param name="txtBox">z.B. ctl00_ContentPlaceHolder1_ddlKunnr1_Input</param>
        private void disableDefaultValueDDL(String txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox,
                "<script type='text/javascript'>disableDefaultValue('" + txtBox + "');</script>", false);
        }

        /// <summary>
        /// Fügt Vorschlagswert einer Textbox hinzu 
        /// </summary>
        /// <param name="txtBox">Control</param>
        private void enableDefaultValue(TextBox txtBox)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), txtBox.ClientID,
                "<script type='text/javascript'>enableDefaultValue('" + txtBox.ClientID + "');</script>", false);
        }

        /// <summary>
        /// Funktion prüft ob Eingaben vorgenommen wurden
        /// </summary>
        private void proofInserted()
        {
            if (ddlKunnr1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input"); }
            if (ddlStVa1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlStVa1_Input"); }
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");

            if (drow.Length > 0)
            {
                if (txtReferenz1.Text != drow[0]["REF_NAME_01"].ToString()) { disableDefaultValue(txtReferenz1); }
                if (txtReferenz2.Text != drow[0]["REF_NAME_02"].ToString()) { disableDefaultValue(txtReferenz2); }
                if (txtReferenz3.Text != drow[0]["REF_NAME_03"].ToString()) { disableDefaultValue(txtReferenz3); }
                if (txtReferenz4.Text != drow[0]["REF_NAME_04"].ToString()) { disableDefaultValue(txtReferenz4); }

            }

            if (txtDatum.Text != "") { disableDefaultValue(txtDatum); }
            ucBemerkungenNotizen.proofInserted();
            ucBankdatenAdresse.proofInserted();

        }

        /// <summary>
        /// Einfügen der bereits vorhandenen Daten
        /// </summary>
        private void SelectValues()
        {
            //Einfügen der bereits vorhandenen Daten

            ddlKunnr1.Items.Clear();
            if (objVorerf.Kundenname.Contains(objVorerf.Kunnr))
            { ddlKunnr1.Items.Add(new RadComboBoxItem(objVorerf.Kundenname, objVorerf.Kunnr)); }
            else
            { ddlKunnr1.Items.Add(new RadComboBoxItem(objVorerf.Kundenname + " ~ " + objVorerf.Kunnr, objVorerf.Kunnr)); }
            ddlKunnr1.SelectedValue = objVorerf.Kunnr;

            ddlStVa1.SelectedValue = objVorerf.KreisKennz;

            String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");
            if (drow.Length > 0)
            {
                Ref1 = drow[0]["REF_NAME_01"].ToString();
                Ref2 = drow[0]["REF_NAME_02"].ToString();
                Ref3 = drow[0]["REF_NAME_03"].ToString();
                Ref4 = drow[0]["REF_NAME_04"].ToString();

            }

            if (objVorerf.Ref1 == String.Empty) { addAttributes(txtReferenz1); txtReferenz1.Text = Ref1; } else { txtReferenz1.Text = objVorerf.Ref1; }
            if (objVorerf.Ref2 == String.Empty) { addAttributes(txtReferenz2); txtReferenz2.Text = Ref2; } else { txtReferenz2.Text = objVorerf.Ref2; }
            if (objVorerf.Ref3 == String.Empty) { addAttributes(txtReferenz3); txtReferenz3.Text = Ref3; } else { txtReferenz3.Text = objVorerf.Ref3; }
            if (objVorerf.Ref4 == String.Empty) { addAttributes(txtReferenz4); txtReferenz4.Text = Ref4; } else { txtReferenz4.Text = objVorerf.Ref4; }


            if (Ref1.Length == 0) { txtReferenz1.Enabled = false; }
            if (Ref2.Length == 0) { txtReferenz2.Enabled = false; }
            if (Ref3.Length == 0) { txtReferenz3.Enabled = false; }
            if (Ref4.Length == 0) { txtReferenz4.Enabled = false; }

            txtDatum.Text = objVorerf.ZulDate;
            ddlFahrzeugart.SelectedValue = objVorerf.Fahrzeugart;

            LongStringToSap LSTS = new LongStringToSap();
            if (objVorerf.NrLangText != "")
            {
                objVorerf.LangText = LSTS.ReadString(m_User, m_App, this, objVorerf.NrLangText);
            }
            txtService2.Text = objVorerf.LangText;

            if (!objVorerf.IsNewVorgang)
            {
                cmdSave.Text = "Speichern/Liste";
            }

            ucBemerkungenNotizen.SelectValues(objVorerf);

            ucBankdatenAdresse.SelectValues(objVorerf);
            
            divHoldData.Visible = false;
            proofInserted();
        }

        /// <summary>
        /// Validierung der eingegebenen Daten
        /// </summary>
        private void ValidateData()
        {
            lblError.Text = "";

            if (ddlKunnr1.SelectedValue == "")
            {
                divKunde.Attributes.Add("class", "formbereich error");
                lblError.Text = "Kein Kunde ausgewählt. ";
            }
            else
            {
                DataRow[] rowKunde = objCommon.tblKundenStamm.Select("Kunnr = '" + ddlKunnr1.SelectedValue + "'");
                if (rowKunde.Length > 0)
                {
                    if (txtReferenz1.Text == rowKunde[0]["REF_NAME_01"].ToString() && rowKunde[0]["REF_NAME_01"].ToString() != "")
                    { divRef1.Attributes.Add("class", "formbereich error"); lblError.Text = rowKunde[0]["REF_NAME_01"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz2.Text == rowKunde[0]["REF_NAME_02"].ToString() && rowKunde[0]["REF_NAME_02"].ToString() != "")
                    { divRef2.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_02"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz3.Text == rowKunde[0]["REF_NAME_03"].ToString() && rowKunde[0]["REF_NAME_03"].ToString() != "")
                    { divRef3.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_03"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                    if (txtReferenz4.Text == rowKunde[0]["REF_NAME_04"].ToString() && rowKunde[0]["REF_NAME_04"].ToString() != "")
                    { divRef4.Attributes.Add("class", "formbereich error"); lblError.Text += rowKunde[0]["REF_NAME_04"].ToString() + " ist ein Pflichtfeld. <br/>"; }
                }
            }            
            if (txtService2.Text == "")
            {
                txtService2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EF2C32");
                lblError.Text += "Bitte tragen Sie die gewünschte Dienstleitung ein.";
            }
            else if (ddlFahrzeugart.SelectedValue == "0")
            {
                divFahrzeugart.Attributes["class"] = "formbereich error";
                lblError.Text += "Bitte wählen Sie eine Fahrzeugart!";
            }
            else if (ddlStVa1.SelectedValue == "")
            {
                divStVa.Attributes.Add("class", "formbereich error");
                lblError.Text += "Keine STVA ausgewählt.";
            }

            checkDate();
        }

        /// <summary>
        /// Validierung Zulassungsdatum
        /// </summary>
        /// <returns>Bei Fehler false</returns>
        private Boolean checkDate()
        {
            Boolean bReturn = true;
            String ZDat = txtDatum.Text;
            if (!String.IsNullOrEmpty(ZDat))
            {
                if (!ZDat.IsDate())
                {
                    divDatum.Attributes["class"] = "formfeld error";
                    lblError.Text = "Ungültiger Ausführungstermin: Falsches Format.";
                    bReturn = false;
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    DateTime DateNew;
                    DateTime.TryParse(ZDat, out DateNew);
                    if (DateNew < tagesdatum)
                    {
                        divDatum.Attributes["class"] = "formfeld error";
                        lblError.Text = "Das Datum darf nicht in der Vergangenheit liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            divDatum.Attributes["class"] = "formfeld error";
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                }
            }
            else
            {
                lblError.Text = "Ungültiger Ausführungstermin!";
                bReturn = false;
            }

            return bReturn;

        }

        /// <summary>
        /// entfernt das Errorstyle der Controls
        /// </summary>
        private void ClearError()
        {
            ucBankdatenAdresse.ClearError();
            divDatum.Attributes["class"] = "formfeld";
            divKunde.Attributes.Add("class", "formbereich");
            divStVa.Attributes.Add("class", "formbereich");
            divRef1.Attributes["class"] = "formfeld";
            divRef2.Attributes["class"] = "formfeld";
            divRef3.Attributes["class"] = "formfeld";
            divRef4.Attributes["class"] = "formfeld";
        }

        /// <summary>
        /// clearen der Eingabemaske nach dem Speichern
        /// </summary>
        private void ClearForm()
        {
            objVorerf = (AHErfassung)Session["objVorerf"];
            if (ddlKunnr1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlKunnr1_Input"); }
            if (ddlStVa1.SelectedValue != "") { disableDefaultValueDDL("ctl00_ContentPlaceHolder1_ddlStVa1_Input"); }

            ucBemerkungenNotizen.ResetFields();
            ucBankdatenAdresse.ResetFields();

            objVorerf.Name1 = "";
            objVorerf.Name2 = "";
            objVorerf.PLZ = "";
            objVorerf.Ort = "";
            objVorerf.Strasse = "";

            objVorerf.SWIFT = "";
            objVorerf.IBAN = "";
            objVorerf.Geldinstitut = "";
            objVorerf.EinzugErm = false;
            objVorerf.Rechnung = false;
            objVorerf.Barzahlung = false;
            objVorerf.Inhaber = "";

            if (!chkHoldData.Checked)
            {
                setReferenz();

                txtDatum.Text = "";
            }
            else
            {
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR ='" + ddlKunnr1.SelectedValue + "'");

                if (drow.Length > 0)
                {
                    String Ref1 = "", Ref2 = "", Ref3 = "", Ref4 = "";

                    if (drow.Length > 0)
                    {
                        Ref1 = drow[0]["REF_NAME_01"].ToString();
                        Ref2 = drow[0]["REF_NAME_02"].ToString();
                        Ref3 = drow[0]["REF_NAME_03"].ToString();
                        Ref4 = drow[0]["REF_NAME_04"].ToString();
                    }

                    if (objVorerf.Ref1 == String.Empty) { addAttributes(txtReferenz1); txtReferenz1.Text = Ref1; } else { removeAttributes(txtReferenz1); txtReferenz1.Text = objVorerf.Ref1; }
                    if (objVorerf.Ref2 == String.Empty) { addAttributes(txtReferenz2); txtReferenz2.Text = Ref2; } else { removeAttributes(txtReferenz2); txtReferenz2.Text = objVorerf.Ref2; }
                    if (objVorerf.Ref3 == String.Empty) { addAttributes(txtReferenz3); txtReferenz3.Text = Ref3; } else { removeAttributes(txtReferenz3); txtReferenz3.Text = objVorerf.Ref3; }
                    if (objVorerf.Ref4 == String.Empty) { addAttributes(txtReferenz4); txtReferenz4.Text = Ref4; } else { removeAttributes(txtReferenz4); txtReferenz4.Text = objVorerf.Ref4; }

                    if (Ref1.Length == 0) { txtReferenz1.Enabled = false; }
                    if (Ref2.Length == 0) { txtReferenz2.Enabled = false; }
                    if (Ref3.Length == 0) { txtReferenz3.Enabled = false; }
                    if (Ref4.Length == 0) { txtReferenz4.Enabled = false; }
                    // CPD-Kunde mit Einzugsermächtigung?
                    if (drow[0]["XCPDEIN"].ToString() == "X")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetEinzug",
                            "<script type='text/javascript'>setZahlartEinzug();</script>", false);
                    }
                    txtService2.Text = ""; removeAttributes(txtDatum);
                }
            }

            ddlFahrzeugart.SelectedValue = "1";
            objVorerf.id_sap = 0;
            Session["objVorerf"] = objVorerf;
        }

        /// <summary>
        /// Läd Anzahl der angelegten Aufträge / Anzeige in der Masterpage 
        /// </summary>
        private void getAuftraege()
        {
            HyperLink lnkMenge = (HyperLink)Master.FindControl("lnkMenge");
            var menge = objVorerf.GetAnzahlAuftraege(Session["AppID"].ToString(), Session.SessionID, this);
            Session["AnzahlAuftraege"] = menge;
            lnkMenge.Text = menge;
        }

        /// <summary>
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System existieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>ok?</returns>
        private bool proofBank()
        {
            bool blnOk = ucBankdatenAdresse.proofBank(ref objCommon);

            if (!blnOk)
            {
                lblError.Text = objCommon.Message;
            }

            return blnOk;
        }

        /// <summary>
        /// Validation Bank- und Adressdaten
        /// </summary>
        /// <param name="cpdKunde"></param>
        /// <returns>ok?</returns>
        private bool proofBankAndAddressData(bool cpdKunde = false)
        {
            bool blnOk = ucBankdatenAdresse.proofBankAndAddressData(cpdKunde);

            if (!blnOk)
            {
                lblError.Text = "Bank-/Adressdaten unvollständig!";
            }

            return blnOk;
        }

        #endregion
    }
}