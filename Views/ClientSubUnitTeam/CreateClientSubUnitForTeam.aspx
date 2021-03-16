<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Client SubUnits</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Add ClientSubUnit to Team</th> 
		        </tr> 
                <tr>
                    <td>Team</td>
                    <td><strong><%= Model.TeamName%></strong></td>
                    <td><%= Html.HiddenFor(model => model.TeamId)%></td>
                </tr>   
                 <tr>
                    <td><label for="ClientSubUnitName">Client SubUnit</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientSubUnitName, new { maxlength = "50" })%><span class="error"> *</span>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientSubUnitName)%>
                        <%= Html.Hidden("ClientSubUnitGuid")%>
                        <label id="lblClientSubUnitNameMsg"/>
                    </td>
                </tr>  
                <tr>
                    <td><label for="IncludeInClientDroplistFlag">Include In Client DropList</label></td>
                    <td><%= Html.CheckBoxFor(model => model.IncludeInClientDroplistFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.IncludeInClientDroplistFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Add Client SubUnit to Team" title="Add Client SubUnit to Team" class="red"/></td>
                </tr>
            </table>
    <% } %>
        </div>
        </div>
    <script src="<%=Url.Content("~/Scripts/ERD/ClientSubUnitTeam.js")%>" type="text/javascript"></script>
</asp:Content>



<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink(Model.TeamName, "Default", new { controller = "Team", action = "View", id = Model.TeamId }, new { title = Model.TeamName })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Default", new { controller = "Team", action = "ListClientSubUnits", id = Model.TeamId }, new { title = "Client SubUnits" })%> &gt;
Add Client SubUnit to Team
</asp:Content>