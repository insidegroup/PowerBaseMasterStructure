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
						<th class="row_header" colspan="3">Edit Drop List Item</th>
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
						<td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Drop List Item" title="Edit Drop List Item" class="red" /></td>
					</tr>
				</table>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemValueId) %>
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.VersionNumber) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId) %>
			<% } %>
		</div>
	</div>
<script type="text/javascript" src="../../Scripts/ERD/ClientDefinedReferenceItemValue.js"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Client Accounts" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitClientAccountClientAccountName"].ToString(), "Main", new {controller = "ClientAccount", action = "ViewItem", ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = ViewData["ClientSubUnitClientAccountClientAccountName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid, ssc = Model.ClientDefinedReferenceItem.SourceSystemCode, can = Model.ClientDefinedReferenceItem.ClientAccountNumber}, new { title = "CDRs" })%> &gt;
<% if (Model.ClientDefinedReferenceItem.DisplayNameAlias != null && !string.IsNullOrEmpty(Model.ClientDefinedReferenceItem.DisplayNameAlias)){ %>
	<%=Html.RouteLink(Model.ClientDefinedReferenceItem.DisplayNameAlias, "Main", new { controller = "ClientDefinedReferenceItem", action = "View", id = Model.ClientDefinedReferenceItem.ClientDefinedReferenceItemId }, new { title = Model.ClientDefinedReferenceItem.DisplayNameAlias })%> &gt;
<% } %>
<%=Html.RouteLink("Drop List Values", "Main", new { controller = "ClientDefinedReferenceItemValue", action = "List", id = Model.ClientDefinedReferenceItemValue.ClientDefinedReferenceItemId }, new { title = "Drop List Values" })%> &gt; 
<%= Model.ClientDefinedReferenceItemValue.Value %>
</asp:Content>
