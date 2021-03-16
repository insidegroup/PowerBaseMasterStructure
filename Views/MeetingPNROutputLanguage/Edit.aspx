<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingPNROutputLanguageVM>" %>

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
						<td><label for="MeetingPNROutput_DefaultRemark">Remark</label></td>
						<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.DefaultRemark)%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNROutputLanguage_LanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.MeetingPNROutputLanguage.LanguageCode , Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutputLanguage.LanguageCode )%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNROutputLanguage_RemarkTranslation">Translation</label></td>
						<td><%= Html.TextAreaFor(model => model.MeetingPNROutputLanguage.RemarkTranslation)%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutputLanguage.RemarkTranslation)%></td>
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
				
				<%=Html.HiddenFor(model => model.MeetingPNROutputLanguage.MeetingPNROutputId) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutputLanguage.VersionNumber) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutputLanguage.CurrentLanguageCode) %>
		
				<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingPNROutputId) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingID) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.DefaultLanguageCode) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.DefaultRemark) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.GDSCode) %>
				
			<% } %>
		</div>
	</div>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_meetings').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#breadcrumb').css('width', 'auto');
			$('#search_wrapper').css('height', '24px');

			//Search
			$('#search').hide();

		})
	 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(ViewData["ClientTopUnitName"].ToString()) %> &gt;
<%=Html.Encode(ViewData["MeetingName"].ToString()) %> &gt;
PNR Format &gt;
Translations &gt;
Edit Translation
</asp:Content>
