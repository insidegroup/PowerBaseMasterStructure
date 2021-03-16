<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemValueVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Accounts – CDRs</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete Drop List Item</th>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_Value">Value</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemValue.Value) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_ValueDescription">Value Description</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemValue.ValueDescription) %></td>
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
						<%= Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.VersionNumber) %>
						<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
						<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId) %>
						<%= Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId) %>
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
		$("#breadcrumb").css("width", "auto");
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "CDRs" })%> &gt;
<%if(ViewData["ClientDefinedReferenceItemDisplayNameAlias"] != "") { %>
	<%=Html.RouteLink(ViewData["ClientDefinedReferenceItemDisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "View", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientDefinedReferenceItemDisplayNameAlias"].ToString() })%> &gt;
<% } %>
<%=Html.RouteLink("Drop List Values", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemValue", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "Drop List Values" })%> &gt;
Delete Drop List Item
</asp:Content>
