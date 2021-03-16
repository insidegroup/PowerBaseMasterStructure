<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CountryRegion>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Country Regions</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Country Region</th> 
		        </tr> 
                <tr>
                    <td><label for="CountryRegionName">Country Region</label></td>
                    <td><strong><%= Html.TextBoxFor(model => model.CountryRegionName, new { maxlength="50"})%></strong><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CountryRegionName)%><label id="lblCountryRegionNameMsg"/></td>
                </tr>   
                <tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td><%= Html.DropDownList("CountryCode", ViewData["Countries"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CountryCode)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Country Region" title="Edit Country Region" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.VersionNumber) %>
            <%= Html.HiddenFor(model => model.CountryRegionId) %>
    <% } %>
        </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/CountryRegion.js")%>" type="text/javascript"></script>
</asp:Content>



