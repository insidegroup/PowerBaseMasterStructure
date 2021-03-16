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
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Edit PNR Format</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
						<td><%= Html.Encode(ViewData["DisplayNameAlias"])%></td>
						<td></td>
					</tr>
                    <tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_GDSCode">GDS</label></td>
						<td><%= Html.DropDownListFor(model => model.ClientDefinedReferenceItemPNROutput.GDSCode, Model.GDSList, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutput.GDSCode)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_PNROutputRemarkTypeCode">Remark Type</label></td>
						<td><%= Html.DropDownListFor(model => model.ClientDefinedReferenceItemPNROutput.PNROutputRemarkTypeCode, Model.PNROutputRemarkTypeCodes, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutput.PNROutputRemarkTypeCode )%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_GDSRemarkQualifier">Remark Qualifier</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItemPNROutput.GDSRemarkQualifier, new { maxlength = 100 })%></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutput.GDSRemarkQualifier)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_DefaultRemark">Remark</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultRemark)%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultRemark)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNRFormat_DefaultLanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultLanguageCode , Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultLanguageCode )%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2">
							<input type="submit" value="Edit PNR Format" title="Edit PNR Format" class="red" />
						</td>
					</tr>
				</table>	
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.VersionNumber) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId) %>
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
			<% } %>
		</div>
	</div>
	<script type="text/javascript" src="../../Scripts/ERD/ClientDefinedReferenceItemPNROutput.js"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink(ViewData["DisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "View", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString()}, new { title = ViewData["DisplayNameAlias"].ToString() })%> &gt;
<%=Html.RouteLink("PNR Formats", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "PNR Formats" })%> &gt;
<%=Html.Encode(Model.ClientDefinedReferenceItemPNROutput.DefaultRemark) %> &gt;
Edit PNR Format
</asp:Content>
