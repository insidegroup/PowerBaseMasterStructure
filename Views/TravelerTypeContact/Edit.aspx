<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelerTypeContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">ClientSubUnit - Client Detail Contacts</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
           <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Contact</th> 
		        </tr> 
                <tr>
                    <td><label for="Contact_FirstName">First Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.FirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.FirstName)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_MiddleName">Middle Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.MiddleName, new { maxlength = "50" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.MiddleName)%></td>
                </tr> 

                <tr>
                    <td><label for="Contact_LastName">Last Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.LastName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.LastName)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_Title">Title</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.Title, new { maxlength = "10" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.Title)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_JobTitle">Job Title</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.JobTitle, new { maxlength = "40" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.JobTitle)%></td>
                </tr>             
                <tr>
                    <td><label for="Contact_WorkPhone">Work Phone</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.WorkPhone, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.WorkPhone)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_WorkMobile">Work Mobile</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.WorkMobile, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.WorkMobile)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_EmailAddress">EmailAddress</label></td>
                    <td><%= Html.TextBoxFor(model => model.Contact.EmailAddress, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.EmailAddress)%></td>
                </tr> 
                <tr>
                    <td><label for="Contact_ContactTypeId">Contact Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.Contact.ContactTypeId, Model.ContactTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Contact.ContactTypeId)%></td>
                </tr>    
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Contact" class="red" title="Edit Contact"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.Contact.ContactId)%>
            <%= Html.HiddenFor(model => model.Contact.VersionNumber)%>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId) %>
            <%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
            <%= Html.HiddenFor(model => model.TravelerType.TravelerTypeGuid) %>
    <% } %>


    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/Contact.js")%>" type="text/javascript"></script>
</asp:Content>


