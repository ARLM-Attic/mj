<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UeberfPrint.aspx.vb" Inherits="AppUeberf.UeberfPrint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" type="text/javascript">

function printPage() {
if (window.print) {
jetztdrucken = confirm('Seite drucken ?');
if (jetztdrucken) window.print();
   }
}
		</script>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table10" cellSpacing="0" cellPadding="0" width="756" border="0" style="WIDTH: 756px; HEIGHT: 949px">
				<TR>
					<TD style="WIDTH: 1058px; HEIGHT: 30px" colSpan="2">
						<TABLE id="Table3" style="WIDTH: 755px; HEIGHT: 34px" cellSpacing="1" cellPadding="1" width="755" border="0">
							<TR>
								<TD style="WIDTH: 228px">
									<P align="right"><asp:label id="lblSchritt" runat="server" Font-Bold="True" Width="116px"> Ausdruck</asp:label></P>
								</TD>
								<TD style="WIDTH: 40px"></TD>
								<TD><asp:image id="imgLogo" Runat="server"></asp:image></TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD class="PageNavigation" style="WIDTH: 1058px" colSpan="2"></TD>
				</TR>
				<tr>
					<TD style="WIDTH: 100%" vAlign="top">
						<TABLE id="Table1" style="WIDTH: 100%; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 317px">
									<asp:label id="lblRefLabel" runat="server" Width="96px" Font-Bold="True" Font-Italic="True">Referenz:</asp:label>
									<asp:label id="lblReferenz" runat="server" Width="206px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD>
									<asp:label id="lblNameLabel" runat="server" Width="96px" Font-Bold="True" Font-Italic="True">Name:</asp:label>
									<asp:label id="lblLeasingnehmerName" runat="server" Width="242px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<asp:label id="lblTypLabel" runat="server" Width="96px" Font-Bold="True" Font-Italic="True">Fahrzeugtyp:</asp:label>
									<asp:label id="lblFahrzeugtyp" runat="server" Width="216px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
								<TD>
									<asp:label id="lblOrtLabel" runat="server" Width="96px" Font-Bold="True" Font-Italic="True">Ort:</asp:label>
									<asp:label id="lblLeasingnehmerOrt" runat="server" Width="278px" Font-Bold="True" Font-Italic="True"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label5" runat="server" Font-Bold="True">Abholung</asp:label></TD>
								<TD><asp:label id="Label15" runat="server" Font-Bold="True">Anlieferung</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label6" runat="server" Width="110px"> Name:</asp:label><asp:label id="lblAbName" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label13" runat="server" Width="106px">Name:</asp:label><asp:label id="lblAnName" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px" height="21"><asp:label id="Label7" runat="server" Width="110px">Stra�e, Nr:</asp:label><asp:label id="lblAbStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD height="21"><asp:label id="Label17" runat="server" Width="106px">Stra�e, Nr:</asp:label><asp:label id="lblAnStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label8" runat="server" Width="110px">PLZ Ort:</asp:label><asp:label id="lblAbOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label18" runat="server" Width="106px">PLZ Ort:</asp:label><asp:label id="lblAnOrt" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label9" runat="server" Width="110px">Ansprechpartner:</asp:label><asp:label id="lblAbAnsprechpartner" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label19" runat="server" Width="106px">Ansprechpartner:</asp:label><asp:label id="lblAnAnspechpartner" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label16" runat="server" Width="110px">Telefon:</asp:label><asp:label id="lblAbTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label20" runat="server" Width="106px">Telefon 1:</asp:label><asp:label id="lblAnTelefon" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<asp:label id="Label29" runat="server" Width="110px">Telefon 2:</asp:label>
									<asp:label id="lblAbTelefon2" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD>
									<asp:label id="Label31" runat="server" Width="110px">Telefon 2:</asp:label>
									<asp:label id="lblAnTelefon2" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lblFahrzeugdaten" runat="server" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label1" runat="server" Width="110px">Hersteller / Typ:</asp:label><asp:label id="lblHerst" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label11" runat="server" Width="242px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lblZugelassen" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label2" runat="server" Width="110px">Kennzeichen:</asp:label><asp:label id="lblKennzeichen" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label26" runat="server" Width="241px">Fahrzeugwert:</asp:label><asp:label id="lblFahrzeugwert" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label3" runat="server" Width="110px">Fgst.-Nummer:</asp:label><asp:label id="lblVin" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label12" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lblBereifung" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label4" runat="server" Width="110px">Referenz-Nr:</asp:label><asp:label id="lblRef" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label10" runat="server" Font-Bold="True">Dienstleistungsdetails</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 317px; BORDER-BOTTOM: black thin solid"><asp:label id="Label14" runat="server" Width="125px">�berf�hrung bis:</asp:label><asp:label id="lblDatumUeberf" runat="server" Font-Size="Smaller"></asp:label>&nbsp;
									<asp:label id="lblUeberfDatumFix" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label22" runat="server" Width="128px">Wagenw�sche:</asp:label><asp:label id="lblWW" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label21" runat="server" Width="125px">Wagen volltanken:</asp:label><asp:label id="lblTanken" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="Label23" runat="server" Width="128px">Fahrzeugeinweisung:</asp:label><asp:label id="lblEinw" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label25" runat="server" Width="125px">Rotes Kennzeichen:</asp:label><asp:label id="lblRotKenn" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD colSpan="2" vAlign="top">
									<asp:label id="Label39" runat="server" Width="125px" Height="35px">Winterreifen:</asp:label><asp:label id="lblWinterText" runat="server" Width="616px" Font-Size="Smaller" Height="34px"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label24" runat="server" Font-Bold="True" Width="125px">Bemerkung:</asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<P><asp:label id="lblBem" runat="server" Width="310px" Font-Size="Smaller"></asp:label></P>
								</TD>
								<TD></TD>
							</TR>
						</TABLE>
						<P>
							<hr style="WIDTH: 100%; HEIGHT: 2px" color="#000000" SIZE="2">
						<P></P>
						<TABLE id="Table5" style="WIDTH: 100%; HEIGHT: 67px" cellSpacing="0" cellPadding="1" width="918" border="0" runat="server">
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Anschluss" runat="server" Font-Bold="True">Anschlussfahrt</asp:label></TD>
								<TD><asp:label id="lbl1FzgDaten" runat="server" Font-Bold="True">Fahrzeugdaten</asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Name" runat="server" Width="107px">Name:</asp:label><asp:label id="lbl2ReName" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Herst" runat="server" Width="95px">Hersteller:</asp:label><asp:label id="lbl2ReHerst" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1StrNr" runat="server" Width="107px">Stra�e, Nr.:</asp:label><asp:label id="lbl2ReStrasse" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Kenn" runat="server" Width="95px">Kennzeichen:</asp:label><asp:label id="lbl2ReKenn" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<P><asp:label id="lbl1PLZOrt" runat="server" Width="107px">PLZ Ort:</asp:label><asp:label id="lbl2RePlzOrt" runat="server" Font-Size="Smaller"></asp:label></P>
								</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Ansprech" runat="server" Width="107px">Ansprechpartner:</asp:label><asp:label id="lbl2ReAnsprech" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Vin" runat="server" Width="95px">Fgst.-Nummer:</asp:label><asp:label id="lbl2ReVin" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lbl1Telefon" runat="server" Width="107px">Telefon 1:</asp:label><asp:label id="lbl2ReTelefon1" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD><asp:label id="lbl1Ref" runat="server" Width="95px">Referenz-Nr.:</asp:label><asp:label id="lbl2ReRef" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">
									<asp:label id="Label30" runat="server" Width="107px">Telefon 2:</asp:label>
									<asp:label id="lbl2ReTelefon2" runat="server" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"></TD>
								<TD><asp:label id="lbl1FzgZugelassen" runat="server" Width="242px">Fahrzeug zugelassen und betriebsbereit?</asp:label><asp:label id="lbl2ReZugelassen" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px">&nbsp;</TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="Label27" runat="server" Font-Bold="True" Width="125px">Bemerkung:</asp:label></TD>
								<TD><asp:label id="lbl1Bereifung" runat="server" Width="66px">Bereifung:</asp:label><asp:label id="lbl2ReBereif" runat="server" Font-Size="Smaller"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 317px"><asp:label id="lblReBemerkung" runat="server" Width="305px" Font-Size="Smaller"></asp:label></TD>
								<TD></TD>
							</TR>
						</TABLE>
						&nbsp;</TD>
				</tr>
			</TABLE>
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="781" border="0" style="WIDTH: 781px; HEIGHT: 57px">
				<TR>
					<TD style="WIDTH: 612px"><asp:label id="lblUser" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px"><asp:label id="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:label></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 612px"></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="WIDTH: 781px; HEIGHT: 60px" cellSpacing="1" cellPadding="1" width="781" border="0">
				<TR>
					<TD style="WIDTH: 384px">
						<P align="right">&nbsp;</P>
					</TD>
					<TD><A href="javascript:window.print()">Seite drucken</A></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 384px"></TD>
					<TD style="WIDTH: 110px">&nbsp;</TD>
					<TD></TD>
				</TR>
			</TABLE>
			</TD></TR></TABLE></form>
	</body>
</HTML>
