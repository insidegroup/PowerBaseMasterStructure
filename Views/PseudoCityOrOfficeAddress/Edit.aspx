<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeAddressVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Address
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Address</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PseudoCityOrOfficeAddress", action = "Edit", id = Model.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Pseudo City/Office ID Address</th> 
				</tr> 
		        <tr>
                    <td><label for="PseudoCityOrOfficeAddress_FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeAddress.FirstAddressLine, new { maxlength = "80" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.FirstAddressLine)%>
						<label id="lblPseudoCityOrOfficeAddressMsg"></label>
                    </td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeAddress.SecondAddressLine, new { maxlength = "80" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.SecondAddressLine)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_CityName">City</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeAddress.CityName, new { maxlength = "40" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.CityName)%></td>
                </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeAddress.CountryCode, Model.Countries as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.CountryCode)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_StateProvinceCode">StateProvince</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeAddress.StateProvinceCode, Model.StateProvinces, "Please Select...")%> <span class="stateProvinceCodeError error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.StateProvinceCode)%>
						 <label id="lblStateProvinceCodeMsg"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeAddress_PostalCode">Postal Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeAddress.PostalCode, new { maxlength = "30" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeAddress.PostalCode)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Pseudo City/Office ID Address" title="Edit Pseudo City/Office ID Address" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PseudoCityOrOfficeAddress.PseudoCityOrOfficeAddressId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PseudoCityOrOfficeAddress.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Pseudo City/Office ID Address", "Main", new { controller = "PseudoCityOrOfficeAddress", action = "List", }, new { title = "Pseudo City/Office ID Address" })%> &gt;
Edit
</asp:Content>