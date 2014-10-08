﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using AppRemarketing.lib;
using System.Configuration;

namespace AppRemarketing.forms
{
    public partial class Report13 : System.Web.UI.Page
    {
        private CKG.Base.Kernel.Security.User m_User;
        private CKG.Base.Kernel.Security.App m_App;
        private bool isExcelExportConfigured1;
        private bool isExcelExportConfigured2;
        private Belastungsanzeigen m_Report;

        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);

            m_App = new App(m_User);
            Common.GetAppIDFromQueryString(this);

            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];
            lblError.Text = "";
            Literal1.Text = "";

            try
            {
                if (!IsPostBack)
                {
                    Common.TranslateTelerikColumns(rgGrid1);
                    Common.TranslateTelerikColumns(rgGrid2);

                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    Session["rgGrid1_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());
                    persister = new GridSettingsPersister(rgGrid2, GridSettingsType.All);
                    Session["rgGrid2_original"] = persister.LoadForUser(m_User, (string)Session["AppID"], GridSettingsType.All.ToString());

                    String strFileName = String.Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName + ".xls";
                    Session["Rechnungen"] = null;

                    m_Report = new Belastungsanzeigen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, strFileName);
                    Session.Add("Rechnungen", m_Report);
                    m_Report.SessionID = this.Session.SessionID;
                    m_Report.AppID = (string)Session["AppID"];
                    FillDate();
                    FillVermieter();
                    FillHC();
                }
                else
                {
                    if ((Session["Rechnungen"] != null))
                    {
                        m_Report = (Belastungsanzeigen)Session["Rechnungen"];
                    }
                }
                if (IsAV())
                {
                    tr_Vermieter.Visible = false;
                }
            }
            catch
            {
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        private void Page_Unload(object sender, System.EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        protected void lbCreate_Click(object sender, EventArgs e)
        {
            DoSubmit();
        }

        private void DoSubmit()
        {
            if ((txtKennzeichen.Text.Length == 0) && (txtFahrgestellnummer.Text.Length == 0) && (txtRechnungsnummer.Text.Length == 0) && (txtInventarnummer.Text.Length == 0))
            {
                if (txtVertragsjahr.Text.Length < 4)
                {
                    if (((txtDatumVon.Text.Length == 0) && txtDatumBis.Text.Length == 0))
                    {
                        lblError.Text = "Bitte geben Sie einen Zeitraum für Ihre Selektion an.";
                        return;
                    }
                }

                if ((txtDatumVon.Text.Length > 0) && (txtDatumBis.Text.Length > 0))
                {
                    DateTime DateFrom = DateTime.Parse(txtDatumVon.Text).Date;
                    DateTime DateTo = DateTime.Parse(txtDatumBis.Text).Date;

                    if (DateTo < DateFrom)
                    {
                        lblError.Text = "Datum von ist größer als Datum bis.";
                        return;
                    }
                    System.TimeSpan diffResult = DateTo.Subtract(DateFrom);
                }
            }

            m_Report.AVNR = "";
            if (IsAV())
            {
                m_Report.AVNR = m_User.Groups[0].GroupName.ToString();
            }
            else if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "VW")
            {
                m_Report.AVNR = (string)ddlVermieter.SelectedValue;
            }
            if (m_Report.AVNR == "")
            {
                lblError.Text = "Gruppe nicht eindeutig!";
                return;
            }
            m_Report.AuswahlStatus = (string)ddlStatus.SelectedValue;

            m_Report.DatumVon = txtDatumVon.Text;
            m_Report.DatumBis = txtDatumBis.Text;
            m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text;
            m_Report.Kennzeichen = txtKennzeichen.Text;
            m_Report.Inventarnummer = txtInventarnummer.Text;
            m_Report.Rechnungsnummer = txtRechnungsnummer.Text;
            m_Report.CarportNr = ddlHC.SelectedValue;
            m_Report.Vertragsjahr = txtVertragsjahr.Text;

            m_Report.ShowRechnung((string)Session["AppID"], (string)Session.SessionID, this);

            if (m_Report.Status == 0)
            {
                Session["Rechnungen"] = m_Report;
                Fillgrid();
            }
            else
            {
                lblError.Text = m_Report.Message;
            }
        }

        private void Fillgrid()
        {
            rgGrid2.Visible = false;

            if (m_Report.Result.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);

                rgGrid1.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
                
                if (!IsAV()) 
                {
                    rgGrid1.MasterTableView.GetColumn("Bearbeiten").Visible = false;              
                }
            }
        }

        private void SearchMode(bool search = true)
        {
            NewSearch.Visible = !search;
            NewSearchUp.Visible = search;
            Panel1.Visible = search;
            lbCreate.Visible = search;
            Result.Visible = !search;
        }

        protected void rgGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Result != null)
            {
                rgGrid1.DataSource = m_Report.Result.DefaultView;
            }
            else
            {
                rgGrid1.DataSource = null;
            }
        }

        private void Fillgrid2()
        {
            if (m_Report.Belastungen.Rows.Count == 0)
            {
                SearchMode();
                lblError.Text = "Keine Dokumente zur Anzeige gefunden.";
            }
            else
            {
                SearchMode(false);

                rgGrid2.Rebind();
                //Setzen der DataSource geschieht durch das NeedDataSource-Event
            }
        }

        protected void rgGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (m_Report.Belastungen != null)
            {
                rgGrid2.DataSource = m_Report.Belastungen.DefaultView;
            }
            else
            {
                rgGrid2.DataSource = null;
            }
        }

        protected void NewSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode();
        }

        protected void NewSearchUp_Click(object sender, ImageClickEventArgs e)
        {
            SearchMode(false);
        }
        
        private void FillVermieter()
        {
            Fahrzeugbestand mVermieter = new Fahrzeugbestand(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");
            m_Report = new Belastungsanzeigen(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mVermieter.getVermieter((string)Session["AppID"], (string)Session.SessionID, this);

            if (mVermieter.Status > 0)
            {
                lblError.Text = mVermieter.Message;
            }
            else
            {
                if (mVermieter.Vermieter.Rows.Count > 0)
                {
                    m_Report.Vermieter = mVermieter.Vermieter;

                    Session["Vorschaden"] = m_Report;

                    ListItem litVermiet;
                    litVermiet = new ListItem();
                    litVermiet.Text = "- alle -";
                    litVermiet.Value = "00";
                    ddlVermieter.Items.Add(litVermiet);

                    foreach (DataRow drow in mVermieter.Vermieter.Rows)
                    {
                        litVermiet = new ListItem();
                        litVermiet.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litVermiet.Value = (string)drow["POS_KURZTEXT"];
                        ddlVermieter.Items.Add(litVermiet);
                    }
                }
            }
        }

        private bool IsAV()
        {
            if (m_User.Groups[0].GroupName.ToString().Substring(0, 2) == "AV")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FillHC()
        {
            HC mHC = new HC(ref m_User, m_App, (string)Session["AppID"], (string)Session.SessionID, "");

            mHC.getHC((string)Session["AppID"], (string)Session.SessionID, this);

            if (mHC.Status > 0)
            {
                lblError.Text = mHC.Message;
            }
            else
            {
                if (mHC.Hereinnahmecenter.Rows.Count > 0)
                {
                    ListItem litHC;
                    litHC = new ListItem();
                    litHC.Text = "- alle -";
                    litHC.Value = "00";
                    ddlHC.Items.Add(litHC);

                    foreach (DataRow drow in mHC.Hereinnahmecenter.Rows)
                    {
                        litHC = new ListItem();
                        litHC.Text = (string)drow["POS_KURZTEXT"] + " " + (string)drow["POS_TEXT"];
                        litHC.Value = (string)drow["POS_KURZTEXT"];
                        ddlHC.Items.Add(litHC);
                    }
                }
            }
        }

        private void FillDate()
        {
            txtDatumVon.Text = Helper.DateFrom;
            txtDatumBis.Text = Helper.DateTo;
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
            Session["Rechnungen"] = null;
            Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx");
        }   

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            rgGrid1.Visible = true;
            rgGrid2.Visible = false;
            lbtnBack.Visible = false;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //if (txtBemerkung.Text.Length == 0)
            //{
            //    lblSaveInfo.Visible = true;
            //    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
            //    lblSaveInfo.Text = "Bitte geben Sie eine Bemerkung ein.";
            //    ModalPopupExtender2.Show();
            //    return;
            //}

            //if (txtBetrag.Text.Length == 0)
            //{
            //    lblSaveInfo.Visible = true;
            //    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
            //    lblSaveInfo.Text = "Bitte geben Sie einen Betrag ein.";
            //    ModalPopupExtender2.Show();
            //    return;
            //}

            //double DummyDouble;

            //if (double.TryParse(txtBetrag.Text, out DummyDouble) == false)
            //{
            //    lblSaveInfo.Visible = true;
            //    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
            //    lblSaveInfo.Text = "Bitte geben Sie gültigen Betrag ein.";
            //    ModalPopupExtender2.Show();
            //    return;
            //}



            //m_Report = (Belastungsanzeigen)Session["Rechnungen"];
            //m_Report.Rechnungsnummer = lblRechnr.Text;
            //m_Report.GutschriftBetrag = txtBetrag.Text;
            //m_Report.GutschriftBemerkung = txtBemerkung.Text;

            //m_Report.SetGutschrift((string)Session["AppID"], (string)Session.SessionID, this.Page);

            //if (m_Report.Status == 102)
            //{
            //    lblSaveInfo.ForeColor = System.Drawing.Color.Blue;
            //    lblSaveInfo.Visible = true;
            //    btnCancel.Text = "Schließen";
            //    btnOK.Width = 0;
            //    ModalPopupExtender2.Show();
            //    lblSaveInfo.Text = "Die Gutschrift wurde gespeichert.";
            //}
            //else
            //{
            //    lblSaveInfo.Visible = true;
            //    lblSaveInfo.ForeColor = System.Drawing.Color.Red; ;
            //    //lblSaveInfo.Text = "Fehler beim Speichern der Gutschrift.";
            //    lblSaveInfo.Text = m_Report.Message;
            //    ModalPopupExtender2.Show();
            //}

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }
        
        private void StoreGridSettings(RadGrid grid, GridSettingsType settingsType)
        {
            var persister = new GridSettingsPersister(grid, settingsType);
            persister.SaveForUser(m_User, (string)Session["AppID"], settingsType.ToString());
        }

        protected void rgGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.CommandItem)
            {
                var gcitem = e.Item as GridCommandItem;

                var rbutton = gcitem.FindControl("RefreshButton") ?? gcitem.FindControl("RebindGridButton");
                if (rbutton == null) return;

                var rbutton_parent = rbutton.Parent;

                var saveLayoutButton = new Button() { ToolTip = "Layout speichern", CommandName = "SaveGridLayout", CssClass = "rgSaveLayout" };
                rbutton_parent.Controls.AddAt(0, saveLayoutButton);

                var resetLayoutButton = new Button() { ToolTip = "Layout zurücksetzen", CommandName = "ResetGridLayout", CssClass = "rgResetLayout" };
                rbutton_parent.Controls.AddAt(1, resetLayoutButton);
            }
        }

        protected void rgGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid1.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("RechnGutschrNachbel_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid1.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid1.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid1.Rebind();
                    rgGrid1.MasterTableView.ExportToExcel();
                    break;

                case "SaveGridLayout":
                    StoreGridSettings(rgGrid1, GridSettingsType.All);
                    break;

                case "ResetGridLayout":
                    var settings = (string)Session["rgGrid1_original"];
                    var persister = new GridSettingsPersister(rgGrid1, GridSettingsType.All);
                    persister.LoadSettings(settings);

                    Fillgrid();
                    break;

                case "Info":
                    m_Report = (Belastungsanzeigen)Session["Rechnungen"];
                    m_Report.Rechnungsnummer = e.CommandArgument.ToString();

                    m_Report.ShowBelastungsanzeigen((string)Session["AppID"], (string)Session.SessionID, this);

                    if (m_Report.Belastungen.Rows.Count > 0)
                    {
                        rgGrid1.Visible = false;

                        Fillgrid2();

                        rgGrid2.Visible = true;
                        lbtnBack.Visible = true;
                    }
                    break;

                case "Bemerkung":
                    m_Report = (Belastungsanzeigen)Session["Rechnungen"];
                    m_Report.Rechnungsnummer = e.CommandArgument.ToString();

                    m_Report.ShowBemerkung((string)Session["AppID"], (string)Session.SessionID, this);

                    if (m_Report.TableBemerkungen.Rows.Count > 0)
                    {
                        lblBemerkung.Text = m_Report.TableBemerkungen.Rows[0]["KTEXT"].ToString();
                    }

                    ModalPopupExtender2.Show();
                    break;

                case "PDF":
                    var parts = ((string)e.CommandArgument).Split('|');
                    var rechnungsnummer = parts[0];
                    var status = parts[1];

                    string dateityp;
                    switch (status)
                    {
                        case "R":
                            dateityp = "Rechnung";
                            break;
                        case "N":
                            dateityp = "Nachbelastung";
                            break;
                        case "G":
                            dateityp = "Gutschrift";
                            break;
                        default:
                            dateityp = status;
                            break;
                    }

                    // Nachberechnung wie Gutschrift
                    if (status.Equals("N", StringComparison.Ordinal))
                    {
                        status = "G";
                    }
                    // Rechnung mit S statt R
                    else if (status.Equals("R", StringComparison.Ordinal))
                    {
                        status = "S";
                    }

                    QuickEasy.Documents qe = new QuickEasy.Documents(".1001=" + status + "&.1003=" + rechnungsnummer,
                    ConfigurationManager.AppSettings["EasyRemoteHosts"],
                    60, ConfigurationManager.AppSettings["EasySessionId"],
                    ConfigurationManager.AppSettings["ExcelPath"],
                    "C:\\TEMP",
                    "SYSTEM",
                    ConfigurationManager.AppSettings["EasyPwdClear"],
                    "C:\\TEMP",
                    "VWR",
                    "VWRRG",
                    "SGW");

                    qe.GetDocument();

                    if (qe.ReturnStatus == 2)
                    {
                        Helper.GetPDF(this, qe.path, dateityp + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                    }
                    else
                    {
                        lblError.Text = "Das Dokument wurde nicht gefunden.";
                    }
                    break;
            }
        }

        protected void rgGrid2_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.ExportToExcelCommandName:
                    var eSettings = rgGrid2.ExportSettings;
                    eSettings.ExportOnlyData = true;
                    eSettings.FileName = string.Format("Belastungsanzeigen_{0:yyyyMMdd}", DateTime.Now);
                    eSettings.HideStructureColumns = true;
                    eSettings.IgnorePaging = true;
                    eSettings.OpenInNewWindow = true;
                    // hide non display columns from excel export
                    var nonDisplayColumns = rgGrid2.MasterTableView.Columns.OfType<GridEditableColumn>().Where(c => !c.Display).Select(c => c.UniqueName).ToArray();
                    foreach (var col in nonDisplayColumns)
                    {
                        rgGrid2.Columns.FindByUniqueName(col).Visible = false;
                    }
                    rgGrid2.Rebind();
                    rgGrid2.MasterTableView.ExportToExcel();
                    break;

            }
        }

        protected void rgGrid1_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured1, ref e);
        }

        protected void rgGrid2_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
        {
            Helper.radGridExcelMLExportRowCreated(ref isExcelExportConfigured2, ref e);
        }

        protected void rgGrid_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
        {
            Helper.radGridExcelMLExportStylesCreated(ref e);
        }

    }
}