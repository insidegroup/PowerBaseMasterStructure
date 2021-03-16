<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Group - Contacts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Group - Contacts</div></div>
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
                    <td><label for="ContactTypeId">Contact Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.MeetingContact.ContactTypeId, Model.ContactTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.ContactTypeId)%></td>
                </tr>    
                <tr>
                    <td><label for="MeetingContactLastName">Last Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingContact.MeetingContactLastName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.MeetingContactLastName)%></td>
                </tr> 
 				<tr>
                    <td><label for="MeetingContactFirstName">First Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingContact.MeetingContactFirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.MeetingContactFirstName)%></td>
                </tr> 
                <tr>
                    <td><label for="MeetingContactPhoneNumber">Phone</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingContact.MeetingContactPhoneNumber, new { maxlength = "20" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.MeetingContactPhoneNumber)%></td>
                </tr> 
                <tr>
                    <td><label for="MeetingContactEmailAddress">Email</label></td>
                    <td><%= Html.TextBoxFor(model => model.MeetingContact.MeetingContactEmailAddress, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.MeetingContactEmailAddress)%></td>
                </tr>  
				<tr>
                    <td><label for="Contact_CountryCode">Country</label></td>
                    <td><%= Html.DropDownListFor(model => model.MeetingContact.CountryCode, Model.Countries, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.CountryCode)%></td>
                </tr>
				<tr>
                    <td><label for="CopyItineraryFlag">Itinerary?</label></td>
                    <td><%= Html.CheckBox("MeetingContact.CopyItineraryFlag")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingContact.CopyItineraryFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Contact" class="red" title="Create Contact"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.Meeting.MeetingID) %>
    <% } %>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_meetings').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search_wrapper').css('height', '24px');
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.Meeting.ClientTopUnit.ClientTopUnitName) %> &gt;
<%=Html.Encode(Model.Meeting.MeetingName) %> &gt;
Contacts &gt;
Create
</asp:Content>