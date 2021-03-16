<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.AddressLocationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Locations</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete = "off" })) {%>
        <%= Html.AntiForgeryToken() %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
	        <tr> 
		        <th class="row_header" colspan="3">Create Location</th> 
	        </tr> 
            <tr>
                <td><label for="LocationName">Location Name</label></td>
                <td><%= Html.TextBoxFor(model => model.Location.LocationName, new { maxlength="50", autocomplete = "off" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.Location.LocationName)%>
                <label id="lblLocationNameMsg"/>
                </td>
            </tr> 
			<tr>
                <td><label for="CountryName">Country</label></td>
                <td><%= Html.TextBoxFor(model => model.Location.CountryName, new { maxlength="100", autocomplete = "off" })%><span class="error"> *</span></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.Location.CountryName)%>
					<%= Html.HiddenFor(model => model.Location.CountryCode)%>
					<%= Html.HiddenFor(model => model.Address.CountryCode)%>
                    <label id="lblCountryNameMsg"/>
                </td>
            </tr>   
            <tr>
                <td><label for="CountryRegionId">Country Region</label></td>
                <td><%= Html.DropDownListFor(model => model.Location.CountryRegionId, new List<SelectListItem>())%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.Location.CountryRegionId)%></td>
            </tr>
			<tr>
                <td><label for="Address_FirstAddressLine">First Address Line</label></td>
                <td><%= Html.TextBoxFor(model => model.Address.FirstAddressLine, new { maxlength = "80", autocomplete = "off" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.Address.FirstAddressLine)%></td>
            </tr> 
            <tr>
                <td><label for="Address_SecondAddressLine">Second Address Line</label></td>
                <td><%= Html.TextBoxFor(model => model.Address.SecondAddressLine, new { maxlength = "80", autocomplete = "off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.Address.SecondAddressLine)%></td>
            </tr> 
            <tr>
                <td><label for="Address_CityName">City</label></td>
                <td><%= Html.TextBoxFor(model => model.Address.CityName, new { maxlength = "40", autocomplete = "off"  })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.Address.CityName)%></td>
            </tr> 
            <tr>
                <td><label for="PseudoCityOrOfficeAddress_StateProvinceCode">State/Province</label></td>
                <td><%= Html.DropDownListFor(model => model.Address.StateProvinceCode, Model.StateProvinces, "Please Select...")%> <span class="stateProvinceCodeError error"> *</span></td>
                <td>
					<%= Html.ValidationMessageFor(model => model.Address.StateProvinceCode)%>
					<label id="lblStateProvinceCodeMsg"/>
                </td>
            </tr>
			<tr>
                <td><label for="Address_PostalCode">Postal Code</label></td>
                <td><%= Html.TextBoxFor(model => model.Address.PostalCode, new { maxlength = "30", autocomplete = "off"  })%></td>
                <td><%= Html.ValidationMessageFor(model => model.Address.PostalCode)%></td>
            </tr>  
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
           <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Create Location" title="Create Location" class="red"/></td>
            </tr>
        </table>
	<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/Location.js")%>" type="text/javascript"></script>
</asp:Content>

 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Locations", "Main", new { controller = "Location", action = "List", }, new { title = "Locations" })%> &gt;
Create Location
</asp:Content>