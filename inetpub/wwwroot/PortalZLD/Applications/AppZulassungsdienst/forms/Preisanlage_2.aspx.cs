﻿using System;
using System.Web.UI.WebControls;
using AppZulassungsdienst.lib;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;
using System.Data;
using System.Configuration;
using CKG.Base.Kernel.DocumentGeneration;

namespace AppZulassungsdienst.forms
{
    /// <summary>
    /// Preisanlage pro Landkreis.
    /// </summary>
    public partial class Preisanlage_2 : System.Web.UI.Page
    {
        private User m_User;
        private clsPreisanlage objPreisanlage;
        private ZLDCommon objCommon;

        #region Events

        /// <summary>
        /// Page_Load Ereignis. Prüfen ob die Anwendung dem Benutzer zugeordnet ist. Evtl. Stammdaten laden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_User = Common.GetUser(this);
            Common.FormAuth(this, m_User);
            Common.GetAppIDFromQueryString(this);
            lblHead.Text = (string)m_User.Applications.Select("AppID = '" + Session["AppID"] + "'")[0]["AppFriendlyName"];

            if (Session["objPreisanlage"] != null)
            {
                objPreisanlage = (clsPreisanlage)Session["objPreisanlage"];
            }
            else
            {
                lblError.Text = "Benötigtes Session-Objekt fehlt!";
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
            if (!IsPostBack)
            {
                fillForm();
            }
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Spaltenübersetzung
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Page_Unload(object sender, EventArgs e)
        {
            Common.SetEndASPXAccess(this);
        }

        /// <summary>
        /// Daten auslesen und in Excel-Vorlage schreiben und an den Innendienst senden.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void cmdCreate_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            ExcelDocumentFactory excel = new ExcelDocumentFactory();
            String Filename = "Zugriff1_" + objPreisanlage.NeueKundenNr + "_" + m_User.UserName + "_" + String.Format("{0:yyyyMMdd_HHmmss}", DateTime.Now) + ".xls";
            DataTable tblHEAD = new DataTable();
            tblHEAD.Columns.Add("Kunnr", typeof(String));
            tblHEAD.Columns.Add("vkbur", typeof(String));
            tblHEAD.Columns.Add("vkorg", typeof(String));
            tblHEAD.Columns.Add("Zugriff", typeof(String));
            tblHEAD.TableName = "Head";
            DataRow tblRowT = tblHEAD.NewRow();
            tblRowT["Kunnr"] = objPreisanlage.NeueKundenNr;
            tblRowT["vkbur"] = objPreisanlage.VKBUR;
            tblRowT["vkorg"] = objPreisanlage.VKORG;
            tblRowT["Zugriff"] = "Zugriff1";

            tblHEAD.Rows.Add(tblRowT);
            DataTable tblData = CreateTableFromGridView();
            DataSet OutputSet = new DataSet();
            OutputSet.Tables.Add(tblData);
            OutputSet.Tables.Add(tblHEAD);

            excel.CreateDocumentAndWriteToFilesystemTemplate(Filename, OutputSet, this, true, "C:\\inetpub\\wwwroot\\PortalZLD\\Applications\\AppZulassungsdienst\\Documents\\Mappe1.xlt", 0, 2);
            if (Sendmail(ConfigurationManager.AppSettings["ExcelPath"] + Filename))
            {
                lblMessage.Text = "Preise gesendet!";
            }
        }

        /// <summary>
        /// Zurück zur Preisanlage Seite1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_zurueck_Click(object sender, EventArgs e)
        {
            Response.Redirect("Preisanlage.aspx?AppID=" + Session["AppID"].ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Daten aufbereiten und Gridview füllen.
        /// </summary>
        private void fillForm()
        {
            lblKunnr.Text = objPreisanlage.NeueKundenNr;
            lblKunnname.Text = objPreisanlage.NeueKundenName;

            Session["objPreisanlage"] = objPreisanlage;
            if (objPreisanlage.ErrorOccured)
            {
                lblError.Text = objPreisanlage.Message;
                return;
            }

            DataTable tblData = new DataTable();
            tblData.Columns.Add("Matnr", typeof(String));
            tblData.Columns.Add("Maktx", typeof(String));
            for (int i = 1; i < 31; i++)
            {
                tblData.Columns.Add("Stva" + i.ToString(), typeof(String));
            }
            int iCount = 1;
            foreach (var mat in objCommon.MaterialStamm)
            {
                DataRow tblRow = tblData.NewRow();

                if (iCount == 1)
                {
                    tblRow["Matnr"] = "Material";
                    tblRow["Maktx"] = "Bezeichnung";
                }
                else
                {
                    tblRow["Matnr"] = mat.MaterialNr;
                    tblRow["Maktx"] = mat.MaterialName;
                }

                for (int i = 1; i < 31; i++)
                {
                    tblRow["Stva" + i.ToString()] = "";
                }

                tblData.Rows.Add(tblRow);
                iCount++;
            }
            Session["tblData"] = tblData;
            GridView1.DataSource = tblData;
            GridView1.DataBind();

            Label lbl = (Label)GridView1.Rows[0].FindControl("lblDienstNr");
            lbl.Attributes.Add("style", "font-weight: bold");
            lbl = (Label)GridView1.Rows[0].FindControl("lblDienst");
            lbl.Attributes.Add("style", "font-weight: bold");

            TextBox txtStva1 = (TextBox)GridView1.Rows[0].FindControl("txtInput1");
            txtStva1.Focus();
            int Rows = 0;
            foreach (GridViewRow Row in GridView1.Rows)
            {
                int iRows = 1;

                for (int i = 2; i < Row.Cells.Count; i++)
                {
                    TextBox txtInput = (TextBox)Row.Cells[i].FindControl("txtInput" + (iRows).ToString());
                    txtInput.Attributes.Add("onkeyup", "keyPressed(this.id, event)");
                    if (Rows == 0)
                    {
                        txtInput.Attributes.Remove("onkeypress");
                    }
                    iRows++;
                }

                Rows++;
            }
        }

        /// <summary>
        /// Mail mit Anhang an Innendienst senden.
        /// </summary>
        /// <param name="Filenname">Datei</param>
        /// <returns>true bei Erfolg, false bei Fehler</returns>
        private Boolean Sendmail(String Filenname)
        {
            try
            {
                System.Net.Mail.MailMessage Mail;
                ZLD_Suche objZLDSuche = new ZLD_Suche();

                objZLDSuche.LeseMailTexte(m_User.Customer.CustomerId, "2");

                String smtpMailSender = ConfigurationManager.AppSettings["SmtpMailSender"];
                String smtpMailServer = ConfigurationManager.AppSettings["SmtpMailServer"];

                String MailText = "Kundennr.: " + objPreisanlage.NeueKundenNr + "<br />";
                MailText += "Kunde: " + objPreisanlage.NeueKundenName + "<br />";
                MailText += "Filiale: " + objPreisanlage.VKBUR + "<br /><br />";
                MailText += "Datum: " + DateTime.Now.ToShortDateString() + "<br />";
                MailText += "Uhrzeit: " + DateTime.Now.ToShortDateString() + "<br />";
                MailText += "Web-Benutzer: " + m_User.UserName + "<br />";

                String[] Adressen;
                if (objZLDSuche.MailAdress.Trim().Split(';').Length > 1)
                {
                    Mail = new System.Net.Mail.MailMessage();
                    System.Net.Mail.MailAddress Mailsender = new System.Net.Mail.MailAddress(smtpMailSender);
                    Mail.Sender = Mailsender;
                    Mail.From = Mailsender;
                    Mail.Body = MailText;


                    Mail.Subject = "Preisanlage Neukunden(" + objPreisanlage.NeueKundenNr + ") Filiale: " + m_User.Reference.Substring(4, 4);
                    Adressen = objZLDSuche.MailAdress.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.To.Add(tmpStr);
                    }
                }
                else
                {
                    Mail = new System.Net.Mail.MailMessage(smtpMailSender, objZLDSuche.MailAdress.Trim(), "Preisanlage Neukunden(" + objPreisanlage.NeueKundenNr + ") Filiale: " + m_User.Reference.Substring(4, 4), MailText);
                }
                if (objZLDSuche.MailAdressCC.Trim().Split(';').Length > 1)
                {
                    Adressen = objZLDSuche.MailAdressCC.Trim().Split(';');
                    foreach (String tmpStr in Adressen)
                    {
                        Mail.CC.Add(tmpStr);
                    }
                }
                else if (!String.IsNullOrEmpty(objZLDSuche.MailAdressCC))
                {
                    Mail.CC.Add(objZLDSuche.MailAdressCC);
                }

                Mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(smtpMailServer);
                System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(Filenname);
                Mail.Attachments.Add(file);

                client.Send(Mail);
                Mail.Attachments.Dispose();
                Mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Fehler beim Senden! " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Daten aus dem Gridview in eine Tabelle schreiben.
        /// </summary>
        /// <returns>Datentabelle</returns>
        private DataTable CreateTableFromGridView()
        {
            DataTable test = new DataTable();
            test.Columns.Add("Material", typeof(String));
            test.Columns.Add("Bezeichnung", typeof(String));
            for (int i = 2; i < GridView1.Rows[0].Cells.Count; i++)
            {
                TableCell gridCell = GridView1.Rows[0].Cells[i];
                TextBox txtInput = (TextBox)gridCell.FindControl("txtInput" + (i - 1).ToString());
                if (txtInput.Text != "")
                {
                    test.Columns.Add(txtInput.Text, typeof(String));
                }
            }

            int iCount = 0;
            foreach (GridViewRow Row in GridView1.Rows)
            {
                if (iCount > 0)
                {
                    DataRow NewRow = test.NewRow();

                    for (int i = 0; i < test.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Label lbl = (Label)Row.Cells[i].FindControl("lblDienstNr");
                            NewRow[i] = lbl.Text;
                        }
                        if (i == 1)
                        {
                            Label lbl = (Label)Row.Cells[i].FindControl("lblDienst");
                            NewRow[i] = lbl.Text;
                        }

                        if (i > 1)
                        {
                            TextBox txtInput = (TextBox)Row.Cells[i].FindControl("txtInput" + (i - 1).ToString());
                            NewRow[i] = txtInput.Text;
                        }
                    }

                    test.Rows.Add(NewRow);
                }
                iCount++;
            }
            return test;

        }

        #endregion
    }
}