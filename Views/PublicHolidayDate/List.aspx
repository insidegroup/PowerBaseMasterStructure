<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPublicHolidayDates_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Public Holiday Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Public Holiday Dates</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%= Html.RouteLink("Date", "List", new { page = 1, sortField = "PublicHolidayDate", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["PublicHolidayGroupId"] })%></th> 
			        <th width="60%" class="row_header_left"><%= Html.RouteLink("Description", "List", new { page = 1, sortField = "PublicHolidayDescription", sortOrder = ViewData["NewSortOrder"].ToString(), id = ViewData["PublicHolidayGroupId"] })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= item.PublicHolidayDate.ToString("MMM dd, yyyy")%></td>
                    <td><%= Html.Encode(item.PublicHolidayDescription) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.PublicHolidayDateId })%> </td>
                    <td>
                         <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PublicHolidayDateId })%>
                        <%} %>
                    </td>
                    <td>
                         <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PublicHolidayDateId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["PublicHolidayGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["PublicHolidayGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>			        
                    <td class="row_footer_blank_right" colspan="4"><%if (ViewData["Access"] == "WriteAccess")
                                                            { %><%= Html.RouteLink("Create Date", "Default", new { action="Create", id = ViewData["PublicHolidayGroupId"] }, new { @class = "red", title = "Create Date" })%><%} %></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_publicholidays').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
