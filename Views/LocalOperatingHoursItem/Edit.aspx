<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.LocalOperatingHoursItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Local Operating Hours Item</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
			<%using (Html.BeginRouteForm("Default", null, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
            <%= Html.AntiForgeryToken() %> 
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Local Operating Hours Item</th> 
		        </tr> 
                <tr>
                    <td><label for="LocalOperatingHoursGroupName">Local Operating Hours Group Name</label></td>
                    <td><strong><%= Html.Label(Model.LocalOperatingHoursGroupName)%></strong></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="WeekDayId">Week Day</label></td>
                    <td><%= Html.DropDownList("WeekDayId", ViewData["Weekdays"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.WeekDayId)%></td>
                </tr> 
               <tr>
                    <td><label for="OpeningTime">Opening Time</label></td>
                    <td><%= Html.TextBoxFor(model => model.OpeningTime)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OpeningTime)%></td>
                </tr> 
                <tr>
                    <td><label for="ClosingTime">Closing Time</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClosingTime)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClosingTime)%></td>
                </tr>   
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.LocalOperatingHoursGroupId }, new { @class = "red", title = "Back To List" })%></td>
                    <td class="row_footer_blank"></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Item" title="Edit Item" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.LocalOperatingHoursGroupId) %>
            <%= Html.Hidden("OpeningDateTimeOriginal", Model.OpeningDateTime)%>
            <%= Html.Hidden("WeekDayIdOriginal", Model.WeekDayId)%>
            <%= Html.HiddenFor(model => model.VersionNumber) %>
        <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/LocalOperatingHoursItem.js")%>" type="text/javascript"></script>
</asp:Content>


