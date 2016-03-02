﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report10.aspx.cs" Inherits="AppRemarketing.forms.Report10"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <ClientEvents OnRequestStart="onRequestStart" />
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <script type="text/javascript">
                    function onRequestStart(sender, args) {
                        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                            args.set_enableAjax(false);
                        }
                    }
                </script>
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading" style="float: none">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <div id="paginationQuery" style="float: none">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="active">
                                    Neue Abfrage
                                </td>
                                <td align="right">
                                    <div id="queryImage">
                                        <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                            ToolTip="Abfrage öffnen" Visible="false" OnClick="NewSearch_Click" />
                                        <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen" ImageUrl="../../../Images/queryArrowUp.gif"
                                            Visible="false" OnClick="NewSearchUp_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lbCreate">
                        <div id="TableQuery" runat="server" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tr id="tr_Fahrgestellnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Fahrgestellnummer:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Kennzeichen" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Kennzeichen:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_Inventarnummer" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Inventarnummer:
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtInventarnummer" runat="server" CssClass="TextBoxNormal" MaxLength="10"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr_Vermieter" class="formquery">
                                    <td class="firstLeft active" style="width: 180px">
                                        Autovermieter:
                                    </td>
                                    <td class="active">
                                        <asp:DropDownList ID="ddlVermieter" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr_Hereinnahmecenter" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap" style="height: 22px">
                                        Hereinnahmecenter:
                                    </td>
                                    <td class="active">
                                        <span>
                                            <asp:DropDownList ID="ddlHC" runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </td>
                                </tr>
                                <tr id="tr_DatumVon" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Erstellungsdatum von:
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtDatumVon" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumvon" runat="server" Format="dd.MM.yyyy"
                                            PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumVon" runat="server" TargetControlID="txtDatumVon"
                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                        <asp:Label ID="lblDatVonError" runat="server" EnableViewState="False" Font-Bold="True"
                                            Font-Names="Verdana" Font-Size="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="tr_DatumBis" runat="server" class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Erstellungsdatum bis:
                                    </td>
                                    <td class="active" nowrap="nowrap">
                                        <asp:TextBox ID="txtDatumBis" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CE_Datumbis" runat="server" Animated="false" Enabled="True"
                                            Format="dd.MM.yyyy" PopupPosition="BottomLeft" TargetControlID="txtDatumBis">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" InputDirection="LeftToRight"
                                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtDatumBis">
                                        </cc1:MaskedEditExtender>
                                        <asp:Label ID="lblDatBisError" runat="server" EnableViewState="False" Font-Bold="True"
                                            Font-Names="Verdana" Font-Size="10px" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="tr_Vertragsjahr" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Vertragsjahr" runat="server">lbl_Vertragsjahr</asp:Label>
                                    </td>
                                    <td class="active">
                                        <asp:TextBox ID="txtVertragsjahr" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClientClick="Show_BusyBox1();" OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false" >
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" EnableHeaderContextMenu="true" 
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemCreated="rgGrid1_ItemCreated" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource" 
                                            OnItemDataBound="rgGrid_ItemDataBound" ShowGroupPanel="True" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowColumnsReorder="true" AllowKeyboardNavigation="true" AllowDragToGroup="true">
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                                <Resizing AllowColumnResize="True" ClipCellContentOnResize="False" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="true" 
                                                CommandItemDisplay="Top"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="FAHRGNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" GroupByExpression="FAHRGNR GROUP BY FAHRGNR" >   
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZ" SortExpression="KENNZ" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="INVENTAR" SortExpression="INVENTAR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AVNR" SortExpression="AVNR" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCORT" SortExpression="HCORT" >
                                                        <HeaderStyle Width="115px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="MODELLGRP" SortExpression="MODELLGRP" >
                                                        <HeaderStyle Width="115px" />
                                                        <ItemStyle Wrap="false" />                                                        
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="HCEINGDAT" SortExpression="HCEINGDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GUTAUFTRAGDAT" SortExpression="GUTAUFTRAGDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn DataField="MODELL" SortExpression="MODELL" >
                                                        <HeaderStyle Width="75px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn Groupable="false" UniqueName="Bearbeiten" >
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="Show"
                                                                ImageUrl="/services/images/info.gif" ToolTip="Vorschäden anzeigen." Visible='<%# Eval("LFDNR").ToString()!="000" %>'
                                                                CommandArgument='<%# Eval("FAHRGNR") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid2" Visible="false" runat="server" PageSize="10" AllowSorting="true"  
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid2_NeedDataSource" OnItemDataBound="rgGrid_ItemDataBound" >
                                            <ClientSettings AllowKeyboardNavigation="true">
                                                <Scrolling ScrollHeight="200px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true"  >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="LFDNR" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="LFDNR" SortExpression="LFDNR" >
                                                        <HeaderStyle Width="50px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="AVNR" SortExpression="AVNR" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="FAHRGNR" SortExpression="FAHRGNR" GroupByExpression="FAHRGNR GROUP BY FAHRGNR" >
                                                        <HeaderStyle Width="140px" />
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkHistorie" Target="_blank" ToolTip="Zur Fahrzeughistorie" runat="server"
                                                                Text='<%# Eval("FAHRGNR") %>'>
                                                            </asp:HyperLink>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="KENNZ" SortExpression="KENNZ" >
                                                        <HeaderStyle Width="85px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="PREIS" SortExpression="PREIS" DataFormatString="{0:c}" >
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SCHAD_DAT" SortExpression="SCHAD_DAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="105px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="BESCHREIBUNG" SortExpression="BESCHREIBUNG" >
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right; width: 100%; text-align: right; margin-top: 10px">
                        <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbtnBack_Click" Visible="false">» Zurück </asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

