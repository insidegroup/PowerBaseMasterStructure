<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Group - Contacts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Group - Contacts</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete Contact</th> 
		        </tr> 
                <tr>
                    <td>Contact Type</td>
                    <td><%= Html.Encode(Model.MeetingContact.ContactType.ContactTypeName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.MeetingContact.MeetingContactLastName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.MeetingContact.MeetingContactFirstName)%></td>
                    <td></td>
				</tr>
                <tr>
                    <td>Phone</td>
                    <td><%= Html.Encode(Model.MeetingContact.MeetingContactPhoneNumber)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Email</td>
                    <td><%= Html.Encode(Model.MeetingContact.MeetingContactEmailAddress)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.MeetingContact.Country.CountryName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Itinerary?</td>
                    <td><%= Html.Encode(Model.MeetingContact.CopyItineraryFlag.HasValue && Model.MeetingContact.CopyItineraryFlag.Value == true ? "Yes" : "No")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                  <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2">
					<% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
						<%= Html.HiddenFor(model => model.Meeting.MeetingID)%>
						<%= Html.HiddenFor(model => Model.MeetingContact.MeetingContactID)%>
                        <%= Html.HiddenFor(model => Model.MeetingContact.VersionNumber)%>
                    <%}%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search_wrapper').css('height', '24px');
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.Meeting.ClientTopUnit.ClientTopUnitName) %> &gt;
<%=Html.Encode(Model.Meeting.MeetingName) %> &gt;
Contacts &gt;
Delete
</asp:Content>