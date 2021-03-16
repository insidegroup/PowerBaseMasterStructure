<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CountryRegion>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Country Regions</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm("Create", "CountryRegion", FormMethod.Post, new { id = "form0", autocomplete = "off" })) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Country Region</th> 
		        </tr> 
                <tr>
                    <td><label for="CountryRegionName">Country Region</label></td>
                    <td><strong><%= Html.TextBoxFor(model => model.CountryRegionName, new { maxlength="50", autocomplete = "off"})%></strong> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CountryRegionName)%><label id="lblCountryRegionNameMsg"/></td>
                </tr>   
                <tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td><%= Html.DropDownList("CountryCode", ViewData["Countries"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.CountryCode)%></td>
                </tr> 
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Country Region" title="Create Country Region" class="red"/></td>
                </tr>
            </table>
    <% } %>
        </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/CountryRegion.js")%>" type="text/javascript"></script>
</asp:Content>



