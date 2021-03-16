<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTelephonyVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Telephony</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Client Telephony</th> 
		        </tr>  
                <tr>
					<td><label for="PhoneNumber">Phone Number</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.PhoneNumber, new { maxlength = "20" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientTelephony.PhoneNumber)%></td>
				</tr>
				<tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTelephony.CountryCode, Model.Countries as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.CountryCode)%>
						<label id="lblCountryNameMsg"></label>
                    </td>
                </tr>
				<tr>
					<td><label for="PhoneNumberwithInternationalPrefixCode">Phone Number with Prefix</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.PhoneNumberwithInternationalPrefixCode, new { maxlength = "60", @readonly = "readonly" })%><span class="error"> *</span></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.PhoneNumberwithInternationalPrefixCode)%>
						<%= Html.HiddenFor(model => model.ClientTelephony.InternationalPrefixCode)%>
					</td>
				</tr>
				<tr>
                    <td><label for="CallerEnteredDigitDefinitionTypeId">Indicator</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTelephony.CallerEnteredDigitDefinitionTypeId, Model.CallerEnteredDigitDefinitionTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTelephony.CallerEnteredDigitDefinitionTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTelephony.HierarchyType, Model.HierarchyTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
					<td>
						<%= Html.HiddenFor(model => model.ClientTelephony.HierarchyItem)%>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.HierarchyItem)%>
					</td>
                </tr>
				<tr>
					<td><label for="HierarchyItem">Hierarchy Item</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.HierarchyName)%><span class="error"> *</span></td>
					<td>
						<%= Html.HiddenFor(model => model.ClientTelephony.HierarchyItem)%>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.HierarchyItem)%>
						<label id="lblHierarchyItemMsg"></label>
					</td>
				</tr>
				<tr>
					<td><label for="MainNumberFlag">Main Number?</label></td>
					<td><%= Html.CheckBoxFor(model => model.ClientTelephony.MainNumberFlagNullable)%></td>
					<td>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.MainNumberFlagNullable)%>
						<label id="lblMainNumberFlagNullableMsg"></label>
					</td>
				</tr>
				<tr>
                    <td><label for="TelephoneType">Telephone Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTelephony.TelephonyTypeId, Model.TelephoneTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTelephony.TelephonyTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="TravelerBackOfficeTypeCode">Back Office Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientTelephony.TravelerBackOfficeTypeCode, Model.TravelerBackOfficeTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientTelephony.TravelerBackOfficeTypeCode)%></td>
                </tr>
				<tr>
					<td><label for="ExpiryDate">Expiry Date</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.ExpiryDate)%></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientTelephony.ExpiryDate)%></td>
				</tr>
				<tr>
					<td><label for="ClientTelephonyDescription">Telephony Description</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.ClientTelephonyDescription, new { maxlength = "50" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientTelephony.ClientTelephonyDescription)%></td>
				</tr>
				<tr>
					<td><label for="ClientSnSButtonText">Client S&S Button Text</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.ClientSnSButtonText, new { maxlength = "25" })%><span class="error" id="ClientSnSButtonTextError"> *</span></td>
					<td>
						<label id="lblClientSnSButtonText" class="error">Client S&S Button Text required if Telephone Type is Safety and Security</label>
						<%= Html.ValidationMessageFor(model => model.ClientTelephony.ClientSnSButtonText)%>
					</td>
				</tr>
				<tr>
					<td><label for="ClientSnSDescription">Client S&S Description</label></td>
					<td><%= Html.TextBoxFor(model => model.ClientTelephony.ClientSnSDescription, new { maxlength = "45" })%></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientTelephony.ClientSnSDescription)%></td>
				</tr>
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Client Telephony" title="Edit ClientTelephony" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ClientTelephony.ClientTelephonyId) %>
			<%= Html.HiddenFor(model => model.ClientTelephony.VersionNumber) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/ClientTelephony.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Client Telephony", "Main", new { controller = "ClientTelephony", action = "List", }, new { title = "Client Telephony" })%> &gt;
Edit Client Telephony &gt;
<%:Model.ClientTelephony.PhoneNumber%>
</asp:Content>