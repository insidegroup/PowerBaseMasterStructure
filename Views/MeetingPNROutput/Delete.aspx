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
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<th class="row_header" colspan="3">Delete PNR Format</th>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput_GDSCode">GDS</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.GDS.GDSName) %></td>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput_PNROutputRemarkTypeCode">Remark Type</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.PNROutputRemarkType.PNROutputRemarkTypeName) %></td>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput__GDSRemarkQualifier">Remark Qualifier</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.GDSRemarkQualifier)%></td>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput_DefaultRemark">Remark</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.DefaultRemark)%></td>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput_Language">Language</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.Language.LanguageName) %></td>
				</tr>
				<tr>
					<td><label for="MeetingPNROutput_CountryCode">Country</label></td>
					<td colspan="2"><%= Html.Encode(Model.MeetingPNROutput.Country != null ? Model.MeetingPNROutput.Country.CountryName : "") %></td>
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
						<%=Html.HiddenFor(model => model.MeetingPNROutput.VersionNumber) %>
						<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingPNROutputId) %>
						<%=Html.HiddenFor(model => model.MeetingPNROutput.MeetingID) %>
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
		$('#search_wrapper').css('height', '24px');
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(ViewData["ClientTopUnitName"].ToString()) %> &gt;
<%=Html.Encode(ViewData["MeetingName"].ToString()) %> &gt;
PNR Format &gt;
Delete PNR Format
</asp:Content>