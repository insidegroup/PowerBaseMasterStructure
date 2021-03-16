<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTripTypeItems_v1Result>>" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - TripTypeGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Type Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="50%" class="row_header_left"><%= Html.RouteLink("Trip Type", "List", new { id = ViewData["TripTypeGroupId"], page = 1, sortField = "TripTypeDescription", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Description" })%></th> 
			        <th width="30%" class="row_header_left"><%= Html.RouteLink("Back Office Trip Type Code", "List", new { id = ViewData["TripTypeGroupId"], page = 1, sortField = "BackOfficeTripTypeCode", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Back Office Trip Type Code" })%></th> 
                    <th width="8%">Default?</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TripTypeDescription) %></td>
                    <td><%= Html.Encode(item.BackOfficeTripTypeCode)%></td>
                    <td><%= Html.Encode(item.DefaultTripTypeFlag)%></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.TripTypeId, tripTypeGroupid = ViewData["TripTypeGroupId"] }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Remove", "Default", new { action = "Delete", id = item.TripTypeId, tripTypeGroupid = ViewData["TripTypeGroupId"] }, new { title = "Remove" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["TripTypeGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["TripTypeGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
		            <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4">
			            <%if (ViewData["Access"] == "WriteAccess"){ %>
                            <%= Html.RouteLink("Add Trip Type Item", "Default", new { controller = "TripTypeitem", action = "Create", id = ViewData["TripTypeGroupId"] }, new { @class = "red", title = "Add Trip Type Item" })%>
                        <%} %></td>
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_triptypegroups').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Trip Type Groups", "Main", new { controller = "TripTypeGroup", action = "ListUnDeleted", }, new { title = "TripTypeGroups" })%> &gt;
<%=Html.RouteLink(ViewData["TripTypeGroupName"].ToString(), "Default", new { controller = "TripTypeGroup", action = "View", id = ViewData["TripTypeGroupId"] }, new { title = ViewData["TripTypeGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Trip Type Items", "Default", new { controller = "TripTypeItem", action = "List", id = ViewData["TripTypeGroupId"] }, new { title = "TripTypeItems" })%>
</asp:Content>