﻿using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using AppZulassungsdienst.lib;
using System.Data;
using CKG.Base.Kernel.DocumentGeneration;
using System.Collections;
using System.Configuration;
using GeneralTools.Models;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Eingabedialog Seite2 Vorerfassuung Versanzulassung erfasst durch Autohaus.
    /// </summary>
    public partial class AHVersandChange_2 : Page
    {
        private User m_User;
        private NacherfZLD objNacherf;
        private ZLDCommon objCommon;
        private Report99 objSuche;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// Form füllen.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"].ToString() + "'")[0]["AppFriendlyName"];

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
            if (!IsPostBack)
            {
                if (Session["objNacherf"] != null)
                {
                    objNacherf = (NacherfZLD)Session["objNacherf"];

                    FillZulUnterlagen();
                    fillForm();
                }
                else
                {
                    lblError.Text = "Fehler: Keine Session-Daten übertragen!";
                }
            }
        }

        /// <summary>
        /// Selektieren der zuständigen Zulassungsdienste.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnStamm_Click(object sender, EventArgs e)
        {
            if (objNacherf.BestLieferanten == null || objNacherf.BestLieferanten.Rows.Count == 0)
                return;

            DataRow[] SelRow = objNacherf.BestLieferanten.Select("LIFNR = '" + ddlLief.SelectedValue + "'");
            if (SelRow.Length > 0)
            {
                lblName.Text = SelRow[0]["NAME1"].ToString() + " " + SelRow[0]["NAME2"].ToString();
                lblStreet.Text = SelRow[0]["STREET"].ToString() + " " + SelRow[0]["HOUSE_NUM1"].ToString();
                lblPLZOrt.Text = SelRow[0]["POST_CODE1"].ToString() + " " + SelRow[0]["CITY1"].ToString();
                lblTel.Text = SelRow[0]["TEL_NUMBER"].ToString() + " " + SelRow[0]["TEL_EXTENS"].ToString();
            }
            divOptions.Visible = true;
            divOptions.Style["height"] = "200";
            divBackDisabled.Visible = true;
        }

        /// <summary>
        /// Senden des Vorganges an SAP. PDF-Erstellung Auftrag/Vorgang.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            ClearErrorBorderColor();
            CheckPrintFields();

            if (!String.IsNullOrEmpty(lblError.Text))
                return;

            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            kopfdaten.FrachtbriefNrZurueck = txtFrachtBack.Text;
            kopfdaten.FrachtbriefNrHin = txtFrachtHin.Text;

            kopfdaten.LieferantenNr = (ddlLief.SelectedValue ?? "");

            objNacherf.IsZLD = false;

            // Prüfung ob es sich bei dem ausgewählten Lieferanten um einen
            // Kroschke Zulassungsdienst handelt.
            if (kopfdaten.LieferantenNr.NotNullOrEmpty().Length >= 2 && kopfdaten.LieferantenNr.TrimStart('0').Substring(0, 2) == "56")
                objNacherf.CheckLieferant();

            //Daten senden.
            objNacherf.SaveAHVersandVorgangToSap(m_User.UserName);

            if (objNacherf.ErrorOccured)
            {
                lblError.Text = "Daten konnten nicht in SAP gespeichert werden!" + objNacherf.Message;
                return;
            }

            Hashtable imageHt = new Hashtable();
            DataTable tblWordData = CreatePrintTable();
            imageHt.Add("Logo", m_User.Customer.LogoImage);
            String sFilePath = "C:\\inetpub\\wwwroot\\Portalzld\\temp\\Excel\\" + m_User.UserName + String.Format("{0:ddMMyyhhmmss}", DateTime.Now);
            WordDocumentFactory docFactory = new WordDocumentFactory(tblWordData, imageHt);
            if (!String.IsNullOrEmpty(objNacherf.DocRueck1))
            {
                docFactory.CreateDocumentAndSave(sFilePath, this.Page, "Applications\\AppZulassungsdienst\\Documents\\ZulassungAbwAdresse.doc");
            }
            else
            {
                docFactory.CreateDocumentAndSave(sFilePath, this.Page, "Applications\\AppZulassungsdienst\\Documents\\ZulassungDEZ.doc");
            }

            Session["App_ContentType"] = "Application/pdf";
            Session["App_Filepath"] = sFilePath + ".pdf";

            ResponseHelper.Redirect("Printpdf.aspx", "_blank", "left=0,top=0,resizable=YES,scrollbars=YES");

            String sZULBELN = Request.QueryString["id"];
            DataRow[] DelRow = objNacherf.AHVersandListe.Select("ZULBELN='" + sZULBELN + "'");
            if (DelRow.Length > 0)
            {
                foreach (DataRow rToDel in DelRow)
                {
                    objNacherf.AHVersandListe.Rows.Remove(rToDel);
                }
            }

            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#269700");
            lblMessage.Text = "Datensätze in SAP gespeichert. Keine Fehler aufgetreten.";
            lbtnErfassen.Visible = true;
            cmdCreate.Visible = false;
            lb_zurueck.Visible = false;
            Sendmail();
        }

        /// <summary>
        /// Anzeige der Daten des Zulassungsdiensten/ ext. Diensleister
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnCloseOption_Click(object sender, EventArgs e)
        {
            divOptions.Visible = false;
            divBackDisabled.Visible = false;
        }

        /// <summary>
        /// Zurück zum Eingabedialog Seite 1.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("AHVersandChange.aspx?AppID=" + Session["AppID"].ToString() + "&ID=" + Request.QueryString["id"] + "&Back=X");
        }

        /// <summary>
        /// Zurück zur Listenansicht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnErfassen_Click(object sender, EventArgs e)
        {
            Response.Redirect("AHVersandListe.aspx?AppID=" + Session["AppID"].ToString());
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkHandRegist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkHandRegist.Items[0];
            ListItem LiKop = chkHandRegist.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkHandRegist$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkHandRegist$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Gewerbe_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = Gewerbe.Items[0];
            ListItem LiKop = Gewerbe.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$Gewerbe$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$Gewerbe$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkPerso_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkPerso.Items[0];
            ListItem LiKop = chkPerso.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkPerso$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkPerso$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkReisepass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkReisepass.Items[0];
            ListItem LiKop = chkReisepass.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkReisepass$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkReisepass$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkZulVoll_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkZulVoll.Items[0];
            ListItem LiKop = chkZulVoll.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulVoll$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulVoll$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkEinzug_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkEinzug.Items[0];
            ListItem LiKop = chkEinzug.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkEinzug$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkEinzug$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkevB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkevB.Items[0];
            ListItem LiKop = chkevB.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkevB$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkevB$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkZulBeschein1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkZulBeschein1.Items[0];
            ListItem LiKop = chkZulBeschein1.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulBeschein1$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulBeschein1$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkZulBeschein2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkZulBeschein2.Items[0];
            ListItem LiKop = chkZulBeschein2.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulBeschein2$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkZulBeschein2$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkCoC_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkCoC.Items[0];
            ListItem LiKop = chkCoC.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkCoC$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkCoC$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkHU_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkHU.Items[0];
            ListItem LiKop = chkHU.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkHU$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkHU$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkAU_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkAU.Items[0];
            ListItem LiKop = chkAU.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkAU$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkAU$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkFrei1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkFrei1.Items[0];
            ListItem LiKop = chkFrei1.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkFrei1$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkFrei1$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Es darf jeweils nur Orginal oder Kopie bei den Dokumentenanforderungen markiert sein.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void chkFrei2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem LiOrg = chkFrei2.Items[0];
            ListItem LiKop = chkFrei2.Items[1];

            String ControlFired = Request.Form["__EVENTTARGET"].ToString();

            if (ControlFired == "ctl00$ContentPlaceHolder1$chkFrei2$0")
            {
                if (LiOrg.Selected)
                {
                    LiKop.Selected = false;
                }
            }
            if (ControlFired == "ctl00$ContentPlaceHolder1$chkFrei2$1")
            {
                if (LiKop.Selected)
                {
                    LiOrg.Selected = false;
                }
            }
        }

        /// <summary>
        /// Öffnen des Eingabedialogs Adresse Hinsendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnAdresseHin_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsopenkDialog",
                        "openDialogAndBlock('AdresseHin');", true);
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsUnblockDialog",
                                    "unblockDialog('AdresseHin');", true);

        }

        /// <summary>
        /// Refresh der Adresse der Hinsendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnRefreshAdressHin_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Schliessen des Eingabedialogs Adresse Hinsendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCloseDialog_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog('AdresseHin');", true);
        }

        /// <summary>
        /// Refresh der Adresse der Rücksendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnRefreshAdresseRueck_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Schliessen der Eingabedialogs Adresse Rücksendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCloseDialogRueck_Click(object sender, EventArgs e)
        {
            if (lblAdrRueckError.Text != "")
            {
                lblAdrRueckError.Text = "Bitte überprüfen Sie folgende Meldung: " + lblAdrRueckError.Text;
            }
            else
            {
                ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog('');", true);
            }
        }

        /// <summary>
        /// Öffnen des Eingabedialogs Adresse Rücksendung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lbtnAdresseRueck_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsopenkDialog",
               "openDialogAndBlock('');", true);
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsUnblockDialog",
                                    "unblockDialog('');", true);

        }

        /// <summary>
        /// Speichern der Adressdaten Hinsendung in den Klasseneigenschaften.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSaveAdrHin_Click(object sender, EventArgs e)
        {
            if (CheckAdrhin())
            {
                lblAdrHinError.Text = "Überprüfen Sie Ihre Eingaben!";
                return;
            }

            objNacherf.Name1Hin = txtNameHin1.Text;
            objNacherf.Name2Hin = txtNameHin2.Text;
            objNacherf.StrasseHin = txtStrasseHin.Text;
            objNacherf.PLZHin = txtPlz.Text;
            objNacherf.OrtHin = txtOrt.Text;
            txtNameHin1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasseHin.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            Session["objNacherf"] = objNacherf;
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog('AdresseHin');", true);
        }

        /// <summary>
        /// Speichern der Adressdaten Rücksendung in den Klasseneigenschaften.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            lblAdrRueckError.Text = "";
            if (CheckAdr1Rueck())
            {
                lblAdrRueckError.Text = "Überprüfen Sie Ihre Eingaben!";
                return;
            }
            objNacherf.DocRueck1 = txtDocRueck1.Text;
            objNacherf.NameRueck1 = txtNameRueck1.Text;
            objNacherf.NameRueck2 = txtNameRueck2.Text;
            objNacherf.StrasseRueck = txtStrasseRueck.Text;
            objNacherf.PLZRueck = txtPLZRueck.Text;
            objNacherf.OrtRueck = txtOrtRueck.Text;
            txtNameRueck1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasseRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPLZRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrtRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            if (CheckAdr2Rueck())
            {
                lblAdrRueckError.Text = "Überprüfen Sie Ihre Eingaben!";
                return;
            }
            objNacherf.Doc2Rueck = txtDoc2Rueck.Text;
            if (objNacherf.DocRueck1.Length == 0 && objNacherf.Doc2Rueck.Length > 0)
            {
                objNacherf.Doc2Rueck = "";
                lblAdrRueckError.Text = "Bei nur einer Rücksendeadresse bitte die ersten Felder benutzen!";
                return;
            }
            objNacherf.Name1Rueck2 = txtName1Rueck2.Text;
            objNacherf.Name2Rueck2 = txtName2Rueck2.Text;
            objNacherf.Strasse2Rueck = txtStrasse2Rueck.Text;
            objNacherf.PLZ2Rueck = txtPLZ2Rueck.Text;
            objNacherf.Ort2Rueck = txtOrt2Rueck.Text;
            txtName1Rueck2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasse2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPLZ2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            Session["objNacherf"] = objNacherf;
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "jsCloseDialg", "closeDialog('');", true);
        }

        /// <summary>
        /// Clearen der Felder und Klasseneigenschaften der Hinsendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSetBackHin_Click(object sender, EventArgs e)
        {
            txtNameHin1.Text = "";
            txtNameHin2.Text = "";
            txtStrasseHin.Text = "";
            txtPlz.Text = "";
            txtOrt.Text = "";
            objNacherf.Name1Hin = txtNameHin1.Text;
            objNacherf.Name2Hin = txtNameHin2.Text;
            objNacherf.StrasseHin = txtStrasseHin.Text;
            objNacherf.PLZHin = txtPlz.Text;
            objNacherf.OrtHin = txtOrt.Text;
            txtNameHin1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasseHin.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            Session["objNacherf"] = objNacherf;
        }

        /// <summary>
        /// Clearen der Felder und Klasseneigenschaften der Rücksendung.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdSetBackRueck_Click(object sender, EventArgs e)
        {
            lblAdrRueckError.Text = "";
            txtDocRueck1.Text = "";
            txtNameRueck1.Text = "";
            txtNameRueck2.Text = "";
            txtStrasseRueck.Text = "";
            txtPLZRueck.Text = "";
            txtOrtRueck.Text = "";
            objNacherf.DocRueck1 = txtDocRueck1.Text;
            objNacherf.NameRueck1 = txtNameRueck1.Text;
            objNacherf.NameRueck2 = txtNameRueck2.Text;
            objNacherf.StrasseRueck = txtStrasseRueck.Text;
            objNacherf.PLZRueck = txtPLZRueck.Text;
            objNacherf.OrtRueck = txtOrtRueck.Text;
            txtNameRueck1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasseRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPLZRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrtRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");

            txtDoc2Rueck.Text = "";
            txtName1Rueck2.Text = "";
            txtName2Rueck2.Text = "";
            txtStrasse2Rueck.Text = "";
            txtPLZ2Rueck.Text = "";
            txtOrt2Rueck.Text = "";
            objNacherf.Doc2Rueck = txtDoc2Rueck.Text;
            objNacherf.Name1Rueck2 = txtName1Rueck2.Text;
            objNacherf.Name2Rueck2 = txtName2Rueck2.Text;
            objNacherf.Strasse2Rueck = txtStrasse2Rueck.Text;
            objNacherf.PLZ2Rueck = txtPLZ2Rueck.Text;
            objNacherf.Ort2Rueck = txtOrt2Rueck.Text;
            txtName1Rueck2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtStrasse2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPLZ2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtOrt2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            Session["objNacherf"] = objNacherf;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Geforderte Zulassungsunterlagen des Amtes selektieren und checkboxes füllen.
        /// </summary>
        private void FillZulUnterlagen()
        {
            lblError.Text = "";
            objSuche = new Report99();

            if (!String.IsNullOrEmpty(objNacherf.AktuellerVorgang.Kopfdaten.Kennzeichen))
            {
                string tmpKennz1;
                string tmpKennz2;
                ZLDCommon.KennzeichenAufteilen(objNacherf.AktuellerVorgang.Kopfdaten.Kennzeichen, out tmpKennz1, out tmpKennz2);

                if (!String.IsNullOrEmpty(tmpKennz1))
                {
                    objSuche.PKennzeichen = tmpKennz1;
                }
            }

            objSuche.Fill();

            Session["objSuche"] = objSuche;

            if (objSuche.ErrorOccured)
            {
                lblError.Text = "Fehler: " + objSuche.Message;
            }
            else if (objSuche.tblResult.Rows.Count == 0)
            {
                lblError.Text = "Fehler bei der Vorbelegung der Zulassungsunterlagen!";
            }
            else
            {
                DataRow resultRow;

                if (objSuche.tblResult.Rows.Count == 1)
                {
                    resultRow = objSuche.tblResult.Rows[0];
                }
                else
                {
                    DataRow[] SelectRow = objSuche.tblResult.Select("Zkba2='00'");
                    if (SelectRow.Length > 0)
                    {
                        resultRow = SelectRow[0];
                    }
                    else
                    {
                        return;
                    }
                }

                FillZulUnterlagen(resultRow["PZUL_HANDEL"].ToString(), chkHandRegist);
                FillZulUnterlagen(resultRow["PZUL_GEWERB"].ToString(), Gewerbe);
                FillZulUnterlagen(resultRow["PZUL_AUSW"].ToString(), chkPerso);
                FillZulUnterlagen(resultRow["PZUL_VOLLM"].ToString(), chkZulVoll);
                FillZulUnterlagen(resultRow["PZUL_LAST"].ToString(), chkEinzug);
                FillZulUnterlagen(resultRow["PZUL_SCHEIN"].ToString(), chkZulBeschein1);
                FillZulUnterlagen(resultRow["PZUL_BRIEF"].ToString(), chkZulBeschein2);
                FillZulUnterlagen(resultRow["PZUL_COC"].ToString(), chkCoC);
            }
        }

        /// <summary>
        /// Geforderte Zulassungsunterlagen des Amte den checkboxes zuweisen.
        /// </summary>
        /// <param name="Kopie_Org">Kopie oder Orginal</param>
        /// <param name="ChkList">ListCheckbox</param>
        private void FillZulUnterlagen(String Kopie_Org, CheckBoxList ChkList)
        {
            switch (Kopie_Org)
            {
                case "O":
                    ChkList.SelectedIndex = 0;
                    break;
                case "K":
                    ChkList.SelectedIndex = 1;
                    break;
                default:
                    ChkList.SelectedIndex = -1;
                    break;
            }
        }

        /// <summary>
        /// Form mit den bereits vorhandenen Daten füllen.
        /// </summary>
        private void fillForm()
        {
            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            txtZLDLief.Text = kopfdaten.Landkreis;
            txtKunde.Text = kopfdaten.KundenNr;

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
            if (kunde != null)
                txtKundeName.Text = kunde.Name1;

            txtKennzWunsch.Text = kopfdaten.Kennzeichen;
            txtReserviertNr.Text = kopfdaten.ReserviertesKennzeichen;
            chkKennzWunsch.Checked = kopfdaten.Wunschkennzeichen.IsTrue();
            chkReserviert.Checked = kopfdaten.KennzeichenReservieren.IsTrue();
            txtEVB.Text = kopfdaten.EvbNr;
            txtEVB.Attributes.Add("onkeyup", "FilterKennz(this,event)");
            objCommon.getFilialadresse();
            if (objNacherf.ErrorOccured)
            {
                lblError.Text = objNacherf.Message;
            }
            objNacherf.getBestLieferant(kopfdaten.Landkreis);
            if (objNacherf.ErrorOccured)
            {
                lblError.Text = "Fehler beim Laden der Lieferanten/Zulassungsdienste! Es kann keine Versandzulassung abgesendet werden!";

                Session["objNacherf"] = objNacherf;
                cmdCreate.Enabled = false;
                return;
            }

            ddlLief.DataSource = objNacherf.BestLieferanten;
            ddlLief.DataValueField = "LIFNR";
            ddlLief.DataTextField = "NAME1";
            ddlLief.DataBind();
            Session["objNacherf"] = objNacherf;

            InitializeAdressen();
        }

        /// <summary>
        /// Prüfen ob Felder für das PDF gefüllt sind.
        /// </summary>
        private void CheckPrintFields()
        {
            Boolean bError = false;

            if (chkevB.Items[0].Selected || chkevB.Items[1].Selected)
            {
                if (String.IsNullOrEmpty(txtEVB.Text))
                {
                    txtEVB.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }

            if (chkFrei1.Items[0].Selected || chkFrei1.Items[1].Selected)
            {
                if (String.IsNullOrEmpty(txtFrei1.Text))
                {
                    txtFrei1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }

            if (chkFrei2.Items[0].Selected || chkFrei2.Items[1].Selected)
            {
                if (String.IsNullOrEmpty(txtFrei2.Text))
                {
                    txtFrei2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }

            if (chkFrei3.Checked)
            {
                if (String.IsNullOrEmpty(txtFrei3.Text))
                {
                    txtFrei3.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }
            if (bError)
            {
                lblError.Text = "Bitte ergänzen Sie die rot markierten Felder!";
            }
        }

        /// <summary>
        /// Fehlerstyle der Controls entfernen.
        /// </summary>
        private void ClearErrorBorderColor()
        {
            txtHandelsregFa.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtGewerb.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtPersoName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtReisepass.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtEVB.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtFrei1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtFrei2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
            txtFrei3.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
        }

        /// <summary>
        /// Tabellenspalten für die PDF-Generierung erstellen.
        /// </summary>
        /// <param name="tblWordData">Tabelle PDF</param>
        private void CreateColumsPrintTable(ref DataTable tblWordData)
        {
            tblWordData.Columns.Add("KreisKennz", typeof(String));
            tblWordData.Columns.Add("Kunnr", typeof(String));
            tblWordData.Columns.Add("Name1", typeof(String));
            tblWordData.Columns.Add("KreisBez", typeof(String));
            tblWordData.Columns.Add("Reserviert", typeof(Boolean));
            tblWordData.Columns.Add("Feinstaub", typeof(String));
            tblWordData.Columns.Add("Kennzeichen", typeof(String));
            tblWordData.Columns.Add("RNr", typeof(String));
            tblWordData.Columns.Add("WunschKennz", typeof(Boolean));
            tblWordData.Columns.Add("EinKennz", typeof(Boolean));
            tblWordData.Columns.Add("HandRegistOrg", typeof(Boolean));
            tblWordData.Columns.Add("HandRegistKopie", typeof(Boolean));
            tblWordData.Columns.Add("GewerbeOrg", typeof(Boolean));
            tblWordData.Columns.Add("GewerbeKopie", typeof(Boolean));
            tblWordData.Columns.Add("PersoOrg", typeof(Boolean));
            tblWordData.Columns.Add("PersoKopie", typeof(Boolean));
            tblWordData.Columns.Add("Anzahl", typeof(String));
            tblWordData.Columns.Add("ReisepassOrg", typeof(Boolean));
            tblWordData.Columns.Add("ReisepassKopie", typeof(Boolean));
            tblWordData.Columns.Add("ZulVollOrg", typeof(Boolean));
            tblWordData.Columns.Add("ZulVollKopie", typeof(Boolean));

            tblWordData.Columns.Add("EinzugOrg", typeof(Boolean));
            tblWordData.Columns.Add("EinzugKopie", typeof(Boolean));

            tblWordData.Columns.Add("eVBOrg", typeof(Boolean));
            tblWordData.Columns.Add("eVBKopie", typeof(Boolean));

            tblWordData.Columns.Add("ZulBeschein1Org", typeof(Boolean));
            tblWordData.Columns.Add("ZulBeschein1Kopie", typeof(Boolean));

            tblWordData.Columns.Add("ZulBeschein2Org", typeof(Boolean));
            tblWordData.Columns.Add("ZulBeschein2Kopie", typeof(Boolean));

            tblWordData.Columns.Add("CoCOrg", typeof(Boolean));
            tblWordData.Columns.Add("CoCKopie", typeof(Boolean));

            tblWordData.Columns.Add("HUOrg", typeof(Boolean));
            tblWordData.Columns.Add("HUKopie", typeof(Boolean));

            tblWordData.Columns.Add("AUOrg", typeof(Boolean));
            tblWordData.Columns.Add("AUKopie", typeof(Boolean));

            tblWordData.Columns.Add("Frei1Org", typeof(Boolean));
            tblWordData.Columns.Add("Frei1Kopie", typeof(Boolean));

            tblWordData.Columns.Add("Frei2Org", typeof(Boolean));
            tblWordData.Columns.Add("Frei2Kopie", typeof(Boolean));
            tblWordData.Columns.Add("Frei1Text", typeof(String));
            tblWordData.Columns.Add("Frei2Text", typeof(String));
            tblWordData.Columns.Add("Frei3Text", typeof(String));
            tblWordData.Columns.Add("KennzJa_Nein", typeof(Boolean));
            tblWordData.Columns.Add("KopieGebuehr", typeof(Boolean));
            tblWordData.Columns.Add("Postexpress", typeof(Boolean));
            tblWordData.Columns.Add("normPostweg", typeof(Boolean));
            tblWordData.Columns.Add("FrachtBack", typeof(String));
            tblWordData.Columns.Add("FrachtHin", typeof(String));

            tblWordData.Columns.Add("HandelsregFa", typeof(String));
            tblWordData.Columns.Add("Gewerb", typeof(String));
            tblWordData.Columns.Add("PersoName", typeof(String));
            tblWordData.Columns.Add("Reisepass", typeof(String));
            tblWordData.Columns.Add("EVB", typeof(String));

            tblWordData.Columns.Add("KstLief", typeof(String));
            tblWordData.Columns.Add("Name1Lief", typeof(String));
            tblWordData.Columns.Add("Name2Lief", typeof(String));
            tblWordData.Columns.Add("StrasseLief", typeof(String));
            tblWordData.Columns.Add("OrtLief", typeof(String));
            tblWordData.Columns.Add("TelLief", typeof(String));
            tblWordData.Columns.Add("FaxLief", typeof(String));

            tblWordData.Columns.Add("KstFil", typeof(String));
            tblWordData.Columns.Add("Name1Fil", typeof(String));
            tblWordData.Columns.Add("Name2Fil", typeof(String));
            tblWordData.Columns.Add("StrasseFil", typeof(String));
            tblWordData.Columns.Add("OrtFil", typeof(String));
            tblWordData.Columns.Add("TelFil", typeof(String));
            tblWordData.Columns.Add("FaxFil", typeof(String));

            tblWordData.Columns.Add("Frei3", typeof(Boolean));
            tblWordData.Columns.Add("ID", typeof(String));
            tblWordData.Columns.Add("KennzSonder", typeof(String));
            tblWordData.Columns.Add("Referenz1", typeof(String));
            tblWordData.Columns.Add("Referenz2", typeof(String));
            tblWordData.Columns.Add("ErfDatum", typeof(String));
            tblWordData.Columns.Add("Zuldat", typeof(String));
            tblWordData.Columns.Add("Bemerkung", typeof(String));

            // Adressen für Rücksendung

            tblWordData.Columns.Add("DocRueck", typeof(String));
            tblWordData.Columns.Add("RueckName1", typeof(String));
            tblWordData.Columns.Add("RueckName2", typeof(String));
            tblWordData.Columns.Add("RueckStrasse", typeof(String));
            tblWordData.Columns.Add("RueckOrtPLZ", typeof(String));

            tblWordData.Columns.Add("DocRueck2", typeof(String));
            tblWordData.Columns.Add("Rueck2Name1", typeof(String));
            tblWordData.Columns.Add("Rueck2Name2", typeof(String));
            tblWordData.Columns.Add("Rueck2Strasse", typeof(String));
            tblWordData.Columns.Add("Rueck2OrtPLZ", typeof(String));
        }

        /// <summary>
        /// Tabellenwerte für die PDF-Generierung einfügen.
        /// </summary>
        /// <returns>Tabelle PDF</returns>
        private DataTable CreatePrintTable()
        {
            var kopfdaten = objNacherf.AktuellerVorgang.Kopfdaten;

            DataTable tblWordData = new DataTable();
            CreateColumsPrintTable(ref tblWordData);
            DataRow dRow = tblWordData.NewRow();

            dRow["KreisKennz"] = kopfdaten.Landkreis;
            dRow["Kunnr"] = kopfdaten.KundenNr;

            var kunde = objCommon.KundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
            if (kunde != null)
                dRow["NAME1"] = kunde.Name;

            dRow["Reserviert"] = kopfdaten.KennzeichenReservieren.IsTrue();
            dRow["Kennzeichen"] = kopfdaten.Kennzeichen;
            dRow["KennzSonder"] = kopfdaten.Kennzeichenform;
            dRow["WunschKennz"] = kopfdaten.Wunschkennzeichen.IsTrue();
            dRow["RNr"] = kopfdaten.ReserviertesKennzeichen;
            dRow["Feinstaub"] = "";
            dRow["EinKennz"] = kopfdaten.NurEinKennzeichen.IsTrue();
            dRow["Referenz1"] = kopfdaten.Referenz1;
            dRow["Referenz2"] = kopfdaten.Referenz2;
            dRow["Zuldat"] = kopfdaten.Zulassungsdatum.ToString("dd.MM.yyyy");
            dRow["Bemerkung"] = kopfdaten.Bemerkung;
            dRow["ErfDatum"] = DateTime.Now.ToShortDateString();

            dRow["Anzahl"] = "1";
            dRow["HandRegistOrg"] = chkHandRegist.Items[0].Selected;
            dRow["HandRegistKopie"] = chkHandRegist.Items[1].Selected;
            dRow["GewerbeOrg"] = Gewerbe.Items[0].Selected;
            dRow["GewerbeKopie"] = Gewerbe.Items[1].Selected;
            dRow["PersoOrg"] = chkPerso.Items[0].Selected;
            dRow["PersoKopie"] = chkPerso.Items[1].Selected;
            dRow["ReisepassOrg"] = chkReisepass.Items[0].Selected;
            dRow["ReisepassKopie"] = chkReisepass.Items[1].Selected;
            dRow["ZulVollOrg"] = chkZulVoll.Items[0].Selected;
            dRow["ZulVollKopie"] = chkZulVoll.Items[1].Selected;
            dRow["EinzugOrg"] = chkEinzug.Items[0].Selected;
            dRow["EinzugKopie"] = chkEinzug.Items[1].Selected;
            dRow["eVBOrg"] = chkevB.Items[0].Selected;
            dRow["eVBKopie"] = chkevB.Items[1].Selected;
            dRow["ZulBeschein1Org"] = chkZulBeschein1.Items[0].Selected;
            dRow["ZulBeschein1Kopie"] = chkZulBeschein1.Items[1].Selected;
            dRow["ZulBeschein2Org"] = chkZulBeschein2.Items[0].Selected;
            dRow["ZulBeschein2Kopie"] = chkZulBeschein2.Items[1].Selected;
            dRow["CoCOrg"] = chkCoC.Items[0].Selected;
            dRow["CoCKopie"] = chkCoC.Items[1].Selected;
            dRow["HUOrg"] = chkHU.Items[0].Selected;
            dRow["HUKopie"] = chkHU.Items[1].Selected;
            dRow["AUOrg"] = chkAU.Items[0].Selected;
            dRow["AUKopie"] = chkAU.Items[1].Selected;

            dRow["Frei1Org"] = chkFrei1.Items[0].Selected;
            dRow["Frei1Kopie"] = chkFrei1.Items[1].Selected;
            dRow["Frei2Org"] = chkFrei2.Items[0].Selected;
            dRow["Frei2Kopie"] = chkFrei2.Items[1].Selected;
            dRow["Frei3"] = chkFrei3.Checked;
            dRow["Frei3Text"] = txtFrei3.Text;
            dRow["Frei1Text"] = txtFrei1.Text;
            dRow["Frei2Text"] = txtFrei2.Text;
            dRow["KennzJa_Nein"] = chkKennzeichen.Checked;
            dRow["KopieGebuehr"] = chkkopie.Checked;
            dRow["Postexpress"] = rbPostexpress.Checked;
            dRow["normPostweg"] = rbNormPost.Checked;
            dRow["FrachtBack"] = txtFrachtBack.Text;
            dRow["FrachtHin"] = txtFrachtHin.Text;

            dRow["HandelsregFa"] = txtHandelsregFa.Text;
            dRow["Gewerb"] = txtGewerb.Text;
            dRow["PersoName"] = txtPersoName.Text;
            dRow["Reisepass"] = txtReisepass.Text;
            dRow["EVB"] = txtEVB.Text;

            var amt = objCommon.StvaStamm.FirstOrDefault(s => s.Landkreis == kopfdaten.Landkreis);
            dRow["Kreisbez"] = (amt != null ? amt.KreisBezeichnung : kopfdaten.KreisBezeichnung);

            dRow["ID"] = kopfdaten.SapId;

            DataRow[] SelRow = objNacherf.BestLieferanten.Select("LIFNR = '" + ddlLief.SelectedValue + "'");
            if (SelRow.Length > 0)
            {
                if (ddlLief.SelectedValue.TrimStart('0').Substring(0, 2) == "56")
                {
                    dRow["KstLief"] = "Kst. " + ddlLief.SelectedValue.TrimStart('0').Substring(2, 4);
                }
                else
                {
                    dRow["KstLief"] = "Kst. " + ddlLief.SelectedValue.TrimStart('0');
                }
                if (!String.IsNullOrEmpty(objNacherf.Name1Hin))
                {
                    dRow["Name1Lief"] = objNacherf.Name1Hin;
                    dRow["Name2Lief"] = objNacherf.Name2Hin;
                    dRow["StrasseLief"] = objNacherf.StrasseHin;
                    dRow["OrtLief"] = objNacherf.PLZHin + " " + objNacherf.OrtHin;
                    dRow["TelLief"] = "";
                    dRow["FaxLief"] = "";
                }
                else
                {
                    dRow["Name1Lief"] = SelRow[0]["NAME1"].ToString();
                    dRow["Name2Lief"] = SelRow[0]["NAME2"].ToString();
                    dRow["StrasseLief"] = SelRow[0]["STREET"].ToString() + " " + SelRow[0]["HOUSE_NUM1"].ToString();
                    dRow["OrtLief"] = SelRow[0]["POST_CODE1"].ToString() + " " + SelRow[0]["CITY1"].ToString();
                    dRow["TelLief"] = SelRow[0]["TEL_NUMBER"].ToString() + " " + SelRow[0]["TEL_EXTENS"].ToString();
                    dRow["FaxLief"] = SelRow[0]["FAX_NUMBER"].ToString() + " " + SelRow[0]["FAX_EXTENS"].ToString();
                }

                if (objCommon.AdresseFiliale != null)
                {
                    var filAdr = objCommon.AdresseFiliale;

                    dRow["KstFil"] = "Kst. " + objNacherf.VKBUR;
                    dRow["Name1Fil"] = filAdr.Name1;
                    dRow["Name2Fil"] = filAdr.Name2;
                    dRow["StrasseFil"] = filAdr.Strasse + " " + filAdr.HausNr;
                    dRow["OrtFil"] = filAdr.Plz + " " + filAdr.Ort;
                    dRow["TelFil"] = filAdr.TelefonNr;
                    dRow["FaxFil"] = filAdr.FaxNr;
                }

                if (!String.IsNullOrEmpty(objNacherf.DocRueck1))
                {
                    dRow["DocRueck"] = objNacherf.DocRueck1 + " an:";
                    dRow["RueckName1"] = objNacherf.NameRueck1;
                    dRow["RueckName2"] = objNacherf.NameRueck2;
                    dRow["RueckStrasse"] = objNacherf.StrasseRueck;
                    dRow["RueckOrtPLZ"] = objNacherf.PLZRueck + " " + objNacherf.OrtRueck;
                }

                if (!String.IsNullOrEmpty(objNacherf.Doc2Rueck))
                {
                    dRow["DocRueck2"] = objNacherf.Doc2Rueck + " an:";
                    dRow["Rueck2Name1"] = objNacherf.Name1Rueck2;
                    dRow["Rueck2Name2"] = objNacherf.Name2Rueck2;
                    dRow["Rueck2Strasse"] = objNacherf.Strasse2Rueck;
                    dRow["Rueck2OrtPLZ"] = objNacherf.PLZ2Rueck + " " + objNacherf.Ort2Rueck;
                }

                tblWordData.Rows.Add(dRow);

            }
            return tblWordData;
        }

        /// <summary>
        /// E-Mail an die durchführenden Zulassungsdienst generieren und senden.
        /// </summary>
        private void Sendmail()
        {
            try
            {
                String MailAdress = "";
                DataRow[] SelRow = objNacherf.BestLieferanten.Select("LIFNR = '" + ddlLief.SelectedValue + "'");
                if (SelRow.Length > 0)
                {
                    MailAdress = SelRow[0]["SMTP_ADDR"].ToString();
                }

                if (MailAdress.Length == 0)
                {
                    lblError.Text = "Für den zuständigen Zulassungsdienst wurde keine E-Mailadresse hinterlegt. <br /> Bitte informieren Sie den Zulassungsdienst telefonisch! <br />";
                    return;
                }

                var kreisText = objNacherf.AktuellerVorgang.Kopfdaten.Landkreis + "..." + objNacherf.AktuellerVorgang.Kopfdaten.KreisBezeichnung;

                System.Net.Mail.MailMessage Mail;

                String smtpMailSender = ConfigurationManager.AppSettings["SmtpMailSender"];
                String smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];

                String MailText = "Sehr geehrte Damen und Herren, " + Environment.NewLine + Environment.NewLine;
                MailText += "hiermit teilen wir Ihnen mit, dass Sie morgen von uns einen Zulassungsvorgang " + Environment.NewLine;
                MailText += "für den Kreis " + kreisText + " per Versand erhalten." + Environment.NewLine;
                MailText += "Wir möchten Sie bitten dies bei Ihrer Planung zu berücksichtigen." + Environment.NewLine;
                MailText += "Alle weiteren Informationen entnehmen Sie bitte den zugesandten Unterlagen." + Environment.NewLine;
                MailText += "Vielen Dank." + Environment.NewLine + Environment.NewLine;
                MailText += "Mit freundlichen Grüßen," + Environment.NewLine + Environment.NewLine;

                if (objCommon.AdresseFiliale != null)
                {
                    var filAdr = objCommon.AdresseFiliale;

                    MailText += filAdr.Name1 + Environment.NewLine;
                    MailText += filAdr.Name2 + Environment.NewLine;
                    MailText += filAdr.Strasse + " " + filAdr.HausNr + Environment.NewLine;
                    MailText += filAdr.Plz + " " + filAdr.Ort + Environment.NewLine;
                    MailText += filAdr.TelefonNr;
                }

                Mail = new System.Net.Mail.MailMessage(smtpMailSender, MailAdress, "Versandzulassungen für den Kreis " + kreisText, MailText);
                Mail.IsBodyHtml = false;
                Mail.BodyEncoding = System.Text.Encoding.Default;
                Mail.SubjectEncoding = System.Text.Encoding.Default;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                client.Send(Mail);
                Mail.Attachments.Dispose();
                Mail.Dispose();
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Senden! " + ex.Message;
            }
        }

        /// <summary>
        /// Klassen-Eigenschaften initialisieren(Adressen).
        /// </summary>
        private void InitializeAdressen()
        {
            objNacherf.Name1Hin = "";
            objNacherf.Name2Hin = "";
            objNacherf.StrasseHin = "";
            objNacherf.PLZHin = "";
            objNacherf.OrtHin = "";
            objNacherf.DocRueck1 = "";
            objNacherf.NameRueck1 = "";
            objNacherf.NameRueck2 = "";
            objNacherf.StrasseRueck = "";
            objNacherf.PLZRueck = "";
            objNacherf.OrtRueck = "";
            objNacherf.Doc2Rueck = "";
            objNacherf.Name1Rueck2 = "";
            objNacherf.Name2Rueck2 = "";
            objNacherf.Strasse2Rueck = "";
            objNacherf.PLZ2Rueck = "";
            objNacherf.Ort2Rueck = "";
        }

        /// <summary>
        /// Validierung Adressdaten Hinsendung.
        /// </summary>
        /// <returns>true bei Fehler</returns>
        private bool CheckAdrhin()
        {
            Boolean bError = false;

            if (String.IsNullOrEmpty(txtNameHin1.Text))
            {
                txtNameHin1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bError = true;
            }
            if (String.IsNullOrEmpty(txtStrasseHin.Text))
            {
                txtStrasseHin.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bError = true;
            }
            else if (txtStrasseHin.Text.Contains("xxx") || txtStrasseHin.Text.Contains("XXX"))
            {
                txtStrasseHin.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bError = true;
            }

            if (String.IsNullOrEmpty(txtPlz.Text) || txtPlz.Text.Length < 5)
            {
                txtPlz.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bError = true;
            }

            if (String.IsNullOrEmpty(txtOrt.Text))
            {
                txtOrt.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                bError = true;
            }

            return bError;
        }

        /// <summary>
        /// Validierung Adressdaten1 Rücksendung.
        /// </summary>
        /// <returns>true bei Fehler</returns>
        private bool CheckAdr1Rueck()
        {
            Boolean bError = false;

            if (!String.IsNullOrEmpty(txtDocRueck1.Text))
            {
                if (String.IsNullOrEmpty(txtNameRueck1.Text))
                {
                    txtNameRueck1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
                if (String.IsNullOrEmpty(txtStrasseRueck.Text))
                {
                    txtStrasseRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
                else if (txtStrasseRueck.Text.Contains("xxx") || txtStrasseRueck.Text.Contains("XXX"))
                {
                    txtStrasseRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }

                if (String.IsNullOrEmpty(txtPLZRueck.Text) || txtPLZRueck.Text.Length < 5)
                {
                    txtPLZRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }

                if (String.IsNullOrEmpty(txtOrtRueck.Text))
                {
                    txtOrtRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }

            return bError;
        }

        /// <summary>
        /// Validierung Adressdaten2 Rücksendung.
        /// </summary>
        /// <returns>true bei Fehler</returns>
        private bool CheckAdr2Rueck()
        {
            Boolean bError = false;
            if (!String.IsNullOrEmpty(txtDoc2Rueck.Text))
            {
                if (String.IsNullOrEmpty(txtName1Rueck2.Text))
                {
                    txtName1Rueck2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
                if (String.IsNullOrEmpty(txtStrasse2Rueck.Text))
                {
                    txtStrasse2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
                else if (txtStrasse2Rueck.Text.Contains("xxx") || txtStrasse2Rueck.Text.Contains("XXX"))
                {
                    txtStrasse2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
                if (String.IsNullOrEmpty(txtPLZ2Rueck.Text) || txtPLZ2Rueck.Text.Length < 5)
                {
                    txtPLZ2Rueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }

                if (String.IsNullOrEmpty(txtOrt2Rueck.Text))
                {
                    txtOrtRueck.BorderColor = System.Drawing.ColorTranslator.FromHtml("#BC2B2B");
                    bError = true;
                }
            }
            return bError;
        }

        #endregion
    }
}
