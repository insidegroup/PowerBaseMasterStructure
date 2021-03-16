<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUserTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUser Teams</div></div>
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
                    <td>SystemUser</td>
                    <td><%= Html.Encode(Model.SystemUserName)%></td>
                    <td><%= Html.HiddenFor(model => model.SystemUserGuid)%></td>
                </tr> 
                 <tr>
                    <td><label for="Team">Team</label></td>
                    <td><%= Html.TextBox("TeamName", "", new { maxlength = "150" })%><span class="error"> *</span>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TeamName)%>
                        <%= Html.Hidden("TeamId")%>
                        <label id="lblTeamNameMsg"/>
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
    <script src="<%=Url.Content("~/Scripts/ERD/SystemUserTeamCreateTeam.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("SystemUsers", "Main", new { controller = "SystemUser", action = "List", }, new { title = "SystemUsers" })%> &gt;
<%=Html.RouteLink(Model.SystemUserName, "Main", new { controller = "SystemUser", action = "ViewItem", id = Model.SystemUserGuid }, new { title = Model.SystemUserName })%> &gt;
<%=Html.RouteLink("Teams", "Main", new { controller = "SystemUser", action = "ListTeams", id = Model.SystemUserGuid, }, new { title = "SystemUsers" })%> &gt;
Add Team to System User
</asp:Content>


