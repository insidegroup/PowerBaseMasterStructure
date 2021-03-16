<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectMeetings_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups (Deleted)</div></div>
        <div id="content">
            <div class="home_button">
				<a href="/" class="red">Home</a>
			</div>
			<table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="11%" class="row_header_left"><%= Html.RouteLink("Client TopUnit", "ListMain", new { page = 1, sortField = "ClientTopUnitName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="10%"><%= Html.RouteLink("Meeting Name", "ListMain", new { page = 1, sortField = "MeetingName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
                    <th width="10%"><%= Html.RouteLink("Meeting Reference", "ListMain", new { page = 1, sortField = "MeetingReferenceNumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
					<th width="5%"><%= Html.RouteLink("Start Date", "ListMain", new { page = 1, sortField = "MeetingStartDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
					<th width="5%"><%= Html.RouteLink("End Date", "ListMain", new { page = 1, sortField = "MeetingEndDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="6%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
					<th width="8%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
					<th width="10%">&nbsp;</th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="3%">&nbsp;</th> 
			        <th width="2%">&nbsp;</th> 
			        <th width="5%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
				<tr>
                    <td><%= Html.Encode(item.ClientTopUnitName) %></td>
                    <td>
						<% if(DateTime.Now > item.MeetingEndDate) { %> 
							<span class="expired"><%= Html.Encode(item.MeetingName)%></span>
						 <%} else { %>
							<%= Html.Encode(item.MeetingName)%>
						 <%} %>
                    </td>
					<td><%= Html.Encode(item.MeetingReferenceNumber)%></td>
					<td><%= Html.Encode(item.MeetingStartDate.ToString("MMM dd, yyyy"))%></td>
					<td><%= Html.Encode(item.MeetingEndDate.ToString("MMM dd, yyyy"))%></td>
                    <td><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%></td>                    
					<td><%= Html.RouteLink("Hierarchy", "List", new { action="LinkedClientSubUnits", id = item.MeetingId }, new { title = "Hierarchy" })%></td>
					<td><%= Html.RouteLink("Contacts", "List", new { controller ="MeetingContact", action="List", id = item.MeetingId }, new { title = "Contacts" })%></td>
					<td><%= Html.RouteLink("Credit Card", "List", new { action="LinkedClientSubUnitCreditCards", id = item.MeetingId }, new { title = "Credit Card" })%></td>
					<td><%= Html.RouteLink("Advice", "List", new { controller ="MeetingAdviceLanguage", action="List", id = item.MeetingId, meetingAdviceTypeId = 1 }, new { title = "Advice" })%></td>
					<td><%= Html.RouteLink("Traveler Advice", "List", new { controller ="MeetingAdviceLanguage", action="List", id = item.MeetingId, meetingAdviceTypeId = 2 }, new { title = "Traveler Advice" })%></td>
					<td><%= Html.RouteLink("PNR Output", "List", new { controller = "MeetingPNROutput", action="List", id = item.MeetingId }, new { title = "PNR Output" })%></td>
					<td><%= Html.RouteLink("View", "Default", new { id = item.MeetingId, action="View" }, new { title = "View" })%> </td>
                    <td>
                       <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.RouteLink("Edit", "Default", new { id = item.MeetingId, action = "Edit" }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.RouteLink("UnDelete", "Default", new { id = item.MeetingId, action = "UnDelete" }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
               <tr>
                    <td colspan="15" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%></div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
		        <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="15"><%= Html.ActionLink("UnDeleted Meetings", "ListUndeleted", null, new { @class = "red" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $('#search_wrapper').css('height', '24px');
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/Meeting.mvc/ListDeleted");
        $("#frmSearch").submit();
    });
 </script>
 </asp:Content>