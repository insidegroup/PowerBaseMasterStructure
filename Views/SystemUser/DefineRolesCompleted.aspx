<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
DesktopDataAdmin - System Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
	<div id="banner">
		<div id="banner_text">System Users Define Roles</div>
	</div>
	<div id="content">
		<% using (Html.BeginForm()) {%>
		<%= Html.AntiForgeryToken()%>
		<table cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<th class="row_header" colspan="3">Define System User Roles : Completed</th>
			</tr>
			<tr>
				<td colspan="2"><%=ViewData["ConfirmationMessage"]%></td>
				<td></td>
			</tr>
			<tr>
				<td width="30%" class="row_footer_left"></td>
				<td width="40%" class="row_footer_centre"></td>
				<td width="30%" class="row_footer_right"></td>
			</tr>
			<tr>
				<td class="row_footer_blank_left"><%= Html.ActionLink("Back", "List", "SystemUser", null, new { @class = "red", title = "Back" })%></td>
				<td class="row_footer_blank_right" colspan="2">&nbsp;</td>
			</tr>
		</table>
		<% } %>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUserDefineRoles.js")%>" type="text/javascript"></script>
</asp:Content>



