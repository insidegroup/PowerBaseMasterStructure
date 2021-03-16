<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingPNROutputVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups - PNR Format
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Meeting Groups - PNR Format</div>
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
						<td><label for="MeetingPNRFormat_GDSCode">GDS</label></td>
						<td><%= Html.DropDownListFor(model => model.MeetingPNROutput.GDSCode, Model.GDSList, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.GDSCode)%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNRFormat_PNROutputRemarkTypeCode">Remark Type</label></td>
						<td><%= Html.DropDownListFor(model => model.MeetingPNROutput.PNROutputRemarkTypeCode, Model.PNROutputRemarkTypeCodes, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.PNROutputRemarkTypeCode )%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNRFormat_GDSRemarkQualifier">Remark Qualifier</label></td>
						<td><%= Html.TextBoxFor(model => model.MeetingPNROutput.GDSRemarkQualifier, new { maxlength = "100" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.GDSRemarkQualifier)%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNRFormat_DefaultRemark">Remark</label></td>
						<td><%= Html.TextBoxFor(model => model.MeetingPNROutput.DefaultRemark, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.DefaultRemark)%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNRFormat_DefaultLanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.MeetingPNROutput.DefaultLanguageCode , Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.DefaultLanguageCode )%></td>
					</tr>
					<tr>
						<td><label for="MeetingPNRFormat_CountryCode">Country</label></td>
						<td><%= Html.DropDownListFor(model => model.MeetingPNROutput.CountryCode , Model.Countries, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.MeetingPNROutput.CountryCode )%></td>
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
				<%=Html.HiddenFor(model => model.MeetingPNROutput.VersionNumber) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingPNROutputId) %>
				<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingID) %>
			<% } %>
		</div>
	</div>
	<script type="text/javascript" src="../../Scripts/ERD/MeetingPNROutput.js"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(ViewData["ClientTopUnitName"].ToString()) %> &gt;
<%=Html.Encode(ViewData["MeetingName"].ToString()) %> &gt;
PNR Format &gt;
Edit PNR Format
</asp:Content>
