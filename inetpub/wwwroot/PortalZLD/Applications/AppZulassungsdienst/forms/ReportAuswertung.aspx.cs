﻿using System;
using System.Linq;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektionsseite Auswertung.
    /// </summary>
    public partial class ReportAuswertung : System.Web.UI.Page
    {
        private User m_User;
        private Listen objListe;
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
                objCommon.GetGruppen_Touren("K");
                objCommon.GetGruppen_Touren("T");
                Session["objCommon"] = objCommon;
            }
            else
            {
                objCommon = (ZLDCommon)Session["objCommon"];

                if (objCommon.Kundengruppen == null)
                    objCommon.GetGruppen_Touren("K");

                if (objCommon.Touren == null)
                    objCommon.GetGruppen_Touren("T");
            }

            InitLargeDropdowns();
            InitJava();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool BackFromList = (Request.QueryString["BackFromList"] != null);

            if (!IsPostBack)
            {
                objListe = new Listen(m_User.Reference);

                if ((BackFromList) && (Session["SelDatum"] != null) && (Session["SelDatumBis"] != null))
                {
                    txtZulDate.Text = Session["SelDatum"].ToString();
                    txtZulDateBis.Text = Session["SelDatumBis"].ToString();
                }
                Session.Remove("SelDatum");
                Session.Remove("SelDatumBis");
                fillForm();
            }
            else
            {
                objListe = (Listen)Session["objListe"];
                objListe.SelID = txtID.Text;
                objListe.SelKunde = txtKunnr.Text;
                objListe.SelStvA = txtStVa.Text;
                objListe.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objListe.SelDatumBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);
                Session["SelDatum"] = txtZulDate.Text;
                Session["SelDatumBis"] = txtZulDateBis.Text;
                objListe.SelMatnr = txtMatnr.Text;
                objListe.SelRef1 = txtRef1.Text;
                objListe.SelKennz = txtKennz.Text;
                objListe.SelZahlart = rbListZahlart.SelectedValue;
                objListe.alleDaten = rbAlle.Checked.BoolToX();
                if (rbAlle.Checked)
                {
                    objListe.Abgerechnet = "*";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbAbgerechnet.Checked)
                {
                    objListe.Abgerechnet = "A";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbnichtAbgerechnet.Checked)
                {
                    objListe.Abgerechnet = "N";
                    objListe.NochNichtDurchgefuehrt = "";
                }
                else if (rbnichtDurchgefuehrt.Checked)
                {
                    objListe.Abgerechnet = "*";
                    objListe.NochNichtDurchgefuehrt = "X";
                }
            }

            Session["objListe"] = objListe;
        }

        /// <summary>
        /// Kreisauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStVa.Text = ddlStVa.SelectedValue;
        }

        /// <summary>
        /// Kundenauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = (ddlKunnr.SelectedValue == "0" ? "" : ddlKunnr.SelectedValue);
        }

        /// <summary>
        /// Dienstleistungsauswahl in der DropDown geändert.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlDienst_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMatnr.Text = ddlDienst.SelectedValue;
        }

        /// <summary>
        /// Funktionsaufruf DoSubmit().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        /// <summary>
        /// Zurück zur Startseite
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sammeln der Selektionsdaten und an Sap übergeben. 
        /// Weiterleiten an Listenansicht Auswertung(ReportAuswertung_2.aspx)
        /// </summary>
        private void DoSubmit()
        {
            lblError.Text = "";

            // Wenn keine Selektion über Id, Ref1, Kennzeichen oder Kunde -> Zulassungsdatum erforderlich
            if ((String.IsNullOrEmpty(txtID.Text)) && (String.IsNullOrEmpty(txtRef1.Text)) && (String.IsNullOrEmpty(txtKennz.Text)) && (ddlKunnr.SelectedValue == "0"))
            {
                if ((String.IsNullOrEmpty(txtZulDate.Text)) || (String.IsNullOrEmpty(txtZulDateBis.Text)))
                {
                    lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                    return;
                }
            }

            // Wenn Selektion nach Zulassungsdatum, dann nur mit beiden Datumsangaben (von und bis)
            if (((!String.IsNullOrEmpty(txtZulDate.Text)) && (String.IsNullOrEmpty(txtZulDateBis.Text)))
                || ((!String.IsNullOrEmpty(txtZulDateBis.Text)) && (String.IsNullOrEmpty(txtZulDate.Text))))
            {
                lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                return;
            }

            // Bei Zeitraumselektion Datumsfelder prüfen
            if ((!String.IsNullOrEmpty(txtZulDate.Text)) && (!String.IsNullOrEmpty(txtZulDateBis.Text)))
            {
                objListe.Zuldat = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objListe.ZuldatBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);

                if (String.IsNullOrEmpty(objListe.Zuldat) || String.IsNullOrEmpty(objListe.ZuldatBis))
                {
                    lblError.Text = "Bitte geben Sie einen gültigen Zeitraum für das Zulassungsdatum an!";
                    return;
                }

                DateTime vonDatum = DateTime.Parse(objListe.Zuldat);
                DateTime bisDatum = DateTime.Parse(objListe.ZuldatBis);
                if (vonDatum > bisDatum)
                {
                    lblError.Text = "Das Bis-Datum muss größer sein als das Von-Datum!";
                    return;
                }
                if ((bisDatum - vonDatum).TotalDays > 92)
                {
                    lblError.Text = "Zeitraum max. 92 Tage möglich!";
                    return;
                }
            }

            if (ddlTour.SelectedValue != "0" && ddlGruppe.SelectedValue != "0")
            {
                lblError.Text = "Bitte wählen Sie entweder Tour oder Gruppe aus!";
                return;
            }
            if (ddlGruppe.SelectedValue != "0")
            {
                objListe.SelGroupTourID = ddlGruppe.SelectedValue;
            }
            else if (ddlTour.SelectedValue != "0")
            {
                objListe.SelGroupTourID = ddlTour.SelectedValue;
            }

            objListe.FillAuswertungNeu();

            if (objListe.ErrorOccured)
            {
                lblError.Text = "Fehler: " + objListe.Message;
            }
            else
            {
                if (objListe.Auswertung.Rows.Count == 0)
                {
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien.";
                }
                else
                {
                    Session["objListe"] = objListe;
                    Response.Redirect("ReportAuswertung_2.aspx?AppID=" + Session["AppID"].ToString());
                }
            }

        }

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
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");
            txtStVa.Attributes.Add("onkeyup", "FilterSTVA(this.value," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
            txtStVa.Attributes.Add("onblur", "SetDDLValueSTVA(this," + ddlStVa.ClientID + "," + txtStVa.ClientID + ")");
            txtMatnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlDienst.ClientID + ")");
            txtMatnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlDienst.ClientID + ")");
            txtKennz.Attributes.Add("onkeyup", "FilterKennz(this,event)");
        }

        /// <summary>
        /// Dropdowns mit den Stammdaten füllen.
        /// </summary>
        private void fillForm()
        {
            if (objListe.ErrorOccured)
            {
                lblError.Text = objListe.Message;
                return;
            }

            ddlDienst.DataSource = objCommon.MaterialStamm.Where(m => !m.Inaktiv).ToList();
            ddlDienst.DataValueField = "MaterialNr";
            ddlDienst.DataTextField = "Name";
            ddlDienst.DataBind();
            ddlDienst.SelectedValue = "0";

            ddlGruppe.DataSource = objCommon.Kundengruppen;
            ddlGruppe.DataValueField = "Gruppe";
            ddlGruppe.DataTextField = "GruppenName";
            ddlGruppe.DataBind();
            ddlGruppe.SelectedValue = "0";

            ddlTour.DataSource = objCommon.Touren;
            ddlTour.DataValueField = "Gruppe";
            ddlTour.DataTextField = "GruppenName";
            ddlTour.DataBind();
            ddlTour.SelectedValue = "0";
        }

        #endregion
    }
}