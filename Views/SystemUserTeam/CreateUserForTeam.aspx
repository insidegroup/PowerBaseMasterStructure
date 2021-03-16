<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUserTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Add System User to Team</th> 
		        </tr> 
                <tr>
                    <td>Team</td>
                    <td><strong><%= Model.TeamName%></strong></td>
                    <td><%= Html.HiddenFor(model => model.TeamId)%></td>
                </tr>   
                 <tr>
                    <td><label for="SystemUserName">System User</label></td>
                    <td><%= Html.TextBox("SystemUserName", "", new { maxlength = "150" })%><span class="error"> *</span>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SystemUserGuid)%>
                        <%= Html.Hidden("SystemUserGuid")%>
                        <label id="lblSystemUserNameMsg"/>
                    </td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Add SystemUser to Team" title="Add SystemUser to Team" class="red"/></td>
                </tr>
            </table>
    <% } %>
        </div>
        </div>
    <script src="<%=Url.Content("~/Scripts/ERD/SystemUserTeamCreateUser.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink(Model.TeamName, "Default", new { controller = "Team", action = "View", id = ViewData["TeamId"] }, new { title = Model.TeamName })%> &gt;
Add System User to Team
</asp:Content>


