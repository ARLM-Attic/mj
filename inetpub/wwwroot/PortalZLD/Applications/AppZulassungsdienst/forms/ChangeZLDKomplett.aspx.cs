﻿using System;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Kompletterfassung Eingabedialog.
    /// </summary>
    public partial class ChangeZLDKomplett : System.Web.UI.Page
    {
        #region Declarations
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private KomplettZLD objKompletterf;
        private ZLDCommon objCommon;
        Boolean _backfromList;// Flag, ob man von der Eingabeliste kommt
        String IDKopf;
        private const string CONST_IDSONSTIGEDL = "570";
        #endregion

        #region Events

        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden. 
        /// Datensatz für die Eingabe laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            _backfromList = false;

            if (Request.QueryString["B"] != null) { _backfromList = true; }
            if (m_User.Reference.Trim(' ').Length == 0)
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(ref m_User, m_App);
                objCommon.VKBUR = m_User.Reference.Substring(4, 4);
                objCommon.VKORG = m_User.Reference.Substring(0, 4);
                objCommon.getSAPDatenStamm(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }
            if (!IsPostBack)
            {
                if (_backfromList)
                {
                    //von der Übersicht einen Vorgang bearbeiten
                    Int32 id = 0;
                    if (Request.QueryString["id"] != null)
                    { IDKopf = Request.QueryString["id"]; }
                    else
                    { lblError.Text = "Fehler beim Laden des Vorganges!"; }

                    objKompletterf = (KomplettZLD)Session["objKompletterf"];
                    if (ZLDCommon.IsNumeric(IDKopf))
                    {
                        Int32.TryParse(IDKopf, out id);
                    }
                    if (id != 0)
                    {
                        objKompletterf.LoadDB_ZLDRecordset(id);// Vorgang laden
                        fillForm();
                        SelectValues();
                    }
                    else { lblError.Text = "Fehler beim Laden des Vorganges!"; }
                }
                else //Vorgang neu erfassen
                {
                    objKompletterf = new KomplettZLD(ref m_User, m_App, "K") { saved = false };
                    cmdCreate.Visible = false;
                    fillForm();
                    Session["KompSucheValue"] = null;
                    Session["KompRowfilter"] = null;
                }
                objKompletterf.ConfirmCPDAdress = false;

            }
        }

        /// <summary>
        /// Page_PreRender-Ereignis. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Page_Unload-Ereignis. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
            lblError.Text = "";
            lblMessage.Text = "";
        }

        /// <summary>
        /// Neue Dienstleistung/Artikel hinzuügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID = 0;
            Int32 NewPosIDData = 0;
            if (objKompletterf.Positionen.Rows.Count > 0)
            {
                Int32.TryParse(objKompletterf.Positionen.Rows[objKompletterf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
                NewPosID += 10;
            }

            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosIDData);
            NewPosIDData += 10;

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";//ID_POS 
            if (NewPosID > NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosID;
            }
            else if (NewPosID < NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosIDData;
            }
            else if (NewPosID == NewPosIDData)
            {
                tblRow["ID_POS"] = NewPosID;
            }
            tblRow["Menge"] = "1";
            tblRow["PosLoesch"] = "";
            tblData.Rows.Add(tblRow);
            Session["tblDienst"] = tblData;
            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataSource = tblData;
            GridView1.DataBind();
            if (objKompletterf.saved)
            {
                GridView1.Columns[4].Visible = true;
                GridView1.Columns[5].Visible = true;
                GridView1.Columns[6].Visible = true;
                if (m_User.Groups[0].Authorizationright == 1)// einige ZLD´s sollen Gebühr Amt nicht sehen
                {
                    GridView1.Columns[6].Visible = false;
                }
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
            }
            addButtonAttr(tblData);
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];

            cmdNewDLPrice.Enabled = true;
            cmdCreate.Enabled = false;

            TextBox txtBox;
            txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            ClearErrorBackcolor();
            lblErrorBank.Text = "";
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                if (chkCPD.Checked)
                {
                    bnoError = proofBankDataCPD();
                }
                else
                {
                    bnoError = proofBankDatawithoutCPD();
                }

                if (bnoError)
                {
                    objKompletterf.Name1 = txtName1.Text;
                    objKompletterf.Name2 = txtName2.Text;
                    objKompletterf.Strasse = txtStrasse.Text;
                    objKompletterf.PLZ = txtPlz.Text;
                    objKompletterf.Ort = txtOrt.Text;
                    objKompletterf.SWIFT = txtSWIFT.Text;
                    objKompletterf.IBAN = txtIBAN.Text;
                    objKompletterf.BankKey = objCommon.Bankschluessel;
                    objKompletterf.Kontonr = objCommon.Kontonr;
                    objKompletterf.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                    objKompletterf.Inhaber = txtKontoinhaber.Text;
                    objKompletterf.EinzugErm = chkEinzug.Checked;
                    objKompletterf.Rechnung = chkRechnung.Checked;
                    objKompletterf.ConfirmCPDAdress = true;
                    Session["objKompletterf"] = objKompletterf;
                    lblErrorBank.Text = "";
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Visible = true;
                    ButtonFooter.Visible = true;
                }
            }
        }

        /// <summary>
        /// Löschen von Dienstleistungen/Artikel.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int number;
                Int32.TryParse(e.CommandArgument.ToString(), out number);
                objKompletterf = (KomplettZLD)Session["objKompletterf"];
                DataTable tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    if (tblRows[0]["NewPos"].ToString() == "0" && objKompletterf.saved)
                    {
                        tblRows[0]["PosLoesch"] = "L";
                        DataRow[] DelRow = objKompletterf.Positionen.Select("id_pos='" + idpos + "'");
                        foreach (DataRow dRow in DelRow)
                        {
                            dRow["PosLoesch"] = "L";
                        }
                        DelRow = objKompletterf.Positionen.Select("UEPOS='" + idpos.PadLeft(6, '0') + "'");
                        foreach (DataRow dRow in DelRow)
                        {
                            dRow["PosLoesch"] = "L";
                        }
                    }
                    else
                    {
                        DataRow[] DelRow = objKompletterf.Positionen.Select("id_pos='" + idpos + "'");
                        if (DelRow.Length > 0)
                        {
                            objKompletterf.Positionen.Rows.Remove(DelRow[0]);
                        }
                        DelRow = objKompletterf.Positionen.Select("UEPOS='" + idpos.PadLeft(6, '0') + "'");
                        if (DelRow.Length > 0)
                        {
                            objKompletterf.Positionen.Rows.Remove(DelRow[0]);
                        }
                        tblData.Rows.Remove(tblRows[0]);
                    }

                    Session["tblDienst"] = tblData;
                    DataView tmpDataView = new DataView();
                    tmpDataView = tblData.DefaultView;
                    tmpDataView.RowFilter = "Not PosLoesch = 'L'";
                    GridView1.DataSource = tmpDataView;
                    GridView1.DataBind();

                    addButtonAttr(tblData);

                    if (objKompletterf.saved == false && tblData.Rows[0]["CALCDAT"].ToString() == "")
                    {
                        GridView1.Columns[4].Visible = false;
                        GridView1.Columns[5].Visible = false;
                        GridView1.Columns[6].Visible = false;
                        lblSteuer.Visible = false;
                        txtSteuer.Visible = false;
                        lblPreisKennz.Visible = false;
                        txtPreisKennz.Visible = false;
                        cmdCreate.Visible = false;
                    }
                    else
                    {
                        GridView1.Columns[4].Visible = true;
                        GridView1.Columns[5].Visible = true;
                        GridView1.Columns[6].Visible = true;
                        if (m_User.Groups[0].Authorizationright == 1)
                        {
                            GridView1.Columns[6].Visible = false;
                        }
                        lblSteuer.Visible = true;
                        txtSteuer.Visible = true;
                        lblPreisKennz.Visible = true;
                        txtPreisKennz.Visible = true;
                        cmdCreate.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Weiterleitung auf das zuständige Verkehrsamt, um Kennzeichen zu reservieren.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnReservierung_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            String sUrl = "";

            if (ddlStVa.SelectedValue != "")
            {
                if (objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa.SelectedValue + "'").Length > 0)
                {
                    sUrl = objCommon.tblStvaStamm.Select("KREISKZ = '" + ddlStVa.SelectedValue + "'")[0]["URL"].ToString();
                }
            }

            if (sUrl.Length > 0)
            {
                if ((!sUrl.Contains("http://")) && (!sUrl.Contains("https://")))
                {
                    sUrl = "http://" + sUrl;
                }
                String popupBuilder;
                if (!ClientScript.IsClientScriptBlockRegistered("clientScript"))
                {
                    popupBuilder = "<script languange=\"Javascript\">";
                    popupBuilder += "window.open('" + sUrl + "', 'POPUP', 'dependent=yes,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');";
                    popupBuilder += "</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "POPUP", popupBuilder, false);
                }
            }
            else { lblError.Text = "Das Straßenverkehrsamt für das Kennzeichen " + ddlStVa.SelectedValue + " bietet keine Weblink hierfür an."; }
        }

        /// <summary>
        /// Bankdialog öffnen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnBank_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Bitte wählen Sie einen Kunden aus!";
            }
            else
            {
                chkCPD.Checked = false;
                chkCPDEinzug.Checked = false;
                chkEinzug.Checked = false;
                chkRechnung.Checked = false;
                Panel1.Visible = false;
                ButtonFooter.Visible = false;
                txtZulDateBank.Text = txtZulDate.Text;
                txtKundebank.Text = ddlKunnr.SelectedItem.Text;
                txtKundeBankSuche.Text = txtKunnr.Text;
                txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
                txtRef2Bank.Text = txtReferenz2.Text.ToUpper();
                objKompletterf = (KomplettZLD)Session["objKompletterf"];

                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
                if (drow.Length == 1)
                {
                    if (drow[0]["XCPDK"].ToString() == "X")
                    {
                        chkCPD.Checked = true;
                        if (drow[0]["XCPDEIN"].ToString() == "X")
                        {
                            chkEinzug.Checked = true;
                            chkCPDEinzug.Checked = true;
                        }
                        else
                        {
                            chkCPDEinzug.Checked = false;
                            chkEinzug.Checked = objKompletterf.EinzugErm;
                        }
                    }
                    else
                    {
                        chkCPD.Checked = false;
                        chkCPDEinzug.Checked = false;
                        chkEinzug.Checked = objKompletterf.EinzugErm;
                        chkRechnung.Checked = objKompletterf.Rechnung;
                    }

                }
                if (objKompletterf.saved && objKompletterf.Kunnr == txtKunnr.Text)
                {
                    chkEinzug.Checked = objKompletterf.EinzugErm;
                    chkRechnung.Checked = objKompletterf.Rechnung;
                }
                txtName1.Focus();
                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
            }
        }

        /// <summary>
        /// Daten speichern
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DropDownList ddl;
            Label lblDLBezeichnung;

            bool blnSonstigeDLOffen = false;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if ((ddl.SelectedValue == CONST_IDSONSTIGEDL) && ((String.IsNullOrEmpty(lblDLBezeichnung.Text)) || (lblDLBezeichnung.Text == "Sonstige Dienstleistung")))
                {
                    blnSonstigeDLOffen = true;
                    break;
                }
            }

            // Wenn "Sonstige Dienstleistung" neu erfasst wurde, Dialog zur Erfassung eines Bezeichnungstextes öffnen, sonst direkt speichern
            if (blnSonstigeDLOffen)
            {
                mpeDLBezeichnung.Show();
            }
            else
            {
                DatenSpeichern();
            }
        }

        /// <summary>
        /// Den im PopUp gesetzten Beschreibungstext übernehmen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlgErfassungDLBez_TexteingabeBestaetigt(object sender, EventArgs e)
        {
            DropDownList ddl;
            Label lblDLBezeichnung;

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Zur Listenansicht(ChangeZLDKomListe.aspx).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            objKompletterf.LadeKompletterfDB_ZLD("K");
            Session["objKompletterf"] = objKompletterf;
            Response.Redirect("ChangeZLDKomListe.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Kennzeichen-Sondergröße Daten für ddlKennzForm laden. Auswählen der Sondergröße. 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            TextBox txtHauptPos = (TextBox)GridView1.Rows[0].FindControl("txtSearch");
            lblError.Text = "";

            objKompletterf = (KomplettZLD)Session["objKompletterf"];

            if (txtHauptPos != null && txtHauptPos.Text.Length > 0)
            {
                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblKennzGroesse.DefaultView;
                tmpDataView.RowFilter = "Matnr = " + txtHauptPos.Text;
                tmpDataView.Sort = "Matnr";
                if (tmpDataView.Count > 0)
                {
                    ddlKennzForm.DataSource = tmpDataView;
                    ddlKennzForm.DataTextField = "Groesse";
                    ddlKennzForm.DataValueField = "ID";
                    ddlKennzForm.DataBind();
                }
                else
                {
                    ddlKennzForm.Items.Clear();
                    ListItem liItem = new ListItem("", "0");
                    ddlKennzForm.Items.Add(liItem);
                }
            }
            SetBar_Pauschalkunde();
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
        }

        /// <summary>
        /// Bankdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancelBank_Click(object sender, EventArgs e)
        {
            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Visible = true;
            ButtonFooter.Visible = true;
        }

        /// <summary>
        /// Auftragsdaten über DAD-Barcode laden. Controls füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate0_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblMessage.Text = "";
                objKompletterf = (KomplettZLD)Session["objKompletterf"];
                objKompletterf.Barcode = txtBarcode.Text;
                objKompletterf.getDataFromBarcode(Session["AppID"].ToString(), Session.SessionID, this);
                if (objKompletterf.Status != 0)
                {
                    lblError.Text = objKompletterf.Message;
                }
                else
                {
                    if (objKompletterf.tblBarcodData.Rows.Count > 0)
                    {
                        ddlKunnr.SelectedValue = objKompletterf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtKunnr.Text = objKompletterf.tblBarcodData.Rows[0]["KUNNR"].ToString().TrimStart('0');
                        txtReferenz1.Text = objKompletterf.tblBarcodData.Rows[0]["ZZREFNR1"].ToString();
                        txtReferenz2.Text = objKompletterf.tblBarcodData.Rows[0]["ZZREFNR2"].ToString().TrimStart('0');
                        if (objKompletterf.tblBarcodData.Rows[0]["WUNSCHKENN_JN"].ToString() == "X")
                        {
                            chkWunschKZ.Checked = true;
                        }

                        if (ZLDCommon.IsDate(objKompletterf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString()))
                        {
                            DateTime dDate;
                            DateTime.TryParse(objKompletterf.tblBarcodData.Rows[0]["ZZZLDAT"].ToString(), out dDate);
                            txtZulDate.Text = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
                        }

                        int i = 0;
                        GridViewRow gvRow;
                        TextBox txtBox;
                        foreach (DataRow dRow in objKompletterf.tblBarcodMaterial.Rows)
                        {
                            if (GridView1.Rows[i] != null)
                            {
                                gvRow = GridView1.Rows[i];

                                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");

                                ddl.SelectedValue = dRow["MATNR"].ToString().TrimStart('0');
                                txtBox.Text = dRow["MATNR"].ToString().TrimStart('0');
                            }
                            i++;
                        }

                        if (GridView1.Rows.Count > 0)
                        {
                            gvRow = GridView1.Rows[0];
                            txtBox = (TextBox)gvRow.FindControl("txtSearch");
                            DataView tmpDView = objCommon.tblKennzGroesse.DefaultView;
                            tmpDView.RowFilter = "Matnr = " + txtBox.Text;
                            tmpDView.Sort = "Matnr";
                            if (tmpDView.Count > 0)
                            {
                                ddlKennzForm.DataSource = tmpDView;
                                ddlKennzForm.DataTextField = "Groesse";
                                ddlKennzForm.DataValueField = "ID";
                                ddlKennzForm.DataBind();
                            }
                        }
                        String[] kreisKz = objKompletterf.tblBarcodData.Rows[0]["ZZKENN"].ToString().Split('-');
                        if (kreisKz.Length > 0)
                        {
                            ddlStVa.SelectedValue = kreisKz[0].ToString();
                            txtStVa.Text = kreisKz[0].ToString();
                            txtKennz1.Text = kreisKz[0].ToString();
                        }
                        if (kreisKz.Length > 1)
                        {
                            txtKennz2.Text = kreisKz[1].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Laden des Auftrages! " + ex.Message;
            }
        }

        /// <summary>
        /// Preis ermitteln. Bei geänderter Hauptdienstleistung und /oder Kunden.
        /// objKompletterf.GetPreise(). Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdFindPrize_Click(object sender, EventArgs e)
        {
            //Daten die für Preisfindung relevant in die Klasse laden
            lblError.Text = "";

            if ((Session["objKompletterf"] == null) || (Session["tblDienst"] == null))
            {
                //Seite neu laden/initialisieren, wenn Session-Variablen verloren gegangen sind
                Response.Redirect(Request.RawUrl);
            }

            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            DataTable tblData = (DataTable)Session["tblDienst"];

            objKompletterf.Kreis = ddlStVa.SelectedItem.Text;
            objKompletterf.KreisKennz = txtStVa.Text;

            objKompletterf.WunschKennz = chkWunschKZ.Checked;
            objKompletterf.Reserviert = chkReserviert.Checked;
            objKompletterf.Barkunde = chkBar.Checked;
            SetBar_Pauschalkunde();

            objKompletterf.PauschalKunde = "";
            if (Pauschal.InnerHtml == "Pauschalkunde") { objKompletterf.PauschalKunde = "X"; }

            if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
            {
                objKompletterf.Kunnr = txtKunnr.Text;
            }
            else { lblError.Text = "Bitte Kunde auswählen!"; return; }
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                objKompletterf.OhneSteuer = drow[0]["OHNEUST"].ToString();
            }
            if (txtZulDate.Text.Length > 0)
            {
                objKompletterf.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
            }
            //OhneSteuer
            objKompletterf.EinKennz = chkEinKennz.Checked;
            if (chkEinKennz.Checked)
            {
                objKompletterf.KennzAnzahl = 1;
            }

            //Ausgewählte Dienstleistungen und dazugehörige
            //Gebührenmaterialien der Positionstabelle übergeben
            GetDiensleitungDataforPrice(ref tblData);

            //Preise ermitteln
            if (cbxSave.Checked == false)
            {
                objKompletterf.SapID = 999;//ID Workaround

                objKompletterf.GetPreise(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
                txtPreisKennz.Text = objKompletterf.PreisKennz.ToString();
                cmdCreate.Visible = true;
            }
            else
            {
                objKompletterf.GetPreise(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblStvaStamm, objCommon.tblMaterialStamm);
            }

            if (objKompletterf.Status != 0)
            {
                if (objKompletterf.Status == -5555)
                {
                    lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                    cmdCreate.Visible = false;
                }
            }
            else
            {
                hfKunnr.Value = txtKunnr.Text;
                tblData.Rows.Clear();
                Int16 PosCount = 1;
                // ermittelte Preise ins Dienstleistungsgrid laden
                foreach (DataRow dRow in objKompletterf.Positionen.Rows)
                {
                    if (dRow["id_Kopf"].ToString() == objKompletterf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "D")
                    {
                        DataRow tblRow = tblData.NewRow();
                        tblRow["Search"] = dRow["Matnr"].ToString().TrimStart('0');
                        tblRow["Value"] = dRow["Matnr"].ToString().TrimStart('0');
                        tblRow["OldValue"] = dRow["Matnr"].ToString().TrimStart('0');
                      
                        tblRow["Text"] = dRow["MatBez"].ToString();
                        tblRow["Preis"] = dRow["Preis"].ToString();
                        tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                        tblRow["GebAmt"] = dRow["Preis_Amt"].ToString();
                        tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                        tblRow["NewPos"] = "0";
                        tblRow["SdRelevant"] = dRow["SDRelevant"];
                        tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                        tblRow["PosLoesch"] = dRow["PosLoesch"].ToString();
                        tblRow["UPREIS"] = dRow["UPREIS"].ToString();
                        tblRow["Differrenz"] = dRow["Differrenz"].ToString();
                        tblRow["Konditionstab"] = dRow["Konditionstab"].ToString();
                        tblRow["Konditionsart"] = dRow["Konditionsart"].ToString();
                        Decimal iMenge = 1;
                        if (ZLDCommon.IsDecimal(dRow["Menge"].ToString().Trim()))
                        {
                            Decimal.TryParse(dRow["Menge"].ToString(), out iMenge);
                        }
                        tblRow["Menge"] = iMenge.ToString("0");
                        if (ZLDCommon.IsDate(dRow["CALCDAT"].ToString()))
                        {
                            tblRow["CALCDAT"] = dRow["CALCDAT"].ToString();
                        }
                        if ((Int32)dRow["id_pos"] == 10)
                        {
                            hfMatnr.Value = dRow["Matnr"].ToString().TrimStart('0');
                            txtPreisKennz.Enabled = true;
                            Boolean bEnabled = proofPauschMat(objKompletterf.PauschalKunde, dRow["Matnr"].ToString().TrimStart('0'));
                            if (bEnabled == false)
                            {
                                txtPreisKennz.Text = "0,00";
                                txtPreisKennz.Enabled = false;
                            }
                        }

                        if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }

                        tblData.Rows.Add(tblRow);
                        PosCount++;
                    }
                    else if (dRow["id_Kopf"].ToString() == objKompletterf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "K")
                    { txtPreisKennz.Text = dRow["Preis"].ToString(); }
                    else if (dRow["id_Kopf"].ToString() == objKompletterf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "S")
                    { txtSteuer.Text = dRow["Preis"].ToString(); }
                }
                DataView tmpDataView = new DataView();
                tmpDataView = tblData.DefaultView;
                tmpDataView.RowFilter = "Not PosLoesch = 'L'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                addButtonAttr(tblData);

                GridView1.Columns[3].Visible = true;
                GridView1.Columns[4].Visible = true;
                GridView1.Columns[5].Visible = true;
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    GridView1.Columns[5].Visible = false;
                }
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
                cmdCreate.Visible = true;
                cmdNewDLPrice.Visible = true;
                objKompletterf.SapID = 0;
            }

            Session["tblDienst"] = tblData;
            Session["objKompletterf"] = objKompletterf;
            cmdCreate.Enabled = true;
            cmdCreate.Focus();
        }

        /// <summary>
        /// Preis ergänzte DL. ermitteln. Bei geänderten Dienstleistungen/Artikel ausser der Haupdienstleistung.
        /// Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNewDLPrice_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            DataTable tblData = (DataTable)Session["tblDienst"];

            cmdCreate.Enabled = true;

            if (proofdifferentHauptMatnr(ref tblData))
            {
                lblError.Text = "Hauptdienstleistung geändert! Bitte auf Preis finden gehen!";
                cmdCreate.Enabled = false;
            }

            if (lblError.Text == "")
            {
                tblData.Rows.Clear();
                Int16 PosCount = 1;
                foreach (DataRow dRow in objKompletterf.Positionen.Rows)
                {
                    if (dRow["WebMTArt"].ToString() == "D")
                    {
                        DataRow tblRow = tblData.NewRow();
                        tblRow["Search"] = dRow["Matnr"].ToString().TrimStart('0');
                        tblRow["Value"] = dRow["Matnr"].ToString().TrimStart('0');
                        tblRow["OldValue"] = dRow["Matnr"].ToString().TrimStart('0');
                        tblRow["Text"] = dRow["MatBez"].ToString();
                        tblRow["Preis"] = dRow["Preis"].ToString();
                        tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                        tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                        tblRow["NewPos"] = "0";
                        tblRow["PosLoesch"] = dRow["PosLoesch"];
                        tblRow["SdRelevant"] = dRow["SDRelevant"];
                        tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                        tblRow["GebAmt"] = dRow["Preis_Amt"];
                        tblRow["Menge"] = dRow["Menge"];

                        tblData.Rows.Add(tblRow);
                        if ((Int32)dRow["id_pos"] == 10)
                        {
                            txtPreisKennz.Enabled = true;
                            Boolean bEnabled = proofPauschMat(objKompletterf.PauschalKunde, dRow["Matnr"].ToString().TrimStart('0'));
                            if (bEnabled == false)
                            {
                                txtPreisKennz.Text = "0,00";
                                txtPreisKennz.Enabled = false;
                            }
                        }

                        if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }

                        PosCount++;
                    }
                    else if (dRow["id_Kopf"].ToString() == objKompletterf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "K")
                    { 
                        txtPreisKennz.Text = dRow["Preis"].ToString();

                        if (dRow["Preis"].ToString().Contains(","))
                        {
                            String[] FormatPreis = dRow["Preis"].ToString().Split(',');
                            if (FormatPreis.Length == 2)
                            {
                                if (FormatPreis[1].Length == 4) { txtPreisKennz.Text = dRow["Preis"].ToString().Substring(0, dRow["Preis"].ToString().Length - 2); }
                            }
                        }                    
                    }
                    else if (dRow["id_Kopf"].ToString() == objKompletterf.KopfID.ToString() && dRow["WebMTArt"].ToString() == "S")
                    {   txtSteuer.Text = dRow["Preis"].ToString();
                        if (dRow["Preis"].ToString().Contains(","))
                        {
                            String[] FormatPreis = dRow["Preis"].ToString().Split(',');
                            if (FormatPreis.Length == 2)
                            {
                                if (FormatPreis[1].Length == 4) { txtSteuer.Text = dRow["Preis"].ToString().Substring(0, dRow["Preis"].ToString().Length - 2); }
                            }
                        }                    
                    }
                }

                DataView tmpDataView = new DataView();
                tmpDataView = tblData.DefaultView;
                tmpDataView.RowFilter = "NOT PosLoesch = 'L'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    GridView1.Columns[5].Visible = false;
                }
                addButtonAttr(tblData);
            }
            else { cmdCreate.Visible = false; }
            Session["tblDienst"] = tblData;
            Session["objKompletterf"] = objKompletterf;
        }

        /// <summary>
        /// FSP vom Amt (Art. 559) hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnFeinstaub_Click(object sender, EventArgs e)
        {
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID = 0;
            Int32 NewPosIDData = 0;
            if (objKompletterf.Positionen.Rows.Count > 0)
            {
                Int32.TryParse(objKompletterf.Positionen.Rows[objKompletterf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
                NewPosID += 10;
            }

            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosIDData);
            NewPosIDData += 10;

            int newPosIdForFSP;
            if (NewPosID < NewPosIDData)
            {
                newPosIdForFSP = NewPosIDData;
            }
            else
            {
                newPosIdForFSP = NewPosID;
            }

            bool found = false;
            for (int i = 0; i < tblData.Rows.Count; i++)
            {
                var row = tblData.Rows[i];

                if (row["Value"].ToString() == "0")
                {
                    row["Search"] = "559";
                    row["Value"] = "559";
                    row["Text"] = "";
                    row["PosLoesch"] = "";
                    row["Menge"] = "1";
                    row["DLBezeichnung"] = "";
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                DataRow tblRow = tblData.NewRow();
                tblRow["Search"] = "559";
                tblRow["Value"] = "559";
                tblRow["Text"] = "";
                tblRow["ID_POS"] = newPosIdForFSP;
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            DataView tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataSource = tblData;
            GridView1.DataBind();
            if (objKompletterf.saved)
            {
                GridView1.Columns[4].Visible = true;
                GridView1.Columns[5].Visible = true;
                GridView1.Columns[6].Visible = true;
                if (m_User.Groups[0].Authorizationright == 1)// einige ZLD´s sollen Gebühr Amt nicht sehen
                {
                    GridView1.Columns[6].Visible = false;
                }
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
            }
            addButtonAttr(tblData);

            cmdNewDLPrice.Enabled = true;
            cmdCreate.Enabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Daten aus den Controls sammeln und in SQL speichern. 
        /// Clearen der Controls um einen neuen Vorgang anzulegen.
        /// </summary>
        private void DatenSpeichern()
        {
            lblError.Text = "";
            lblMessage.Text = "";
            objKompletterf = (KomplettZLD)Session["objKompletterf"];

            GetData();

            if (lblError.Text.Length == 0)
            {
                objKompletterf.Barcode = txtBarcode.Text;
                if (txtKunnr.Text != String.Empty && txtKunnr.Text != "0")
                {
                    String[] stemp = ddlKunnr.SelectedItem.Text.Split('~');
                    if (stemp.Length == 2)
                    {
                        objKompletterf.Kundenname = stemp[0].ToString();
                    }
                    if (objKompletterf.Kunnr != txtKunnr.Text)
                    {
                        objKompletterf.Kunnr = txtKunnr.Text;
                        lblError.Text = "Kunde geändert! Klicken Sie bitte auf 'Preis Finden'!";
                        cmdCreate.Enabled = false;
                        return;
                    }
                }

                objKompletterf.Ref1 = txtReferenz1.Text.ToUpper();
                objKompletterf.Ref2 = txtReferenz2.Text.ToUpper();

                objKompletterf.KreisKennz = txtStVa.Text;
                objKompletterf.Kreis = ddlStVa.SelectedItem.Text;

                objKompletterf.WunschKennz = chkWunschKZ.Checked;
                objKompletterf.Reserviert = chkReserviert.Checked;
                objKompletterf.ReserviertKennz = txtNrReserviert.Text;

                objKompletterf.ZulDate = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objKompletterf.Kennzeichen = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                objKompletterf.Kennztyp = "";
                objKompletterf.KennzAnzahl = 2;
                objKompletterf.EinKennz = chkEinKennz.Checked;
                if (ddlKennzForm.Items.Count > 0)
                {
                    objKompletterf.KennzForm = ddlKennzForm.SelectedItem.Text;
                }
                else { objKompletterf.KennzForm = ""; }
                objKompletterf.Bar = rbECBar.Checked;
                objKompletterf.EC = rbECGeb.Checked;
                objKompletterf.RE = rbRE.Checked;
                objKompletterf.Barkunde = chkBar.Checked;
                DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
                if (drow.Length == 1)
                {
                    objKompletterf.OhneSteuer = drow[0]["OHNEUST"].ToString();
                }
                DataTable tblData = (DataTable)Session["tblDienst"];
                if (GetDiensleitungData(ref tblData))
                {
                    lblError.Text = "Dienstleistung geändert! Bitte auf Preis finden gehen!";
                    Session["tblDienst"] = tblData;
                    cmdCreate.Enabled = false;
                    return;
                }

                Session["tblDienst"] = tblData;
                Decimal Preis = 0;
                if (ZLDCommon.IsDecimal(txtPreisKennz.Text))
                {
                    Decimal.TryParse(txtPreisKennz.Text, out Preis);
                    objKompletterf.PreisKennz = Preis;
                }
                else
                {
                    objKompletterf.PreisKennz = 0;
                }

                if (ZLDCommon.IsDecimal(txtSteuer.Text))
                {
                    Decimal.TryParse(txtSteuer.Text, out Preis);
                    objKompletterf.Steuer = Preis;
                }
                else
                {
                    objKompletterf.Steuer = 0;
                }
                Boolean bnoError = false;

                proofCPDonSave();
                if (chkCPD.Checked)
                {
                    bnoError = proofBankDataCPD();
                    if (bnoError && objKompletterf.ConfirmCPDAdress == false)
                    {
                        bnoError = false;
                    }
                }
                else { bnoError = proofBankDatawithoutCPD(); }
                if (bnoError)
                {
                    Boolean bEinzug = chkEinzug.Checked;
                    Boolean bRechnung = chkRechnung.Checked;
                    objKompletterf.Name1 = txtName1.Text;
                    objKompletterf.Partnerrolle = objKompletterf.Name1.Length > 0 ? objKompletterf.Partnerrolle = "AG" : objKompletterf.Partnerrolle = "";
                    objKompletterf.Name2 = txtName2.Text;
                    objKompletterf.Strasse = txtStrasse.Text;
                    objKompletterf.PLZ = txtPlz.Text;
                    objKompletterf.Ort = txtOrt.Text;
                    objKompletterf.SWIFT = txtSWIFT.Text;
                    objKompletterf.IBAN = txtIBAN.Text;
                    objKompletterf.Geldinstitut = txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "";
                    objKompletterf.Inhaber = txtKontoinhaber.Text;
                    objKompletterf.EinzugErm = bEinzug;
                    objKompletterf.Rechnung = bRechnung;
                    Session["objKompletterf"] = objKompletterf;
                    lblErrorBank.Text = "";
                }
                else
                {
                    lbtnBank_Click(this, new EventArgs());
                    return;
                }

                objKompletterf.EinKennz = chkEinKennz.Checked;
                if (chkEinKennz.Checked)
                {
                    objKompletterf.KennzAnzahl = 1;
                }

                objKompletterf.Bemerkung = txtBemerk.Text;

                if (cbxSave.Checked == false)
                {
                    objKompletterf.saved = true;
                    objKompletterf.InsertDB_ZLD(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm, objCommon.tblMaterialStamm);
                    cmdCreate.Visible = false;
                }
                else
                {
                    objKompletterf.saved = true;
                    objKompletterf.bearbeitet = true;
                    objKompletterf.UpdateDB_ZLD(Session.SessionID, objCommon.tblKundenStamm, objCommon.tblMaterialStamm, this);
                    if (objKompletterf.Status == 0)
                    {
                        LinkButton1_Click(this, new EventArgs());
                    }

                }
                objKompletterf.ConfirmCPDAdress = false;
                ClearForm();
                SetBar_Pauschalkunde();
                txtBarcode.Focus();
                if (objKompletterf.Status == 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
                    lblMessage.Text = "Datensatz unter ID " + objKompletterf.SapID + " gespeichert.";
                }
                else
                {
                    lblError.Text = "Fehler beim anlegen des Datensatzes: " + objKompletterf.Message;
                }
            }
        }

        /// <summary>
        /// Füllen der Controls mit den bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues()
        {
            txtBarcode.Text = objKompletterf.Barcode;
            txtKunnr.Text = objKompletterf.Kunnr;
            hfKunnr.Value = objKompletterf.Kunnr;
            ddlKunnr.SelectedValue = objKompletterf.Kunnr;
            txtReferenz1.Text = objKompletterf.Ref1;
            txtReferenz2.Text = objKompletterf.Ref2;
            txtStVa.Text = objKompletterf.KreisKennz;
            ddlStVa.SelectedValue = objKompletterf.KreisKennz;
            txtKennz1.Text = objKompletterf.KreisKennz;
            chkWunschKZ.Checked = objKompletterf.WunschKennz;
            chkReserviert.Checked = objKompletterf.Reserviert;
            txtNrReserviert.Text = objKompletterf.ReserviertKennz;
            String tmpDate = objKompletterf.ZulDate;
            txtZulDate.Text = tmpDate.Substring(0, 2) + tmpDate.Substring(3, 2) + tmpDate.Substring(8, 2);
            String[] tmpKennz = objKompletterf.Kennzeichen.Split('-');
            txtKennz1.Text = "";
            txtKennz2.Text = "";
            txtSteuer.Text = objKompletterf.Steuer.ToString();
            txtPreisKennz.Text = objKompletterf.PreisKennz.ToString();
            if (objKompletterf.Steuer.ToString().Contains(","))
            {
                txtSteuer.Text = objKompletterf.Steuer.ToString().Substring(0, objKompletterf.Steuer.ToString().Length - 2);
            }
            if (objKompletterf.PreisKennz.ToString().Contains(","))
            {
                txtPreisKennz.Text = objKompletterf.PreisKennz.ToString().Substring(0, objKompletterf.PreisKennz.ToString().Length - 2);
            }

            cbxSave.Checked = objKompletterf.saved;

            if (objKompletterf.saved)
            {
                cmdCreate.Text = "» Speichern/Liste";
                cmdFindPrize.Visible = true;
            }

            if (tmpKennz.Length == 1)
            {
                txtKennz1.Text = tmpKennz[0].ToString();
            }
            else if (tmpKennz.Length == 2)
            {
                txtKennz1.Text = tmpKennz[0].ToString();
                txtKennz2.Text = tmpKennz[1].ToString();
            }
            else if (tmpKennz.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
            {
                txtKennz1.Text = tmpKennz[0].ToString();
                txtKennz2.Text = tmpKennz[1].ToString() + "-" + tmpKennz[2].ToString();
            }
            txtBemerk.Text = objKompletterf.Bemerkung;
            rbECBar.Checked = objKompletterf.Bar;
            rbECGeb.Checked = objKompletterf.EC;
            chkBar.Checked = objKompletterf.Barkunde;
            rbRE.Checked = objKompletterf.RE;
            chkEinKennz.Checked = objKompletterf.EinKennz;
            DataTable tblData = new DataTable();
            tblData.Columns.Add("Search", typeof(String));
            tblData.Columns.Add("Value", typeof(String));
            tblData.Columns.Add("Text", typeof(String));
            tblData.Columns.Add("Preis", typeof(Decimal));
            tblData.Columns.Add("GebPreis", typeof(Decimal));
            tblData.Columns.Add("ID_POS", typeof(Int32));
            tblData.Columns.Add("NewPos", typeof(String));
            tblData.Columns.Add("PosLoesch", typeof(String));
            tblData.Columns.Add("SdRelevant", typeof(String));
            tblData.Columns.Add("GebMatPflicht", typeof(String));
            tblData.Columns.Add("GebAmt", typeof(Decimal));
            tblData.Columns.Add("UPreis", typeof(Decimal));
            tblData.Columns.Add("Differrenz", typeof(Decimal));
            tblData.Columns.Add("Konditionstab", typeof(String));
            tblData.Columns.Add("Konditionsart", typeof(String));
            tblData.Columns.Add("CALCDAT", typeof(DateTime));
            tblData.Columns.Add("Menge", typeof(String));
            tblData.Columns.Add("DLBezeichnung", typeof(String));
            tblData.Columns.Add("OldValue", typeof(String));

            foreach (DataRow dRow in objKompletterf.Positionen.Rows)
            {
                if (dRow["id_Kopf"].ToString() == IDKopf && dRow["WebMTArt"].ToString() == "D")
                {
                    DataRow tblRow = tblData.NewRow();
                    tblRow["Search"] = dRow["Matnr"].ToString();
                    tblRow["Value"] = dRow["Matnr"].ToString();
                    tblRow["OldValue"] = dRow["Matnr"].ToString();
                    tblRow["Text"] = dRow["MatBez"].ToString();
                    tblRow["Preis"] = dRow["Preis"].ToString();
                    tblRow["GebPreis"] = dRow["GebPreis"].ToString();
                    tblRow["ID_POS"] = (Int32)dRow["id_pos"];
                    if ((Int32)dRow["id_pos"] == 10) hfMatnr.Value = dRow["Matnr"].ToString();
                    tblRow["NewPos"] = "0";
                    tblRow["PosLoesch"] = dRow["PosLoesch"];
                    tblRow["SdRelevant"] = dRow["SDRelevant"];
                    tblRow["GebMatPflicht"] = dRow["GebMatPflicht"];
                    tblRow["GebAmt"] = dRow["Preis_Amt"];
                    if ((Int32)dRow["id_pos"] == 10)
                    {
                        txtPreisKennz.Enabled = true;
                        Boolean bEnabled = proofPauschMat(objKompletterf.PauschalKunde, dRow["Matnr"].ToString().TrimStart('0'));
                        if (bEnabled == false)
                        {
                            txtPreisKennz.Text = "0,00";
                            txtPreisKennz.Enabled = false;
                        }
                    }
                    tblRow["Menge"] = dRow["Menge"].ToString();
                    if (dRow["Matnr"].ToString() == CONST_IDSONSTIGEDL)
                    {
                        tblRow["DLBezeichnung"] = dRow["MatBez"].ToString();
                    }
                    else
                    {
                        tblRow["DLBezeichnung"] = "";
                    }
                    tblData.Rows.Add(tblRow);
                }
            }
            DataView tmpDataView = new DataView();
            tmpDataView = tblData.DefaultView;
            tmpDataView.RowFilter = "Not PosLoesch = 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);
            if (objKompletterf.saved == false)
            {
                GridView1.Columns[3].Visible = false;
                GridView1.Columns[4].Visible = false;
                GridView1.Columns[5].Visible = false;
                lblSteuer.Visible = false;
                txtSteuer.Visible = false;
                lblPreisKennz.Visible = false;
                txtPreisKennz.Visible = false;
                cmdCreate.Visible = false;
                cmdNewDLPrice.Visible = false;
                if (objKompletterf.Positionen.Rows.Count > 0) { cmdNewDLPrice.Visible = true; }
            }
            else
            {
                GridView1.Columns[3].Visible = true;
                GridView1.Columns[4].Visible = true;
                GridView1.Columns[5].Visible = true;
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    GridView1.Columns[5].Visible = false;
                }
                lblSteuer.Visible = true;
                txtSteuer.Visible = true;
                lblPreisKennz.Visible = true;
                txtPreisKennz.Visible = true;
                cmdCreate.Visible = true;
                cmdNewDLPrice.Visible = true;
            }

            GridViewRow gridRow = GridView1.Rows[0];
            TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");
            DataView tmpDView = new DataView();
            tmpDView = objCommon.tblKennzGroesse.DefaultView;
            tmpDView.RowFilter = "Matnr = " + txtHauptPos.Text;
            tmpDView.Sort = "Matnr";
            if (tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();
                if (objKompletterf.KennzForm.Length > 0)
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + objKompletterf.KennzForm + "' AND Matnr= '" + txtHauptPos.Text + "'");
                    if (kennzRow.Length > 0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                    }
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ListItem liItem = new ListItem("", "0");
                ddlKennzForm.Items.Add(liItem);
            }

            Session["tblDienst"] = tblData;

            txtName1.Text = objKompletterf.Name1;
            txtName2.Text = objKompletterf.Name2;
            txtPlz.Text = objKompletterf.PLZ;
            txtOrt.Text = objKompletterf.Ort;
            txtStrasse.Text = objKompletterf.Strasse;

            txtSWIFT.Text = objKompletterf.SWIFT;
            txtIBAN.Text = objKompletterf.IBAN;
            if (objKompletterf.Geldinstitut.Length > 0)
            {
                txtGeldinstitut.Text = objKompletterf.Geldinstitut;
            }
            txtKontoinhaber.Text = objKompletterf.Inhaber;
            chkEinzug.Checked = objKompletterf.EinzugErm;
            chkRechnung.Checked = objKompletterf.Rechnung;
            SetBar_Pauschalkunde();
        }

        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// verknüpft Texboxen und DropDowns mit JS-Funktionen
        /// Initialisiert die interne Dienstleistungstabelle
        /// </summary>
        private void fillForm()
        {
            objKompletterf.VKBUR = m_User.Reference.Substring(4, 4);
            objKompletterf.VKORG = m_User.Reference.Substring(0, 4);
            Session["objKompletterf"] = objKompletterf;

            ListItem liItem = new ListItem("520x114", "574");
            ddlKennzForm.Items.Add(liItem);
            if (objKompletterf.Status > 0)
            {
                lblError.Text = objKompletterf.Message;
                return;
            }
            else
            {   //Positionstablle erstellen(Dienstleistung/Artikel)
                DataTable tblData = new DataTable();
                tblData.Columns.Add("Search", typeof(String));
                tblData.Columns.Add("Value", typeof(String));
                tblData.Columns.Add("Text", typeof(String));
                tblData.Columns.Add("Preis", typeof(Decimal));
                tblData.Columns.Add("GebPreis", typeof(Decimal));
                tblData.Columns.Add("ID_POS", typeof(Int32));
                tblData.Columns.Add("NewPos", typeof(String));
                tblData.Columns.Add("PosLoesch", typeof(String));
                tblData.Columns.Add("SdRelevant", typeof(String));
                tblData.Columns.Add("GebMatPflicht", typeof(String));
                tblData.Columns.Add("GebAmt", typeof(Decimal));
                tblData.Columns.Add("UPreis", typeof(Decimal));
                tblData.Columns.Add("Differrenz", typeof(Decimal));
                tblData.Columns.Add("Konditionstab", typeof(String));
                tblData.Columns.Add("Konditionsart", typeof(String));
                tblData.Columns.Add("CALCDAT", typeof(DateTime));
                tblData.Columns.Add("Menge", typeof(String));
                tblData.Columns.Add("OldValue", typeof(String));
                tblData.Columns.Add("DLBezeichnung", typeof(String));

                for (int i = 1; i < 4; i++)
                {
                    DataRow tblRow = tblData.NewRow();

                    tblRow["Search"] = "";
                    tblRow["Value"] = "0";
                    tblRow["OldValue"] = "";
                    tblRow["ID_POS"] = i * 10;
                    tblRow["Preis"] = 0;
                    tblRow["GebPreis"] = 0;
                    tblRow["PosLoesch"] = "";
                    tblRow["NewPos"] = "0";
                    tblRow["SdRelevant"] = "";
                    tblRow["GebMatPflicht"] = "";
                    tblRow["GebAmt"] = 0;
                    tblRow["Menge"] = "";
                    tblRow["DLBezeichnung"] = "";
 
                    tblData.Rows.Add(tblRow);
                }

                GridView1.DataSource = tblData;
                GridView1.DataBind();
                if (objKompletterf.saved == false)
                {   // Spalten wie Preis, Gebühr, Gebühr Amt, Steuer, Preis Kennz. ausblenden,
                    // erst nachdem Preise gezogen wurden
                    GridView1.Columns[3].Visible = false;
                    GridView1.Columns[4].Visible = false;
                    GridView1.Columns[5].Visible = false;
                    lblSteuer.Visible = false;
                    txtSteuer.Visible = false;
                    lblPreisKennz.Visible = false;
                    txtPreisKennz.Visible = false;
                    cmdCreate.Visible = false;// Speichern/Neu
                    cmdNewDLPrice.Visible = false;//Preis ergänzte DL
                    if (objKompletterf.Positionen.Rows.Count > 0) { cmdNewDLPrice.Visible = true; }
                }
                else
                {
                    GridView1.Columns[3].Visible = true;
                    GridView1.Columns[4].Visible = true;
                    GridView1.Columns[5].Visible = true;
                    if (m_User.Groups[0].Authorizationright == 1)
                    {
                        GridView1.Columns[6].Visible = false;
                    }
                    lblSteuer.Visible = true;
                    txtSteuer.Visible = true;
                    lblPreisKennz.Visible = true;
                    txtPreisKennz.Visible = true;
                    cmdCreate.Visible = true;
                    cmdNewDLPrice.Visible = true;
                }
                //javascript-Funktionen anhängen im Grid
                addButtonAttr(tblData);
                TableToJSArrayMengeErlaubt();
                GridViewRow gridRow = GridView1.Rows[0];
                TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");

                Session["tblDienst"] = tblData;

                // Kundenstamm 
                DataView tmpDView = new DataView();
                tmpDView = objCommon.tblKundenStamm.DefaultView;
                tmpDView.Sort = "NAME1";
                tmpDView.RowFilter = "INAKTIV <> 'X'";
                ddlKunnr.DataSource = tmpDView;
                ddlKunnr.DataValueField = "KUNNR";
                ddlKunnr.DataTextField = "NAME1";
                ddlKunnr.DataBind();
                ddlKunnr.SelectedValue = "0";
                txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
                txtKunnr.Attributes.Add("onblur", "SetDDLValuewithBarkunde(this," + ddlKunnr.ClientID + ", " + chkBar.ClientID + ")");
                ddlKunnr.Attributes.Add("onchange", "SetDDLValuewithBarkunde(" + txtKunnr.ClientID + "," + ddlKunnr.ClientID + "," + chkBar.ClientID + ")");
                lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
                lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
                lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
                txtReferenz2.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$GridView1$ctl02$txtSearch.select();");
                // Aufbau des javascript-Arrays für Barkunden, Pauschalkunden, CPD-Kunden für Javasript-Funktion "SetDDLValuewithBarkunde"
                // Auswahl Barkunde == chkBar checked
                // Auswahl Pauschalkunde = Label Pauschal.Value = Pauschalkunde
                // Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder
                TableToJSArrayBarkunde();

                if (objKompletterf.Status == 0)
                {   // Javascript-Funktionen anhängen (helper.js)
                    Session["tblDienst"] = tblData;
                    tmpDView = new DataView();
                    tmpDView = objCommon.tblStvaStamm.DefaultView;
                    tmpDView.Sort = "KREISTEXT";
                    ddlStVa.DataSource = tmpDView;
                    ddlStVa.DataValueField = "KREISKZ";
                    ddlStVa.DataTextField = "KREISTEXT";
                    ddlStVa.DataBind();
                    ddlStVa.SelectedValue = "0";
                    txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
                    txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");
                    ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + "," + txtKennz1.ClientID + ")");

                    // Aufbau des javascript-Arrays für Zulassungskreise wie HH1, HH2 .. 
                    // Dabei soll bei der Auswahl von z.B. HH1 im Kennzeichen Teil1(txtKennz1) HH stehen
                    TableToJSArray();
                    Session["objKompletterf"] = objKompletterf;
                }
                else
                {
                    lblError.Text = objKompletterf.Message;
                    return;
                }
            }
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblMaterialStamm.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("ArrayMengeERL = \n[\n");
                }

                DataRow dataRow = objCommon.tblMaterialStamm.Rows[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + dataRow[2].ToString().Trim() + "'");// Kundennummer
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[dataRow.Table.Columns.Count - 1].ToString().Trim() + "'");//MengeERL
                javaScript.Append(" ]");

                if ((i + 1) == objCommon.tblMaterialStamm.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript3", javaScript.ToString(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Stva und Sonderstva Bsp.: HH und HH1
        /// Eingabe Stva HH1 dann soll im Kennz.-teil1 HH stehen
        /// JS-Funktion: SetDDLValueSTVA
        /// </summary>
        private void TableToJSArray()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblSonderStva.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("var ArraySonderStva = \n[\n");
                }

                DataRow dataRow = objCommon.tblSonderStva.Rows[i];

                for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    if (j == 0)
                        javaScript.Append(" [ ");

                    javaScript.Append("'" + dataRow[j].ToString().Trim() + "'");
                    if ((j + 1) == dataRow.Table.Columns.Count)
                        javaScript.Append(" ]");
                    else
                        javaScript.Append(",");
                }

                if ((i + 1) == objCommon.tblSonderStva.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", javaScript.ToString(), true);
        }

        /// <summary>
        /// in Javascript Array aufbauen mit den Flags füt Barkunde, Pauschalkunde, CPD-Kunde und Kundennummer
        /// JS-Funktion: SetDDLValuewithBarkunde
        /// Überprüfung ob Barkunde, Pauschalkunde, CPD-Kunde 
        /// Auswahl Barkunde == chkBar.Checked = true
        /// Auswahl Pauschalkunde = Label Pauschal.Visible = true
        /// Auswahl CPD-Kunde = clearen der Bank.- und Adressfelder
        /// </summary>
        private void TableToJSArrayBarkunde()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < objCommon.tblKundenStamm.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("ArrayBarkunde = \n[\n");
                }

                DataRow dataRow = objCommon.tblKundenStamm.Rows[i];

                javaScript.Append(" [ ");
                javaScript.Append("'" + dataRow[2].ToString().Trim() + "'");// Kundennummer
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[dataRow.Table.Columns.Count - 2].ToString().Trim() + "'");//Barkunde
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[9].ToString().Trim() + "'");//Pauschalkunde
                javaScript.Append(",");
                javaScript.Append("'" + dataRow[11].ToString().Trim() + "'");//CPD-Kunde
                javaScript.Append(" ]");

                if ((i + 1) == objCommon.tblKundenStamm.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript2", javaScript.ToString(), true);
        }

        /// <summary>
        /// beim Postback Bar und Pauschalkunde setzen
        /// </summary>
        private void SetBar_Pauschalkunde()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                if (drow[0][9].ToString().Trim() == "X")
                {
                    Pauschal.InnerHtml = "Pauschalkunde";
                }
                else
                {
                    Pauschal.InnerHtml = "";
                }
                if (drow[0][objCommon.tblKundenStamm.Columns.Count - 1].ToString().Trim() == "X")
                {
                    chkBar.Checked = true;
                }
                else if (objKompletterf.Barkunde)
                {
                    chkBar.Checked = true;
                }
                else
                {
                    chkBar.Checked = false;
                }
            }

            Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl;
                Label lblID_POS;
                TextBox txtMenge;

                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                txtMenge.Style["display"] = "none";

                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
            }
        }

        /// <summary>
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System exisitieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>Bei Fehler true</returns>
        private Boolean ProofBank()
        {
            Boolean bError = false;
            if (txtIBAN.Text.Trim(' ').Length > 0 || chkEinzug.Checked)
            {
                objCommon.IBAN = txtIBAN.Text.Trim(' ');
                objCommon.ProofIBAN(Session["AppID"].ToString(), Session.SessionID, this);
                if (objCommon.Message != String.Empty)
                {
                    bError = true;
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.Text = objCommon.Message;
                }
                else
                {
                    txtSWIFT.Text = objCommon.SWIFT;
                    txtGeldinstitut.Text = objCommon.Bankname;
                }
            }

            return !bError;
        }

        /// <summary>
        /// Entfernt das Errorstyle der Controls.
        /// </summary>
        private void ClearErrorBackcolor()
        {
            txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtName2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        /// <summary>
        /// bei Auswahl CPD-Kunde Bankdaten prüfen
        /// </summary>
        /// <returns>false bei Fehler</returns>
        private Boolean proofBankDataCPD()
        {
            Boolean bEdited = true;
            if (txtName1.Text.Length == 0)
            {
                txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (txtStrasse.Text.Length == 0)
            {
                txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (txtPlz.Text.Length < 5)
            {
                txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }
            if (txtOrt.Text.Length == 0)
            {
                txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bEdited = false;
            }

            if (chkCPDEinzug.Checked)
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (!bEdited)
            {
                lblErrorBank.Text = "Es müssen alle Pflichtfelder ausgefüllt sein!";
            }
            return bEdited;
        }

        /// <summary>
        /// bei Bankdaten prüfen wenn kein CPD ausgewählt
        /// trotzdem sind können Eingaben vorgenommen werden
        /// </summary>
        /// <returns>false bei Fehler</returns>
        private Boolean proofBankDatawithoutCPD()
        {
            Boolean bEdited = true;
            if (txtName1.Text.Length > 0)
            {
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtPlz.Text.Length < 5)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtStrasse.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");

                    bEdited = false;
                }
                if (txtPlz.Text.Length == 0)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtPlz.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtOrt.Text.Length == 0)
                {
                    txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtOrt.Text.Length > 0)
            {
                if (txtName1.Text.Length == 0)
                {
                    txtName1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtStrasse.Text.Length == 0)
                {
                    txtStrasse.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
                if (txtPlz.Text.Length == 0)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtKontoinhaber.Text.Length > 0)
            {
                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtIBAN.Text.Length > 0)
            {
                if (txtIBAN.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtSWIFT.Text.Length > 0)
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtGeldinstitut.Text == "Wird automatisch gefüllt!")
                {
                    txtGeldinstitut.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (txtGeldinstitut.Text.Length > 0 && txtGeldinstitut.Text != "Wird automatisch gefüllt!")
            {
                if (txtKontoinhaber.Text.Length == 0)
                {
                    txtKontoinhaber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtIBAN.Text.Length == 0)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }

                if (txtSWIFT.Text.Length == 0)
                {
                    txtSWIFT.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (!bEdited)
            {
                lblErrorBank.Text = "Prüfen Sie Ihre Eingaben auf Vollständigkeit!";
            }
            return bEdited;
        }

        /// <summary>
        /// Sammeln von Eingabedaten. 
        /// </summary>
        private void GetData()
        {
            lblError.Text = "";

            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Session["tblDienst"] = tblData;
            if (ddlKunnr.SelectedIndex < 1)
            {
                lblError.Text = "Kein Kunde ausgewählt.";
            }
            else if (String.IsNullOrEmpty(txtReferenz1.Text))
            {
                lblError.Text = "Referenz1 ist ein Pflichtfeld.";
            }
            else if (checkDienst(tblData) == false)
            {
                lblError.Text = "Keine Dienstleistung ausgewählt.";
            }
            else if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
            }
            else if (txtKennz1.Text.Length == 0)
            {
                lblError.Text = "1.Teil des Kennzeichen muss mit dem Amt gefüllt sein!";
            }
            else if (txtKennz2.Text.Length == 0)
            {
                lblError.Text = "2.Teil des Kennzeichen muss gefüllt sein!";
            }

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                DropDownList ddl;
                TextBox txtMenge;
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");

                DataRow[] Row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (Row.Length > 1 && ddl.SelectedValue != "0")
                {
                    ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblError.Text = "Dienstleistungen und Artikel können nur einmal ausgewählt werden!";

                }
                if ((ddl.SelectedValue == "700") && (tblData.Select("Value = '559'").Length > 0))
                {
                    ddl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    txtBox.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                }
                // matnr Menge Prüfung
                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (txtMenge.Text.Length == 0 && dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                        txtMenge.Style["display"] = "block";
                        lblError.Text = "Bitte geben Sie für diesen Artikel eine Menge ein!";
                    }
                }
            }

            checkDate();
        }

        /// <summary>
        /// vor dem Speichern prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPDonSave()
        {
            DataRow[] drow = objCommon.tblKundenStamm.Select("KUNNR = '" + ddlKunnr.SelectedValue + "'");
            if (drow.Length == 1)
            {
                if (drow[0]["XCPDK"].ToString() == "X")
                {
                    chkCPD.Checked = true;
                    if (drow[0]["XCPDEIN"].ToString() == "X")
                    {
                        chkCPDEinzug.Checked = true;
                    }
                    else
                    {
                        chkCPDEinzug.Checked = false;
                    }
                }
                else
                {
                    chkCPD.Checked = false;
                }
            }
        }

        /// <summary>
        /// prüft ob eine Dienstleistung audgewählt wurde
        /// </summary>
        /// <param name="tblDienst">Diensteistungstabelle</param>
        /// <returns>bei Leer false</returns>
        private Boolean checkDienst(DataTable tblDienst)
        {
            Boolean bReturn = false;
            foreach (DataRow dRow in tblDienst.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    bReturn = true;
                }
            }
            return bReturn;
        }

        /// <summary>
        /// Validierung Datum
        /// </summary>
        /// <returns>bei Fehler false</returns>
        private Boolean checkDate()
        {
            Boolean bReturn = true;
            String ZDat = "";

            ZDat = ZLDCommon.toShortDateStr(txtZulDate.Text);
            if (ZDat != String.Empty)
            {
                if (ZLDCommon.IsDate(ZDat) == false)
                {
                    lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                    bReturn = false;
                }
                else
                {
                    DateTime tagesdatum = DateTime.Today;
                    int i = 60;
                    do
                    {
                        if (tagesdatum.DayOfWeek != DayOfWeek.Saturday && tagesdatum.DayOfWeek != DayOfWeek.Sunday)
                        {
                            i--;
                        }
                        tagesdatum = tagesdatum.AddDays(-1);
                    } while (i > 0);
                    DateTime DateNew;
                    DateTime.TryParse(ZDat, out DateNew);
                    if (DateNew < tagesdatum)
                    {
                        lblError.Text = "Das Datum darf max. 60 Werktage zurück liegen!";
                        bReturn = false;
                    }
                    else
                    {
                        tagesdatum = DateTime.Today;
                        tagesdatum = tagesdatum.AddYears(1);
                        if (DateNew > tagesdatum)
                        {
                            lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                            bReturn = false;
                        }
                    }
                    if (ihDatumIstWerktag.Value == "false")
                    {
                        lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                        bReturn = false;
                    }
                }
            }
            else
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// Eingaben im Gridview1 sammeln und 
        /// updaten der Dienstleistungstabelle 
        /// </summary>
        /// <param name="tblData">Diensteistungstabelle</param>
        private void proofDienstGrid(ref DataTable tblData)
        {
            int i = 0;
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                TextBox txtMenge;
                DropDownList ddl;
                Label lblID_POS;
                Label lblDLBezeichnung;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                DataRow[] dRows = tblData.Select("PosLoesch <> 'L' AND ID_POS =" + lblID_POS.Text);
                if (dRows.Length == 0)
                {
                    tblData.Rows[i]["Search"] = txtBox.Text;
                    tblData.Rows[i]["Value"] = ddl.SelectedValue;
                    tblData.Rows[i]["Text"] = ddl.SelectedItem.Text;
                    tblData.Rows[i]["Menge"] = txtMenge.Text;
                }
                else
                {
                    dRows[0]["Search"] = txtBox.Text;
                    dRows[0]["Value"] = ddl.SelectedValue;
                    dRows[0]["Text"] = ddl.SelectedItem.Text;
                    dRows[0]["Menge"] = txtMenge.Text;
                }

                txtBox = (TextBox)gvRow.FindControl("txtPreis");
                Decimal Preis = 0;
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["Preis"] = Preis;
                    }
                    else
                    {
                        dRows[0]["Preis"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["Preis"] = 0;
                    }
                    else
                    {
                        dRows[0]["Preis"] = 0;
                    }
                }

                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebPreis"] = Preis;
                    }
                    else
                    {
                        dRows[0]["GebPreis"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebPreis"] = 0;
                    }
                    else
                    {
                        dRows[0]["GebPreis"] = 0;
                    }
                }
                txtBox = (TextBox)gvRow.FindControl("txtGebAmt");
                if (ZLDCommon.IsDecimal(txtBox.Text))
                {
                    Decimal.TryParse(txtBox.Text, out Preis);
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebAmt"] = Preis;
                    }
                    else
                    {
                        dRows[0]["GebAmt"] = Preis;
                    }
                }
                else
                {
                    if (dRows.Length == 0)
                    {
                        tblData.Rows[i]["GebAmt"] = 0;
                    }
                    else
                    {
                        dRows[0]["GebAmt"] = 0;
                    }
                }
                if (ddl.SelectedValue == CONST_IDSONSTIGEDL)
                {
                    tblData.Rows[i]["DLBezeichnung"] = lblDLBezeichnung.Text;
                }
                else
                {
                    tblData.Rows[i]["DLBezeichnung"] = "";
                }
                i++;
            }
        }

        /// <summary>
        /// Gridview an Diensteistungstabelle binden
        /// JS-Funktionen an Eingabelfelder des Gridviews binden
        /// </summary>
        /// <param name="tblData"></param>
        private void addButtonAttr(DataTable tblData)
        {
            int i = 0;

            Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                DropDownList ddl;
                Label lblID_POS;
                Label lblOldMatnr;
                TextBox txtMenge;

                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                lblOldMatnr = (Label)gvRow.FindControl("lblOldMatnr");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                txtMenge.Style["display"] = "none";
                String temp = "<%=" + ddl.ClientID + "%>";

                DataView tmpDataView = new DataView();
                tmpDataView = objCommon.tblMaterialStamm.DefaultView;
                tmpDataView.RowFilter = "INAKTIV <> 'X'";
                tmpDataView.Sort = "MAKTX";
                ddl.DataSource = tmpDataView;
                ddl.DataValueField = "MATNR";
                ddl.DataTextField = "MAKTX";
                ddl.DataBind();

                txtBox.Attributes.Add("onkeyup", "SetNurEinKennzFuerDL(this.value," + gvRow.RowIndex + "," + chkEinKennz.ClientID + ");FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");

                DataRow[] dRows = tblData.Select("PosLoesch <> 'L' AND ID_POS =" + lblID_POS.Text);
                if (dRows.Length == 0)
                {
                    txtBox.Text = tblData.Rows[i]["Search"].ToString();
                    ddl.SelectedValue = tblData.Rows[i]["Value"].ToString();
                    ddl.SelectedItem.Text = tblData.Rows[i]["Text"].ToString();
                }
                else
                {
                    txtBox.Text = dRows[0]["Search"].ToString();
                    ddl.SelectedValue = dRows[0]["Value"].ToString();
                }
                ddl.Attributes.Add("onchange", "SetNurEinKennzFuerDL(this.options[this.selectedIndex].value," + gvRow.RowIndex + "," + chkEinKennz.ClientID + ");SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID + 
                                    "," + lblMenge.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");

                DataRow[] dRow = objCommon.tblMaterialStamm.Select("Matnr = '" + ddl.SelectedValue + "'");
                if (dRow.Length == 1)
                {
                    if (dRow[0]["MENGE_ERL"].ToString() == "X")
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
                if (i + 1 == GridView1.Rows.Count)
                {
                    ddl.Attributes.Add("onblur", "ctl00$ContentPlaceHolder1$txtStVa.select();");
                }

                i++;
            }
        }
        
        /// <summary>
        /// Form clearen für Neuanlage eines Vorganges
        /// </summary>
        private void ClearForm()
        {
            txtBarcode.Text = "";
            txtReferenz2.Text = "";
            txtNrReserviert.Text = "";
            txtBemerk.Text = "";
            txtKennz2.Text = "";
            txtSteuer.Text = "";
            txtPreisKennz.Text = "";
            txtSteuer.Visible = false;
            txtPreisKennz.Visible = false;

            if (ddlKennzForm.Items.Count > 0)
            {
                ddlKennzForm.SelectedIndex = 0;
            }
            chkEinKennz.Checked = false;
            chkWunschKZ.Checked = false;
            chkReserviert.Checked = false;
            chkKennzSonder.Checked = false;
            chkCPD.Checked = false;
            chkCPDEinzug.Checked = false;
            chkBar.Checked = false;
            objKompletterf = (KomplettZLD)Session["objKompletterf"];
            DataTable tblData = (DataTable)Session["tblDienst"];

            while (tblData.Rows.Count > 1)
            {
                tblData.Rows.RemoveAt(tblData.Rows.Count - 1);
            }

            tblData.Rows[0]["Menge"] = "";

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = 20;
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["Text"] = "";
            tblRow["ID_POS"] = 30;
            tblRow["PosLoesch"] = "";
            tblRow["NewPos"] = false;
            tblRow["Menge"] = "";
            tblRow["DLBezeichnung"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);
            objKompletterf.EinzugErm = false;
            objKompletterf.Rechnung = false;
            objKompletterf.saved = false;
            objKompletterf.Positionen.Rows.Clear();
            cmdNewDLPrice.Visible = false;
            Session["objKompletterf"] = objKompletterf;
            Session["tblDienst"] = tblData;
            if (cbxSave.Checked == false)
            {
                GridView1.Columns[3].Visible = false;
                GridView1.Columns[4].Visible = false;
                GridView1.Columns[5].Visible = false;
                if (m_User.Groups[0].Authorizationright == 1)
                {
                    GridView1.Columns[5].Visible = false;
                }
                Panel1.Visible = true;
                ButtonFooter.Visible = true;
            }
        }

        /// <summary>
        /// Neuaufbau der Positionstabelle für die Preisfindung
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        private void GetDiensleitungDataforPrice(ref DataTable tblData)
        {
            proofDienstGrid(ref tblData);
            int i = 1;
            objKompletterf.Positionen.Clear();
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    DataRow NewRow = objKompletterf.Positionen.NewRow();
                    NewRow["id_Kopf"] = objKompletterf.KopfID;

                    NewRow["id_pos"] = (Int32)dRow["ID_POS"];
                    NewRow["Menge"] = dRow["Menge"].ToString();
                    String[] sMateriel = dRow["Text"].ToString().Split('~');
                    if (sMateriel.Length == 2)
                    {
                        NewRow["Matbez"] = sMateriel[0].ToString().TrimEnd(' ');
                    }
                    NewRow["Matnr"] = dRow["Value"].ToString();
                    NewRow["Preis"] ="0";
                    NewRow["Preis_Amt"] = "0";
                    NewRow["Preis_Amt_Add"] = "0";
                    NewRow["PosLoesch"] = dRow["PosLoesch"];
                    DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                    if (MatRow.Length == 1)
                    {
                        NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                        NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                        NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                        NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                        NewRow["KennzMat"] = MatRow[0]["KENNZMAT"].ToString();
                    }
                    objKompletterf.Positionen.Rows.Add(NewRow);
                    i++;
                }
            }
        }

        /// <summary>
        /// Dienstleistungsdaten für die Speicherung sammeln.
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        /// <returns>true wenn Positionen geändert wurden</returns>
        private Boolean GetDiensleitungData(ref DataTable tblData)
        {
            Boolean differentMatnr = false;
            proofDienstGrid(ref tblData);
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    String GebMatnr = "";
                    String GebMatbez = "";
                    String GebMatnrSt = "";
                    String GebMatBezSt = "";
                    String tmpKennzmat = "";
                    DataRow[] SelRow = objKompletterf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);
                    if (SelRow.Length == 1)
                    {
                        if (SelRow[0]["WebMTArt"].ToString() == "D")
                        {

                            String[] sMateriel = dRow["Text"].ToString().Split('~');
                            if (dRow["Value"].ToString() == CONST_IDSONSTIGEDL)
                            {
                                SelRow[0]["Matbez"] = dRow["DLBezeichnung"].ToString();
                            }
                            else if (sMateriel.Length == 2)
                            {
                                SelRow[0]["Matbez"] = sMateriel[0].ToString().TrimEnd(' ');
                            }
                            // ist die Hauptdienstleistung(ID_POS=10) geändert wurden, dann zurück und Aufforderung zur Preisfindung
                            if (SelRow[0]["Matnr"].ToString().PadLeft(18, '0') != dRow["Value"].ToString().PadLeft(18, '0'))
                            {
                                SelRow[0]["Matnr"] = dRow["Value"].ToString();
                                return true;
                            }
                            //Gebührenmaterial einfügen aus Stammtabelle
                            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                SelRow[0]["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                SelRow[0]["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                SelRow[0]["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                SelRow[0]["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                                tmpKennzmat = MatRow[0]["KENNZMAT"].ToString();
                            }
                            //Preise einfügen aus internen Dienstleistungstabelle
                            SelRow[0]["Preis"] = dRow["Preis"];
                            SelRow[0]["GebPreis"] = dRow["GebPreis"];
                            SelRow[0]["PreisKZ"] = objKompletterf.PreisKennz;
                            SelRow[0]["PosLoesch"] = dRow["PosLoesch"];
                            SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                            SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                            SelRow[0]["Menge"] = dRow["Menge"];

                        }
                        //Gebührenmaterial update, Prüfung ob  Gebührenmaterial mit oder ohne Steuer
                        SelRow = objKompletterf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='G'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "G")
                            {
                                //eingegebene Preise übernehmen
                                SelRow[0]["Preis"] = dRow["GebPreis"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["Menge"] = dRow["Menge"];
                            }
                        }
                        // eingegebenen Kennzeichenpreis übernehmen
                        SelRow = objKompletterf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='K'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "K")
                            {
                                SelRow[0]["Menge"] = dRow["Menge"];
                                SelRow[0]["Preis"] = objKompletterf.PreisKennz;
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                            }
                        }
                        // eingegebene Steuer übernehmen
                        SelRow = objKompletterf.Positionen.Select("UEPos = " + (Int32)dRow["ID_POS"] + " AND WEBMTART ='S'");
                        if (SelRow.Length == 1)
                        {
                            if (SelRow[0]["WebMTArt"].ToString() == "S")
                            {
                                SelRow[0]["Preis"] = objKompletterf.Steuer;
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                                SelRow[0]["Menge"] = dRow["Menge"];
                                if (objKompletterf.Steuer == 0)// Löschenkennzeichen wenn 0 oder ""
                                { SelRow[0]["PosLoesch"] = "L"; }
                            }
                        }
                    }
                    else
                    {
                        if (dRow["Value"].ToString() == "559")
                        {
                            lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                        }
                        else
                        {
                            // wenn Position nicht vorhanden, Position neu aufbauen
                            DataTable NewPositionen = objKompletterf.Positionen.Clone();
                            Int32 NewPosID = 0;
                            DataRow NewRow = NewPositionen.NewRow();
                            NewRow["id_Kopf"] = objKompletterf.KopfID;
                            NewRow["id_pos"] = (Int32)dRow["ID_POS"];
                            NewPosID = (Int32)dRow["ID_POS"]; ;
                            NewRow["UEPOS"] = 0;
                            NewRow["WEBMTART"] = "D";
                            NewRow["Menge"] = dRow["Menge"];
                            String[] sMateriel = dRow["Text"].ToString().Split('~');
                            if (sMateriel.Length == 2)
                            {
                                NewRow["Matbez"] = sMateriel[0].ToString().TrimEnd(' ');
                            }
                            NewRow["Matnr"] = dRow["Value"].ToString();
                            NewRow["Preis"] = dRow["Preis"];
                            NewRow["GebPreis"] = dRow["GebPreis"];
                            NewRow["PosLoesch"] = dRow["PosLoesch"];
                            NewRow["SDRelevant"] = dRow["SdRelevant"];
                            NewRow["Preis_Amt"] = 0;
                            NewRow["Preis_Amt_Add"] = 0;

                            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
                            if (MatRow.Length == 1)
                            {
                                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                                NewRow["Kennzmat"] = MatRow[0]["KENNZMAT"].ToString();
                                if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                {
                                    NewRow["GebMatPflicht"] = "X";
                                }
                            }
                            NewPositionen.Rows.Add(NewRow);
                            // gleich Preise/SDRelevant aus SAP ziehen 
                            objKompletterf.GetPreiseNewPositionen(Session["AppID"].ToString(), Session.SessionID,
                                                                  this, NewPositionen, objCommon.tblStvaStamm,
                                                                  objCommon.tblMaterialStamm);
                            if (objKompletterf.Status == -5555)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                            }
                            else // Daten in tblData aktualisieren
                            {
                                DataRow[] SelNewRow = objKompletterf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);

                                dRow["SdRelevant"] = SelNewRow[0]["SdRelevant"];
                                dRow["Menge"] = SelNewRow[0]["Menge"];
                            }
                        }
                    }
                }
            }
            return differentMatnr;
        }

        /// <summary>
        /// Prüfen ob sich die Hauptdienstleistung(["ID_POS"] == 10) geändert hat
        /// Preise der hinzugefügten Positionen ermitteln
        /// </summary>
        /// <param name="tblData">interne Dienstleistungstabelle</param>
        /// <returns></returns>
        private Boolean proofdifferentHauptMatnr(ref DataTable tblData)
        {
            objKompletterf.ChangeMatnr = false;
            proofDienstGrid(ref tblData);
            DataTable NewPosTable = objKompletterf.Positionen.Clone();
            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    DataRow[] SelRow = objKompletterf.Positionen.Select("id_pos = " + (Int32)dRow["ID_POS"]);

                    if (SelRow.Length == 1)
                    {
                        if (SelRow[0]["WebMTArt"].ToString() == "D")
                        {
                            if (SelRow[0]["Matnr"].ToString().PadLeft(18, '0') != dRow["Value"].ToString().PadLeft(18, '0') && (Int32)dRow["ID_POS"] == 10)
                            {
                                objKompletterf.ChangeMatnr = true;
                                NewHauptPosition(dRow, ref NewPosTable);//neue Hauptposition aufbauen
                                foreach (DataRow preiseRow in NewPosTable.Rows)// in die bestehende Positionstabelle schieben
                                {
                                    Int32 idPos;
                                    Int32.TryParse(preiseRow["id_pos"].ToString(), out idPos);
                                    DataRow[] dPosRow = objKompletterf.Positionen.Select("id_pos= " + idPos);

                                    if (dPosRow.Length == 1)
                                    {
                                        dPosRow[0]["UEPOS"] = preiseRow["UEPOS"];
                                        dPosRow[0]["id_pos"] = preiseRow["id_pos"];
                                        dPosRow[0]["PosLoesch"] = preiseRow["PosLoesch"];
                                        dPosRow[0]["GebMatnr"] = preiseRow["GebMatnr"];
                                        dPosRow[0]["GebMatbez"] = preiseRow["GebMatbez"];
                                        dPosRow[0]["GebMatnrSt"] = preiseRow["GebMatnrSt"];
                                        dPosRow[0]["GebMatBezSt"] = preiseRow["GebMatBezSt"];
                                        dPosRow[0]["Kennzmat"] = preiseRow["Kennzmat"];
                                        dPosRow[0]["MATNR"] = preiseRow["MATNR"];
                                        dPosRow[0]["Matbez"] = preiseRow["Matbez"];
                                        dPosRow[0]["Menge"] = preiseRow["Menge"];
                                        dPosRow[0]["Preis"] = preiseRow["Preis"];
                                        dPosRow[0]["Preis_Amt"] = preiseRow["Preis_Amt"];
                                        dPosRow[0]["Preis_Amt_Add"] = preiseRow["Preis_Amt_Add"];
                                        dPosRow[0]["Menge"] = preiseRow["Menge"];
                                        dPosRow[0]["WEBMTART"] = preiseRow["WEBMTART"];
                                        dPosRow[0]["SDRelevant"] = preiseRow["SdRelevant"];
                                    }
                                }
                                if (NewPosTable.Rows.Count < objKompletterf.Positionen.Rows.Count)
                                {
                                    foreach (DataRow tRow in objKompletterf.Positionen.Rows)
                                    {
                                        Int32 idPos;
                                        Int32.TryParse(tRow["id_pos"].ToString(), out idPos);
                                        DataRow[] dPosRow = NewPosTable.Select("id_pos= " + idPos);

                                        if (dPosRow.Length == 0)
                                        {
                                            tRow["PosLoesch"] = "L";
                                        }
                                    }
                                }

                                NewPosTable.Rows.Clear();
                            }
                            else if (SelRow[0]["Matnr"].ToString().PadLeft(18, '0') == dRow["Value"].ToString().PadLeft(18, '0') && (Int32)dRow["ID_POS"] == 10)
                            { // eingegebene Preise übernehmen
                                SelRow[0]["Preis"] = dRow["Preis"];
                                SelRow[0]["GebPreis"] = dRow["GebPreis"];
                                SelRow[0]["PreisKZ"] = objKompletterf.PreisKennz;
                                SelRow[0]["PosLoesch"] = dRow["PosLoesch"];
                                SelRow[0]["SDRelevant"] = dRow["SdRelevant"];
                                SelRow[0]["Preis_Amt"] = dRow["GebAmt"];
                            }
                            else if (SelRow[0]["Matnr"].ToString().PadLeft(18, '0') != dRow["Value"].ToString().PadLeft(18, '0') && (Int32)dRow["ID_POS"] != 10)
                            {
                                SelRow[0]["PosLoesch"] = "L";// alle zur alten Hauptposition gehörenden Unterpositionen wenn sie unterschiedlich sind löschen
                                DataRow[] SelUPosRow =
                                    objKompletterf.Positionen.Select("uepos = " + (Int32)dRow["ID_POS"]);
                                foreach (DataRow Row in SelUPosRow)
                                {
                                    Row["PosLoesch"] = "L";
                                }
                                // und die neue Unterposition einfügen ohne Geb.-Positionen, wird später in der Preisfindung aufgebaut
                                NewPosOhneGebMat(dRow, ref NewPosTable);
                            }
                        }
                    }
                    else
                    {
                        if ((Int32)dRow["ID_POS"] == 10) { objKompletterf.ChangeMatnr = true; }
                        NewPosOhneGebMat(dRow, ref NewPosTable);
                    }
                }
            }
            // Gibt es neue Positionen dann ab in die Preisfindung
            if (NewPosTable.Rows.Count > 0)
            {
                if (NewPosTable.Select("Matnr='559'").Length > 0)
                {
                    lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                }
                else
                {
                    objKompletterf.GetPreiseNewPositionen(Session["AppID"].ToString(), Session.SessionID,
                                      this, NewPosTable, objCommon.tblStvaStamm,
                                      objCommon.tblMaterialStamm);
                    if (objKompletterf.Status == -5555)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objKompletterf.Message;
                    }
                } 
            }
            return objKompletterf.ChangeMatnr;
        }

        /// <summary>
        /// Neue Hauptposition aufbauen
        /// </summary>
        /// <param name="dRow"></param>
        /// <param name="NewPosTable"></param>
        private void NewHauptPosition(DataRow dRow, ref DataTable NewPosTable)
        {
            String GebMatnr = "";
            String GebMatbez = "";
            String GebMatnrSt = "";
            String GebMatBezSt = "";

            Int32 NewPosID = 10, NewUePosID = 10;
            DataRow NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objKompletterf.KopfID;
            NewRow["id_pos"] = NewPosID;
            NewRow["UEPOS"] = 0;
            NewRow["WEBMTART"] = "D";
            NewRow["Menge"] = "1";
            String[] sMateriel = dRow["Text"].ToString().Split('~');
            if (sMateriel.Length == 2)
            {
                NewRow["Matbez"] = sMateriel[0].ToString().TrimEnd(' ');
            }
            NewRow["Matnr"] = dRow["Value"].ToString();
            NewRow["Preis"] = dRow["Preis"];
            NewRow["Preis_Amt"] = dRow["GebAmt"];
            NewRow["PosLoesch"] = dRow["PosLoesch"];
            NewRow["SDRelevant"] = dRow["SdRelevant"];
            GebMatnr = "";
            GebMatbez = "";
            GebMatnrSt = "";
            GebMatBezSt = "";
            String Kennzmat = "";
            // Geb.Material aus der Stammtabelle
            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
            if (MatRow.Length == 1)
            {
                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();
                GebMatnr = MatRow[0]["GEBMAT"].ToString();
                GebMatbez = MatRow[0]["GMAKTX"].ToString();
                GebMatnrSt = MatRow[0]["GBAUST"].ToString();
                GebMatBezSt = MatRow[0]["GUMAKTX"].ToString();
                Kennzmat = MatRow[0]["KENNZMAT"].ToString();
            }
            NewPosTable.Rows.Add(NewRow);

            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
            {
                if (objKompletterf.OhneSteuer == "X")
                {
                    NewRow = NewPosTable.NewRow();
                    NewRow["id_Kopf"] = objKompletterf.KopfID;
                    NewRow["UEPOS"] = NewUePosID;
                    NewRow["id_pos"] = NewPosID + 10;
                    NewRow["PosLoesch"] = dRow["PosLoesch"];
                    NewRow["Matnr"] = GebMatnr;
                    NewRow["Matbez"] = GebMatbez;
                    NewRow["Menge"] = "1";
                    NewRow["GebMatnr"] = "";
                    NewRow["GebMatbez"] = "";
                    NewRow["GebMatnrSt"] = "";
                    NewRow["GebMatBezSt"] = "";
                    NewRow["Kennzmat"] = "";
                    NewRow["PreisKZ"] = 0;
                    NewRow["Matnr"] = GebMatnr;
                    NewRow["Matbez"] = GebMatbez;
                    NewRow["Preis"] = 0;
                    NewRow["Preis_Amt"] = 0;
                    NewRow["Preis_Amt_Add"] = 0;
                    NewRow["WEBMTART"] = "G";
                    NewPosTable.Rows.Add(NewRow);

                    NewPosID = NewPosID + 10;
                }
                else
                {
                    NewRow = NewPosTable.NewRow();
                    NewRow["id_Kopf"] = objKompletterf.KopfID;
                    NewRow["UEPOS"] = NewUePosID;
                    NewRow["id_pos"] = NewPosID + 10;
                    NewRow["PosLoesch"] = dRow["PosLoesch"];
                    NewRow["Matnr"] = GebMatnrSt;
                    NewRow["Matbez"] = GebMatBezSt;
                    NewRow["Menge"] = "1";
                    NewRow["GebMatnr"] = "";
                    NewRow["GebMatbez"] = "";
                    NewRow["GebMatnrSt"] = "";
                    NewRow["GebMatBezSt"] = "";
                    NewRow["Kennzmat"] = "";
                    NewRow["PreisKZ"] = 0;
                    NewRow["MATNR"] = GebMatnrSt;
                    NewRow["Matbez"] = GebMatBezSt;
                    NewRow["Preis"] = 0;
                    NewRow["Preis_Amt"] = 0;
                    NewRow["Preis_Amt_Add"] = 0;
                    NewRow["WEBMTART"] = "G";

                    NewPosTable.Rows.Add(NewRow);
                    NewPosID = NewPosID + 10;
                }

            }
            // neues Kennzeichenmaterial
            if (objKompletterf.PauschalKunde != "X")
            {
                if (Kennzmat.Trim(' ') != "")
                {
                    NewRow = NewPosTable.NewRow();
                    NewRow["id_Kopf"] = objKompletterf.KopfID;
                    NewRow["UEPOS"] = NewUePosID;
                    NewRow["id_pos"] = NewPosID + 10;
                    NewRow["PosLoesch"] = dRow["PosLoesch"]; ;
                    NewRow["GebMatnr"] = "";
                    NewRow["GebMatbez"] = "";
                    NewRow["GebMatnrSt"] = "";
                    NewRow["GebMatBezSt"] = "";
                    NewRow["Kennzmat"] = "";
                    NewRow["Menge"] = "1";
                    NewRow["MATNR"] = Kennzmat;
                    NewRow["Matbez"] = "";
                    NewRow["Preis"] = 0;
                    NewRow["Preis_Amt"] = 0;
                    NewRow["Preis_Amt_Add"] = 0;
                    NewRow["WEBMTART"] = "K";
                    NewPosTable.Rows.Add(NewRow);
                    NewPosID = NewPosID + 10;
                }
            }
            // neues Steuermaterial
            NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objKompletterf.KopfID;
            NewRow["UEPOS"] = NewUePosID;
            NewRow["id_pos"] = NewPosID + 10;
            NewRow["Menge"] = "1";
            NewRow["PosLoesch"] = "";
            NewRow["GebMatnr"] = "";
            NewRow["GebMatbez"] = "";
            NewRow["GebMatnrSt"] = "";
            NewRow["GebMatBezSt"] = "";
            NewRow["Kennzmat"] = "";
            NewRow["MATNR"] = "591".PadLeft(18, '0');
            NewRow["Matbez"] = "";
            NewRow["Preis"] = 0;
            NewRow["Preis_Amt"] = 0;
            NewRow["Preis_Amt_Add"] = 0;
            NewRow["WEBMTART"] = "S";
            NewPosTable.Rows.Add(NewRow);
        }

        /// <summary>
        /// Neue Positionen ohne Geb.-Positionen aufbauen
        /// </summary>
        /// <param name="dRow"></param>
        /// <param name="NewPosTable"></param>
        private void NewPosOhneGebMat(DataRow dRow, ref DataTable NewPosTable)
        {
            Int32 NewPosID = 0;
            if (NewPosTable.Rows.Count == 0)
            {
                NewPosTable = objKompletterf.Positionen.Clone();
                Int32.TryParse(objKompletterf.Positionen.Rows[objKompletterf.Positionen.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            }
            else
            {
                Int32.TryParse(NewPosTable.Rows[NewPosTable.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);
            }
            NewPosID += 10;
            DataRow NewRow = NewPosTable.NewRow();
            NewRow["id_Kopf"] = objKompletterf.KopfID;
            NewRow["id_pos"] = NewPosID;
            NewRow["UEPOS"] = 0;
            NewRow["WEBMTART"] = "D";
            NewRow["Menge"] = "1";
            if (ZLDCommon.IsNumeric(dRow["Menge"].ToString()))
            {
                NewRow["Menge"] = dRow["Menge"].ToString();
            }
            String[] sMateriel = dRow["Text"].ToString().Split('~');
            if (sMateriel.Length == 2)
            {
                NewRow["Matbez"] = sMateriel[0].ToString().TrimEnd(' ');
            }
            NewRow["Matnr"] = dRow["Value"].ToString();
            NewRow["Preis"] = dRow["Preis"];
            NewRow["GebPreis"] = dRow["GebPreis"];
            NewRow["PosLoesch"] = dRow["PosLoesch"];
            NewRow["SDRelevant"] = dRow["SdRelevant"];
            NewRow["Preis_Amt"] = 0;
            NewRow["Preis_Amt_Add"] = 0;

            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("Matnr = '" + dRow["Value"].ToString() + "'");
            if (MatRow.Length == 1)
            {
                NewRow["GebMatnr"] = MatRow[0]["GEBMAT"].ToString();
                NewRow["GebMatbez"] = MatRow[0]["GMAKTX"].ToString();
                NewRow["GebMatnrSt"] = MatRow[0]["GBAUST"].ToString();
                NewRow["GebMatBezSt"] = MatRow[0]["GUMAKTX"].ToString();

                NewRow["Kennzmat"] = MatRow[0]["KENNZMAT"].ToString();
                if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                {
                    NewRow["GebMatPflicht"] = "X";
                }
            }
            NewPosTable.Rows.Add(NewRow);

        }

        /// <summary>
        /// Kennzeichen Preis darf nicht eingegeben werden wenn
        /// es sich um einen Pauschalkunden handelt
        /// oder das ausgewählte Haupmaterial nicht Kennzeichenrelevant ist
        /// </summary>
        /// <param name="Pauschal"></param>
        /// <param name="Matnr"></param>
        /// <returns></returns>
        protected bool proofPauschMat(String Pauschal, String Matnr)
        {
            bool bReturn = (Pauschal != "X");

            DataRow[] MatRow = objCommon.tblMaterialStamm.Select("MATNR='" + Matnr.TrimStart('0') + "'");
            if (MatRow.Length == 1)
            {
                if (MatRow[0]["KENNZMAT"].ToString() == "")
                {
                    bReturn = false;
                }
            }

            return bReturn;
        }

        /// <summary>
        /// Prüfen ob an der Position ein Gebührenpacket hängt, wenn ja 
        /// sperren.
        /// </summary>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Ja-False, Nein-True</returns>
        protected bool proofGebPak(String IDPos)
        {
            bool bReturn = true;
            DataRow[] Rows = objKompletterf.Positionen.Select("id_pos = '" + IDPos + "'");
            if (Rows.Length == 1)
            {
                if (Rows[0]["GebPak"].ToString() == "X")
                {
                    bReturn = false;
                }
            }
            return bReturn;
        }

        #endregion
    }

}