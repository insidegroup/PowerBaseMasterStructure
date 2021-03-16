<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PublicHolidayGroupHolidayDate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Public Holiday Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Public Holiday Date</div></div>

        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Public Holiday Date</th> 
		        </tr> 
                <tr>
                    <td><label for="PublicHolidayGroupName">Public Holiday Group Name</label></td>
                    <td><strong><%= Html.Label(Model.PublicHolidayGroupName)%></strong></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="PublicHolidayDate">Public Holiday Date</label></td>
                    <td><%= Html.EditorFor(model => model.PublicHolidayDate1)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PublicHolidayDate1)%></td>
                </tr> 
                <tr>
                    <td><label for="PublicHolidayDescription">Public Holiday Description</label></td>
                    <td><%= Html.TextBoxFor(model => model.PublicHolidayDescription, new { maxlength="50"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PublicHolidayDescription)%></td>
                </tr> 
               
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.PublicHolidayGroupId }, new { @class = "red", title = "Back To List" })%></td>
                    <td></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit PublicHolidayDate" title="Edit PublicHolidayDate" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.PublicHolidayGroupId) %>
<%= Html.HiddenFor(model => model.VersionNumber) %></td>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/PublicHolidayDate.js")%>" type="text/javascript"></script>
</asp:Content>


