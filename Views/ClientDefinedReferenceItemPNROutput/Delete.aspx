<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemPNROutputVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Account – CDRs – PNR Format
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Account – CDRs – PNR Format</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete PNR Format</th>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNRFormat_ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
					<td colspan="2">
                        <% if (Model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem != null && Model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem.DisplayNameAlias != null) { %>
                           <%= Html.Encode(!string.IsNullOrEmpty(Model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem.DisplayNameAlias) ? Model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem.DisplayNameAlias : "")%>
                        <% }  %>
                    </td>
				</tr>
                <tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_GDSCode">GDS</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.GDS.GDSName) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_PNROutputRemarkTypeCode">Remark Type</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.PNROutputRemarkType.PNROutputRemarkTypeName) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNROutput__GDSRemarkQualifier">Remark Qualifier</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.GDSRemarkQualifier)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_DefaultRemark">Remark</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.DefaultRemark)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_Language">Language</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.Language.LanguageName) %></td>
				</tr>
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
					</td>                    
					<td class="row_footer_blank_right">
					<% using (Html.BeginForm()) { %>
						<%= Html.AntiForgeryToken() %>
						<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
						<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.VersionNumber) %>
						<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId) %>
						<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId) %>
						<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
					<%}%>
					</td> 
				</tr>
			</table>
		</div>
	</div>
	<script type="text/javascript">
	$(document).ready(function () {
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#breadcrumb').css('width', 'auto');
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Client Accounts" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitClientAccountClientAccountName"].ToString(), "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = ViewData["ClientSubUnitClientAccountClientAccountName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid, ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = "CDRs" })%> &gt;
<%=Html.RouteLink(ViewData["DsplayName"].ToString(), "Main", new { controller = "ClientDefinedReferenceItem", action = "View",  id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu =Model.ClientSubUnit.ClientSubUnitGuid.ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = Model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItem.DisplayNameAlias })%> &gt;
<%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.DefaultRemark)%>
</asp:Content>