<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemPNROutputVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnits – CDRs – PNR Format
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnits – CDRs – PNR Format</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete PNR Format</th>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNRFormat_ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
					<td colspan="2"><%= Html.Encode(ViewData["DisplayNameAlias"])%></td>
				</tr>
                <tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_GDSCode">GDS</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.GDS.GDSName) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItemPNROutput_PNROutputRemarkTypeCode">Remark Type</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.PNROutputRemarkTypeCode) %></td>
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
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink(ViewData["DisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "View", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString()}, new { title = ViewData["DisplayNameAlias"].ToString() })%> &gt;
<%=Html.RouteLink("PNR Formats", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "PNR Formats" })%> &gt;
<%=Html.Encode(Model.ClientDefinedReferenceItemPNROutput.DefaultRemark) %> &gt;
Delete PNR Format
</asp:Content>