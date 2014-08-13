<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InfocenterAdmin.aspx.vb"
    Inherits="Admin.InfocenterAdmin" MasterPageFile="MasterPage/Admin.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI"  TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zur�ck"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading" style="float: none">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                            <asp:LinkButton ID="lbtnEditDocTypes" runat="server" Text="Dokumentarten pflegen" style="margin-left: 600px; color: #FFFFFF; text-decoration: underline" ></asp:LinkButton>
                        </div>
                        <div id="TableQuery">
                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                        </div>
                        <div id="Result" runat="Server" style="width: 100%">
                            <div style="display: none">
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            </div>
                            <div id="data" runat="server">
                                <div>
                                    <telerik:RadGrid ID="rgDokumente" runat="server" PageSize="10" AllowPaging="true" AllowSorting="true" 
                                        AutoGenerateColumns="False" GridLines="None">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" GroupsDefaultExpanded="true">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="DocTypeName" SortOrder="Ascending" />
                                                <telerik:GridSortExpression FieldName="LastEdited" SortOrder="Descending" />
                                            </SortExpressions>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="DocTypeId"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="DocumentId" DataField="DocumentId" visible="false" />
                                                <telerik:GridTemplateColumn SortExpression="FileName" HeaderText="Dokument" HeaderButtonType="TextButton">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lbtPDF" runat="server" CommandName="showDocument" Height="18px" ImageUrl="images/iconPDF.gif" 
                                                            ToolTip="PDF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "pdf" %>' />
                                                        <asp:ImageButton ID="lbtExcel" runat="server" CommandName="showDocument" Height="18px" ImageUrl="Images/iconXLS.gif" 
                                                            ToolTip="Excel" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("xls") %>' />
                                                        <asp:ImageButton ID="lbtWord" runat="server" CommandName="showDocument" Height="18px" ImageUrl="Images/Word_Logo.jpg"
                                                            ToolTip="Word" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("doc") %>' />
                                                        <asp:ImageButton ID="lbtJepg" runat="server" CommandName="showDocument" Height="18px" ImageUrl="Images/Fotos.jpg"
                                                            ToolTip="JPG" ImageAlign="Middle" Visible='<%# Eval("FileType").StartsWith("jp") %>' />
                                                        <asp:ImageButton ID="lbtGif" runat="server" CommandName="showDocument" Height="18px" ImageUrl="Images/Fotos.jpg"
                                                            ToolTip="GIF" ImageAlign="Middle" Visible='<%# Eval("FileType").ToString() = "gif" %>' />                                         
                                                        <span>
                                                            <asp:LinkButton ID="lbtDateiOeffnen" runat="server" CommandName="showDocument" Text='<%# Eval("FileName") %>' >
                                                            </asp:LinkButton>
                                                        </span>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn SortExpression="FileType" DataField="FileType" visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeId" DataField="docTypeId" Visible="false" />
                                                <telerik:GridBoundColumn SortExpression="DocTypeName" HeaderText="Art" AllowSorting="false" 
                                                    DataField="DocTypeName" HeaderStyle-Width="200px" />
                                                <telerik:GridButtonColumn UniqueName="colBearbeiten" HeaderStyle-Width="50px" ButtonType="ImageButton" ImageUrl="Images/Edit_01.gif" Text="bearbeiten" CommandName="editDocument" >
                                                </telerik:GridButtonColumn>
                                                <telerik:GridBoundColumn SortExpression="LastEdited" HeaderText="Letzte �nderung" HeaderButtonType="TextButton" 
                                                    DataField="LastEdited" DataFormatString="{0:dd.MM.yyyy HH:mm:ss}" HeaderStyle-Width="150px">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="colLoeschen" HeaderStyle-Width="100px">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ckb_SelectAll" runat="server" ToolTip="Auf dieser Seite alle Dokumente l�schen setzen."
                                                            AutoPostBack="True" OnCheckedChanged="ckb_SelectAll_CheckedChanged" />
                                                        <asp:Label ID="Label1" runat="server" Text="L�schen"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="rb_sel" runat="server" GroupName="Auswahl" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>
                            <div id="dataFooter" style="float: none">
                                <asp:Panel ID="PanelUpload" runat="server" style="margin-left: 430px">
                                    <asp:FileUpload ID="upFile" runat="server"/>
                                    <asp:LinkButton ID="lbtnUpload" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" style="vertical-align: baseline" >� hochladen</asp:LinkButton>
                                    <asp:LinkButton ID="lbtnLoeschen" runat="server" CssClass="Tablebutton" Width="78px" 
                                        Height="16px" style="vertical-align: baseline"  >� L�schen</asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Wollen Sie die markierten Dateien wirklich l�schen?"
                                        OnClientCancel="cancelClick" TargetControlID="lbtnLoeschen" />
                                </asp:Panel>
                            </div>
                        </div>
                        <div id="divEditDocument" runat="server" visible="false" style="width: 100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <div class="ExcelDiv">
                                            <asp:Label ID="lbl_EditDocumentHeader" runat="server" Text="Dokument bearbeiten" style="margin-left: 15px" ></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trDocumentRights" runat="server" class="formquery">
                                    <td class="firstLeft active" style="width: 90px">
                                        Dokument:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtFileName" runat="server" ReadOnly="true" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active" style="width: 90px">
                                        Dokumentart:
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList ID="ddlDokumentart" runat="server" Width="300px" />
                                    </td>
                                    <td class="firstLeft active" style="width: 90px; text-align: right">
                                        Gruppen:&nbsp;
                                    </td>
                                    <td class="active">
                                        <asp:ListBox ID="lbxDocumentGroups" runat="server" Width="300px" Rows="8" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelDocumentEditButtons" runat="server" style="margin-top: 10px; margin-left: 720px">
                                <asp:LinkButton class="Tablebutton" ID="lbtnSaveDocument" runat="server" Width="78px" Height="16px" Text="Speichern" ></asp:LinkButton>
                                <asp:LinkButton class="Tablebutton" ID="lbtnCancelEditDocument" runat="server" Width="78px" Height="16px" Text="Verwerfen" ></asp:LinkButton>
                            </asp:Panel>
                            <div>
                                &nbsp;
                            </div>
                            <input type="hidden" id="ihSelectedDocumentId" runat="server" />
                        </div>
                        <div id="divEditDocTypes" runat="server" visible="false" style="width: 100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <div class="ExcelDiv">
                                            <asp:Label ID="lbl_EditDocTypesHeader" runat="server" Text="Dokumentarten pflegen" style="margin-left: 15px" ></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trEditDocType" runat="server">
                                    <td class="firstLeft active" style="width: 90px">
                                        Dokumentart:
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList ID="ddlDocTypeSelection" runat="server" Width="300px" AutoPostBack="true" />
                                    </td>
                                    <td class="firstLeft active" style="width: 90px; text-align: right">
                                        Bezeichnung:&nbsp;
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtDocTypeName" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trNewDocType" runat="server" visible="false">
                                    <td class="firstLeft active" style="width: 90px">
                                        Bezeichnung:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtNewDocType" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panelDocTypeEditButtons" runat="server" style="margin-top: 10px; margin-left: 400px">
                                <asp:LinkButton class="TablebuttonXLarge" ID="lbtnAddNewDocType" runat="server" Width="155px" Height="16px" Text="Neue Dok.art anlegen" ></asp:LinkButton>
                                <asp:LinkButton class="TablebuttonXLarge" ID="lbtnSaveNewDocType" runat="server" Width="155px" Height="16px" Text="Neue Dok.art speichern" Visible="false" ></asp:LinkButton>
                                <asp:LinkButton class="Tablebutton" ID="lbtnDeleteDocType" runat="server" Width="78px" Height="16px" Text="L�schen" ></asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Wollen Sie die Dokumentenart wirklich l�schen?"
                                    OnClientCancel="cancelClick" TargetControlID="lbtnDeleteDocType" />
                                <asp:LinkButton class="Tablebutton" ID="lbtnSaveDocType" runat="server" Width="78px" Height="16px" Text="Speichern" ></asp:LinkButton>
                                <asp:LinkButton class="TablebuttonXLarge" ID="lbtnCancelEditDocType" runat="server" Width="155px" Height="16px" Text="Verwerfen/Zur�ck" ></asp:LinkButton>
                            </asp:Panel>
                            <div>
                                &nbsp;
                            </div>
                        </div>
                    </div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="false">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" BackColor="White" Width="280" Height="150"
                        Style="display: none; border: solid 2px #C0C0C0">
                        <div style="padding-top: 10px; padding-bottom: 10px; text-align: center">
                            <asp:Label ID="lblFileUploadDialog" runat="server" Font-Bold="True">Die hochzuladende Datei existiert bereits.<br />Wie wollen Sie vorgehen?</asp:Label>
                        </div>
                        <div style="margin-left: 5px">
                            <asp:RadioButtonList ID="rblPopupOptions" runat="server">
                                <asp:ListItem Value="Beibehalten" Text="die vorhandene Datei beibehalten und die neue Datei unter anderem Namen speichern" Selected="True"/>
                                <asp:ListItem Value="Ersetzen" Text="die vorhandene Datei ersetzen" />
                            </asp:RadioButtonList>
                        </div>
                        <div>
                            &nbsp;
                        </div>
                        <div>
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:LinkButton Width="78px" Height="16px" class="Tablebutton" ID="lbtnPopupOK" runat="server" Text="OK" />
                                    </td>
                                    <td>
                                        <asp:LinkButton Width="78px" Height="16px" class="Tablebutton" ID="lbtnPopupCancel" runat="server" Text="Abbrechen" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

