<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.GSTIdentificationNumber>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GST Identification Number</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GST Identification Number</th> 
		        </tr>  
                <tr>
					<td><label for="ClientTopUnitGuid">Client Top Name</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTopUnitName)%><span class="error"> *</span></td>
					<td>
                        <%= Html.HiddenFor(model => model.ClientTopUnitGuid)%>
						<%= Html.ValidationMessageFor(model => model.ClientTopUnitName)%>
						<label id="lblClientTopUnitGuid_Msg"/>
                    </td>
				</tr>   
				<tr>
                    <td><label for="ClientEntityName">Client Entity Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientEntityName, new { maxlength = "35" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientEntityName)%> </td>
                </tr>
				<tr>
					<td><label for="GSTIdentificationNumber1">GST Identification Number</label></td>
                    <td><%= Html.TextBoxFor(model => model.GSTIdentificationNumber1, new { maxlength = "15" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.GSTIdentificationNumber1)%>
                    </td>
				</tr>
                <tr>
					<td><label for="BusinessPhoneNumber">Business Phone Number</label></td>
                    <td><%= Html.TextBoxFor(model => model.BusinessPhoneNumber, new { maxlength = "20" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.BusinessPhoneNumber)%>
                    </td>
				</tr>
                <tr>
					<td><label for="BusinessEmailAddress">Business Email Address</label></td>
                    <td><%= Html.TextBoxFor(model => model.BusinessEmailAddress, new { maxlength = "20" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.BusinessEmailAddress)%>
                    </td>
				</tr>
                <tr>
					<td><label for="FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.FirstAddressLine, new { maxlength = "150" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.FirstAddressLine)%>
                    </td>
				</tr>
                <tr>
					<td><label for="SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.SecondAddressLine, new { maxlength = "150" })%></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.SecondAddressLine)%>
                    </td>
				</tr>
				<tr>
					<td><label for="CityName">City</label></td>
                    <td><%= Html.TextBoxFor(model => model.CityName, new { maxlength = "40" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.CityName)%>
                    </td>
				</tr>
				<tr>
                    <td><label for="CountryName">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.CountryCode, ViewData["CountryList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.CountryCode)%>
                        <label id="lblCountryNameMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td><label for="StateProvinceCode">State/Province</label></td>
                    <td><%= Html.DropDownListFor(model => model.StateProvinceCode, ViewData["StateProvinceList"] as SelectList, "Please Select...")%><span class="stateProvinceCodeError error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.StateProvinceCode)%>
						 <label id="lblStateProvinceCodeMsg"/>
                    </td>
                </tr>
				<tr>
					<td><label for="PostalCode">Postal Code</label></td>
                    <td><%= Html.TextBoxFor(model => model.PostalCode, new { maxlength = "30" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.PostalCode)%>
                    </td>
				</tr>
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GST Identification Number" title="Edit GST Identification Number" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.GSTIdentificationNumberId) %>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/GSTIdentificationNumber.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("GST Identification Numbers", "Main", new { controller = "GSTIdentificationNumber", action = "List", }, new { title = "GST Identification Numbers" })%> &gt;
<%= Html.Encode(Model.GSTIdentificationNumber1) %>
</asp:Content>