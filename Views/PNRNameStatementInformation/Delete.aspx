<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PNRNameStatementInformationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Account – SubUnit – Name Statement Information
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnit – Name Statement Information</div>
		</div>
		<div id="content">
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete Name Statement Information </th>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_GDSCode">GDS</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.GDS.GDSName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter1">Delimiter 1</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter1) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Field1_DisplayName">Statement Info 1</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Field1_DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter2">Delimiter 2</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter2) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Field2_DisplayName">Statement Info 2</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Field2_DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter3">Delimiter 3</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter3) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Field3_DisplayName">Statement Info 3</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Field3_DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter4">Delimiter 4</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter4) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Field4_DisplayName">Statement Info 4</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Field4_DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter5">Delimiter 5</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter5) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Field5_DisplayName">Statement Info 5</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Field5_DisplayName) %></td>
				</tr>
				<tr>
					<td><label for="PNRNameStatementInformation_Delimiter6">Delimiter 6</label></td>
					<td colspan="2"><%= Html.Encode(Model.PNRNameStatementInformation.Delimiter6) %></td>
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
						<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
						<%=Html.HiddenFor(model => model.PNRNameStatementInformation.PNRNameStatementInformationId) %>
						<%=Html.HiddenFor(model => model.PNRNameStatementInformation.ClientSubUnit.ClientSubUnitGuid) %>
						<%=Html.HiddenFor(model => model.PNRNameStatementInformation.ClientSubUnitGuid) %>
						<%=Html.HiddenFor(model => model.PNRNameStatementInformation.VersionNumber) %>
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
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
NSI &gt;
Delete
</asp:Content>