<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report45.aspx.vb" Inherits="AppFFD.Report45" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TBODY>
								<TR>
									<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="top">
										<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD class="TaskTitle" width="150">&nbsp;</TD>
											</TR>
											<TR>
												<TD vAlign="center" width="150"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:linkbutton></TD>
											</TR>
										</TABLE>
										<P>&nbsp;</P>
										<P>&nbsp;</P>
									</TD>
									<TD vAlign="top">
										<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
											</TR>
										</TABLE>
										<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR>
												<TD vAlign="top" align="left">
													<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
														<TR>
															<TD class="TextLarge" vAlign="center" width="150"></TD>
															<TD class="TextLarge" vAlign="center"></TD>
														</TR>
													</TABLE>
													<table id="tableMain" runat="server">
														<tr>
															<td><asp:label id="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer</asp:label>&nbsp;
															</td>
															<td><asp:textbox id="txtFahrgestellnummer" MaxLength="17" Runat="server">WF0_XX</asp:textbox></td>
														</tr>
														<tr>
															<td><asp:label id="lbl_Kennzeichen" runat="server">Kennzeichen</asp:label>&nbsp;
															</td>
															<td><asp:textbox id="txtKennzeichen" MaxLength="15" Runat="server"></asp:textbox></td>
														</tr>
														<tr>
															<td><asp:label id="lbl_Ordernummer" runat="server">Ordernummer</asp:label>&nbsp;
															</td>
															<td><asp:textbox id="txtOrdernummer" MaxLength="4" Runat="server"></asp:textbox></td>
														</tr>
													</table>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<tr>
									<td></td>
									<td><asp:datagrid id="DataGrid1" runat="server" Width="100%" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" CellPadding="2">
											<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
											<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
										</asp:datagrid></td>
								</tr>
								<TR>
									<TD vAlign="top" width="150">&nbsp;</TD>
									<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
								</TR>
								<TR>
									<TD vAlign="top" width="150"></TD>
					</TD>
					<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
				</TR>
			</TABLE>
			</TD></TR></TBODY></TABLE></form>
	</body>
</HTML>