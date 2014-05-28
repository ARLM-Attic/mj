<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Change01_1.aspx.vb" Inherits="AppFW.Change01_1" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
        <script language="JavaScript">										
				<!--
						function openinfo (url) {
								fenster=window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0");
								fenster.focus();
						}

				-->
        </script>
			<table cellSpacing="0" cellPadding="2" width="100%" align="center">
				<tr>
					<td><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:label></td>
							</TR>
							<tr>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"><asp:linkbutton id="cmdSave" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:linkbutton></TD>
										</TR>
									</TABLE>
								</TD>
								<td vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top"><asp:hyperlink id="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Change04.aspx">Adressauswahl</asp:hyperlink></TD>
										</TR>
									</TABLE>
									<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left" colSpan="3" height="41">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD class="LabelExtraLarge" align="left" width="618" height="9"><asp:label id="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:label>
														<!--<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl="/StartApplication/Anwendungen/Templates/Datum.htm" Target="_blank">Zulassungsdatum:</asp:HyperLink></TD> -->
														<TD noWrap align="right" height="9">
															<P align="right">&nbsp;
																<asp:dropdownlist id="ddlPageSize" runat="server" AutoPostBack="True" Height="14px"></asp:dropdownlist></P>
														</TD>
													</TR>
												</table>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
										<tr>
										<td colspan="3">
                                            <table id="tblUpload" cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr>
                                                    <td class="TextLarge" nowrap align="right">
                                                        Dateiauswahl <a href="javascript:openinfo('Info01.htm');">
                                                            <img src="../../../images/fragezeichen.gif" border="0"></a>:&nbsp;&nbsp;
                                                    </td>
                                                    <td class="TextLarge">
                                                        <input id="upFile" type="file" size="49" name="File1" runat="server" >&nbsp;
                                                    <asp:linkbutton id="lbDateiHinzufuegen" runat="server" CssClass="StandardButton">Hinzuf�gen</asp:linkbutton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TextLarge" nowrap align="right">
                                                        &nbsp;
                                                    </td>
                                                    <td class="TextLarge">
                                                        &nbsp;
                                                        <asp:Label ID="lblExcelfile" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
										</td>
										</tr>
										<tr>
											<TD vAlign="top" align="right">Weitere Fahrgestellnummer:&nbsp;&nbsp;</TD>
											<TD vAlign="top" align="left" colSpan="2"><asp:textbox id="txtVIN" runat="server">WF0_XX</asp:textbox>&nbsp;&nbsp;
												<asp:linkbutton id="cmdNeueVIN" runat="server" CssClass="StandardButton">Hinzuf�gen</asp:linkbutton></TD>
										</tr>
										<TR>
											<TD vAlign="top" align="left" colSpan="3"><asp:datagrid id="DataGrid1" runat="server" Width="100%" BackColor="White" PageSize="50" headerCSS="tableHeader" bodyCSS="tableBody" cssclass="tableMain" bodyHeight="400" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
													<AlternatingItemStyle CssClass="GridTableAlternate"></AlternatingItemStyle>
													<HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" SortExpression="CHASSIS_NUM"></asp:BoundColumn>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:LinkButton id="LinkButton1" runat="server" CssClass="StandardButtonTable" Text="L�schen" CausesValidation="false" CommandName="Delete">L�schen</asp:LinkButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Left" Position="Top" Wrap="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD width="120">&nbsp;</TD>
								<TD></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
