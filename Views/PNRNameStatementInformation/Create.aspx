<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PNRNameStatementInformationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - Name Statement Information
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnit - Name Statement Information</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Create Name Statement Information</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_GDSCode">GDS</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.GDSCode, Model.GDSList, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.GDSCode)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter1">Delimeter 1</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter1, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter1)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_StatementInfo1">Statement Info 1</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Field1_ReferToRecordIdentifier, Model.StatementInformationItems, "Please Select...", new { @Class = "referToRecordIdentifier" })%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Field1_ReferToRecordIdentifier)%>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field1_DisplayName) %>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field1_PNRMappingTypeCode) %>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter2">Delimeter 2</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter2, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter2)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_StatementInfo2">Statement Info 2</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Field2_ReferToRecordIdentifier, Model.StatementInformationItems, "Please Select...", new { @Class = "referToRecordIdentifier"} )%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Field2_ReferToRecordIdentifier)%>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field2_DisplayName) %>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field2_PNRMappingTypeCode) %>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter3">Delimeter 3</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter3, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter3)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_StatementInfo3">Statement Info 3</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Field3_ReferToRecordIdentifier, Model.StatementInformationItems, "Please Select...", new { @Class = "referToRecordIdentifier"} )%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Field3_ReferToRecordIdentifier)%>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field3_DisplayName) %>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field3_PNRMappingTypeCode) %>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter4">Delimeter 4</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter4, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter4)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_StatementInfo4">Statement Info 4</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Field4_ReferToRecordIdentifier, Model.StatementInformationItems, "Please Select...", new { @Class = "referToRecordIdentifier"} )%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Field4_ReferToRecordIdentifier)%>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field4_DisplayName) %>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field4_PNRMappingTypeCode) %>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter5">Delimeter 5</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter5, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter5)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_StatementInfo5">Statement Info 5</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Field5_ReferToRecordIdentifier, Model.StatementInformationItems, "Please Select...", new { @Class = "referToRecordIdentifier"} )%></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Field5_ReferToRecordIdentifier)%>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field5_DisplayName) %>
							<%= Html.HiddenFor(model => model.PNRNameStatementInformation.Field2_PNRMappingTypeCode) %>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_Delimiter6">Delimeter 6</label></td>
						<td><%= Html.DropDownListFor(model => model.PNRNameStatementInformation.Delimiter6, Model.Delimiters, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PNRNameStatementInformation.Delimiter6)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2">
							<input type="submit" value="Create NSI" title="Create NSI" class="red" />
						</td>
					</tr>
				</table>
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
				<%=Html.HiddenFor(model => model.PNRNameStatementInformation.ClientSubUnit.ClientSubUnitGuid) %>
				<%=Html.HiddenFor(model => model.PNRNameStatementInformation.ClientSubUnitGuid) %>
			<% } %>
		</div>
	</div>
	<script type="text/javascript" src="../../Scripts/ERD/PNRNameStatementInformation.js"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
NSI &gt;
Create 
</asp:Content>
