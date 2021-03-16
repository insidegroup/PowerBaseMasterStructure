<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnit – CDRs</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete CDR</th>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.DisplayNameAlias) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_DisplayName">Back Office Display Name</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_EntryFormat">Entry Format</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.EntryFormat) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_Mandatory">Mandatory?</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.MandatoryFlag)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_MinLength">Min Length</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.MinLength)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_MaxLength">Max Length</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.MaxLength) %></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_Droplist">Drop List?</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.TableDrivenFlag)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_OnlineDefaultValue">Online Default Value</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItem.OnlineDefaultValue)%></td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_ClientDefinedReferenceItemProducts">Product</label></td>
					<td colspan="2">
						<% if(Model.ClientDefinedReferenceItem.ClientDefinedReferenceItemProducts != null) { %>
							<% =Html.Encode(String.Join(", ", Model.ClientDefinedReferenceItem.ClientDefinedReferenceItemProducts.Select(x => x.Product.ProductName))) %>
						<% } %>
					</td>
				</tr>
				<tr>
					<td><label for="ClientDefinedReferenceItem_ClientDefinedReferenceItemContexts">Context</label></td>
					<td colspan="2">
						<% if(Model.ClientDefinedReferenceItem.ClientDefinedReferenceItemContexts != null) { %>
							<% =Html.Encode(String.Join(", ", Model.ClientDefinedReferenceItem.ClientDefinedReferenceItemContexts.Select(x => x.Context.ContextName))) %>
						<% } %>
					</td>
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
						<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.VersionNumber) %>
						<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemId) %>
						<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
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
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List" }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDRs" })%> &gt;
Delete CDR
</asp:Content>