<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitAddressVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Detail Address</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Address</th> 
		        </tr> 
                <tr>
                    <td><label for="Address_FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.FirstAddressLine, new { maxlength = "80" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.FirstAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.SecondAddressLine, new { maxlength = "80" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.SecondAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_CityName">City</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.CityName, new { maxlength = "40" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.CityName)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_CountyName">County</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.CountyName, new { maxlength = "40" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.CountyName)%></td>
                </tr> 
                 <tr>
                    <td><label for="Address_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.Address.CountryCode, Model.Countries, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.CountryCode)%></td>
                </tr>    
                <tr>
                    <td><label for="Address_StateProvinceName">State/Province</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.StateProvinceName, new { maxlength = "30" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.StateProvinceName)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_LatitudeDecimal">Latitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.LatitudeDecimal, new { maxlength = "11" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.LatitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_LongitudeDecimal">Longitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.LongitudeDecimal, new { maxlength = "11" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.LongitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_MappingQualityCode">Mapping Quality</label></td>
                    <td><%= Html.DropDownListFor(model => model.Address.MappingQualityCode, Model.MappingQualityCodes, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.MappingQualityCode)%></td>
                </tr>    
                
                <tr>
                    <td><label for="Address_PostalCode">Postal Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.PostalCode, new { maxlength = "30" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.PostalCode)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_ReplicatedFromClientMaintenanceFlag">Replicated From ClientMaintenance</label></td>
                    <td><%= Html.CheckBoxFor(model => model.Address.ReplicatedFromClientMaintenanceFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.ReplicatedFromClientMaintenanceFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Address" class="red" title="Edit Address"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.Address.AddressId) %>
            <%= Html.HiddenFor(model => model.Address.VersionNumber)%>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId) %>
    <% } %>


    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/Address.js")%>" type="text/javascript"></script>
</asp:Content>


