<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Report19.aspx.vb" Inherits="AppSIXT.Report19" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:Styles id="ucStyles" runat="server"></uc1:Styles>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table4" width="100%" align="center">
				<TR>
					<td><uc1:Header id="ucHeader" runat="server"></uc1:Header></td>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table0" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="PageNavigation" colSpan="2" height="19">
									<asp:Label id="lblHead" runat="server"></asp:Label>
									<asp:label id="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:label>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle" width="150">
												&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:LinkButton id="cmdCreate" runat="server" CssClass="StandardButton"> &#149;&nbsp;Erstellen</asp:LinkButton></TD>
										</TR>
									</TABLE>
									<asp:calendar id="calVon" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
									<asp:calendar id="calBis" runat="server" Visible="False" Width="120px" BorderStyle="Solid" BorderColor="Black" CellPadding="0">
										<TodayDayStyle Font-Bold="True"></TodayDayStyle>
										<NextPrevStyle ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Bold="True" BackColor="Silver"></DayHeaderStyle>
										<SelectedDayStyle BackColor="#FF8080"></SelectedDayStyle>
										<TitleStyle Font-Bold="True" ForeColor="White" BackColor="Black"></TitleStyle>
										<WeekendDayStyle ForeColor="Silver"></WeekendDayStyle>
										<OtherMonthDayStyle ForeColor="Silver"></OtherMonthDayStyle>
									</asp:calendar>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;</TD>
										</TR>
										<TR>
											<TD class="" vAlign="top">
												<asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" align="left">
												<TABLE id="Table1" cellSpacing="0" cellPadding="5" width="100%" border="0" bgColor="white">
													<TR>
														<TD class="TextLarge" vAlign="center">PDI:
														</TD>
														<TD class="TextLarge" vAlign="center" width="100%">
															<asp:TextBox id="txtPDI" runat="server"></asp:TextBox>&nbsp;
														</TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center" noWrap>Zulassungsdatum von:</TD>
														<TD class="StandardTableAlternate" vAlign="center">
															<asp:TextBox id="txtZulassungsdatumVon" runat="server"></asp:TextBox>
															<asp:Label id="lblInputReq" runat="server" CssClass="TextError">*</asp:Label>
															&nbsp;<asp:LinkButton id="btnCal1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD class="TextLarge" vAlign="center">Zulassungsdatum bis:</TD>
														<TD class="TextLarge" vAlign="center">
															<asp:TextBox id="txtZulassungsdatumBis" runat="server"></asp:TextBox>
															<asp:Label id="Label1" runat="server" CssClass="TextError">*</asp:Label>
															&nbsp;<asp:LinkButton id="LinkButton1" runat="server" CssClass="StandardButtonTable" Width="100px">Kalender...</asp:LinkButton></TD>
													</TR>
													<TR>
														<TD class="StandardTableAlternate" vAlign="center">Fahrgestellnummer:</TD>
														<TD class="StandardTableAlternate" vAlign="center">
															<asp:TextBox id="txtFahrgestellnummer" runat="server"></asp:TextBox></TD>
													</TR>
												</TABLE>
												&nbsp;
												<BR>
											</TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD vAlign="top">
									<asp:Label id="Label2" runat="server" CssClass="TextError">*</asp:Label>
									<asp:label id="lblInfo" runat="server" CssClass="TextInfo" EnableViewState="False">Eingabe erforderlich, wenn PDI gef�llt. Format: TT.MM.JJJJ. Der Datumsbereich darf maximal einen Monat (30 Tage) umfassen.</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">&nbsp;</TD>
								<TD><!--#include File="../../../PageElements/Footer.html" --></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>