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
				<th class="row_header" colspan="3">Define System User Roles : Step 2</th>
			</tr>
			<tr>
				<td colspan="2">Define Roles for <%=ViewData["UserProfileIdentifier"]%></td>
				<td></td>
			</tr>
			<tr>
				<td colspan="2">Copy Roles from <%=ViewData["NewUserProfileIdentifier"]%></td>
				<td></td>
			</tr>
			<tr>
				<td colspan="2">This will copy all Roles to <%=ViewData["UserProfileIdentifier"]%> from <%=ViewData["NewUserProfileIdentifier"]%>.</td>
				<td></td>
			</tr>
			<tr>
				<td width="30%" class="row_footer_left"></td>
				<td width="40%" class="row_footer_centre"></td>
				<td width="30%" class="row_footer_right"></td>
			</tr>
			<tr>
				<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
				<td class="row_footer_blank_right" colspan="2">
					<%if (ViewData["Access"] == "WriteAccess"){%><input type="submit" value="Continue" title="Continue" class="red" /><%} %></td>
			</tr>
		</table>
		<% } %>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUserDefineRoles.js")%>" type="text/javascript"></script>
</asp:Content>



