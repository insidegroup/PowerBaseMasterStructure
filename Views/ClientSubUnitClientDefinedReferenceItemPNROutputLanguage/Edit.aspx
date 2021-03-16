<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemPNROutputLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Sub Unit – CDRs – PNR Format - Remark Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Remark Translations</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Edit Remark Translation</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNROutput_DefaultRemark">Remark</label></td>
						<td colspan="2"><%= Html.Encode(Model.ClientDefinedReferenceItemPNROutput.DefaultRemark)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNROutputLanguage_LanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.LanguageCode , Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.LanguageCode )%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItemPNROutputLanguage_RemarkTranslation">Translation</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.RemarkTranslation)%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.RemarkTranslation)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2">
							<input type="submit" value="Edit Translation" title="Edit Translation" class="red" />
						</td>
					</tr>
				</table>	
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
				
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.ClientDefinedReferenceItemPNROutputId) %>
                <%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.CurrentLanguageCode) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutputLanguage.VersionNumber) %>
				
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemPNROutputId) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.ClientDefinedReferenceItemId) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultLanguageCode) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.DefaultRemark) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItemPNROutput.GDSCode) %>
				
			<% } %>
		</div>
	</div>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_clients').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#breadcrumb').css('width', 'auto');

			//Search
			$('#search').hide();

		})
	 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink(ViewData["DisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["DisplayNameAlias"].ToString() })%> &gt;
<%=Html.RouteLink("PNR Formats", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "PNR Formats" })%> &gt;
<%=Html.RouteLink(ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List",  id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"].ToString() })%> &gt;
<%=Html.RouteLink("Translations", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutputLanguage", action = "List",  id = ViewData["ClientDefinedReferenceItemPNROutputId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "Translations" })%> &gt;
Edit Translation
</asp:Content>
