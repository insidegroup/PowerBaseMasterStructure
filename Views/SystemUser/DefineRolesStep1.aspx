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
		<% using (Html.BeginForm("DefineRolesStep1", "SystemUser", FormMethod.Post, new { id = "form0" })) {%>
		<%= Html.AntiForgeryToken()%>
		<table cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<th class="row_header" colspan="3">Define Roles : Step 1</th>
			</tr>
			<tr>
				<td>Define roles for</td>
				<td><%=Html.Encode(Model.UserProfileIdentifier)%></td>
				<td></td>
			</tr>
			<tr>
				<td><label for="NewUserProfileIdentifier">Copy roles from</label></td>
				<td><%= Html.TextBox("NewUserProfileIdentifier")%><span class="error"> *</span></td>
				<td><label id="lblNewUserProfileIdentifier" class="field-validation-error"></label></td>
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
		<%=Html.Hidden("id", ViewData["UserProfileIdentifier"])%>
		<% } %>
	</div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUserDefineRoles.js")%>" type="text/javascript"></script>
</asp:Content>



