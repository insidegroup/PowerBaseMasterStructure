<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitClientLocationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Locations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Locations</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm("Edit", "ClientTopUnitClientLocation", FormMethod.Post, new { Id = "mainForm" })) {%>
			<%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Client Location</th> 
		        </tr> 
                <tr>
                    <td><label for="Address_LocationName">Client Location Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.AddressLocationName, new { maxlength = "80" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.AddressLocationName)%></td>
                </tr>
				<tr>
                    <td><label for="Address_FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.FirstAddressLine, new { maxlength = "150" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.FirstAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.SecondAddressLine, new { maxlength = "150" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.SecondAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_CityName">City</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.CityName, new { maxlength = "40" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.CityName)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_PostalCode">Postal Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.PostalCode, new { maxlength = "30" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.PostalCode)%></td>
                </tr>  
                 <tr>
                    <td><label for="Address_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTopUnitClientLocation.CountryCode, Model.Countries, "None", new { @class = "required" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.CountryCode)%></td>
                </tr>    
                <tr>
                    <td><label for="Address_StateProvinceName">State/Province</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTopUnitClientLocation.StateProvinceName, new SelectList(new List<string>()))%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.StateProvinceName)%></td>
                </tr>
                <tr>
                    <td><label for="Address_LatitudeDecimal">Latitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.LatitudeDecimal, new { maxlength = "11" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.LatitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_LongitudeDecimal">Longitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientTopUnitClientLocation.LongitudeDecimal, new { maxlength = "11" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.LongitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_Ranking">Ranking</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTopUnitClientLocation.Ranking, (SelectList)ViewData["RankingList"], "None", new { })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTopUnitClientLocation.Ranking)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Client Location" class="red" title="Edit Client Location"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ClientTopUnit.ClientTopUnitGuid) %>
			<%= Html.HiddenFor(model => model.ClientTopUnitClientLocation.AddressId) %>
            <%= Html.HiddenFor(model => model.ClientTopUnitClientLocation.VersionNumber)%>
		<% } %>
    </div>
</div>
    <script>
        var selectedState = <%= string.IsNullOrEmpty(Model.ClientTopUnitClientLocation.StateProvinceName) ? "null" : "'" + Model.ClientTopUnitClientLocation.StateProvinceName + "'" %>;
    </script>
<script src="<%=Url.Content("~/Scripts/ERD/Address.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/ERD/ClientTopUnitLocation2.js")%>" type="text/javascript"></script>
</asp:Content>


