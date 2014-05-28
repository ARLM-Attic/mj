
Namespace Kernel.Security
    <Serializable()> Public Class Customer
        REM � Dient der Haltung und Bearbeitung von Kundendaten aus der SQL DB

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intCustomerID As Integer
        Private m_strCustomerName As String
        Private m_strDocuPath As String
        Private m_strKUNNR As String
        Private m_blnReadDealer As Boolean
        Private m_selfAdministration As Integer
        Private m_selfAdministrationInfo As String
        Private m_Locked As Boolean
        Private m_blnMaster As Boolean
        Private m_blnAllowMultipleLogin As Boolean
        Private m_CustomerContact As Contact
        Private m_CustomerPasswordRules As PasswordRules
        Private m_CustomerLoginRules As LoginRules
        Private m_CustomerStyle As Style
        Private m_intMaxUser As Integer
        Private m_blnShowOrganization As Boolean
        Private m_blnShowDistrikte As Boolean
        Private m_strLogoPath2 As String
        Private m_strLogoPath As String
        Private m_blnOrgAdminRestrictToCustomerGroup As Boolean
        Private m_blnNameInputOptional As Boolean
        Private m_blnPwdDontSendEmail As Boolean
        <NonSerialized()> Private m_blnForcePasswordQuestion As Boolean
        <NonSerialized()> Private m_blnIpRestriction As Boolean
        <NonSerialized()> Private m_strIpStandardUser As String
        <NonSerialized()> Private m_intAccountingArea As Integer
        <NonSerialized()> Private m_tblIpAddresses As DataTable
        Private m_DaysUntilLock As Integer
        Private m_DaysUntilDelete As Integer
        Private m_CustomerUsernameRules As UsernameRules = New UsernameRules(False)
        Private m_blnUsernameDontSendEmail As Boolean = False
#End Region

#Region " Constructor "
        Private _intCustomerId As Integer
        Private _p2 As String
        Private _p3 As String
        Private _p4 As Boolean
        Private _p5 As Boolean
        Private _p6 As String
        Private _p7 As String
        Private _p8 As String
        Private _p9 As String
        Private _p10 As String
        Private _p11 As String
        Private _p12 As String
        Private _p13 As String
        Private _p14 As String
        Private _p15 As Integer
        Private _p16 As Integer
        Private _p17 As Integer
        Private _p18 As Integer
        Private _p19 As Integer
        Private _p20 As Integer
        Private _p21 As Integer
        Private _p22 As String
        Private _p23 As String
        Private _p24 As String
        Private _p25 As String
        Private _p26 As Boolean
        Private _p27 As Integer
        Private _p28 As Boolean
        Private _p29 As Boolean
        Private _p30 As Boolean
        Private _p31 As Boolean
        Private _p32 As Boolean
        Private _p33 As Boolean
        Private _p34 As Boolean
        Private _p35 As String
        Private _p36 As Integer
        Private _kundenAdministration As Integer
        Private _p38 As String
        Private _p39 As Boolean
        Private _p40 As Boolean

        Public Sub New(ByVal intCustomerID As Integer)
            m_intCustomerID = intCustomerID
        End Sub
        '��� JVE 18.09.2006: Parameterliste um "strLogoPath2" erweitert.
        Public Sub New(ByVal intCustomerID As Integer, _
                       ByVal strCustomerName As String, _
                       ByVal strKUNNR As String, _
                       ByVal blnMaster As Boolean, _
                       ByVal blnReadDealer As Boolean, _
                       ByVal strCName As String, _
                       ByVal strCAddress As String, _
                       ByVal strCMailDisplay As String, _
                       ByVal strCMail As String, _
                       ByVal strKundePostf As String, _
                       ByVal strKundeHotl As String, _
                       ByVal strKundeFax As String, _
                       ByVal strCWebDisplay As String, _
                       ByVal strCWeb As String, _
                       ByVal intNewPwdAfterNDays As Integer, _
                       ByVal intLockedAfterNLogins As Integer, _
                       ByVal intPwdNNumeric As Integer, _
                       ByVal intPwdLength As Integer, _
                       ByVal intPwdNCapitalLetter As Integer, _
                       ByVal intPwdNSpecialCharacter As Integer, _
                       ByVal intPwdHistoryNEntries As Integer, _
                       ByVal strLogoPath As String, _
                       ByVal strLogoPath2 As String, _
                       ByVal strDocuPath As String, _
                       ByVal strCssPath As String, _
                       ByVal blnAllowMultipleLogin As Boolean, _
                       ByVal intMaxUser As Integer, _
                       ByVal blnShowOrganization As Boolean, _
                       ByVal blnOrgAdminRestrictToCustomerGroup As Boolean, _
                       ByVal blnPwdDontSendEmail As Boolean, _
                       ByVal blnNameInputOptional As Boolean, _
                       ByVal blnShowDistrikte As Boolean, _
                       ByVal blnForcePasswordQuestion As Boolean, _
                       ByVal blnIpRestriction As Boolean, _
                       ByVal strIpStandardUser As String, _
                       ByVal intAccountingArea As Integer, _
                       ByVal intSelfAdministration As Integer, _
                       ByVal strSelfAdministrationInfo As String, _
                       ByVal blnLocked As Boolean, _
                       Optional ByVal blnUsernameDontSendEmail As Boolean = False)

            m_intCustomerID = intCustomerID
            m_strCustomerName = strCustomerName
            m_strDocuPath = strDocuPath
            m_strKUNNR = strKUNNR
            m_blnReadDealer = blnReadDealer
            m_blnMaster = blnMaster
            m_CustomerContact = New Contact(strCName, strCAddress, strCMailDisplay, strCMail, strCWebDisplay, strCWeb, strKundePostf, strKundeHotl, strKundeFax)
            m_CustomerPasswordRules = New PasswordRules(intPwdNNumeric, intPwdLength, intPwdNCapitalLetter, intPwdNSpecialCharacter, intPwdHistoryNEntries, blnPwdDontSendEmail, blnNameInputOptional)
            m_CustomerLoginRules = New LoginRules(intLockedAfterNLogins, intNewPwdAfterNDays)
            m_CustomerStyle = New Style(strLogoPath, strCssPath)
            m_strLogoPath2 = strLogoPath2
            m_blnAllowMultipleLogin = blnAllowMultipleLogin
            m_intMaxUser = intMaxUser
            m_blnShowOrganization = blnShowOrganization
            m_blnOrgAdminRestrictToCustomerGroup = blnOrgAdminRestrictToCustomerGroup
            m_blnPwdDontSendEmail = blnPwdDontSendEmail
            m_blnNameInputOptional = blnNameInputOptional
            m_blnShowDistrikte = blnShowDistrikte
            m_blnForcePasswordQuestion = blnForcePasswordQuestion
            m_blnIpRestriction = blnIpRestriction
            m_strIpStandardUser = strIpStandardUser
            m_intAccountingArea = intAccountingArea
            m_selfAdministration = intSelfAdministration
            m_selfAdministrationInfo = Left(strSelfAdministrationInfo, 200)
            m_Locked = blnLocked
            m_CustomerUsernameRules = New UsernameRules(blnUsernameDontSendEmail)
            m_blnUsernameDontSendEmail = blnUsernameDontSendEmail
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal _user As User)
            Me.New(intCustomerID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal strConnectionString As String)
            Me.New(intCustomerID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
            Dim blnCloseOnEnd As Boolean = False
            m_intCustomerID = intCustomerID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetCustomer(cn)
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub
#End Region

#Region " Properties "

        'Sub New(intCustomerId As Integer, p2 As String, p3 As String, p4 As Boolean, p5 As Boolean, p6 As String, p7 As String, p8 As String, p9 As String, p10 As String, p11 As String, p12 As String, p13 As String, p14 As String, p15 As Integer, p16 As Integer, p17 As Integer, p18 As Integer, p19 As Integer, p20 As Integer, p21 As Integer, p22 As String, p23 As String, p24 As String, p25 As String, p26 As Boolean, p27 As Integer, p28 As Boolean, p29 As Boolean, p30 As Boolean, p31 As Boolean, p32 As Boolean, p33 As Boolean, p34 As Boolean, p35 As String, p36 As Integer, KundenAdministration As Integer, p38 As String, p39 As Boolean)
        '    ' TODO: Complete member initialization 
        '    _intCustomerId = intCustomerId
        '    _p2 = p2
        '    _p3 = p3
        '    _p4 = p4
        '    _p5 = p5
        '    _p6 = p6
        '    _p7 = p7
        '    _p8 = p8
        '    _p9 = p9
        '    _p10 = p10
        '    _p11 = p11
        '    _p12 = p12
        '    _p13 = p13
        '    _p14 = p14
        '    _p15 = p15
        '    _p16 = p16
        '    _p17 = p17
        '    _p18 = p18
        '    _p19 = p19
        '    _p20 = p20
        '    _p21 = p21
        '    _p22 = p22
        '    _p23 = p23
        '    _p24 = p24
        '    _p25 = p25
        '    _p26 = p26
        '    _p27 = p27
        '    _p28 = p28
        '    _p29 = p29
        '    _p30 = p30
        '    _p31 = p31
        '    _p32 = p32
        '    _p33 = p33
        '    _p34 = p34
        '    _p35 = p35
        '    _p36 = p36
        '    _kundenAdministration = KundenAdministration
        '    _p38 = p38
        '    _p39 = p39
        'End Sub

        Public Property AccountingArea() As Integer
            Get
                Return m_intAccountingArea
            End Get
            Set(ByVal value As Integer)
                m_intAccountingArea = value
            End Set
        End Property

        Public Property IpAddresses() As DataTable
            Get
                Return m_tblIpAddresses
            End Get
            Set(ByVal Value As DataTable)
                m_tblIpAddresses = Value
            End Set
        End Property

        Public Property IpStandardUser() As String
            Get
                Return m_strIpStandardUser
            End Get
            Set(ByVal Value As String)
                m_strIpStandardUser = Value
            End Set
        End Property

        Public Property IpRestriction() As Boolean
            Get
                Return m_blnIpRestriction
            End Get
            Set(ByVal Value As Boolean)
                m_blnIpRestriction = Value
            End Set
        End Property

        Public Property ForcePasswordQuestion() As Boolean
            Get
                Return m_blnForcePasswordQuestion
            End Get
            Set(ByVal Value As Boolean)
                m_blnForcePasswordQuestion = Value
            End Set
        End Property

        Public ReadOnly Property PwdDontSendEmail() As Boolean
            Get
                Return m_blnPwdDontSendEmail
            End Get
        End Property

        Public ReadOnly Property NameInputOptional() As Boolean
            Get
                Return m_blnNameInputOptional
            End Get
        End Property

        Public ReadOnly Property LogoPath2() As String
            Get
                Return m_strLogoPath2
            End Get
        End Property

        Public ReadOnly Property LogoPath() As String
            Get
                Return m_strLogoPath
            End Get
        End Property

        Public ReadOnly Property LogoImage() As IO.MemoryStream
            Get
                'Dim fs As New IO.FileStream(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "")) & "\" & Me.LogoPath2, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim fs As New IO.FileStream("C:\Inetpub\wwwroot" & Replace(LogoPath2, "/", "\"), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim br As New IO.BinaryReader(fs)
                Dim ms As New IO.MemoryStream(br.ReadBytes(fs.Length))
                br.Close()
                fs.Close()
                Return ms
            End Get
        End Property

        Public ReadOnly Property LogoImage2() As IO.MemoryStream
            Get

                Dim fs As New IO.FileStream("C:\Inetpub\wwwroot" & Replace(LogoPath, "/", "\"), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim br As New IO.BinaryReader(fs)
                Dim ms As New IO.MemoryStream(br.ReadBytes(fs.Length))
                br.Close()
                fs.Close()
                Return ms
            End Get
        End Property

        Public ReadOnly Property DocuPath() As String
            Get
                Return m_strDocuPath
            End Get
        End Property

        Public ReadOnly Property CustomerId() As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property Selfadministration() As Integer
            Get
                Return m_selfAdministration
            End Get
        End Property

        Public ReadOnly Property Locked() As Boolean
            Get
                Return m_Locked
            End Get
        End Property

        Public ReadOnly Property SelfadministrationInfo() As String
            Get
                Return m_selfAdministrationInfo
            End Get
        End Property

        Public ReadOnly Property CustomerName() As String
            Get
                Return m_strCustomerName
            End Get
        End Property

        Public ReadOnly Property KUNNR() As String
            Get
                Return m_strKUNNR
            End Get
        End Property

        Public ReadOnly Property ReadDealer() As Boolean
            Get
                Return m_blnReadDealer
            End Get
        End Property

        Public ReadOnly Property IsMaster() As Boolean
            Get
                Return m_blnMaster
            End Get
        End Property

        Public ReadOnly Property AllowMultipleLogin() As Boolean
            Get
                Return m_blnAllowMultipleLogin
            End Get
        End Property

        Public ReadOnly Property CustomerContact() As Contact
            Get
                Return m_CustomerContact
            End Get
        End Property

        Public ReadOnly Property CustomerPasswordRules() As PasswordRules
            Get
                Return m_CustomerPasswordRules
            End Get
        End Property

        Public ReadOnly Property CustomerUsernameRules() As UsernameRules
            Get
                Return m_CustomerUsernameRules
            End Get
        End Property

        Public ReadOnly Property CustomerLoginRules() As LoginRules
            Get
                Return m_CustomerLoginRules
            End Get
        End Property

        Public ReadOnly Property CustomerStyle() As Style
            Get
                Return m_CustomerStyle
            End Get
        End Property

        Public ReadOnly Property MaxUser() As Integer
            Get
                Return m_intMaxUser
            End Get
        End Property

        Public ReadOnly Property ShowOrganization() As Boolean
            Get
                Return m_blnShowOrganization
            End Get
        End Property

        Public ReadOnly Property ShowDistrikte() As Boolean
            Get
                Return m_blnShowDistrikte
            End Get
        End Property
        '28.9. herausgenommen durch Seidel. Kann spaeter geloescht
        'werden, wenn Report20 mit der anderen Loesung (siehe dort)
        'funktioniert:
        'Public Property Applications() As DataTable
        '    Get
        '        Return m_tblApplications
        '    End Get
        '    Set(ByVal Value As DataTable)
        '        m_tblApplications = Value
        '    End Set
        'End Property

        Public ReadOnly Property OrgAdminRestrictToCustomerGroup() As Boolean
            Get
                Return m_blnOrgAdminRestrictToCustomerGroup
            End Get
        End Property

        Public Property DaysUntilLock As Integer
            Get
                Return m_DaysUntilLock
            End Get
            Set(value As Integer)
                m_DaysUntilLock = value
            End Set
        End Property

        Public Property DaysUntilDelete As Integer
            Get
                Return m_DaysUntilDelete
            End Get
            Set(value As Integer)
                m_DaysUntilDelete = value
            End Set
        End Property

#End Region

#Region " Functions "
        Private Sub GetCustomer(ByVal cn As SqlClient.SqlConnection)


            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM Customer WHERE CustomerID=@CustomerID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try
                While dr.Read
                    m_strCustomerName = dr("Customername").ToString
                    m_strDocuPath = dr("DocuPath").ToString


                    'ITA 2156 JJU20090219
                    '---------------------------------------------------------------
                    If dr("SelfAdministration") Is DBNull.Value Then
                        m_selfAdministration = 0
                    Else
                        m_selfAdministration = CInt((dr("SelfAdministration")))
                    End If

                    If dr("SelfAdministrationInfo") Is DBNull.Value Then
                        m_selfAdministrationInfo = ""
                    Else
                        m_selfAdministrationInfo = dr("SelfAdministrationInfo")
                    End If

                    If dr("Locked") Is DBNull.Value Then
                        m_Locked = False
                    Else
                        m_Locked = CBool((dr("Locked")))
                    End If
                    '---------------------------------------------------------------


                    m_blnForcePasswordQuestion = CBool(dr("ForcePasswordQuestion"))
                    m_strLogoPath2 = dr("LogoPath2").ToString
                    m_strKUNNR = dr("KUNNR").ToString
                    m_blnReadDealer = CBool(dr("ReadDealer"))
                    m_blnMaster = CBool(dr("Master"))
                    m_blnAllowMultipleLogin = CBool(dr("AllowMultipleLogin"))
                    m_CustomerContact = New Contact(dr("CName").ToString, _
                                                    dr("CAddress").ToString, _
                                                    dr("CMailDisplay").ToString, _
                                                    dr("CMail").ToString, _
                                                    dr("CWebDisplay").ToString, _
                                                    dr("CWeb").ToString, _
                                                    dr("KundePostfach").ToString, _
                                                    dr("KundeHotline").ToString, _
                                                    dr("KundeFax").ToString)
                    m_CustomerPasswordRules = New PasswordRules(CInt(dr("PwdNNumeric")), _
                                                                CInt(dr("PwdLength")), _
                                                                CInt(dr("PwdNCapitalLetters")), _
                                                                CInt(dr("PwdNSpecialCharacter")), _
                                                                CInt(dr("PwdHistoryNEntries")), _
                                                                CBool(dr("PwdDontSendEmail")), _
                                                                CBool(dr("NameInputOptional")))

                    m_CustomerLoginRules = New LoginRules(CInt(dr("LockedAfterNLogins")), _
                                                          CInt(dr("NewPwdAfterNDays")))
                    m_CustomerStyle = New Style(dr("LogoPath").ToString, dr("CssPath").ToString)
                    m_intMaxUser = CInt(dr("MaxUser"))
                    If TypeOf dr("AccountingArea") Is System.DBNull Then
                        m_intAccountingArea = -1
                    Else
                        m_intAccountingArea = CInt(dr("AccountingArea"))
                    End If
                    m_blnShowOrganization = CBool(dr("ShowOrganization"))
                    m_blnOrgAdminRestrictToCustomerGroup = CBool(dr("OrgAdminRestrictToCustomerGroup"))
                    m_blnPwdDontSendEmail = CBool(dr("PwdDontSendEmail"))
                    m_blnIpRestriction = CBool(dr("IpRestriction"))
                    If Not TypeOf dr("IpStandardUser") Is System.DBNull Then
                        m_strIpStandardUser = CStr(dr("IpStandardUser"))
                    Else
                        m_strIpStandardUser = ""
                    End If
                    m_blnNameInputOptional = CBool(dr("NameInputOptional"))
                    m_blnShowDistrikte = CBool(dr("Distrikte"))

                    m_strLogoPath2 = dr("LogoPath2").ToString
                    m_strLogoPath = dr("LogoPath").ToString

                    If Not TypeOf dr("DaysUntilLock") Is System.DBNull Then
                        m_DaysUntilLock = CInt(dr("DaysUntilLock"))
                    End If
                    If Not TypeOf dr("DaysUntilDelete") Is System.DBNull Then
                        m_DaysUntilDelete = CInt(dr("DaysUntilDelete"))
                    End If

                End While
            Catch ex As Exception
                Throw ex
            Finally
                dr.Close()
            End Try

            GetIpAddresses(cn)
        End Sub

        Public Sub GetIpAddresses(ByVal cn As SqlClient.SqlConnection)
            Dim da As New SqlClient.SqlDataAdapter()
            Dim tblReturn As New DataTable()
            Try
                Dim cmdGetAddresse As New SqlClient.SqlCommand("SELECT IpAddress FROM IpAddresses WHERE CustomerID=@CustomerID ORDER BY IpAddress", cn)
                cmdGetAddresse.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                da.SelectCommand = cmdGetAddresse
                da.Fill(tblReturn)
            Catch ex As Exception
                Throw ex
            Finally
                da.Dispose()
            End Try

            m_tblIpAddresses = tblReturn
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim L�schen des Kunden!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteGroupApps As String = "DELETE " & _
                                                   "FROM Rights " & _
                                                   "WHERE GroupID IN (SELECT DISTINCT Rights.GroupID " & _
                                                                     "FROM Rights INNER JOIN WebGroup " & _
                                                                       "ON Rights.GroupID = WebGroup.GroupID " & _
                                                                     "WHERE (WebGroup.CustomerID = @CustomerID))"

                Dim strDeleteGroup As String = "DELETE " & _
                                               "FROM WebGroup " & _
                                               "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomerApps As String = "DELETE " & _
                                                      "FROM CustomerRights " & _
                                                      "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomer As String = "DELETE " & _
                                                  "FROM Customer " & _
                                                  "WHERE CustomerID=@CustomerID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                'Group-Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteGroupApps
                cmd.ExecuteNonQuery()

                'Group loeschen
                cmd.CommandText = strDeleteGroup
                cmd.ExecuteNonQuery()

                'Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteCustomerApps
                cmd.ExecuteNonQuery()

                'Customer loeschen
                cmd.CommandText = strDeleteCustomer
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim L�schen des Kunden!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Kunden!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO Customer(Customername, " & _
                                               "KUNNR, " & _
                                               "ReadDealer, " & _
                                               "CName, " & _
                                               "CAddress, " & _
                                               "CMailDisplay, " & _
                                               "CMail, " & _
                                               "CWebDisplay, " & _
                                               "CWeb, " & _
                                               "KundePostfach, " & _
                                               "KundeHotline, " & _
                                               "KundeFax, " & _
                                               "Master, " & _
                                               "NewPwdAfterNDays, " & _
                                               "LockedAfterNLogins, " & _
                                               "PwdNNumeric, " & _
                                               "PwdLength, " & _
                                               "PwdNCapitalLetters, " & _
                                               "PwdNSpecialCharacter, " & _
                                               "PwdHistoryNEntries, " & _
                                               "LogoPath, " & _
                                               "LogoPath2, " & _
                                               "DocuPath, " & _
                                               "CssPath, " & _
                                               "AllowMultipleLogin, " & _
                                               "MaxUser, " & _
                                               "ShowOrganization, " & _
                                               "OrgAdminRestrictToCustomerGroup, " & _
                                               "PwdDontSendEmail, " & _
                                               "NameInputOptional, " & _
                                               "ForcePasswordQuestion, " & _
                                               "Distrikte, " & _
                                               "IpRestriction, " & _
                                               "IpStandardUser, " & _
                                               "AccountingArea, " & _
                                               "SelfAdministration, " & _
                                               "SelfAdministrationInfo, " & _
                                               "DaysUntilLock, " & _
                                               "DaysUntilDelete)" & _
                          "VALUES(@Customername, " & _
                                 "@KUNNR, " & _
                                 "@ReadDealer, " & _
                                 "@CName, " & _
                                 "@CAddress, " & _
                                 "@CMailDisplay, " & _
                                 "@CMail, " & _
                                 "@CWebDisplay, " & _
                                 "@CWeb, " & _
                                 "@KundePostfach, " & _
                                 "@KundeHotline, " & _
                                 "@KundeFax, " & _
                                 "@Master, " & _
                                 "@NewPwdAfterNDays, " & _
                                 "@LockedAfterNLogins, " & _
                                 "@PwdNNumeric, " & _
                                 "@PwdLength, " & _
                                 "@PwdNCapitalLetters, " & _
                                 "@PwdNSpecialCharacter, " & _
                                 "@PwdHistoryNEntries, " & _
                                 "@LogoPath, " & _
                                 "@LogoPath2, " & _
                                 "@DocuPath, " & _
                                 "@CssPath, " & _
                                 "@AllowMultipleLogin, " & _
                                 "@MaxUser, " & _
                                 "@ShowOrganization, " & _
                                 "@OrgAdminRestrictToCustomerGroup, " & _
                                 "@PwdDontSendEmail, " & _
                                 "@NameInputOptional," & _
                                 "@ForcePasswordQuestion," & _
                                 "@Distrikte," & _
                                 "@IpRestriction," & _
                                 "@IpStandardUser," & _
                                 "@AccountingArea," & _
                                 "@SelfAdministration," & _
                                 "@SelfAdministrationInfo," & _
                                 "@DaysUntilLock, " & _
                                 "@DaysUntilDelete); " & _
                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Customer " & _
                                          "SET Customername=@Customername, " & _
                                              "KUNNR=@KUNNR," & _
                                              "ReadDealer=@ReadDealer, " & _
                                              "CName=@CName, " & _
                                              "CAddress=@CAddress, " & _
                                              "CMailDisplay=@CMailDisplay, " & _
                                              "CMail=@CMail, " & _
                                              "CWebDisplay=@CWebDisplay, " & _
                                              "CWeb=@CWeb, " & _
                                              "KundePostfach=@Kundepostfach, " & _
                                              "KundeHotline=@KundeHotline, " & _
                                              "KundeFax=@KundeFax, " & _
                                              "Master=@Master, " & _
                                              "NewPwdAfterNDays=@NewPwdAfterNDays, " & _
                                              "LockedAfterNLogins=@LockedAfterNLogins," & _
                                              "PwdNNumeric=@PwdNNumeric, " & _
                                              "PwdLength=@PwdLength, " & _
                                              "PwdNCapitalLetters=@PwdNCapitalLetters, " & _
                                              "PwdNSpecialCharacter=@PwdNSpecialCharacter, " & _
                                              "PwdHistoryNEntries=@PwdHistoryNEntries, " & _
                                              "LogoPath=@LogoPath, " & _
                                              "LogoPath2=@LogoPath2, " & _
                                              "DocuPath=@DocuPath, " & _
                                              "CssPath=@CssPath, " & _
                                              "AllowMultipleLogin=@AllowMultipleLogin, " & _
                                              "MaxUser=@MaxUser, " & _
                                              "ShowOrganization=@ShowOrganization, " & _
                                              "OrgAdminRestrictToCustomerGroup=@OrgAdminRestrictToCustomerGroup, " & _
                                              "PwdDontSendEmail=@PwdDontSendEmail, " & _
                                              "NameInputOptional=@NameInputOptional, " & _
                                              "ForcePasswordQuestion=@ForcePasswordQuestion, " & _
                                              "Distrikte=@Distrikte, " & _
                                              "IpRestriction=@IpRestriction, " & _
                                              "IpStandardUser=@IpStandardUser, " & _
                                              "AccountingArea=@AccountingArea, " & _
                                              "SelfAdministration=@SelfAdministration, " & _
                                              "SelfAdministrationInfo=@SelfAdministrationInfo, " & _
                                              "Locked=@Locked, " & _
                                              "DaysUntilLock=@DaysUntilLock, " & _
                                              "DaysUntilDelete=@DaysUntilDelete " & _
                                          "WHERE CustomerID=@CustomerID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intCustomerID = -1 Then
                    cmd.CommandText = strInsert

                    Dim cmdCheckUserExits As New SqlClient.SqlCommand("SELECT COUNT(CustomerID) FROM Customer WHERE Customername=@Customername", cn)
                    cmdCheckUserExits.Parameters.AddWithValue("@Customername", m_strCustomerName)
                    If cmdCheckUserExits.ExecuteScalar.ToString <> "0" Then
                        Throw New Exception("Es existiert bereits ein Kunde  mit dieser Firmenbezeichnung im System! Bitte w�hlen sie eine andere Bezeichnung!")
                    End If
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                End If
                With cmd.Parameters
                    .AddWithValue("@Customername", m_strCustomerName)
                    .AddWithValue("@KUNNR", m_strKUNNR)
                    .AddWithValue("@ReadDealer", m_blnReadDealer)
                    .AddWithValue("@CName", m_CustomerContact.Name)
                    .AddWithValue("@CAddress", m_CustomerContact.Address)
                    .AddWithValue("@CMailDisplay", m_CustomerContact.MailDisplay)
                    .AddWithValue("@CMail", m_CustomerContact.Mail)
                    .AddWithValue("@CWebDisplay", m_CustomerContact.WebDisplay)
                    .AddWithValue("@CWeb", m_CustomerContact.Web)
                    .AddWithValue("@KundePostfach", m_CustomerContact.Kundenpostfach)
                    .AddWithValue("@KundeHotline", m_CustomerContact.Kundenhotline)
                    .AddWithValue("@KundeFax", m_CustomerContact.Kundenfax)

                    .AddWithValue("@AccountingArea", m_intAccountingArea)
                    .AddWithValue("@Master", m_blnMaster)
                    .AddWithValue("@NewPwdAfterNDays", m_CustomerLoginRules.NewPasswordAfterNDays)
                    .AddWithValue("@LockedAfterNLogins", m_CustomerLoginRules.LockedAfterNLogins)
                    .AddWithValue("@PwdNNumeric", m_CustomerPasswordRules.Numeric)
                    .AddWithValue("@PwdLength", m_CustomerPasswordRules.Length)
                    .AddWithValue("@PwdNCapitalLetters", m_CustomerPasswordRules.CapitalLetters)
                    .AddWithValue("@PwdNSpecialCharacter", m_CustomerPasswordRules.SpecialCharacter)
                    .AddWithValue("@PwdHistoryNEntries", m_CustomerPasswordRules.PasswordHistoryEntries)
                    .AddWithValue("@LogoPath", m_CustomerStyle.LogoPath)
                    .AddWithValue("@LogoPath2", m_strLogoPath2)
                    .AddWithValue("@DocuPath", m_strDocuPath)
                    .AddWithValue("@CssPath", m_CustomerStyle.CssPath)
                    .AddWithValue("@AllowMultipleLogin", m_blnAllowMultipleLogin)
                    .AddWithValue("@MaxUser", m_intMaxUser)
                    .AddWithValue("@ShowOrganization", m_blnShowOrganization)
                    .AddWithValue("@OrgAdminRestrictToCustomerGroup", OrgAdminRestrictToCustomerGroup)
                    .AddWithValue("@PwdDontSendEmail", m_CustomerPasswordRules.DontSendEmail)
                    .AddWithValue("@NameInputOptional", m_CustomerPasswordRules.NameInputOptional)
                    .AddWithValue("@ForcePasswordQuestion", m_blnForcePasswordQuestion)
                    .AddWithValue("@Distrikte", m_blnShowDistrikte)
                    .AddWithValue("@IpRestriction", m_blnIpRestriction)
                    .AddWithValue("@IpStandardUser", m_strIpStandardUser)
                    .AddWithValue("@SelfAdministration", m_selfAdministration)
                    .AddWithValue("@SelfAdministrationInfo", m_selfAdministrationInfo)
                    .AddWithValue("@Locked", m_Locked)
                    .AddWithValue("@DaysUntilLock", m_DaysUntilLock)
                    .AddWithValue("@DaysUntilDelete", m_DaysUntilDelete)

                End With


                If m_intCustomerID = -1 Then
                    'Wenn Customer neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intCustomerID = CInt(cmd.ExecuteScalar)

                    'Standard-Organisation anlegen
                    cmd = New SqlClient.SqlCommand()
                    cmd.Connection = cn

                    cmd.CommandText = "INSERT INTO Organization(OrganizationName, " & _
                                                                   "CustomerID, " & _
                                                                   "OrganizationReference) " & _
                                              "VALUES(@OrganizationName, " & _
                                                     "@CustomerID, " & _
                                                     "@OrganizationReference)"
                    With cmd.Parameters
                        .AddWithValue("@OrganizationName", "Standard")
                        .AddWithValue("@CustomerID", m_intCustomerID)
                        .AddWithValue("@OrganizationReference", "999")
                    End With
                    cmd.ExecuteNonQuery()

                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Kunden!", ex)
            End Try
        End Sub

        Public Function HasUser(ByVal strConnectionString As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Try
                Return HasUser(cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

        End Function
        Public Function HasUser(ByVal cn As SqlClient.SqlConnection) As Boolean
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebUser WHERE CustomerID=@CustomerID", cn)
            cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            If CInt(cmd.ExecuteScalar) > 0 Then
                Return True
            End If
            Return False
        End Function
#End Region

#Region " SubClasses "
        <Serializable()> Class PasswordRules
#Region " Membervariables "
            Private m_intPwdNNumeric As Integer
            Private m_intPwdLength As Integer
            Private m_intPwdNCapitalLetters As Integer
            Private m_intPwdNSpecialCharacter As Integer
            Private m_intPwdHistoryNEntries As Integer
            Private m_blnPwdDontSendEmail As Boolean
            Private m_blnNameInputOptional As Boolean
            Private m_strErrorMessage As String
            Private m_blnUsernameDontSendEmail As Boolean
#End Region

#Region " Constructor "
            Friend Sub New(ByVal intNumeric As Integer, _
                           ByVal intPwdLength As Integer, _
                           ByVal intPwdNCapitalLetters As Integer, _
                           ByVal intSpecialCharacter As Integer, _
                           ByVal intPwdHistoryNEntries As Integer, _
                           ByVal blnPwdDontSendEmail As Boolean, _
                           ByVal blnNameInputOptional As Boolean)
                m_intPwdNNumeric = intNumeric
                m_intPwdLength = intPwdLength
                m_intPwdNCapitalLetters = intPwdNCapitalLetters
                m_intPwdNSpecialCharacter = intSpecialCharacter
                m_intPwdHistoryNEntries = intPwdHistoryNEntries
                m_blnPwdDontSendEmail = blnPwdDontSendEmail
                m_blnNameInputOptional = blnNameInputOptional
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property Numeric() As Integer
                Get
                    Return m_intPwdNNumeric
                End Get
            End Property

            Public ReadOnly Property Length() As Integer
                Get
                    Return m_intPwdLength
                End Get
            End Property

            Public ReadOnly Property CapitalLetters() As Integer
                Get
                    Return m_intPwdNCapitalLetters
                End Get
            End Property

            Public ReadOnly Property SpecialCharacter() As Integer
                Get
                    Return m_intPwdNSpecialCharacter
                End Get
            End Property

            Public ReadOnly Property PasswordHistoryEntries() As Integer
                Get
                    Return m_intPwdHistoryNEntries
                End Get
            End Property

            Public ReadOnly Property DontSendEmail() As Boolean
                Get
                    Return m_blnPwdDontSendEmail
                End Get
            End Property

            Public ReadOnly Property NameInputOptional() As Boolean
                Get
                    Return m_blnNameInputOptional
                End Get
            End Property

            Public ReadOnly Property ErrorMessage() As String
                Get
                    Return m_strErrorMessage
                End Get
            End Property
#End Region

#Region " Functions "
            Public Function PasswordIsValid(ByVal strPassword As String) As Boolean
                m_strErrorMessage = ""

                If strPassword.Length < m_intPwdLength Then
                    m_strErrorMessage &= String.Format("<li>Das Kennwort muss mindestens {0} Zeichen lang sein.", m_intPwdLength)
                End If

                Dim intNNumeric As Integer
                Dim intNLetters As Integer
                Dim intNCapitalLetters As Integer
                Dim intNSpecialCharacter As Integer
                Dim chrPassword() As Char = strPassword.ToCharArray
                Dim _chr As Char
                Dim _int As Integer
                For Each _chr In chrPassword
                    _int = Asc(_chr)
                    If (_int >= 97) AndAlso (_int <= 122) Then
                        'Kleinbuchstabe
                        intNLetters += 1
                    ElseIf _int >= 65 AndAlso _int <= 90 Then
                        'Grossbuchstabe
                        intNCapitalLetters += 1
                    ElseIf _int >= 48 AndAlso _int <= 57 Then
                        'Zahl
                        intNNumeric += 1
                    Else
                        'Sonderzeichen
                        intNSpecialCharacter += 1
                    End If
                Next

                If m_intPwdNNumeric > intNNumeric Then
                    m_strErrorMessage &= String.Format("<li>Das Kennwort muss mindestens  {0} numerischen Zeichen enthalten.", m_intPwdNNumeric)
                End If
                If m_intPwdNCapitalLetters > intNCapitalLetters Then
                    m_strErrorMessage &= String.Format("<li>Das Kennwort muss mindestens  {0} alphanumerischen Zeichen (gross)  enthalten.", m_intPwdNCapitalLetters)
                End If
                If m_intPwdNSpecialCharacter > intNSpecialCharacter Then
                    m_strErrorMessage &= String.Format("<li>Das Kennwort muss mindestens  {0} Sonderzeichen enthalten.", m_intPwdNSpecialCharacter)
                End If

                If m_strErrorMessage.Length = 0 Then
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Function CreateNewPasswort(ByRef errorMessage As String) As String
                Dim pw As String = String.Empty
                Dim status As String = String.Empty

                pw = Crypto.RandomPassword(Length, Numeric, CapitalLetters, SpecialCharacter, status)

                If status <> String.Empty Then
                    errorMessage = status
                    Return String.Empty
                Else
                    Return pw
                End If
            End Function
#End Region
        End Class

        <Serializable()> Class UsernameRules
#Region " Membervariables "
            Private m_strErrorMessage As String
            Private m_blnUsernameDontSendEmail As Boolean
#End Region

#Region " Constructor "
            Friend Sub New(ByVal blnUsernameDontSendEmail As Boolean)
                m_blnUsernameDontSendEmail = blnUsernameDontSendEmail
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property DontSendEmail() As Boolean
                Get
                    Return m_blnUsernameDontSendEmail
                End Get
            End Property

            Public ReadOnly Property ErrorMessage() As String
                Get
                    Return m_strErrorMessage
                End Get
            End Property
#End Region

        End Class

        <Serializable()> Class LoginRules
#Region " Membervariables "
            Private m_intNewPwdAfterNDays As Integer
            Private m_intLockedAfterNLogins As Integer
#End Region

#Region " Constructor "
            Friend Sub New(ByVal intLockedAfterNLogins As Integer, _
                           ByVal intNewPasswordAfterNDays As Integer)
                m_intNewPwdAfterNDays = intNewPasswordAfterNDays
                m_intLockedAfterNLogins = intLockedAfterNLogins
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property NewPasswordAfterNDays() As Integer
                Get
                    Return m_intNewPwdAfterNDays
                End Get
            End Property

            Public ReadOnly Property LockedAfterNLogins() As Integer
                Get
                    Return m_intLockedAfterNLogins
                End Get
            End Property
#End Region
        End Class

        <Serializable()> Class Style
#Region " Membervariables "
            Private m_strLogoPath As String
            Private m_strCssPath As String
#End Region

#Region " Constructor "
            Friend Sub New(ByVal strLogoPath As String, _
                           ByVal strCssPath As String)
                m_strLogoPath = strLogoPath
                m_strCssPath = strCssPath
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property LogoPath() As String
                Get
                    Return m_strLogoPath
                End Get
            End Property

            Public ReadOnly Property CssPath() As String
                Get
                    Return m_strCssPath
                End Get
            End Property
#End Region
        End Class

#End Region

    End Class
End Namespace

' ************************************************
' $History: Customer.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 3.05.11    Time: 10:55
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 19.03.09   Time: 16:25
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2156 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.03.09   Time: 11:27
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2156 fertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.02.09   Time: 14:37
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2588
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2152 und 2158
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 13  *****************
' User: Uha          Date: 21.01.08   Time: 18:09
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1644: Erm�glicht Login nur mit IP und festgelegtem Benutzer
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 11  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Pa�wortversand im Web auf Benutzerwunsch
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 13.06.07   Time: 14:28
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' *****************  Version 9  *****************
' User: Uha          Date: 23.05.07   Time: 12:45
' Updated in $/CKG/Base/Base/Kernel/Security
' TESTSAPUsername und SAPUsername aus Tabelle Customer entfernt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Updated in $/CKG/Base/Base/Kernel/Security
' �nderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' *****************  Version 7  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************