<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectLocalOperatingHoursItems_v1Result>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Local Operating Hours Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="31%" class="row_header_left"><%= Html.RouteLink("WeekDay", "List", new { id = ViewData["localOperatingHoursGroupId"], page = 1, sortField = "WeekdayName", sortOrder = ViewData["NewSortOrder"] })%></th> 
			        <th width="29%"><%= Html.RouteLink("Opening DateTime", "List", new { id = ViewData["localOperatingHoursGroupId"], page = 1, sortField = "OpeningDateTime", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="29%"><%= Html.RouteLink("Closing DateTime", "List", new { id = ViewData["localOperatingHoursGroupId"], page = 1, sortField = "ClosingDateTime", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
               
                foreach (var item in Model) { 
                  
                %>
                <tr>
                    <td><%= Html.Encode(item.WeekdayName)%></td>
                    <td><%= Html.Encode(string.Format("{0}{1}{2}", item.OpeningDateTime.ToString("h:mm"), item.OpeningDateTime.Second != 0 ? item.OpeningDateTime.ToString(":ss") : "", item.OpeningDateTime.ToString(" tt")))%></td>
                    <td><%= Html.Encode(string.Format("{0}{1}{2}", item.ClosingDateTime.ToString("h:mm"), item.ClosingDateTime.Second != 0 ? item.ClosingDateTime.ToString(":ss") : "", item.ClosingDateTime.ToString(" tt")))%></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = ViewData["localOperatingHoursGroupId"], weekDayId = item.WeekdayId, openingDateTime = item.OpeningDateTime })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = ViewData["localOperatingHoursGroupId"], weekDayId = item.WeekdayId, openingDateTime = item.OpeningDateTime })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["localOperatingHoursGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["localOperatingHoursGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>			        
                    <td class="row_footer_blank_right" colspan="3">
			        <%if (ViewData["Access"] == "WriteAccess")
             {%>
			        <%= Html.RouteLink("Create Local Operating Hours Item", "Default", new { action="Create", id = ViewData["localOperatingHoursGroupId"] }, new { @class = "red", title = "Create Local Operating Hours Item" })%>
			        <%} %></td> 
		        </tr> 
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_localoperatinghours').click();
		$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>
