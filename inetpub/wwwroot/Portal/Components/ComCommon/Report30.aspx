<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report30.aspx.vb" Inherits="CKG.Components.ComCommon.Report30" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../PageElements/Styles.ascx" %>
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
							<TR>
								<TD class="PageNavigation" colSpan="2"><asp:label id="lblHead" runat="server"></asp:label><asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center"><asp:linkbutton id="cmdCreate" runat="server" CssClass="StandardButton">&#149;&nbsp;Erstellen</asp:linkbutton>&nbsp;</TD>
										</TR>
									</TABLE>
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
														<TD class="StandardTableAlternate" vAlign="top" width="150">Fahrgestellnummer</TD>
														<TD class="StandardTableAlternate" vAlign="center" width="170"><asp:textbox id="txtFahrgestellnummer" runat="server" Width="160px"></asp:textbox></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="30">&nbsp;&nbsp;</TD>
														<TD class="StandardTableAlternate" vAlign="center">&nbsp;</TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" width="150">Kennzeichen</TD>
														<TD class="TextLarge" vAlign="center" width="170"><asp:textbox id="txtKennzeichen" runat="server" Width="160px"></asp:textbox></TD>
														<TD class="TextLarge" vAlign="top" width="30">&nbsp;&nbsp;</TD>
														<TD class="TextLarge" vAlign="center">&nbsp;&nbsp;</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="top" width="150">Zulassungsdatum ab
														</TD>
														<TD class="StandardTableAlternate" vAlign="top" width="170"><asp:textbox id="txtAbDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAb" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button><asp:calendar id="calAbDatum" runat="server" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
														<TD class="StandardTableAlternate" vAlign="top" width="30">bis
														</TD>
														<TD class="StandardTableAlternate" vAlign="top"><asp:textbox id="txtBisDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBis" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button><asp:calendar id="calBisDatum" runat="server" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="top" width="150">Vergabedatum ab</TD>
														<TD class="TextLarge" vAlign="top" width="170"><asp:textbox id="txtAbVergDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectAbVerg" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button><asp:calendar id="calAbVergDatum" runat="server" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
														<TD class="TextLarge" vAlign="top" width="30">bis</TD>
														<TD class="TextLarge" vAlign="top"><asp:textbox id="txtBisVergDatum" runat="server" Width="130px"></asp:textbox><asp:button id="btnOpenSelectBisVerg" runat="server" Width="30px" CausesValidation="False" Height="22px" Text="..."></asp:button><asp:calendar id="calBisVergDatum" runat="server" Width="160px" BorderColor="Black" BorderStyle="Solid" CellPadding="0" Visible="False">
																<TodayDayStyle Font-Bold="True"></TodayDayStyle>
																<NextPrevStyle ForeColor="White"></NextPrevStyle>
																<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
																<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
																<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
																<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
																<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
															</asp:calendar></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center" width="150">Plakettenart</TD>
														<TD class="StandardTableAlternate" vAlign="center" colSpan="3"><asp:checkbox id="cbPlakettenartGruen" runat="server" Text="Gr�n" Checked="True"></asp:checkbox>&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:checkbox id="cbPlakettenartGelb" runat="server" Text="Gelb" Checked="True"></asp:checkbox>&nbsp;&nbsp;&nbsp;&nbsp;
															<asp:checkbox id="cbPlakettenartRot" runat="server" Text="Rot" Checked="True"></asp:checkbox></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
									&nbsp;&nbsp;
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top"><asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
