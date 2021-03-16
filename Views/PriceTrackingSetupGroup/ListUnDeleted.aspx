<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPriceTrackingSetupGroups_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Price Tracking Setups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Setups</div></div>
    <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
			    <th width="27%" class="row_header_left"><%= Html.RouteLink("Price Tracking Setup Name", "ListMain", new { page = 1, sortField = "PriceTrackingSetupGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			    <th width="12%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                <th width="10%"><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                <th width="8%"><%= Html.RouteLink("PCC/OID", "ListMain", new { page = 1, sortField = "PseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                <th width="5%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			    <th width="3%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="5%">&nbsp;</th>
                <th width="5%">&nbsp;</th>
			    <th width="5%">&nbsp;</th> 
			    <th width="5%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.PriceTrackingSetupGroupName, 45))%></td>
                <td><%= Html.Encode(item.HierarchyType) %></td>
                <td><%= Html.Encode(item.GDSCode) %></td>
                <td><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
                <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%><%} %></td>
                <td>
                    <%if (item.HasWriteAccess == true) { %>
                        <%if (item.PriceTrackingSetupGroupItemAirId != null && item.PriceTrackingSetupGroupItemAirId > 0) { %>
                            <%= Html.RouteLink("Air", "Default", new { controller = "PriceTrackingSetupGroupItemAir", action = "Edit", id = item.PriceTrackingSetupGroupItemAirId }, new { title = "Air" })%>
                        <% } else { %>
                            <%= Html.RouteLink("Air", "Main", new { controller = "PriceTrackingSetupGroupItemAir", id = item.PriceTrackingSetupGroupId, action = "Create" }, new { title = "Air" })%>
                        <% } %>
                    <% } %>
                </td>
                <td>
                     <%if (item.HasWriteAccess == true) { %>
                        <%if (item.PriceTrackingSetupGroupItemHotelId != null && item.PriceTrackingSetupGroupItemHotelId > 0) { %>
                            <%= Html.RouteLink("Hotel", "Default", new { controller = "PriceTrackingSetupGroupItemHotel", action = "Edit", id = item.PriceTrackingSetupGroupItemHotelId }, new { title = "Hotel" })%>
                        <% } else { %>
                            <%= Html.RouteLink("Hotel", "Main", new { controller = "PriceTrackingSetupGroupItemHotel", id = item.PriceTrackingSetupGroupId, action = "Create" }, new { title = "Hotel" })%>
                        <% } %>
                    <% } %>
                </td>
                <td><%= Html.RouteLink("Export", "Default", new { action = "Export", id = item.PriceTrackingSetupGroupId }, new { title = "Export" })%></td>
                <td><%= Html.RouteLink("Contacts", "Default", new { controller = "PriceTrackingContact", action = "List", id = item.PriceTrackingSetupGroupId }, new { title = "Contacts" })%></td>
                <td>
                    <%if (item.HierarchyType == "ClientSubUnit") { %>
                        <%= Html.RouteLink("Hierarchy", "Default", new { id = item.PriceTrackingSetupGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%>
                    <%} %>
                </td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.PriceTrackingSetupGroupId }, new { title = "View" })%></td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PriceTrackingSetupGroupId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PriceTrackingSetupGroupId }, new { title = "Delete" })%>
                    <%} %>
                </td>
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="13" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%></div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr>   
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="12">
                    <%= Html.ActionLink("Deleted Price Tracking Setups", "ListDeleted", null, new { @class = "red", title="Deleted Price Tracking Setups" })%>
			        <%if (ViewData["Access"] == "WriteAccess") { %>
			            &nbsp;<%= Html.ActionLink("Create Price Tracking Setup", "Create", null, new { @class = "red", title = "Create Price Tracking Setup" })%>
			        <% } %>
			    </td> 
		    </tr> 
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function() {
        $('.full-width #search_wrapper').css('height', 'auto');
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/PriceTrackingSetupGroup.mvc/ListUnDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>