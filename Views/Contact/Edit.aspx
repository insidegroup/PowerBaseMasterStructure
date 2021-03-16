<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnits - Contacts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Contacts</div></div>
        <div id="content">
		<% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
	            <table cellpadding="0" cellspacing="0" width="100%"> 
					<tr> 
						<th class="row_header" colspan="3">Edit Contact</th> 
					</tr> 
					<tr>
						<td><label for="Contact_ContactTypeId">Contact Type</label></td>
						<td><%= Html.DropDownListFor(model => model.Contact.ContactTypeId, Model.ContactTypes, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.ContactTypeId)%></td>
					</tr>    
					<tr>
						<td><label for="Contact_LastName">Last Name</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.LastName, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.LastName)%></td>
					</tr> 
 					<tr>
						<td><label for="Contact_FirstName">First Name</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.FirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.FirstName)%></td>
					</tr> 
					<tr>
						<td><label for="Contact_PrimaryTelephoneNumber">Phone</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.PrimaryTelephoneNumber, new { maxlength = "20" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.PrimaryTelephoneNumber)%></td>
					</tr> 
					<tr>
						<td><label for="Contact_EmergencyTelephoneNumber">Emergency Phone</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.EmergencyTelephoneNumber, new { maxlength = "20" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.EmergencyTelephoneNumber)%></td>
					</tr>  
					<tr>
						<td><label for="Contact_EmailAddress">Email</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.EmailAddress, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.EmailAddress)%></td>
					</tr>
					<tr>
						<td><label for="Contact_CountryCode">Country</label></td>
						<td><%= Html.DropDownListFor(model => model.Contact.CountryCode, Model.Countries, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.CountryCode)%></td>
					</tr>
					<tr>
						<td><label for="Contact_FirstAddressLine">First Address Line</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.FirstAddressLine, new { maxlength = "80" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.FirstAddressLine)%></td>
					</tr>
					<tr>
						<td><label for="Contact_SecondAddressLine">Second Address Line</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.SecondAddressLine, new { maxlength = "80" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.SecondAddressLine)%></td>
					</tr>
					<tr>
						<td><label for="Contact_CityName">City</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.CityName, new { maxlength = "40" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.CityName)%></td>
					</tr>
					<tr>
						<td><label for="Contact_StateProvinceName">State/Province</label></td>
						<td><%= Html.DropDownListFor(model => model.Contact.StateProvinceName, Model.StateProvinces, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.StateProvinceName)%></td>
					</tr>
					<tr>
						<td><label for="Contact_PostalCode">Postal Code</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.PostalCode, new { maxlength = "30" })%></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.PostalCode)%></td>
					</tr>
					<tr>
						<td><label for="Contact_ResponsibilityDescription">Responsibility</label></td>
						<td><%= Html.TextBoxFor(model => model.Contact.ResponsibilityDescription, new { maxlength = "100" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.Contact.ResponsibilityDescription)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					  <tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
						<td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Contact" title="Edit Contact" class="red"/></td>
					</tr>
				</table>
                <%= Html.HiddenFor(model => model.Contact.ContactId)%>
				<%= Html.HiddenFor(model => model.Contact.ClientSubUnitGuid)%>
                <%= Html.HiddenFor(model => model.Contact.VersionNumber)%>
            <%}%>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/Contact.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientTopUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnitGuid.ToString() }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientSubUnitName.ToString() })%> &gt;
Edit Contact
</asp:Content>