Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change01Edit
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objChange01_objFDDBank As BankBaseCredit
    Private m_strInitiator As String

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblInformation As System.Web.UI.WebControls.Label
    Protected WithEvents FocusScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ConfirmMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAuthorisize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAuthorize As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit_old As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        cmdConfirm.Enabled = False
        cmdReset.Enabled = False
        lblPageTitle.Text = "Werte �ndern"

        lblInformation.Text = ""
        m_strInitiator = ""

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("SelectedDealer").ToString.Length = 0 OrElse (Session("objSuche") Is Nothing) Then
                If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                    (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten f�r den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                        objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
                        objSuche.ReNewSAPDestination(Session.SessionID.ToString, Session("AppID").ToString)
                    End If
                Else
                    Try
                    Catch
                        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
                    End Try
                End If
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                lblHaendlerNummer.Text = objSuche.REFERENZ
                lblHaendlerName.Text = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                End If
                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Else
                lblError.Text = "Fehler bei der Ermittlung der H�ndlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
            lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString & "&ID=" & Request.QueryString("ID") & "&Back=1"

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                ConfirmMessage.Visible = False

                '#################### Debug only ####################
                'Session("Authorization") = True
                'Session("AuthorizationID") = 6
                '#################### Debug only ####################
            End If

            If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                'Seite wurde mit dem Merkmal "Autorisieren" aufgerufen

                If (Session("AuthorizationID") Is Nothing) OrElse Session("AuthorizationID").ToString.Length = 0 Then
                    'AuthorizationID leer
                    lblError.Text = "Kein Vorgang zum Autorisieren �bergeben."
                    DataGrid1.Visible = False
                Else
                    'AuthorizationID gef�llt -> Vorgang wird aus DB geladen
                    Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objChange01_objFDDBank")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten f�r den Vorgang vorhanden."
                    Else
                        Dim formatter As New BinaryFormatter()
                        objChange01_objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange01_objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), BankBaseCredit)
                        FillGrid()
                        DoSubmit1()
                        FocusScript.Visible = False

                        cmdSave.Visible = False
                        cmdConfirm.Visible = False
                        cmdReset.Visible = False
                        cmdBack.Visible = True
                        cmdAuthorize.Visible = True
                        cmdDelete.Visible = True
                    End If
                End If
            Else
                Dim intAuthorizationID As Int32
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, Session("SelectedDealer").ToString, "", m_User.IsTestUser, m_strInitiator, intAuthorizationID)
                If Not m_strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da H�ndlerdaten in Autorisierung

                    LoadAuthorizatioData(intAuthorizationID)
                    lblError.Text = "Die Angaben zum H�ndler " & Session("SelectedDealer").ToString & " wurden vom Benutzer """ & m_strInitiator & """ ge�ndert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                Else
                    'Seite im normalen �nderungsmodus aufgerufen

                    If Not IsPostBack Then
                        objChange01_objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                        objChange01_objFDDBank.Customer = "60" & Session("SelectedDealer").ToString
                        'objChange01_objFDDBank.KUNNR = m_User.KUNNR
                        objChange01_objFDDBank.CreditControlArea = "ZDAD"
                        If (Not Request.QueryString("ID") Is Nothing) AndAlso (Request.QueryString("ID").Length > 0) Then
                            objChange01_objFDDBank.ZeigeGesperrt = True
                        Else
                            objChange01_objFDDBank.ZeigeGesperrt = False
                        End If
                    Else
                        objChange01_objFDDBank = CType(m_context.Cache("objChange01_objFDDBank"), BankBaseCredit)
                    End If
                    objChange01_objFDDBank.Show()
                    If objChange01_objFDDBank.Status = 0 Then
                        If Not IsPostBack Then
                            StartLoadData()
                        End If

                        cmdSave.Enabled = True
                        cmdConfirm.Enabled = True
                        cmdReset.Enabled = True
                        cmdAuthorize.Enabled = False
                        cmdBack.Enabled = False
                        cmdDelete.Enabled = False
                    Else
                        lblError.Text = objChange01_objFDDBank.Message
                        FocusScript.Visible = False
                    End If
                End If
            End If

            If Not IsPostBack Then
                m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change01Edit", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = objChange01_objFDDBank.Kontingente
        DataGrid1.DataBind()

        Dim intKreditlimit As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt As Boolean

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim label As Label
        Dim textbox As TextBox
        Dim control As Control

        For Each item In DataGrid1.Items
            Dim blnZeigeKontingentart As Boolean
            cell = item.Cells(item.Cells.Count - 1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnZeigeKontingentart = chkBox.Checked
                End If
            Next
            cell = item.Cells(2)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If label.ID = "lblKontingent_Alt" And blnZeigeKontingentart Then
                        label.Visible = True
                        intKreditlimit = CInt(label.Text)
                    Else
                        label.Visible = False
                        If label.ID = "lblRichtwert_Alt" And (Not blnZeigeKontingentart) Then
                            label.Visible = True
                            intKreditlimit = CInt(label.Text)
                        Else
                            label.Visible = False
                        End If
                    End If
                End If
            Next

            intAusschoepfung = CInt(item.Cells(3).Text)

            cell = item.Cells(4)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If Not blnZeigeKontingentart Then
                        label.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(5)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt = chkBox.Checked
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            cell = item.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                        textbox.Visible = True
                    Else
                        textbox.Visible = False
                        If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                            textbox.Visible = True
                        Else
                            textbox.Visible = False
                        End If
                    End If
                End If
            Next

            cell = item.Cells(7)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            If blnZeigeKontingentart Then
                If blnGesperrt Then
                    For Each cell In item.Cells
                    Next
                Else
                    If Not objChange01_objFDDBank.ZeigeGesperrt Then
                        If (intAusschoepfung > intKreditlimit) Then
                            cell.ForeColor = System.Drawing.Color.Red
                        End If
                    End If
                End If
            End If
        Next

        If objChange01_objFDDBank.ZeigeGesperrt Then
            DataGrid1.Columns(DataGrid1.Columns.Count - 3).Visible = False
            DataGrid1.Columns(DataGrid1.Columns.Count - 5).Visible = False
        Else
            DataGrid1.Columns(DataGrid1.Columns.Count - 2).Visible = False
            DataGrid1.Columns(DataGrid1.Columns.Count - 4).Visible = False
        End If
    End Sub

    Private Sub StartLoadData()
        FocusScript.Visible = True

        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False

        If (objChange01_objFDDBank.Kontingente Is Nothing) OrElse (objChange01_objFDDBank.Kontingente.Rows.Count = 0) Then
            lblError.Text = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."
            FocusScript.Visible = False
            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub DoSubmit1()
        Dim intKreditlimit_Alt As Int32
        Dim intKreditlimit_Neu As Int32
        Dim intRichtwert_Alt As Int32
        Dim intRichtwert_Neu As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt_Alt As Boolean
        Dim blnGesperrt_Neu As Boolean
        Dim strChangeMessage As String = ""

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim textbox As TextBox
        Dim image As System.Web.UI.WebControls.Image
        Dim control As Control
        Dim blnChanged As Boolean = False

        Dim i As Int32 = 0

        For Each item In DataGrid1.Items
            'Werte ermitteln

            'Alt
            Dim blnZeigeKontingentart As Boolean = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("ZeigeKontingentart"))
            If blnZeigeKontingentart Then
                intKreditlimit_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Alt"))
                intRichtwert_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Alt"))
                intRichtwert_Neu = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Neu"))
                blnGesperrt_Alt = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Alt"))
            Else
                intKreditlimit_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Alt"))
                intKreditlimit_Neu = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Neu"))
                intRichtwert_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Alt"))
                blnGesperrt_Alt = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                blnGesperrt_Neu = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Neu"))
            End If
            intAusschoepfung = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Ausschoepfung"))
            i += 1

            'Neu
            If Not objChange01_objFDDBank.ZeigeGesperrt Then
                cell = item.Cells(6)
                For Each control In cell.Controls
                    If TypeOf control Is TextBox Then
                        textbox = CType(control, TextBox)
                        If IsNumeric(textbox.Text) AndAlso (textbox.Text.Length < 5) AndAlso (Not CInt(textbox.Text) < 0) Then

                            If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                                intKreditlimit_Neu = CInt(textbox.Text)
                            Else
                                If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                    intRichtwert_Neu = CInt(textbox.Text)
                                End If
                            End If

                        Else
                            strChangeMessage &= "Bitte geben Sie numerische, positive und max. vierstellige Kontigentwerte ein.<br>"
                        End If
                    End If
                Next
            End If

            cell = item.Cells(7)
            blnGesperrt_Neu = False
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt_Neu = chkBox.Checked
                End If
            Next

            cell = item.Cells(6)
            If (Not (intKreditlimit_Alt = intKreditlimit_Neu)) Or (Not (intRichtwert_Alt = intRichtwert_Neu)) Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/arrow.gif"
                    End If
                Next
                blnChanged = True
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/empty.gif"
                    End If
                Next
            End If

            cell = item.Cells(7)
            If blnGesperrt_Alt = blnGesperrt_Neu Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/empty.gif"
                    End If
                Next
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Portal/Images/arrow.gif"
                    End If
                Next
                blnChanged = True
            End If

            For Each cell In item.Cells
                cell.ForeColor = System.Drawing.Color.Black
            Next
            If blnZeigeKontingentart Then
                If blnGesperrt_Neu Then
                    For Each cell In item.Cells
                        cell.ForeColor = System.Drawing.Color.Red
                    Next
                Else
                    If Not objChange01_objFDDBank.ZeigeGesperrt Then
                        If (intAusschoepfung > intKreditlimit_Neu) Then
                            For Each cell In item.Cells
                                cell.ForeColor = System.Drawing.Color.Red
                            Next
                        End If
                    End If
                End If
            End If
        Next

        If blnChanged Then
            If strChangeMessage.Length = 0 Then
                For Each item In DataGrid1.Items
                    cell = item.Cells(6)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.Enabled = False
                        End If
                    Next
                    cell = item.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            chkBox.Enabled = False
                        End If
                    Next
                Next
            End If
            FocusScript.Visible = False

            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True
            cmdAuthorize.Visible = False
            cmdBack.Visible = False
            cmdDelete.Visible = False
        Else
            strChangeMessage &= "Die Werte wurden nicht ge�ndert."
            'Kn�ppe ausblenden/einblenden?
            'Tut nicht not!
        End If
        lblInformation.Text = strChangeMessage
        lblError.Text = strChangeMessage

        m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit1()
    End Sub

    Private Sub DoSubmit2()
        If m_strInitiator.Length = 0 Then
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            Try

                Dim strKontingentart As String
                Dim intKreditlimit_Alt As Int32
                Dim intKreditlimit_Neu As Int32
                Dim intRichtwert_Alt As Int32
                Dim intRichtwert_Neu As Int32
                Dim intAusschoepfung As Int32
                Dim blnGesperrt_Alt As Boolean
                Dim blnGesperrt_Neu As Boolean

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim chkBox As CheckBox
                Dim textbox As TextBox
                Dim control As Control
                Dim i As Int32 = 0

                For Each item In DataGrid1.Items
                    'Werte ermitteln

                    'Alt
                    Dim blnZeigeKontingentart As Boolean = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("ZeigeKontingentart"))
                    If blnZeigeKontingentart Then
                        intKreditlimit_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Alt"))
                        intRichtwert_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Alt"))
                        intRichtwert_Neu = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Neu"))
                        blnGesperrt_Alt = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                    Else
                        intKreditlimit_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Alt"))
                        intKreditlimit_Neu = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingent_Neu"))
                        intRichtwert_Alt = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Richtwert_Alt"))
                        blnGesperrt_Alt = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                        blnGesperrt_Neu = CBool(objChange01_objFDDBank.Kontingente.Rows(i)("Gesperrt_Neu"))
                    End If
                    intAusschoepfung = CInt(objChange01_objFDDBank.Kontingente.Rows(i)("Ausschoepfung"))
                    strKontingentart = CStr(objChange01_objFDDBank.Kontingente.Rows(i)("Kontingentart"))
                    i += 1

                    'Neu
                    If Not objChange01_objFDDBank.ZeigeGesperrt Then
                        cell = item.Cells(6)
                        For Each control In cell.Controls
                            If TypeOf control Is TextBox Then
                                textbox = CType(control, TextBox)
                                If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                                    intKreditlimit_Neu = CInt(textbox.Text)
                                Else
                                    If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                        intRichtwert_Neu = CInt(textbox.Text)
                                    End If
                                End If
                            End If
                        Next
                    End If

                    cell = item.Cells(7)
                    blnGesperrt_Neu = False
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            blnGesperrt_Neu = chkBox.Checked
                        End If
                    Next

                    If Not ((intKreditlimit_Alt = intKreditlimit_Neu) And (intRichtwert_Alt = intRichtwert_Neu) And (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                        objChange01_objFDDBank.Kontingente.AcceptChanges()
                        Dim tmpRows As DataRow()
                        tmpRows = objChange01_objFDDBank.Kontingente.Select("Kontingentart = '" & strKontingentart & "'")
                        tmpRows(0).BeginEdit()
                        If objChange01_objFDDBank.ZeigeGesperrt Then
                            tmpRows(0).Item("Kontingent_Neu") = tmpRows(0).Item("Kontingent_Alt")
                            tmpRows(0).Item("Richtwert_Neu") = tmpRows(0).Item("Richtwert_Alt")
                            tmpRows(0).Item("Gesperrt_Neu") = blnGesperrt_Neu
                        Else
                            tmpRows(0).Item("Kontingent_Neu") = intKreditlimit_Neu
                            tmpRows(0).Item("Richtwert_Neu") = intRichtwert_Neu
                            tmpRows(0).Item("Gesperrt_Neu") = tmpRows(0).Item("Gesperrt_Alt")
                        End If
                        tmpRows(0).EndEdit()
                        objChange01_objFDDBank.Kontingente.AcceptChanges()

                    End If
                Next

                Dim tblLogDetails As DataTable = GetChanges()

                If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                    'Anwendung erfordert Autorisierung (Level>0)

                    Dim DetailArray(2, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objChange01_objFDDBank)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "objChange01_objFDDBank"


                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objSuche)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(1, 0) = ms
                    DetailArray(1, 1) = "objSuche"
                    Dim iDistrictID As Integer
                    If Not Session("SelectedDistrict") = Nothing Then
                        iDistrictID = Session("SelectedDistrict")
                    Else
                        iDistrictID = m_User.Organization.OrganizationId
                    End If
                    If Right(objChange01_objFDDBank.Customer, 5) = Right(Session("SelectedDealer").ToString, 5) Then
                        Dim intAuthorizationID As Int32 = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, Right(objChange01_objFDDBank.Customer, 5), "", "", "", m_User.IsTestUser, DetailArray)

                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Kontingent�nderung f�r H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich zur Autorisierung gespeichert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)

                        LoadAuthorizatioData(intAuthorizationID)
                        ConfirmMessage.Visible = True
                        lblInformation.Text = "<b>Ihre Daten wurden zur Autorisierung gespeichert.</b><br>&nbsp;"
                    Else
                        lblError.Text = "Die Daten konnten aufgrund technischer Probleme NICHT gespeichert werden.<br>Bitte starten Sie die H�ndlersuche erneut."
                        Session("SelectedDealer") = Nothing
                        HttpContext.Current.Cache.Remove("objChange01_objFDDBank")
                        cmdAuthorize.Visible = False
                        cmdBack.Visible = False
                        cmdConfirm.Visible = False
                        cmdDelete.Visible = False
                        cmdReset.Visible = False
                        cmdSave.Visible = False
                        cmdBack2.Visible = True
                        cmdBack2.NavigateUrl = lnkKreditlimit.NavigateUrl
                        Exit Sub
                    End If

                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)

                    objChange01_objFDDBank.Change()
                    If objChange01_objFDDBank.Status = 0 Then

                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Kontingent von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich ge�ndert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
                        lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                        ConfirmMessage.Visible = True
                        FocusScript.Visible = False
                    Else
                        logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Kontingent�nderung von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & objChange01_objFDDBank.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                        lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objChange01_objFDDBank.Message & ")"
                        lblError.CssClass = "TextError"
                        ConfirmMessage.Visible = False
                        FocusScript.Visible = True
                    End If
                    logApp.WriteStandardDataAccessSAP(objChange01_objFDDBank.IDSAP)
                    objChange01_objFDDBank.Show()
                    StartLoadData()
                End If

            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "Change01Edit", "DoSubmit2", ex.ToString)

                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Kontingent�nderung von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False
                FocusScript.Visible = False
                objChange01_objFDDBank.Show()
            End Try
            m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        End If
    End Sub

    Private Sub DoSubmit3()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        Dim blnError As Boolean = False
        Try

            Dim tblLogDetails As DataTable = GetChanges()

            objChange01_objFDDBank.SessionID = Session.SessionID.ToString
            objChange01_objFDDBank.Change()
            If objChange01_objFDDBank.Status = 0 Then

                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Kontingent von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich ge�ndert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
                logApp.WriteStandardDataAccessSAP(objChange01_objFDDBank.IDSAP)
                lblInformation.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                Session("objSuche") = objSuche
                ConfirmMessage.Visible = True
                FocusScript.Visible = False
                blnError = False
                cmdSave.Enabled = True
                objChange01_objFDDBank.Show()
                StartLoadData()
                m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                If Not blnError Then
                    'zurueck zur Liste oder Hauptmenue
                    Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                    Try
                        If Not strLastRecord = "True" Then
                            Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")), False)
                        Else
                            Response.Redirect("../../../Start/Selection.aspx", False)
                        End If
                    Catch
                    End Try
                End If
            Else
                m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Kontingent�nderung von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & objChange01_objFDDBank.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                logApp.WriteStandardDataAccessSAP(objChange01_objFDDBank.IDSAP)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objChange01_objFDDBank.Message & ")"
                lblError.CssClass = "TextError"
                ConfirmMessage.Visible = False
                FocusScript.Visible = False
                blnError = True
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change01Edit", "DoSubmit3", ex.ToString)

            m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Kontingent�nderung von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.CssClass = "TextError"
            ConfirmMessage.Visible = False
            FocusScript.Visible = False
            blnError = True
        End Try
    End Sub

    Private Sub DoSubmit4()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        Try

            Dim tblLogDetails As DataTable = GetChanges()

            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))

            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Kontingent�nderung f�r H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") aus Autorisierung gel�scht.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
            lblInformation.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            Session("objSuche") = objSuche
            ConfirmMessage.Visible = True
            FocusScript.Visible = False
            cmdSave.Enabled = True

            'zurueck zur Liste oder Hauptmenue
            Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
            Try
                If Not strLastRecord = "True" Then
                    Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")), False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Catch
            End Try
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change01Edit", "DoSubmit4", ex.ToString)

            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Fehler bei der Kontingent�nderung von H�ndler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            ConfirmMessage.Visible = False
            FocusScript.Visible = True
        End Try
        objChange01_objFDDBank.Show()
        StartLoadData()
        m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    Private Sub cmdConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
        DoSubmit2()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        objChange01_objFDDBank.Show()
        StartLoadData()
        m_context.Cache.Insert("objChange01_objFDDBank", objChange01_objFDDBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change01Edit.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    If cmdConfirm.Visible Then
    '        DoSubmit2()
    '    Else
    '        DoSubmit1()
    '    End If
    'End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        DoSubmit4()
    End Sub

    Private Sub cmdAuthorize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAuthorize.Click
        DoSubmit3()
    End Sub

    Private Function GetChanges() As DataTable
        Dim m_tblKontingenteChanged As DataTable
        m_tblKontingenteChanged = New DataTable()
        m_tblKontingenteChanged.Columns.Add("Status", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("H�ndler", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingent", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Richtwert", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Ausschoepfung", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Frei", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Gesperrt", System.Type.GetType("System.Boolean"))

        Dim rowTemp As DataRow
        For Each rowTemp In objChange01_objFDDBank.Kontingente.Rows
            Dim tmpRow2 As DataRow
            tmpRow2 = m_tblKontingenteChanged.NewRow
            tmpRow2("Status") = "Alt"
            tmpRow2("H�ndler") = objSuche.REFERENZ
            tmpRow2("Kontingentart") = rowTemp("Kontingentart")
            tmpRow2("Kontingent") = rowTemp("Kontingent_Alt")
            tmpRow2("Richtwert") = rowTemp("Richtwert_Alt")
            tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
            tmpRow2("Frei") = CInt(rowTemp("Kontingent_Alt")) - CInt(rowTemp("Ausschoepfung"))
            tmpRow2("Gesperrt") = rowTemp("Gesperrt_Alt")
            m_tblKontingenteChanged.Rows.Add(tmpRow2)
            If (Not CInt(rowTemp("Kontingent_Alt")) = CInt(rowTemp("Kontingent_Neu"))) Or (Not CInt(rowTemp("Richtwert_Alt")) = CInt(rowTemp("Richtwert_Neu"))) Or (Not CBool(rowTemp("Gesperrt_Alt")) = CBool(rowTemp("Gesperrt_Neu"))) Then
                tmpRow2 = m_tblKontingenteChanged.NewRow
                tmpRow2("Status") = "Neu"
                tmpRow2("H�ndler") = objSuche.REFERENZ
                tmpRow2("Kontingentart") = rowTemp("Kontingentart")
                tmpRow2("Kontingent") = rowTemp("Kontingent_Neu")
                tmpRow2("Richtwert") = rowTemp("Richtwert_Neu")
                tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
                tmpRow2("Frei") = CInt(rowTemp("Kontingent_Neu")) - CInt(rowTemp("Ausschoepfung"))
                tmpRow2("Gesperrt") = rowTemp("Gesperrt_Neu")
                m_tblKontingenteChanged.Rows.Add(tmpRow2)
            End If
        Next
        Return m_tblKontingenteChanged
    End Function

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, AuthorizationID, "objChange01_objFDDBank")
        Dim formatter As New BinaryFormatter()
        objChange01_objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objChange01_objFDDBank = DirectCast(formatter.Deserialize(OutPutStream), BankBaseCredit)
        FillGrid()
        DoSubmit1()

        FocusScript.Visible = False
        cmdSave.Visible = False
        cmdConfirm.Visible = False
        cmdReset.Visible = False
        cmdAuthorize.Visible = False
        cmdBack.Visible = False
        cmdDelete.Visible = False
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
        Catch
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change01Edit.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' R�ckg�ngig: Dynproxy-Zugriff
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' �nderungen aus StartApplication vom 02.05.2007 Mittags �bernommen
' 
' ************************************************