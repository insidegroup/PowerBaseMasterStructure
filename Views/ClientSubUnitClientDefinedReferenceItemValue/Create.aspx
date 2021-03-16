<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemValueVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">CDR – Drop List Values</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Create Drop List Item</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_Value">Value</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItemValue.Value, new { maxlength = 50 } )%><span class="error"> *</span></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemValue.Value)%>
							<label id="lblClientDefinedReferenceItem_Value"/>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_ValueDescription">Description</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItemValue.ValueDescription, new { maxlength = 50 } )%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemValue.ValueDescription)%></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Drop List Item" title="Create Drop List Item" class="red" /></td>
					</tr>
				</table>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId) %>
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
			<% } %>
		</div>
	</div>
<script type="text/javascript" src="../../Scripts/ERD/ClientDefinedReferenceItemValue.js"></script>
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
Create Drop List Item
</asp:Content>
