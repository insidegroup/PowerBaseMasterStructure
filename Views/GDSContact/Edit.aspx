<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Contact</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Contact</th> 
		        </tr>  
                <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSContact.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSContact.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSContact.CountryCode, Model.Countries as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSContact.CountryCode)%></td>
                </tr>
				<tr>
                    <td><label for="GlobalRegionId">Global Region</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSContact.GlobalRegionCode, Model.GlobalRegions as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSContact.GlobalRegionCode)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeDefinedRegionId">Pseudo City/Office ID Defined Region</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSContact.PseudoCityOrOfficeDefinedRegionId, Model.PseudoCityOrOfficeDefinedRegions as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSContact.PseudoCityOrOfficeDefinedRegionId)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeBusinessId">Pseudo City/Office ID Business</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSContact.PseudoCityOrOfficeBusinessId, Model.PseudoCityOrOfficeBusinesses as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSContact.PseudoCityOrOfficeBusinessId)%></td>
                </tr>
				 <tr>
                    <td><label for="GDSRequestTypes">GDS Request Type</label></td>
                    <td><%= Html.ListBoxFor(model => model.GDSContact.GDSRequestTypeIds, Model.GDSRequestTypes as SelectList, "Please Select...")%> </td>
                    <td>
						Hold CNTRL to select multiple<br />
						<%= Html.ValidationMessageFor(model => model.GDSContact.GDSRequestTypeIds)%>
                    </td>
                </tr>
                <tr>
					<td><label for="LastName">Last Name</label></td>
					<td><%= Html.TextBoxFor(model => model.GDSContact.LastName, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.GDSContact.LastName)%></td>
				</tr>
				<tr>
					<td><label for="FirstName">First Name</label></td>
					<td><%= Html.TextBoxFor(model => model.GDSContact.FirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.GDSContact.FirstName)%></td>
				</tr>
				<tr>
					<td><label for="EmailAddress">Email</label></td>
					<td><%= Html.TextBoxFor(model => model.GDSContact.EmailAddress, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.GDSContact.EmailAddress)%></td>
				</tr>
				<tr>
					<td><label for="Phone">Phone</label></td>
					<td><%= Html.TextBoxFor(model => model.GDSContact.Phone, new { maxlength = "20" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.GDSContact.Phone)%></td>
				</tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GDS Contact" title="Edit GDSContact" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.GDSContact.GDSContactId) %>
			<%= Html.HiddenFor(model => model.GDSContact.VersionNumber) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/GDSContact.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("GDS Contact", "Main", new { controller = "GDSContact", action = "List", }, new { title = "GDS Contact" })%> &gt;
Edit &gt;
</asp:Content>