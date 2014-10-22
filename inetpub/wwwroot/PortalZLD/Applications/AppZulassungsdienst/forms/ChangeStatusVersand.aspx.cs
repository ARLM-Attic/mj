﻿using System;
using System.Web.UI;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;


namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Selektion Übersicht offener Versandzulassungen und Rechnungsprüfung.
    /// </summary>
    public partial class ChangeStatusVersand : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private VorVersand objVersandZul;
        private ZLDCommon objCommon;
        /// <summary>
        /// Page_Load-Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Stammdaten laden.
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
            if (IsPostBack != true )
            {

                objVersandZul = new VorVersand(ref m_User, ref m_App, Session["AppID"].ToString(), Session.SessionID);
                objVersandZul.VKBUR = m_User.Reference.Substring(4, 4);
                objVersandZul.VKORG = m_User.Reference.Substring(0, 4);
                fillForm();
            }
            else
            {
                objVersandZul = (VorVersand)Session["objVersandZul"];
                objVersandZul.SelID = txtID.Text;
                objVersandZul.SelKunde = txtKunnr.Text;
                objVersandZul.SelKreis = txtStVa.Text;
                objVersandZul.SelLief = ddlLief.SelectedValue;
                objVersandZul.SelDatum = ZLDCommon.toShortDateStr(txtZulDate.Text);
                objVersandZul.SelDatumBis = ZLDCommon.toShortDateStr(txtZulDateBis.Text);

                if (rbAuswahl1.Checked) { objVersandZul.SelStatus = "1"; }
                if (rbAuswahl2.Checked) { objVersandZul.SelStatus = "2"; }
                if (rbAuswahl3.Checked) { objVersandZul.SelStatus = "3"; }
                Session["objVersandZul"] = objVersandZul;
            }
        }
        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kreises. Laden des 
        /// zuständigen ZLD/externen Dienstleiters(Z_ZLD_EXPORT_INFOPOOL).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlStVa_SelectedIndexChanged(object sender, EventArgs e)
        {
            objVersandZul = (VorVersand)Session["objVersandZul"];
            txtStVa.Text = ddlStVa.SelectedValue;
            objVersandZul.SelLief = txtStVa.Text;

            if (objVersandZul.BestLieferanten == null)
            { objVersandZul.getBestLieferant(Session["AppID"].ToString(), Session.SessionID, this); }


            if (objVersandZul.Status > 0)
            {
                ddlLief.Items.Clear();
                lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
                Session["objVersandZul"] = objVersandZul;
            }
            else
            {
                ddlLief.DataSource = objVersandZul.BestLieferanten;
                ddlLief.DataValueField = "LIFNR";
                ddlLief.DataTextField = "NAME1";
                ddlLief.DataBind();
                ddlLief.SelectedValue = "0";
                Session["objNacherf"] = objVersandZul;
            }
        }
        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kunden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlKunnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKunnr.Text = ddlKunnr.SelectedValue;
        }

        /// <summary>
        /// Binden der DropDowns an Stammdaten und Javascript-Funktionen.
        /// </summary>
        private void fillForm()
        {
            DataView tmpDView = new DataView();
            tmpDView = objCommon.tblKundenStamm.DefaultView;
            tmpDView.Sort = "NAME1";
            ddlKunnr.DataSource = tmpDView;
            ddlKunnr.DataValueField = "KUNNR";
            ddlKunnr.DataTextField = "NAME1";
            ddlKunnr.DataBind();
            ddlKunnr.SelectedValue = "0";
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunnr.ClientID + ")");
            txtKunnr.Attributes.Add("onblur", "SetDDLValue(this," + ddlKunnr.ClientID + ")");

            /// objVersandZul.getSAPZulStellen(Session["AppID"].ToString(), Session.SessionID, this);
            tmpDView = new DataView();
            tmpDView = objCommon.tblStvaStamm.DefaultView;
            tmpDView.Sort = "KREISTEXT";
            ddlStVa.DataSource = tmpDView;
            ddlStVa.DataValueField = "KREISKZ";
            ddlStVa.DataTextField = "KREISTEXT";
            ddlStVa.DataBind();
            ddlStVa.SelectedValue = "0";
            Session["objVersandZul"] = objVersandZul;
        }
        
        /// <summary>
        /// Selektionsdaten an SAP übergeben(Z_ZLD_EXPORT_VZOZUERL)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            objVersandZul = (VorVersand)Session["objVersandZul"];
            objVersandZul.FillVersanZul(Session["AppID"].ToString(), Session.SessionID, this, objCommon.tblKundenStamm);
            Session["objVersandZul"] = objVersandZul;
            if (objVersandZul.Status != 0)
            {
                lblError.Text = objVersandZul.Message;
            }
            else if (objVersandZul.Liste.Rows.Count == 0)
            {
                lblError.Text = objVersandZul.Message;
            }
            else 
            {
                Response.Redirect("ChangeStatusVersandList.aspx?AppID=" + Session["AppID"].ToString());
            }
        }
        /// <summary>
        /// Enter-Button-Dummy.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnEmpty_Click(object sender, ImageClickEventArgs e)
        {
            cmdCreate_Click(sender, e);
        }
        /// <summary>
        /// Auswahl/Eingabe des zu selektierenden Kreises. Laden des 
        /// zuständigen ZLD/externen Dienstleiters(Z_ZLD_EXPORT_INFOPOOL).
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void txtStVa_TextChanged(object sender, EventArgs e)
        {
            objVersandZul = (VorVersand)Session["objVersandZul"];
            objVersandZul.SelKreis = txtStVa.Text.ToUpper();
            ddlStVa.SelectedValue = txtStVa.Text.ToUpper();
            txtStVa.Text=txtStVa.Text.ToUpper();

            if (objVersandZul.SelKreis.Length > 0)
            {
                objVersandZul.getBestLieferant(Session["AppID"].ToString(), Session.SessionID, this);
                if (objVersandZul.Status > 0)
                {
                    ddlLief.Items.Clear();
                    lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste!";
                    Session["objNacherf"] = objVersandZul;
                }
                else
                {
                    ddlLief.DataSource = objVersandZul.BestLieferanten;
                    ddlLief.DataValueField = "LIFNR";
                    ddlLief.DataTextField = "NAME1";
                    ddlLief.DataBind();
                    ddlLief.SelectedValue = "0";
                    Session["objNacherf"] = objVersandZul;
                }

            }
            else
            {
                ddlLief.Items.Clear();
            }
            ddlStVa.Focus();
        }

    }
}