<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TeamOutOfOfficeItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Out of Office Backup Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Out of Office Backup Teams</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "TeamOutOfOfficeItem", action = "Edit", id = Model.TeamOutOfOfficeItemId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Out of Office Backup Teams</th> 
		        </tr> 
		        <tr>
                    <td><label for="PrimaryTeam_TeamName" style="font-weight:bold;">Primary Team</label></td>
                    <td colspan="2">
                        <% if (Model.HasPrimaryTeam) { %>
                            <%= Html.Encode(Model.PrimaryTeam.TeamName)%>
                        <% } else { %>
                            <span class="error">Team or Primary Team not setup.  Please complete in Client Wizard</span>
                        <% } %>
                    </td>
                </tr> 
                <tr>
                    <td><label for="Team_TeamName">1st Backup Team</label></td>
                    <td colspan="2">
                        <% if (Model.HasPrimaryTeam) { %>
                            <%= Html.TextBoxFor(model => model.PrimaryBackupTeam.TeamName, new { size = "30" })%>
                            <%= Html.HiddenFor(model => model.PrimaryBackupTeam.TeamId)%>
                            <label id="lblBackupTeam1ItemMsg"></label>
                        <% } %>
                    </td>
                </tr> 
                <tr>
                    <td><label for="Team_TeamName">2nd Backup Team</label></td>
                    <td colspan="2">
                        <% if (Model.HasPrimaryTeam) { %>
                            <%= Html.TextBoxFor(model => model.SecondaryBackupTeam.TeamName, new { size = "30" })%>
                            <%= Html.HiddenFor(model => model.SecondaryBackupTeam.TeamId)%>
                            <label id="lblBackupTeam2ItemMsg"></label>
                        <% } %>
                    </td>
                </tr>
                <tr>
                    <td><label for="Team_TeamName">3rd Backup Team</label></td>
                    <td colspan="2">
                        <% if (Model.HasPrimaryTeam) { %>
                            <%= Html.TextBoxFor(model => model.TertiaryBackupTeam.TeamName, new { size = "30" })%>
                            <%= Html.HiddenFor(model => model.TertiaryBackupTeam.TeamId)%>
                            <label id="lblBackupTeam3ItemMsg"></label>
                        <% } %>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right">
                         <% if (Model.HasPrimaryTeam) { %>
                            <input type="submit" value="Confirm Edit" title="Confirm Edit" class="red"/>
                        <% } %>
                    </td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.VersionNumber) %>
            <%= Html.HiddenFor(model => model.TeamOutOfOfficeItemId) %>
    <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/TeamOutOfOfficeItem.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Team Out of Office Groups", "Main", new { controller = "TeamOutOfOfficeGroup", action = "ListUnDeleted" }, new { title = "Team Out of Office Groups" })%> &gt;
<%=Html.Encode(Model.TeamOutOfOfficeGroup.TeamOutOfOfficeGroupName) %> &gt;
<%=Html.RouteLink("Out of Office Backup Teams", "Main", new { controller = "TeamOutOfOfficeItem", action = "List", id = Model.TeamOutOfOfficeGroup.TeamOutOfOfficeGroupId }, new { title = "Out of Office Backup Teams" })%> &gt;
Edit
</asp:Content>