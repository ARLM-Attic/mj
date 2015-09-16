<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report203_01.aspx.vb"
    Inherits="AppARVAL.Report203_01" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Anzeige Report)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:LinkButton ID="cmdBack" runat="server" Visible="False" CssClass="StandardButton"
                                Enabled="False">&#149;&nbsp;Zur�ck</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" colspan="2">
                                        <asp:LinkButton ID="lnkCreateExcel" runat="server">Excelformat</asp:LinkButton>
                                        <asp:HyperLink ID="lnkShowCSV" runat="server" Visible="False" Target="_blank">CSV-Datei</asp:HyperLink>&nbsp;
                                        <asp:Label ID="lblDownloadTip" runat="server" Visible="False" Font-Size="8pt" Font-Bold="True">rechte Maustaste => Ziel speichern unter...</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink
                                            ID="lnkBack" runat="server" NavigateUrl="javascript:history.back()">zur�ck</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:LinkButton ID="cmdSave" runat="server" Visible="False" CssClass="StandardButton">Sichern</asp:LinkButton>
                                    </td>
                                    <tr>
                                        <td class="" width="100%" colspan="1">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td class="LabelExtraLarge" align="right">
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AllowPaging="True" AllowSorting="True"
                                                bodyHeight="300" CssClass="tableMain" bodyCSS="tableBody" headerCSS="tableHeader"
                                                PageSize="50" BackColor="White" AutoGenerateColumns="False">
                                                <AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
                                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="Hyperlink3" runat="server" Target='<%# "_blank" %>' CssClass="StandardButtonTable"
                                                                NavigateUrl='<%# "Report203_02.aspx?kunnr=" &amp; DataBinder.Eval(Container.DataItem, "Kundennr") %>'>Details</asp:HyperLink>&nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Halter" SortExpression="Halter" HeaderText="Halter">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="HOrt" SortExpression="HOrt" HeaderText="HOrt"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Kundennr" SortExpression="Kundennr" HeaderText="Kundennr.">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Vollmacht" SortExpression="Vollmacht"
                                                        HeaderText="Vollmacht"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Register" SortExpression="Register" HeaderText="Register">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Personalausweis" SortExpression="Personalausweis"
                                                        HeaderText="Personalausweis"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Datum Vollmacht" SortExpression="Datum Vollmacht"
                                                        HeaderText="Datum Vollmacht"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Gewerbeanmeld" SortExpression="Gewerbeanmeld."
                                                        HeaderText="Gewerbeanmeld."></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Einzugserm" SortExpression="Einzugserm"
                                                        HeaderText="Einzugserm."></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Versich_Best&#228;tigung" SortExpression="Versich_Best&#228;tigung"
                                                        HeaderText="Versich.Best&#228;tigung"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="RegDat" SortExpression="RegDat" HeaderText="RegDat">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Neue Vollmacht" SortExpression="Neue Vollmacht"
                                                        HeaderText="Neue Vollmacht"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="EVB Nummer" SortExpression="EVB Nummer"
                                                        HeaderText="EVB Nummer"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="g&#252;ltig ab" SortExpression="g&#252;ltig ab"
                                                        HeaderText="g&#252;ltig ab"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="g&#252;ltig bis" SortExpression="g&#252;ltig bis"
                                                        HeaderText="g&#252;ltig bis"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Bemerkung" SortExpression="Bemerkung"
                                                        HeaderText="Bemerkung"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Vollst" SortExpression="Vollst" HeaderText="Vollst.">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="KUNNR" SortExpression="KUNNR" HeaderText="KUNNR">
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle NextPageText="n&#228;chste&amp;gt;" PrevPageText="&amp;lt;vorherige"
                                                    HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                            </table>
                            <asp:Label ID="lblInfo" runat="server" Font-Bold="True" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="../../../PageElements/Footer.html" -->
                            <asp:Label ID="lblHidden" runat="server" Visible="False" Width="39px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ShowScript" runat="server" visible="False">
            <td>
                <script language="Javascript">
						<!--                    //
                    function FreigebenConfirm(Fahrgest, Vertrag, BriefNr, Kennzeichen) {
                        var Check = window.confirm("Wollen Sie f�r dieses Fahrzeug wirklich den Status 'Bezahlt' setzen?\t\n\tFahrgestellnr.\t" + Fahrgest + "\t\n\tVertrag\t\t" + Vertrag + "\t\n\tKfz-Briefnr.\t" + BriefNr + "\n\tKfz-Kennzeichen\t" + Kennzeichen);
                        return (Check);
                    }
						//-->
                </script>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>