﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change17Edit.aspx.vb" Inherits="AppFFE.Change17Edit" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
		<uc1:styles id="ucStyles" runat="server"></uc1:styles>
	    <style type="text/css">
            .style2
            {
                width: 115px;
            }
            #Table4
            {
                width: 874px;
            }
            .style3
            {
                width: 130px;
            }
        </style>
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center">
				<tr>
					<td colSpan="3"><uc1:header id="ucHeader" runat="server"></uc1:header></td>
				</tr>
				<TR>
					<TD colSpan="3">
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<td class="PageNavigation" colSpan="2" height="19"><asp:label id="lblHead" runat="server"></asp:label>&nbsp;(<asp:label id="lblPageTitle" runat="server"></asp:label>)</td>
							</TR>
							<TR>
								<TD vAlign="top" width="120">
									<TABLE id="Table2" borderColor="#ffffff" cellSpacing="0" cellPadding="0" width="120" border="0">
										<TR>
											<TD class="TaskTitle">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:linkbutton id="cmdSave" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Sichern</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:linkbutton id="cmdAuthorize" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Autorisieren</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:linkbutton id="cmdDelete" runat="server" CssClass="StandardButton" Visible="False">&#149;&nbsp;Löschen</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150">
												<asp:linkbutton id="cmdBack" runat="server" Visible="False" CssClass="StandardButton"> &#149;&nbsp;Zurück</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150" height="19"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
										<TR>
											<TD vAlign="center" width="150"></TD>
										</TR>
									</TABLE>
								</TD>
								<TD>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="TaskTitle" vAlign="top">&nbsp;<asp:hyperlink id="lnkHaendlerSuche" runat="server" CssClass="TaskTitle" NavigateUrl="Change17.aspx">Händlersuche</asp:hyperlink>&nbsp;</TD>
										</TR>
									</TABLE>
									<TABLE id="Table3" cellSpacing="0" cellPadding="5" width="100%" bgColor="white" border="0">
										<tr>
											<td class="TextLarge" vAlign="top">Händlernummer:</td>
											<td class="TextLarge" vAlign="top" width="100%" colSpan="2">
												<asp:label id="lblHaendlerNummer" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="StandardTableAlternate" vAlign="top">Name:&nbsp;&nbsp;
											</td>
											<td class="StandardTableAlternate" vAlign="top" colSpan="2">
												<asp:label id="lblHaendlerName" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="TextLarge" vAlign="top">Adresse:</td>
											<td class="TextLarge" vAlign="top" colSpan="2">
												<asp:label id="lblAdresse" runat="server"></asp:label></td>
										</tr>
									</TABLE>
									<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td></td>
										</tr>
										<TR>
											<td height="64">
												<TABLE id="Table4" height="58" cellSpacing="0" cellPadding="0" border="0">
													<TR id="trPflegelabel" runat="server">
														<TD class="style2">Mehrfachpflege</TD>
														<TD width="54"></TD>
														<TD width="54">&nbsp;</TD>
														<TD></TD>
														<TD width="111"></TD>
														<TD width="111"></TD>
													</TR>
													<TR id="trPflege" runat="server">
														<TD class="style2"><asp:label id="lblPflege" runat="server" Width="104px" 
                                                                style="margin-right: 0px">Neue Fälligkeit <br> in Tagen:</asp:label></TD>
														<TD width="54"><asp:textbox id="txt_NF" runat="server" Width="36px" MaxLength="3"></asp:textbox></TD>
														<TD width="54">&nbsp;</TD>
														<TD>&nbsp;<asp:label id="lblPflege0" runat="server" Width="56px" Height="16px">gültig 
                                                            für:</asp:label></TD>
														<TD width="111"><asp:checkbox id="chk_temp" runat="server" Width="142px" 
                                                                Text="Standard Temporär" Visible="False" Height="21px"></asp:checkbox></TD>
														<TD width="140"><asp:checkbox id="chk_endgueltig" runat="server" Width="136px" Text="Standard Endgültig" Checked="True" Visible="False"></asp:checkbox></TD>
														<TD width="94"><asp:checkbox id="chk_Retail" runat="server" Width="67px" 
                                                                Text="Retail" Checked="True" Visible="False"></asp:checkbox></TD>
														<TD class="style3">
                                                            <asp:checkbox id="chk_dp" runat="server" Width="66px" Text="KF\KL" 
                                                                Visible="False" style="margin-right: 0px" Height="20px"></asp:checkbox></TD>
														<TD width="159"><asp:checkbox id="chk_kf" runat="server" Width="128px" Text="Delayed Payment" Visible="False"></asp:checkbox></TD>
														<TD width="159"><asp:linkbutton id="btn_Ok" runat="server" CssClass="StandardButton"> &#149;&nbsp;Übernehmen</asp:linkbutton></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
										<TR>
											<TD></TD>
										</TR>
										<TR >
											<TD><asp:datagrid id="DataGrid1" runat="server"  Width="100%" BackColor="White" AutoGenerateColumns="False" CellPadding="3">
													<AlternatingItemStyle CssClass="StandardTableAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="TextLarge"></ItemStyle>
													<HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="KontingentID" HeaderText="KontingentID"></asp:BoundColumn>
														<asp:BoundColumn DataField="Kontingentart" HeaderText="Kontingentart"></asp:BoundColumn>
														<asp:BoundColumn DataField="Alte Zahlungsfrist"  HeaderText="Alte F&#228;lligkeit "></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Neue F&#228;lligkeit in Tagen">
															<ItemTemplate>
																<asp:TextBox id="txtZahlungsfristNeu" runat="server" Width="34px" MaxLength="2"></asp:TextBox>
															</ItemTemplate>
															<EditItemTemplate>
																<asp:TextBox id=TextBox1 runat="server" Width="31px" Text='<%# DataBinder.Eval(Container, "DataItem.Neue Zahlungsfrist") %>' MaxLength="2">
																</asp:TextBox>
															</EditItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="true" >
                                                         
														<ItemTemplate>
                                                          
														<asp:Literal Text='</TD></TR><TR align="center"><TD colspan="4">' ID="litHirarBeginn"
                                                            runat="server" EnableViewState="true">
														</asp:Literal>
														<asp:DataGrid ID="DataGridLvL2" runat="server" AutoGenerateColumns="False" BorderWidth="1" BackColor="Transparent" Width="80%">
                                                             <ItemStyle CssClass="TextLarge"></ItemStyle>
                                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead"></HeaderStyle>
														<Columns>
														<asp:BoundColumn DataField="ZZVGRUND"  Visible="false"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AbrufGrundText" HeaderText="Versandgrund" Visible="True">
                                                            </asp:BoundColumn >
                                                     
                                                     
                                                            <asp:BoundColumn DataField="ZZFRIST" HeaderText="Alte Fälligkeit"></asp:BoundColumn>
														<asp:TemplateColumn HeaderText="Neue Fälligkeit in Tagen">
                                                            <ItemTemplate >
                                                                <asp:TextBox ID="txtNeueZahlungsfristProAbrufgrund" runat="server" Width="31px" Text='<%# DataBinder.Eval(Container, "DataItem.NeueZFProAG") %>'
                                                                    MaxLength="2">
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                           </asp:TemplateColumn>
														</Columns>
														</asp:DataGrid>
                                                            <asp:Literal Text='</TD></TR>' ID="litHirarEnd" runat="server"
                                                                EnableViewState="true">
																		
                                                            </asp:Literal>
                                                          
														</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle NextPageText="n&#228;chste&amp;gt;" Font-Size="12pt" Font-Bold="True" PrevPageText="&amp;lt;vorherige" HorizontalAlign="Center" Wrap="False"></PagerStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
									<TABLE id="Table8" cellSpacing="0" cellPadding="5" width="100%" border="0">
										<TR id="ConfirmMessage" runat="server">
											<TD class="LabelExtraLarge"></TD>
										</TR>
										<TR>
											<TD>
												<asp:label id="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
									<asp:label id="lblInformation" CssClass="TextLarge" runat="server"></asp:label>
								</TD>
							</TR>
							<tr>
								<td>&nbsp;</td>
								<td><!--#include File="../../../PageElements/Footer.html" --></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<tr id="FocusScript" runat="server" Visible="False">
					<td colSpan="3">
						<script language="JavaScript">
							<!-- //
							window.document.Form1.elements[4].focus();
							//-->
						</script>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
