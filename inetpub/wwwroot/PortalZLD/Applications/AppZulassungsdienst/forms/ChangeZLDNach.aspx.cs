﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Nacherfassung/Nacherfassung beauftragter Versandzulassungen Eingabedialog.
    /// </summary>
    public partial class ChangeZLDNach : System.Web.UI.Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (String.IsNullOrEmpty(m_User.Reference))
            {
                lblError.Text = "Es wurde keine Benutzerreferenz angegeben! Somit können keine Stammdaten ermittelt werden!";
                return;
            }
            if (Session["objCommon"] == null)
            {
                objCommon = new ZLDCommon(m_User.Reference);
                objCommon.getSAPDatenStamm();
                objCommon.getSAPZulStellen();
                objCommon.LadeKennzeichenGroesse();
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];
            }

            objNacherf = (NacherfZLD)Session["objNacherf"];

            InitLargeDropdowns();
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var BackfromList = (Request.QueryString["B"] != null);

            if (!IsPostBack)
            {
                if (!BackfromList)
                {
                    Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
                }
                else
                {
                    if (Request.QueryString["id"] != null && Request.QueryString["id"].IsNumeric())
                    {
                        objNacherf.LoadVorgang(Request.QueryString["id"], objCommon.MaterialStamm, objCommon.StvaStamm);
                        fillForm();
                        SelectValues();
                    }
                    else
                    {
                        lblError.Text = "Fehler beim Laden des Vorganges!";
                    }
                }
            }
        }

        /// <summary>
        /// Preis finden. Bei geänderter Hauptdienstleistung und /oder Kunden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdFindPrize_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            DataTable tblData = (DataTable)Session["tblDienst"];

            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
            {
                kopfdaten.KundenNr = txtKunnr.Text;
            }
            else
            {
                lblError.Text = "Bitte Kunde auswählen!";
                return;
            }

            proofdifferentHauptMatnr(ref tblData);

            GetDiensleitungDataForPrice(ref tblData);

            kopfdaten.Landkreis = txtStVa.Text;

            var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
            if (amt != null)
                kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

            kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
            kopfdaten.KennzeichenReservieren = chkReserviert.Checked;

            if (!String.IsNullOrEmpty(txtKennz2.Text))
            {
                kopfdaten.Kennzeichen = txtKennz1.Text + "-" + txtKennz2.Text;
            }

            kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");

            kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
            kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");

            kopfdaten.Bemerkung = txtBemerk.Text;

            objNacherf.GetPreise(objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                return;
            }

            hfKunnr.Value = txtKunnr.Text;

            UpdateDlTableWithPrizes(ref tblData);
            
            DataView tmpDataView = new DataView(tblData);
            tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);

            cmdCreate.Enabled = true;
            SetBar_Pauschalkunde();
            Session["tblDienst"] = tblData;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Daten speichern
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            bool blnSonstigeDLOffen = false;

            lblError.Text = "";

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if ((ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL) && ((String.IsNullOrEmpty(lblDLBezeichnung.Text)) || (lblDLBezeichnung.Text == "Sonstige Dienstleistung")))
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
                // Bei Annahme "Neue AH-Vorgänge" automatische Preisfindung
                if (objNacherf.SelAnnahmeAH)
                {
                    cmdFindPrize_Click(this, new EventArgs());
                }
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
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");
                if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                {
                    lblDLBezeichnung.Text = dlgErfassungDLBez.DLBezeichnung;
                }
            }

            mpeDLBezeichnung.Hide();
        }

        /// <summary>
        /// Neue Dienstleistung/Artikel hinzuügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate1_Click(object sender, EventArgs e)
        {
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

            var maxPosId = objNacherf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr);

            NewPosID = Math.Max(NewPosID, maxPosId.ToInt(0));

            DataRow tblRow = tblData.NewRow();
            tblRow["Search"] = "";
            tblRow["Value"] = "0";
            tblRow["ID_POS"] = (NewPosID + 10).ToString();
            tblRow["NewPos"] = true;
            tblRow["Menge"] = "";
            tblRow["SdRelevant"] = false;
            tblRow["PosLoesch"] = "";
            tblData.Rows.Add(tblRow);

            Session["tblDienst"] = tblData;
            DataView tmpDataView = new DataView(tblData);
            tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            addButtonAttr(tblData);
            GridViewRow gvRow = GridView1.Rows[GridView1.Rows.Count - 1];

            // in den Fällen "Nachbearbeitung durchzuführeder Versandzulassungen" und 
            // "Neue AH-Vorgänge" Speichern ermöglichen, sonst immer Preisfindung erforderlich
            cmdNewDLPrice.Enabled = (!objNacherf.SelEditDurchzufVersZul && !objNacherf.SelAnnahmeAH);
            cmdCreate.Enabled = objNacherf.SelEditDurchzufVersZul || objNacherf.SelAnnahmeAH;

            var txtBox = (TextBox)gvRow.FindControl("txtSearch");
            txtBox.Focus();
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
                DataTable tblData = (DataTable)Session["tblDienst"];
                proofDienstGrid(ref tblData);

                GridViewRow gvRow = GridView1.Rows[number];
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                string idpos = lblID_POS.Text;
                DataRow[] tblRows = tblData.Select("id_pos='" + idpos + "'");

                if (tblRows.Length > 0)
                {
                    if ((bool)tblRows[0]["NewPos"])
                    {
                        if (objNacherf.AktuellerVorgang.Positionen.Any(p => p.PositionsNr == idpos))
                            objNacherf.AktuellerVorgang.Positionen.RemoveAll(p => p.PositionsNr == idpos || p.UebergeordnetePosition == idpos);

                        tblData.Rows.Remove(tblRows[0]);
                    }
                    else
                    {
                        if (objNacherf.AktuellerVorgang.Positionen.Any(p => p.PositionsNr == idpos))
                            objNacherf.AktuellerVorgang.Positionen.RemoveAll(p => p.PositionsNr == idpos || p.UebergeordnetePosition == idpos);
                    }

                    Session["tblDienst"] = tblData;
                    DataView tmpDataView = new DataView(tblData);
                    tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
                    GridView1.DataSource = tmpDataView;
                    GridView1.DataBind();

                    addButtonAttr(tblData);
                }
            }
        }

        /// <summary>
        /// nach der Grid-Datenbindung durchzuführende Aktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            // Wenn Seite in besonderem Modus aufgerufen, dann best. Grid-Felder sperren
            if (objNacherf.SelAnnahmeAH || objNacherf.SelSofortabrechnung || objNacherf.SelEditDurchzufVersZul)
            {
                disableGridfelder();
            }
        }

        /// <summary>
        /// Das gestrige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnGestern_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today.AddDays(-1);
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Das heutige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnHeute_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today;
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Das morgige Datum in das Control txtZulDate einfügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnMorgen_Click(object sender, EventArgs e)
        {
            DateTime dDate;
            String sDate;
            dDate = DateTime.Today.AddDays(1);
            sDate = dDate.Day.ToString().PadLeft(2, '0') + dDate.Month.ToString().PadLeft(2, '0') + dDate.Year.ToString().Substring(2, 2);
            txtZulDate.Text = sDate;
            txtKennz2.Focus();
        }

        /// <summary>
        /// Sonderkennzeichen markiert bzw. Markierung entfernt.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkKennzSonder_CheckedChanged(object sender, EventArgs e)
        {
            ddlKennzForm.Enabled = chkKennzSonder.Checked;
            SetBar_Pauschalkunde();
        }

        /// <summary>
        /// Zurück zur Listenansicht.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            if (objNacherf.SelEditDurchzufVersZul)
            {
                Response.Redirect("ChangeZLDNachVersandListe.aspx?AppID=" + Session["AppID"].ToString());
            }
            else
            {
                Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
            }
        }

        /// <summary>
        /// Bankdaten und abweichende Adresse in der Klasse speichern.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveBank_Click(object sender, EventArgs e)
        {
            ClearErrorBackcolor();
            lblErrorBank.Text = "";
            proofCPD();
            Boolean bnoError = ProofBank();

            if (bnoError)
            {
                bnoError = (chkCPD.Checked ? proofBankDataCPD() : proofBankDatawithoutCPD());
                if (bnoError)
                {
                    SaveBankAdressdaten();

                    Session["objNacherf"] = objNacherf;
                    lblErrorBank.Text = "";
                    pnlBankdaten.Attributes.Remove("style");
                    pnlBankdaten.Attributes.Add("style", "display:none");
                    Panel1.Attributes.Remove("style");
                    Panel1.Attributes.Add("style", "display:block");
                    dataQueryFooter.Visible = true;
                }
            }
        }

        /// <summary>
        /// Bankdialog schließen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCancelBank_Click(object sender, EventArgs e)
        {
            ResetBankAdressdaten();

            pnlBankdaten.Attributes.Remove("style");
            pnlBankdaten.Attributes.Add("style", "display:none");
            Panel1.Attributes.Remove("style");
            Panel1.Attributes.Add("style", "display:block");
            dataQueryFooter.Visible = true;
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
                pnlBankdaten.Attributes.Remove("style");
                pnlBankdaten.Attributes.Add("style", "display:block");
                Panel1.Attributes.Remove("style");
                Panel1.Attributes.Add("style", "display:none");
                dataQueryFooter.Visible = false;
                txtZulDateBank.Text = txtZulDate.Text;
                txtKundebank.Text = ddlKunnr.SelectedItem.Text;
                txtKundeBankSuche.Text = txtKunnr.Text;
                txtRef1Bank.Text = txtReferenz1.Text.ToUpper();
                txtRef2Bank.Text = txtReferenz2.Text.ToUpper();

                proofCPD();

                var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;
                var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

                if (!String.IsNullOrEmpty(kopfdaten.SapId) && kopfdaten.KundenNr == txtKunnr.Text)
                {
                    chkEinzug.Checked = (chkCPDEinzug.Checked || bankdaten.Einzug.IsTrue());
                    chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
                }

                txtName1.Focus();
            }
        }

        /// <summary>
        /// Preis ergänzte DL. ziehen. Bei geänderten Dienstleistungen/Artikel ausser der Haupdienstleistung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdNewDLPrice_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            DataTable tblData = (DataTable)Session["tblDienst"];

            cmdCreate.Enabled = true;

            proofDienstGrid(ref tblData);
            if (proofdifferentHauptMatnr(ref tblData))
            {
                lblError.Text = "Hauptdienstleistung geändert! Bitte auf Preis finden gehen!";
                cmdCreate.Enabled = false;
            }

            if (String.IsNullOrEmpty(lblError.Text))
            {
                UpdateDlTableWithPrizes(ref tblData);

                DataView tmpDataView = new DataView(tblData);
                tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
                GridView1.DataSource = tmpDataView;
                GridView1.DataBind();
                addButtonAttr(tblData);
                SetBar_Pauschalkunde();
            }
            else
            {
                cmdCreate.Enabled = false;
            }
            Session["tblDienst"] = tblData;
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Bei StVa-Änderung neue Preisfindung erzwingen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStVa.SelectedItem != null && String.Compare(objNacherf.AktuellerVorgang.Kopfdaten.Landkreis, ddlStVa.SelectedValue) != 0)
            {
                cmdCreate.Enabled = objNacherf.SelAnnahmeAH;
                cmdNewDLPrice.Enabled = false;
                cmdFindPrize.Enabled = !objNacherf.SelAnnahmeAH;
            }
        }

        /// <summary>
        /// FSP vom Amt (Art. 559) hinzufügen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnFeinstaub_Click(object sender, EventArgs e)
        {
            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);

            Int32 NewPosID;
            Int32.TryParse(tblData.Rows[tblData.Rows.Count - 1]["ID_POS"].ToString(), out NewPosID);

            var maxPosId = objNacherf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr);

            NewPosID = Math.Max(NewPosID, maxPosId.ToInt(0));

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
                tblRow["ID_POS"] = (NewPosID + 10).ToString();
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = true;
                tblRow["Menge"] = "1";
                tblRow["SdRelevant"] = false;
                tblRow["DLBezeichnung"] = "";
                tblData.Rows.Add(tblRow);
            }

            Session["tblDienst"] = tblData;
            DataView tmpDataView = new DataView(tblData);
            tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();

            addButtonAttr(tblData);

            // in den Fällen "Nachbearbeitung durchzuführeder Versandzulassungen" und 
            // "Neue AH-Vorgänge" Speichern ermöglichen, sonst immer Preisfindung erforderlich
            cmdNewDLPrice.Enabled = (!objNacherf.SelEditDurchzufVersZul && !objNacherf.SelAnnahmeAH);
            cmdCreate.Enabled = objNacherf.SelEditDurchzufVersZul || objNacherf.SelAnnahmeAH;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dropdowns mit großen Datenmengen (ohne ViewState!)
        /// </summary>
        private void InitLargeDropdowns()
        {
            //Kunde
            ddlKunnr.DataSource = objCommon.KundenStamm.Where(k => !k.Inaktiv).ToList();
            ddlKunnr.DataValueField = "KundenNr";
            ddlKunnr.DataTextField = "Name";
            ddlKunnr.DataBind();

            //StVa
            ddlStVa.DataSource = objCommon.StvaStamm;
            ddlStVa.DataValueField = "Landkreis";
            ddlStVa.DataTextField = "Bezeichnung";
            ddlStVa.DataBind();
        }

        private void InitJava()
        {
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValuewithBarkunde(this," + ddlKunnr.ClientID + ", " + chkBar.ClientID + ", 'X')");
            ddlKunnr.Attributes.Add("onchange", "SetDDLValuewithBarkunde(" + txtKunnr.ClientID + "," + ddlKunnr.ClientID + "," + chkBar.ClientID + ", 'X')");
            txtStVa.Attributes.Add("onkeyup", "DisableButton(" + cmdCreate.ClientID + ");FilterSTVA(this.value," + ddlStVa.ClientID + ", null)");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + ", null)");
            ddlStVa.Attributes.Add("onchange", "SetDDLValueSTVA(" + txtStVa.ClientID + "," + ddlStVa.ClientID + ", null)");
            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" + txtZulDate.ClientID + "'); return false;");
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" + txtZulDate.ClientID + "'); return false;");
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" + txtZulDate.ClientID + "'); return false;");
        }

        /// <summary>
        /// Füllt die Form mit geladenen Stammdaten
        /// verknüpft Texboxen und DropDowns mit JS-Funktionen
        /// Initialisiert die interne Dienstleistungstabelle
        /// </summary>
        private void fillForm()
        {
            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            DataTable tblData = CreatePosTable();

            for (int i = 1; i < 4; i++)
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = "";
                tblRow["Value"] = "0";
                tblRow["OldValue"] = "";
                tblRow["ID_POS"] = (i * 10).ToString();
                tblRow["Preis"] = 0;
                tblRow["GebPreis"] = 0;
                tblRow["PosLoesch"] = "";
                tblRow["NewPos"] = false;
                tblRow["GebAmt"] = 0;
                tblRow["Menge"] = "";
                tblRow["SdRelevant"] = false;
                tblRow["DLBezeichnung"] = "";

                tblData.Rows.Add(tblRow);
            }

            GridView1.DataSource = tblData;
            GridView1.DataBind();

            addButtonAttr(tblData);

            Session["tblDienst"] = tblData;

            txtKunnr.Text = objNacherf.AktuellerVorgang.Kopfdaten.KundenNr;
            hfKunnr.Value = objNacherf.AktuellerVorgang.Kopfdaten.KundenNr;
            ddlKunnr.SelectedValue = objNacherf.AktuellerVorgang.Kopfdaten.KundenNr;

            TableToJSArrayBarkunde();

            if (!objNacherf.ErrorOccured)
            {
                TableToJSArray();
                Session["objNacherf"] = objNacherf;
            }
            else
            {
                lblError.Text = objNacherf.Message;
                return;
            }

            // Wenn Seite in besonderem Modus aufgerufen, dann best. Felder sperren
            if (objNacherf.SelAnnahmeAH || objNacherf.SelSofortabrechnung || objNacherf.SelEditDurchzufVersZul)
            {
                disableEingabefelder();
            }
        }

        private DataTable CreatePosTable()
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add("Search", typeof(String));
            tbl.Columns.Add("Value", typeof(String));
            tbl.Columns.Add("Text", typeof(String));
            tbl.Columns.Add("ID_POS", typeof(String));
            tbl.Columns.Add("NewPos", typeof(Boolean));
            tbl.Columns.Add("Menge", typeof(String));
            tbl.Columns.Add("SdRelevant", typeof(Boolean));
            tbl.Columns.Add("DLBezeichnung", typeof(String));
            tbl.Columns.Add("OldValue", typeof(String));
            tbl.Columns.Add("Preis", typeof(Decimal));
            tbl.Columns.Add("GebPreis", typeof(Decimal));
            tbl.Columns.Add("GebAmt", typeof(Decimal));
            tbl.Columns.Add("PosLoesch", typeof(String));

            return tbl;
        }

        /// <summary>
        /// Einfügen der bereits vorhandenen Daten aus der Datenbank
        /// </summary>
        private void SelectValues()
        {
            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            txtBarcode.Text = kopfdaten.Barcode;
            txtKunnr.Text = kopfdaten.KundenNr;

            txtReferenz1.Text = kopfdaten.Referenz1;
            txtReferenz2.Text = kopfdaten.Referenz2;
            txtStVa.Text = kopfdaten.Landkreis;
            ddlStVa.SelectedValue = kopfdaten.Landkreis;
            txtKennz1.Text = kopfdaten.Landkreis;
            chkWunschKZ.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
            chkReserviert.Checked = kopfdaten.KennzeichenReservieren.IsTrue();
            txtNrReserviert.Text = kopfdaten.ReserviertesKennzeichen;
            chkFlieger.Checked = kopfdaten.Flieger.IsTrue();
            txtZulDate.Text = kopfdaten.Zulassungsdatum.ToString("ddMMyy");

            string tmpKennz1;
            string tmpKennz2;
            ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out tmpKennz1, out tmpKennz2);
            txtKennz1.Text = tmpKennz1;
            txtKennz2.Text = tmpKennz2;

            txtBemerk.Text = kopfdaten.Bemerkung;
            txtInfotext.Text = kopfdaten.Infotext;

            LongStringToSap LSTS = new LongStringToSap();
            if (!String.IsNullOrEmpty(kopfdaten.LangtextNr))
            {
                txtService.Text = LSTS.ReadString(kopfdaten.LangtextNr);
            }
            else
            {
                trFreitext.Visible = false;
            }

            var steuerPos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == "10" && p.WebMaterialart == "S");
            txtSteuer.Text = (steuerPos != null ? steuerPos.Preis.ToString("f") : "");

            var kennzeichenPos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == "10" && p.WebMaterialart == "K");
            txtPreisKennz.Text = (kennzeichenPos != null ? kennzeichenPos.Preis.ToString("f") : "");

            chkEinKennz.Checked = kopfdaten.NurEinKennzeichen.IsTrue();
            chkBar.Checked = kopfdaten.BarzahlungKunde.IsTrue();

            DataTable tblData = CreatePosTable();

            foreach (var item in objNacherf.AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D").OrderBy(p => p.PositionsNr))
            {
                DataRow tblRow = tblData.NewRow();

                tblRow["Search"] = item.MaterialNr;
                tblRow["Value"] = item.MaterialNr;
                tblRow["OldValue"] = item.MaterialNr;
                tblRow["Text"] = item.MaterialName;
                tblRow["ID_POS"] = item.PositionsNr;
                tblRow["NewPos"] = false;
                tblRow["Menge"] = item.Menge.ToString("F0");
                tblRow["Preis"] = item.Preis.GetValueOrDefault(0);
                tblRow["PosLoesch"] = item.Loeschkennzeichen;

                var gebuehrenPos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == item.PositionsNr && p.WebMaterialart == "G");

                tblRow["GebPreis"] = (gebuehrenPos != null ? gebuehrenPos.Preis.GetValueOrDefault(0) : 0);
                tblRow["GebAmt"] = (gebuehrenPos != null ? gebuehrenPos.GebuehrAmt.GetValueOrDefault(0) : 0);

                if (item.PositionsNr == "10")
                {
                    hfMatnr.Value = item.MaterialNr;
                    txtPreisKennz.Enabled = true;
                    if (!proofPauschMat(item.MaterialNr))
                    {
                        txtPreisKennz.Text = "0,00";
                        txtPreisKennz.Enabled = false;
                    }
                }

                tblRow["SdRelevant"] = item.SdRelevant.IsTrue();

                if (item.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL)
                    tblRow["DLBezeichnung"] = item.MaterialName;
                else
                    tblRow["DLBezeichnung"] = "";

                tblData.Rows.Add(tblRow);
            }

            DataView tmpDataView = new DataView(tblData);
            tmpDataView.RowFilter = "IsNull(PosLoesch,'') <> 'L'";
            GridView1.DataSource = tmpDataView;
            GridView1.DataBind();
            addButtonAttr(tblData);
            TableToJSArrayMengeErlaubt();

            string matNr = "";
            DataView tmpDView = null;
            if (GridView1.Rows.Count > 0)
            {
                GridViewRow gridRow = GridView1.Rows[0];
                TextBox txtHauptPos = (TextBox)gridRow.FindControl("txtSearch");
                matNr = txtHauptPos.Text;

                tmpDView = new DataView(objCommon.tblKennzGroesse, "Matnr = " + matNr, "Matnr", DataViewRowState.CurrentRows);
            }

            if (tmpDView != null && tmpDView.Count > 0)
            {
                ddlKennzForm.DataSource = tmpDView;
                ddlKennzForm.DataTextField = "Groesse";
                ddlKennzForm.DataValueField = "ID";
                ddlKennzForm.DataBind();
                if (!String.IsNullOrEmpty(kopfdaten.Kennzeichenform))
                {
                    DataRow[] kennzRow = objCommon.tblKennzGroesse.Select("Groesse ='" + kopfdaten.Kennzeichenform + "' AND Matnr= '" + matNr + "'");
                    if (kennzRow.Length > 0)
                    {
                        ddlKennzForm.SelectedValue = kennzRow[0]["ID"].ToString();
                    }
                    chkKennzSonder.Checked = (kopfdaten.Kennzeichenform != "520x114");
                    ddlKennzForm.Enabled = chkKennzSonder.Checked;
                }
            }
            else
            {
                ddlKennzForm.Items.Clear();
                ddlKennzForm.Items.Add(new ListItem("", "0"));
            }

            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;
            txtStrasse.Text = adressdaten.Strasse;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            if (!String.IsNullOrEmpty(bankdaten.Geldinstitut))
            {
                txtGeldinstitut.Text = bankdaten.Geldinstitut;
            }
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
            SetBar_Pauschalkunde();
            Session["tblDienst"] = tblData;
        }

        /// <summary>
        /// Fügt dem im Gridview vorhanden Ctrls Javascript-Funktionen hinzu.
        /// </summary>
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

                ddl.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv).ToList();
                ddl.DataValueField = "MaterialNr";
                ddl.DataTextField = "Name";
                ddl.DataBind();

                txtBox.Attributes.Add("onkeyup", "FilterItems(this.value," + ddl.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");
                if (objNacherf.SelAnnahmeAH || objNacherf.SelEditDurchzufVersZul)
                {
                    txtBox.Attributes.Add("onblur", "SetDDLValueWithoutDisablingButtons(this," + ddl.ClientID + ")");
                }
                else
                {
                    txtBox.Attributes.Add("onblur", "SetDDLValue(this," + ddl.ClientID + "," + lblID_POS.ClientID + "," + lblOldMatnr.ClientID + ")");
                }

                DataRow[] dRows = tblData.Select("IsNull(PosLoesch,'') <> 'L' AND ID_POS =" + lblID_POS.Text);
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
                ddl.Attributes.Add("onchange", "SetTexttValue(" + ddl.ClientID + "," + txtBox.ClientID + "," + txtMenge.ClientID + "," + lblMenge.ClientID + ")");

                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                if (mat != null)
                {
                    if (mat.MengeErlaubt)
                    {
                        txtMenge.Style["display"] = "block";
                        lblMenge.Style["display"] = "block";
                    }
                }
                i++;
            }

            GridView1.Columns[5].Visible = (m_User.Groups[0].Authorizationright != 1);// einige ZLD´s sollen Gebühr Amt nicht sehen
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Flag Menge erlaubt und Kundennummer
        /// um später, je nach Kunnde, das Mengenfeld einblenden zu können
        /// JS-Funktion: FilterItems
        /// </summary>
        private void TableToJSArrayMengeErlaubt()
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript3", objCommon.MaterialStammToJsArray(), true);
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
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript2", objCommon.KundenStammToJsArray(), true);
        }

        /// <summary>
        /// beim Postback Bar und Pauschalkunde setzen
        /// </summary>
        private void SetBar_Pauschalkunde()
        {
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == ddlKunnr.SelectedValue);
            if (kunde != null)
            {
                Pauschal.InnerHtml = (kunde.Pauschal ? "Pauschalkunde" : "");
                chkBar.Checked = kunde.Bar;
            }

            Label lblMenge = (Label)GridView1.HeaderRow.FindControl("lblMenge");
            lblMenge.Style["display"] = "none";
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                DropDownList ddl;
                TextBox txtMenge;

                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                txtMenge.Style["display"] = "none";

                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                if (mat != null && mat.MengeErlaubt)
                {
                    txtMenge.Style["display"] = "block";
                    lblMenge.Style["display"] = "block";
                }
            }
        }

        /// <summary>
        /// Anzeige der erfassten Dienstleistungen
        /// Databind Gridrview
        /// </summary>
        private bool GetData()
        {
            lblError.Text = "";

            // ddlKunnr nur dann prüfen, wenn Kunde änderbar
            if ((ddlKunnr.Enabled) && (ddlKunnr.SelectedIndex < 1))
            {
                lblError.Text = "Kein Kunde ausgewählt.";
                return false;
            }

            DataTable tblData = (DataTable)Session["tblDienst"];
            proofDienstGrid(ref tblData);
            Session["tblDienst"] = tblData;

            if (ddlStVa.SelectedIndex < 1)
            {
                lblError.Text = "Keine STVA ausgewählt.";
                return false;
            }

            var normalColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            var errorColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");

            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtBox;
                TextBox txtMenge;
                DropDownList ddl;
                ddl = (DropDownList)gvRow.FindControl("ddlItems");
                txtBox = (TextBox)gvRow.FindControl("txtSearch");
                txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                ddl.BorderColor = normalColor;
                txtBox.BorderColor = normalColor;

                DataRow[] Row = tblData.Select("Value = '" + ddl.SelectedValue + "'");
                if (Row.Length > 1 && ddl.SelectedValue != "0")
                {
                    ddl.BorderColor = errorColor;
                    txtBox.BorderColor = errorColor;
                    lblError.Text = "Dienstleistungen und Artikel können nur einmal ausgewählt werden!";
                    return false;
                }
                if ((ddl.SelectedValue == "700") && (tblData.Select("Value = '559'").Length > 0))
                {
                    ddl.BorderColor = errorColor;
                    txtBox.BorderColor = errorColor;
                    lblError.Text = "Artikel 559 und 700 können nicht gemeinsam ausgewählt werden!";
                    return false;
                }
                // matnr Menge Prüfung
                var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == ddl.SelectedValue);
                if (mat != null)
                {
                    if (String.IsNullOrEmpty(txtMenge.Text) && mat.MengeErlaubt)
                    {
                        txtMenge.BorderColor = errorColor;
                        txtMenge.Style["display"] = "block";
                        lblError.Text = "Bitte geben Sie für diesen Artikel eine Menge ein!";
                        return false;
                    }
                }
            }

            return checkDate();
        }

        /// <summary>
        /// Validation Zulassungsdatum
        /// </summary>
        private bool checkDate()
        {
            String ZDat = ZLDCommon.toShortDateStr(txtZulDate.Text);

            if (String.IsNullOrEmpty(ZDat))
            {
                lblError.Text = "Ungültiges Zulassungsdatum!";
                return false;
            }

            if (!ZDat.IsDate())
            {
                lblError.Text = "Ungültiges Zulassungsdatum: Falsches Format.";
                return false;
            }

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
                return false;
            }

            tagesdatum = DateTime.Today;
            tagesdatum = tagesdatum.AddYears(1);
            if (DateNew > tagesdatum)
            {
                lblError.Text = "Das Datum darf max. 1 Jahr in der Zukunft liegen!";
                return false;
            }

            if (ihDatumIstWerktag.Value == "false")
            {
                lblError.Text = "Bitte wählen Sie einen Werktag für das Zulassungsdatum aus!";
                return false;
            }

            return true;
        }

        /// <summary>
        /// in Javascript Array aufbauen mit Stva und Sonderstva Bsp.: HH und HH1
        /// Eingabe Stva HH1 dann soll im Kennz.-teil1 HH stehen
        /// JS-Funktion: SetDDLValueSTVA
        /// </summary>
        private void TableToJSArray()
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ArrayScript", objCommon.SonderStvaStammToJsArray(), true);
        }

        /// <summary>
        /// Daten aus dem Controls sammeln und in SAP speichern. Zurück zur Listenansicht.
        /// </summary>
        private void DatenSpeichern()
        {
            lblError.Text = "";
            lblMessage.Text = "";

            if (GetData())
            {
                var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

                kopfdaten.Landkreis = txtStVa.Text;

                var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
                if (amt != null)
                    kopfdaten.KreisBezeichnung = amt.KreisBezeichnung;

                kopfdaten.Wunschkennzeichen = chkWunschKZ.Checked;
                kopfdaten.KennzeichenReservieren = chkReserviert.Checked;
                kopfdaten.BarzahlungKunde = chkBar.Checked;
                kopfdaten.Flieger = chkFlieger.Checked;
                kopfdaten.Referenz1 = txtReferenz1.Text.ToUpper();
                kopfdaten.Referenz2 = txtReferenz2.Text.ToUpper();

                if (String.IsNullOrEmpty(txtKennz2.Text) && !objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung)
                {
                    lblError.Text = "Bitte geben Sie das vollständige Kennzeichen ein.";
                    return;
                }

                kopfdaten.Zulassungsdatum = txtZulDate.Text.ToNullableDateTime("ddMMyy");

                var kennz = txtKennz1.Text.ToUpper() + "-" + txtKennz2.Text.ToUpper();
                // Kennzeichen geändert? dann wieder für die 
                // Prägeliste freigeben
                if (kopfdaten.Kennzeichen != kennz)
                {
                    kopfdaten.Kennzeichen = kennz;
                    kopfdaten.PraegelisteErstellt = false;
                }

                kopfdaten.Kennzeichenform = (ddlKennzForm.SelectedItem != null ? ddlKennzForm.SelectedItem.Text : "");

                kopfdaten.NurEinKennzeichen = chkEinKennz.Checked;
                kopfdaten.AnzahlKennzeichen = (chkEinKennz.Checked ? "1" : "2");

                kopfdaten.Bemerkung = txtBemerk.Text;

                if (!String.IsNullOrEmpty(txtKunnr.Text) && txtKunnr.Text != "0")
                {
                    if (kopfdaten.KundenNr != txtKunnr.Text)
                    {
                        kopfdaten.KundenNr = txtKunnr.Text;

                        proofCPD();
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
                                SaveBankAdressdaten();
                            }
                        }

                        if (!bnoError)
                        {
                            lbtnBank_Click(this, new EventArgs());
                            return;
                        }

                        if (!objNacherf.SelAnnahmeAH)
                        {
                            lblError.Text = "Kunde geändert! Klicken Sie bitte auf 'Preis Finden'!";
                            cmdCreate.Enabled = false;
                        }
                        return;
                    }
                    else
                    {
                        proofCPD();
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
                                SaveBankAdressdaten();
                            }
                        }

                        if (!bnoError)
                        {
                            lbtnBank_Click(this, new EventArgs());
                            return;
                        }
                    }
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

                if (!chkFlieger.Checked)
                {
                    kopfdaten.WebBearbeitungsStatus = (objNacherf.SelAnnahmeAH ? "A" : "O");
                }

                objNacherf.SaveVorgangToSap(objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);

                // Bei Änderung von StVa oder Zulassungsdatum, Vorgang aus Selektion ausschliessen
                if ((!String.IsNullOrEmpty(objNacherf.SelKreis) && kopfdaten.Landkreis != objNacherf.SelKreis)
                    || (!String.IsNullOrEmpty(objNacherf.SelDatum) && kopfdaten.Zulassungsdatum.ToString("dd.MM.yyyy") != objNacherf.SelDatum))
                {
                    objNacherf.Vorgangsliste.RemoveAll(vg => vg.SapId == kopfdaten.SapId);
                }

                Session["objNacherf"] = objNacherf;

                if (objNacherf.SelEditDurchzufVersZul)
                {
                    Response.Redirect("ChangeZLDNachVersandListe.aspx?AppID=" + Session["AppID"].ToString());
                }
                else
                {
                    Response.Redirect("ChangeZLDNachListe.aspx?AppID=" + Session["AppID"].ToString());
                }
            }
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
                DropDownList ddl = (DropDownList)gvRow.FindControl("ddlItems");
                TextBox txtBox = (TextBox)gvRow.FindControl("txtSearch");
                TextBox txtMenge = (TextBox)gvRow.FindControl("txtMenge");
                Label lblID_POS = (Label)gvRow.FindControl("lblID_POS");
                Label lblDLBezeichnung = (Label)gvRow.FindControl("lblDLBezeichnung");

                DataRow[] dRows = tblData.Select("IsNull(PosLoesch,'') <> 'L' AND ID_POS =" + lblID_POS.Text);

                DataRow targetRow;
                if (dRows.Length == 0)
                    targetRow = tblData.Rows[i];
                else
                    targetRow = dRows[0];

                targetRow["Search"] = txtBox.Text;
                targetRow["Value"] = ddl.SelectedValue;
                targetRow["Text"] = ddl.SelectedItem.Text;
                targetRow["Menge"] = txtMenge.Text;

                txtBox = (TextBox)gvRow.FindControl("txtPreis");
                targetRow["Preis"] = txtBox.Text.ToDecimal(0);

                txtBox = (TextBox)gvRow.FindControl("txtGebPreis");
                targetRow["GebPreis"] = txtBox.Text.ToDecimal(0);

                txtBox = (TextBox)gvRow.FindControl("txtGebAmt");
                targetRow["GebAmt"] = txtBox.Text.ToDecimal(0);

                if (ddl.SelectedValue == ZLDCommon.CONST_IDSONSTIGEDL)
                {
                    targetRow["DLBezeichnung"] = lblDLBezeichnung.Text;
                }
                else
                {
                    targetRow["DLBezeichnung"] = "";
                }
                i++;
            }
        }

        /// <summary>
        /// beim Kundenwechsel prüfen ob sich um CPD handelt
        /// wenn ja chkCPD.Checked = true und  prüfen ob CPD mit Einzugserm.
        /// </summary>
        private void proofCPD()
        {
            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == ddlKunnr.SelectedValue);
            if (kunde != null)
            {
                chkCPD.Checked = kunde.Cpd;
                chkCPDEinzug.Checked = (kunde.Cpd && kunde.CpdMitEinzug);
            }
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

            if (chkEinzug.Checked)
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
                if (txtKontoinhaber.Text.Length == 0)
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
                if (txtPlz.Text.Length < 5)
                {
                    txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bEdited = false;
                }
            }

            if (chkEinzug.Checked)
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
                if (txtKontoinhaber.Text.Length == 0)
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
        /// Prüfung ob anhand der eingebenen IBAN die Daten im System exisitieren
        /// Aufruf objCommon.ProofIBAN
        /// </summary>
        /// <returns>Bei Fehler true</returns>
        private Boolean ProofBank()
        {
            if (!String.IsNullOrEmpty(txtIBAN.Text) || chkCPDEinzug.Checked)
            {
                objCommon.IBAN = txtIBAN.Text.NotNullOrEmpty().Trim().ToUpper();
                objCommon.ProofIBAN();

                if (objCommon.ErrorOccured)
                {
                    txtIBAN.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.ForeColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    lblErrorBank.Text = objCommon.Message;
                    return false;
                }

                txtSWIFT.Text = objCommon.SWIFT;
                txtGeldinstitut.Text = objCommon.Bankname;
            }

            return true;
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
        /// Je nach Modus bestimmte Eingabefelder deaktivieren bzw. für Eingaben sperren
        /// </summary>
        private void disableEingabefelder()
        {
            cmdNewDLPrice.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
            cmdFindPrize.Enabled = !objNacherf.SelAnnahmeAH;
            txtBarcode.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKunnr.Enabled = !objNacherf.SelEditDurchzufVersZul;
            ddlKunnr.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtReferenz1.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtReferenz2.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtPreisKennz.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtSteuer.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkBar.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkWunschKZ.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkReserviert.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtNrReserviert.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnFeinstaub.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnGestern.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnHeute.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnMorgen.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKennz1.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtKennz2.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkEinKennz.Enabled = !objNacherf.SelEditDurchzufVersZul;
            chkKennzSonder.Enabled = !objNacherf.SelEditDurchzufVersZul;
            ddlKennzForm.Enabled = !objNacherf.SelEditDurchzufVersZul;
            txtService.Enabled = !objNacherf.SelEditDurchzufVersZul;
            lbtnBank.Enabled = !objNacherf.SelEditDurchzufVersZul;
        }

        /// <summary>
        /// Je nach Modus Felder im Grid deaktivieren bzw. für Eingaben sperren
        /// </summary>
        private void disableGridfelder()
        {
            foreach (GridViewRow gvRow in GridView1.Rows)
            {
                TextBox txtPreis = (TextBox)gvRow.FindControl("txtPreis");
                TextBox txtGebPreis = (TextBox)gvRow.FindControl("txtGebPreis");
                TextBox txtGebAmt = (TextBox)gvRow.FindControl("txtGebAmt");

                txtPreis.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
                txtGebPreis.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelEditDurchzufVersZul);
                txtGebAmt.Enabled = (!objNacherf.SelAnnahmeAH && !objNacherf.SelSofortabrechnung && !objNacherf.SelEditDurchzufVersZul);
            }
        }

        /// <summary>
        /// Aufbau einer neuen Position(Dinstleistung/Artikel) ohne Gebührenmat. für die Preisfindung.
        /// </summary>
        /// <param name="dRow">Zeile</param>
        /// <param name="neuePositionen"></param>
        private void NewPosOhneGebMat(DataRow dRow, ref List<ZLDPosition> neuePositionen)
        {
            var NewPosID = (neuePositionen.Any() ? neuePositionen.Max(p => p.PositionsNr).ToInt(0) : objNacherf.AktuellerVorgang.Positionen.Max(p => p.PositionsNr).ToInt(0));

            var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

            NewPosID += 10;

            neuePositionen.Add(new ZLDPosition
            {
                SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = "0",
                WebMaterialart = "D",
                Menge = (dRow["Menge"].ToString().IsNumeric() ? dRow["Menge"].ToString().ToDecimal() : 1),
                MaterialName = matbez,
                MaterialNr = dRow["Value"].ToString(),
                Preis = dRow["Preis"].ToString().ToDecimal(),
                SdRelevant = (bool)dRow["SdRelevant"],
                GebuehrAmt = 0,
                GebuehrAmtAdd = 0,
                Loeschkennzeichen = dRow["PosLoesch"].ToString()
            });
        }

        /// <summary>
        /// Neue Hauptposition mit Gebührenmat. für die Preisfindung.
        /// </summary>
        /// <param name="dRow">Zeile mit Material</param>
        private List<ZLDPosition> NewHauptPosition(DataRow dRow)
        {
            var posListe = new List<ZLDPosition>();

            var NewPosID = 10;
            var NewUePosID = 10;

            var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == objNacherf.AktuellerVorgang.Kopfdaten.KundenNr);

            posListe.Add(new ZLDPosition
            {
                SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = "0",
                WebMaterialart = "D",
                Menge = 1,
                MaterialName = matbez,
                MaterialNr = dRow["Value"].ToString(),
                Preis = dRow["Preis"].ToString().ToDecimal(),
                SdRelevant = (bool)dRow["SdRelevant"],
                Loeschkennzeichen = dRow["PosLoesch"].ToString()
            });

            // Geb.Material aus der Stammtabelle
            var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == dRow["Value"].ToString());
            if (mat != null && !String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
            {
                NewPosID += 10;

                var ohneUst = (kunde != null && kunde.OhneUst);

                posListe.Add(new ZLDPosition
                {
                    SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = NewUePosID.ToString(),
                    WebMaterialart = "G",
                    Menge = 1,
                    MaterialName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                    MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                    Preis = 0,
                    GebuehrAmt = 0,
                    GebuehrAmtAdd = 0,
                    Loeschkennzeichen = dRow["PosLoesch"].ToString()
                });
            }

            // neues Kennzeichenmaterial
            if ((kunde == null || !kunde.Pauschal) && mat != null && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
            {
                NewPosID += 10;

                posListe.Add(new ZLDPosition
                {
                    SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                    PositionsNr = NewPosID.ToString(),
                    UebergeordnetePosition = NewUePosID.ToString(),
                    WebMaterialart = "K",
                    Menge = 1,
                    MaterialName = "",
                    MaterialNr = mat.KennzeichenMaterialNr,
                    Preis = 0,
                    GebuehrAmt = 0,
                    GebuehrAmtAdd = 0,
                    Loeschkennzeichen = dRow["PosLoesch"].ToString()
                });
            }

            // neues Steuermaterial
            NewPosID += 10;

            posListe.Add(new ZLDPosition
            {
                SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                PositionsNr = NewPosID.ToString(),
                UebergeordnetePosition = NewUePosID.ToString(),
                WebMaterialart = "S",
                Menge = 1,
                MaterialName = "",
                MaterialNr = "591",
                Preis = 0,
                GebuehrAmt = 0,
                GebuehrAmtAdd = 0,
                Loeschkennzeichen = dRow["PosLoesch"].ToString()
            });

            return posListe;
        }

        /// <summary>
        /// Prüfen ob Dienstleistungen/Artikel geändert oder hinzugefügt wurden. 
        /// Positionen für Preisfindung aufbauen.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        /// <returns>true bei Änderungen</returns>
        private Boolean proofdifferentHauptMatnr(ref DataTable tblData)
        {
            bool blnChangeMatnr = false;
            proofDienstGrid(ref tblData);

            List<ZLDPosition> neuePos = new List<ZLDPosition>();

            foreach (DataRow tblRow in tblData.Rows)
            {
                var dRow = tblRow;

                if (dRow["Value"].ToString() != "0")
                {
                    var positionen = objNacherf.AktuellerVorgang.Positionen;

                    var selPos = positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                    if (selPos != null)
                    {
                        if (selPos.WebMaterialart == "D")
                        {
                            if (selPos.MaterialNr != dRow["Value"].ToString() && dRow["ID_POS"].ToString() == "10")
                            {
                                blnChangeMatnr = true;
                                var neueHpPos = NewHauptPosition(dRow);//neue Hauptposition aufbauen
                                foreach (var item in neueHpPos)// in die bestehende Positionstabelle schieben
                                {
                                    var pos = positionen.FirstOrDefault(p => p.PositionsNr == item.PositionsNr);
                                    if (pos != null)
                                    {
                                        var idx = positionen.IndexOf(pos);
                                        positionen[idx] = item;
                                    }
                                }
                                if (neueHpPos.Count < positionen.Count)
                                {
                                    foreach (var delPos in positionen.Where(p => neueHpPos.None(np => np.PositionsNr == p.PositionsNr)))
                                    {
                                        delPos.Loeschkennzeichen = "L";
                                    }
                                }
                            }
                            else if (selPos.MaterialNr == dRow["Value"].ToString() && dRow["ID_POS"].ToString() == "10")
                            {
                                // eingegebene Preise übernehmen
                                selPos.Preis = dRow["Preis"].ToString().ToDecimal();
                                selPos.SdRelevant = (bool)dRow["SdRelevant"];
                            }
                            else if (selPos.MaterialNr != dRow["Value"].ToString() && dRow["ID_POS"].ToString() != "10")
                            {
                                // alle zur alten Hauptposition gehörenden Unterpositionen wenn sie unterschiedlich sind löschen
                                selPos.Loeschkennzeichen = "L";

                                foreach (var delPos in positionen.Where(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString()))
                                {
                                    delPos.Loeschkennzeichen = "L";
                                }

                                // und die neue Unterposition einfügen ohne Geb.-Positionen, wird später in der Preisfindung aufgebaut
                                NewPosOhneGebMat(dRow, ref neuePos);
                            }
                        }
                        else if (blnChangeMatnr && selPos.MaterialNr != dRow["Value"].ToString())
                        {
                            NewPosOhneGebMat(dRow, ref neuePos);
                        }
                    }
                    else
                    {
                        if (dRow["ID_POS"].ToString() == "10")
                            blnChangeMatnr = true;

                        NewPosOhneGebMat(dRow, ref neuePos);
                    }
                }
            }
            // Gibt es neue Positionen dann ab in die Preisfindung
            if (neuePos.Any())
            {
                if (neuePos.Any(p => p.MaterialNr == "559"))
                {
                    lblError.Text = "Material 559 kann nicht nachträglich hinzugefügt werden!";
                }
                else
                {
                    objNacherf.GetPreiseNewPositionen(neuePos, objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);
                    if (objNacherf.ErrorOccured)
                    {
                        lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                    }
                }
            }

            return blnChangeMatnr;
        }

        /// <summary>
        /// Dienstleistungsdaten für die Preisfindung sammeln.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        private void GetDiensleitungDataForPrice(ref DataTable tblData)
        {
            proofDienstGrid(ref tblData);

            var positionen = objNacherf.AktuellerVorgang.Positionen;

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == objNacherf.AktuellerVorgang.Kopfdaten.KundenNr);
            var ohneUst = (kunde != null && kunde.OhneUst);

            foreach (DataRow item in tblData.Rows)
            {
                var dRow = item;

                if (dRow["Value"].ToString() != "0")
                {
                    var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                    var pos = positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                    if (pos != null)
                    {
                        pos.MaterialName = matbez;
                        pos.MaterialNr = dRow["Value"].ToString();

                        pos.Preis = dRow["Preis"].ToString().ToDecimal();
                        pos.SdRelevant = (bool)dRow["SdRelevant"];
                        pos.Menge = dRow["Menge"].ToString().ToDecimal();
                        pos.Loeschkennzeichen = dRow["PosLoesch"].ToString();

                        var mat = objCommon.MaterialStamm.FirstOrDefault(m => m.MaterialNr == pos.MaterialNr);

                        var gebuehrenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "G");
                        if (gebuehrenPos != null && mat != null)
                        {
                            if (kunde == null || !kunde.Pauschal)
                            {
                                gebuehrenPos.MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);
                                gebuehrenPos.MaterialName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName);
                            }
                            gebuehrenPos.Preis = dRow["GebPreis"].ToString().ToDecimal();
                            gebuehrenPos.GebuehrAmt = dRow["GebAmt"].ToString().ToDecimal();
                            gebuehrenPos.SdRelevant = (bool)dRow["SdRelevant"];
                        }

                        var kennzeichenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "K");
                        if (kennzeichenPos != null && mat != null)
                        {
                            if (chkEinKennz.Checked)
                            {
                                kennzeichenPos.Menge = dRow["Menge"].ToString().ToDecimal();
                            }
                            else
                            {
                                kennzeichenPos.Menge = 2;
                                if (dRow["Menge"].ToString().IsNumeric())
                                {
                                    kennzeichenPos.Menge = (dRow["Menge"].ToString().ToDecimal() * 2);
                                }
                            }

                            kennzeichenPos.Preis = txtPreisKennz.Text.ToDecimal();
                            kennzeichenPos.SdRelevant = (bool)dRow["SdRelevant"];
                        }

                        var steuerPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "S");
                        if (steuerPos != null)
                        {
                            steuerPos.Preis = txtSteuer.Text.ToDecimal();
                            steuerPos.SdRelevant = (bool)dRow["SdRelevant"];
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
                            var neuePos = new List<ZLDPosition> {
                                new ZLDPosition
                                {
                                    SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                                    PositionsNr = dRow["ID_POS"].ToString(),
                                    WebMaterialart = "D",
                                    Menge = 1,
                                    MaterialNr = dRow["Value"].ToString(),
                                    MaterialName = matbez,
                                    Preis = dRow["Preis"].ToString().ToDecimal(),
                                    SdRelevant = (bool)dRow["SdRelevant"],
                                    Loeschkennzeichen = dRow["PosLoesch"].ToString()
                                }
                            };

                            objNacherf.GetPreiseNewPositionen(neuePos, objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);
                            if (objNacherf.ErrorOccured)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dienstleistungsdaten für die Speicherung sammeln.
        /// </summary>
        /// <param name="tblData">Gridtabelle</param>
        private Boolean GetDiensleitungData(ref DataTable tblData)
        {
            proofDienstGrid(ref tblData);

            var positionen = objNacherf.AktuellerVorgang.Positionen;

            foreach (DataRow dRow in tblData.Rows)
            {
                if (dRow["Value"].ToString() != "0")
                {
                    var matbez = objCommon.GetMaterialNameFromDienstleistungRow(dRow);

                    var pos = positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                    if (pos != null)
                    {
                        if (pos.WebMaterialart == "D")
                        {
                            pos.MaterialName = matbez;

                            if (pos.MaterialNr != dRow["Value"].ToString())
                            {
                                pos.MaterialNr = dRow["Value"].ToString();
                                return true;
                            }

                            pos.Preis = dRow["Preis"].ToString().ToDecimal();
                            pos.SdRelevant = (bool)dRow["SdRelevant"];
                            pos.Menge = dRow["Menge"].ToString().ToDecimal();
                            pos.Loeschkennzeichen = dRow["PosLoesch"].ToString();
                        }

                        var gebuehrenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "G");
                        if (gebuehrenPos != null)
                        {
                            gebuehrenPos.Preis = dRow["GebPreis"].ToString().ToDecimal();
                            gebuehrenPos.GebuehrAmt = dRow["GebAmt"].ToString().ToDecimal();
                            gebuehrenPos.Menge = dRow["Menge"].ToString().ToDecimal();
                        }

                        var kennzeichenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "K");
                        if (kennzeichenPos != null)
                        {
                            if (chkEinKennz.Checked)
                            {
                                kennzeichenPos.Menge = dRow["Menge"].ToString().ToDecimal();
                            }
                            else
                            {
                                kennzeichenPos.Menge = 2;
                                if (dRow["Menge"].ToString().IsNumeric())
                                {
                                    kennzeichenPos.Menge = (dRow["Menge"].ToString().ToDecimal() * 2);
                                }
                            }

                            kennzeichenPos.Preis = txtPreisKennz.Text.ToDecimal();
                        }

                        var steuerPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == dRow["ID_POS"].ToString() && p.WebMaterialart == "S");
                        if (steuerPos != null)
                        {
                            steuerPos.Preis = txtSteuer.Text.ToDecimal();
                            steuerPos.Menge = dRow["Menge"].ToString().ToDecimal();
                            steuerPos.SdRelevant = (bool)dRow["SdRelevant"];
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
                            var neuePos = new List<ZLDPosition> {
                                new ZLDPosition
                                {
                                    SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId,
                                    PositionsNr = dRow["ID_POS"].ToString(),
                                    WebMaterialart = "D",
                                    Menge = 1,
                                    MaterialNr = dRow["Value"].ToString(),
                                    MaterialName = matbez,
                                    Preis = dRow["Preis"].ToString().ToDecimal(),
                                    SdRelevant = (bool)dRow["SdRelevant"],
                                    Loeschkennzeichen = dRow["PosLoesch"].ToString()
                                }
                            };

                            // gleich Preise/SDRelevant aus SAP ziehen
                            objNacherf.GetPreiseNewPositionen(neuePos, objCommon.KundenStamm, objCommon.MaterialStamm, m_User.UserName);
                            if (objNacherf.ErrorOccured)
                            {
                                lblError.Text = "Fehler bei der Kommunikation. Daten konnten nicht aus SAP gezogen werden! " + objNacherf.Message;
                            }
                            else // Daten in tblData aktualisieren
                            {
                                var newPos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == dRow["ID_POS"].ToString());
                                if (newPos != null)
                                {
                                    dRow["SdRelevant"] = newPos.SdRelevant.IsTrue();
                                    dRow["Menge"] = newPos.Menge;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Prüfen ob an der Position ein Gebührenpacket hängt, wenn ja 
        /// sperren.
        /// </summary>
        /// <param name="IDPos">ID der Position</param>
        /// <returns>Ja-False, Nein-True</returns>
        protected bool proofGebPak(String IDPos)
        {
            var pos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.PositionsNr == IDPos);
            if (pos != null && pos.Gebuehrenpaket.IsTrue())
                return false;

            return true;
        }

        protected bool proofPauschMat(String Matnr)
        {
            return objCommon.proofPauschMat(objNacherf.AktuellerVorgang.Kopfdaten.KundenNr, Matnr);
        }

        /// <summary>
        /// Gebührenmaterial vorhanden?
        /// </summary>
        /// <param name="Matnr"></param>
        /// <returns></returns>
        protected bool proofGebMat(String Matnr)
        {
            return objCommon.proofGebMat(Matnr);
        }

        private void SaveBankAdressdaten()
        {
            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            adressdaten.SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId;
            adressdaten.Name1 = txtName1.Text;
            adressdaten.Name2 = txtName2.Text;
            adressdaten.Partnerrolle = "AG";
            adressdaten.Strasse = txtStrasse.Text;
            adressdaten.Plz = txtPlz.Text;
            adressdaten.Ort = txtOrt.Text;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            adressdaten.SapId = objNacherf.AktuellerVorgang.Kopfdaten.SapId;
            bankdaten.SWIFT = txtSWIFT.Text;
            bankdaten.IBAN = (String.IsNullOrEmpty(txtIBAN.Text) ? "" : txtIBAN.Text.ToUpper());
            bankdaten.Bankleitzahl = objCommon.Bankschluessel;
            bankdaten.KontoNr = objCommon.Kontonr;
            bankdaten.Geldinstitut = (txtGeldinstitut.Text != "Wird automatisch gefüllt!" ? txtGeldinstitut.Text : "");
            bankdaten.Kontoinhaber = txtKontoinhaber.Text;
            bankdaten.Einzug = chkEinzug.Checked;
            bankdaten.Rechnung = chkRechnung.Checked;
        }

        private void ResetBankAdressdaten()
        {
            var adressdaten = objNacherf.AktuellerVorgang.Adressdaten;

            txtName1.Text = adressdaten.Name1;
            txtName2.Text = adressdaten.Name2;
            txtStrasse.Text = adressdaten.Strasse;
            txtPlz.Text = adressdaten.Plz;
            txtOrt.Text = adressdaten.Ort;

            var bankdaten = objNacherf.AktuellerVorgang.Bankdaten;

            txtSWIFT.Text = bankdaten.SWIFT;
            txtIBAN.Text = bankdaten.IBAN;
            txtGeldinstitut.Text = (String.IsNullOrEmpty(bankdaten.Geldinstitut) ? "Wird automatisch gefüllt!" : bankdaten.Geldinstitut);
            txtKontoinhaber.Text = bankdaten.Kontoinhaber;
            chkEinzug.Checked = bankdaten.Einzug.IsTrue();
            chkRechnung.Checked = bankdaten.Rechnung.IsTrue();
        }

        private void UpdateDlTableWithPrizes(ref DataTable tblData)
        {
            tblData.Rows.Clear();

            // ermittelte Preise ins Dienstleistungsgrid laden
            foreach (var pos in objNacherf.AktuellerVorgang.Positionen)
            {
                switch (pos.WebMaterialart)
                {
                    case "D":
                        DataRow tblRow = tblData.NewRow();

                        tblRow["Search"] = pos.MaterialNr;
                        tblRow["Value"] = pos.MaterialNr;
                        tblRow["OldValue"] = pos.MaterialNr;
                        tblRow["Text"] = pos.MaterialName;
                        tblRow["Preis"] = pos.Preis.GetValueOrDefault(0);

                        var gebuehrPos = objNacherf.AktuellerVorgang.Positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");

                        tblRow["GebPreis"] = (gebuehrPos != null ? gebuehrPos.Preis.GetValueOrDefault(0) : 0);
                        tblRow["GebAmt"] = (gebuehrPos != null ? gebuehrPos.GebuehrAmt.GetValueOrDefault(0) : 0);
                        tblRow["ID_POS"] = pos.PositionsNr;
                        tblRow["NewPos"] = false;
                        tblRow["Menge"] = (pos.Menge.ToString().IsNumeric() ? pos.Menge.ToString("F0") : "1");

                        if (pos.PositionsNr == "10")
                        {
                            hfMatnr.Value = pos.MaterialNr;
                            txtPreisKennz.Enabled = true;

                            if (!proofPauschMat(pos.MaterialNr))
                            {
                                txtPreisKennz.Text = "0,00";
                                txtPreisKennz.Enabled = false;
                            }
                        }

                        tblRow["SdRelevant"] = pos.SdRelevant.IsTrue();

                        if (pos.MaterialNr == ZLDCommon.CONST_IDSONSTIGEDL)
                        {
                            tblRow["DLBezeichnung"] = pos.MaterialName;
                        }
                        else
                        {
                            tblRow["DLBezeichnung"] = "";
                        }

                        tblData.Rows.Add(tblRow);
                        break;

                    case "K":
                        txtPreisKennz.Text = pos.Preis.ToString("f");
                        break;

                    case "S":
                        txtSteuer.Text = pos.Preis.ToString("f");
                        break;
                }
            }
        }

        #endregion
    }
}
