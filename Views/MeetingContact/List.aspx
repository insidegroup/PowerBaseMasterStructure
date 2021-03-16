<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingContactsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups -  Contacts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups -  Contacts</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="15%" class="row_header_left"><%=Html.RouteLink("Contact Type", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ContactTypeName" }, new { title = "Contact Type" })%></th>
			        <th width="15%"><%=Html.RouteLink("LastName", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MeetingContactLastName" }, new { title = "Last Name" })%></th>
			        <th width="15%"><%=Html.RouteLink("FirstName", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MeetingContactFirstName" }, new { title = "First Name" })%></th>
			        <th width="15%"><%=Html.RouteLink("Phone", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MeetingContactPhoneNumber" }, new { title = "Phone Number" })%></th>
					<th width="15%"><%=Html.RouteLink("Email", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MeetingContactEmailAddress" }, new { title = "EmailAddress" })%></th>
					<th width="10%"><%=Html.RouteLink("Country", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryCode" }, new { title = "Country" })%></th>
					<th width="7%"><%=Html.RouteLink("Itinerary", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CopyItineraryFlag" }, new { title = "Itinerary" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.Contacts) { %>
                <tr>
                    <td><%= Html.Encode(item.ContactTypeName) %></td>
					<td><%= Html.Encode(item.MeetingContactLastName) %></td>
					<td><%= Html.Encode(item.MeetingContactFirstName) %></td>
					<td><%= Html.Encode(item.MeetingContactPhoneNumber) %></td>
					<td><%= Html.Encode(item.MeetingContactEmailAddress) %></td>
					<td><%= Html.Encode(item.CountryCode) %></td>
					<td><%= Html.Encode(item.CopyItineraryFlag.HasValue && item.CopyItineraryFlag.Value == true ? "Yes" : "No") %></td>
					<td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { controller = "MeetingContact", action = "Edit", id = item.MeetingContactID })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { controller = "MeetingContact", action = "Delete", id = item.MeetingContactID })%>
                        <%} %>
                    </td>
                </tr>        
                <% } %>
                <tr>
					<td colspan="9" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
							  <% if (Model.Contacts.HasPreviousPage){ %>
							<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = (Model.Contacts.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
							<%}%>
							</div>
							<div class="paging_right">
							 <% if (Model.Contacts.HasNextPage){ %>
							<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "MeetingContact", action = "List", id = Model.Meeting.MeetingID, page = (Model.Contacts.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
							<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.Contacts.TotalPages > 0){ %>Page <%=Model.Contacts.PageIndex%> of <%=Model.Contacts.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>  
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="8">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Contact", "Create", new { id = Model.Meeting.MeetingID }, new { @class = "red", title = "Create Contact" })%>
						<%} %>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search_wrapper').css('height', '24px');

		//Search
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val('<%=Model.Meeting.MeetingID%>');
		$('#search').show();
		$('#btnSearch').click(function () {
			if ($('#filter').val() == "") {
				alert("No Search Text Entered");
				return false;
			}
			$("#frmSearch").attr("action", "/MeetingContact.mvc/List");
			$("#frmSearch").submit();
		});
	})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.Meeting.ClientTopUnit.ClientTopUnitName) %> &gt;
<%=Html.Encode(Model.Meeting.MeetingName) %> &gt;
Contacts
</asp:Content>