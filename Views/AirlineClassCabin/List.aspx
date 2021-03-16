<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectAirlineClassCabins_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - AirlineClassCabin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Airline Class Cabins</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="10%" class="row_header_left"><%=  Html.RouteLink("Sequence", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PolicyRoutingSequenceNumber" }, new { title = "Sort By Policy Routing Sequence" })%></th>
                    <th width="8%"><%=  Html.RouteLink("Class", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "AirlineClassCode" }, new { title = "Sort By Airline Class Code" })%></th>
                     <th width="33%"><%=  Html.RouteLink("Supplier", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "SupplierName" }, new { title = "Sort By Supplier Name" })%></th>
                     <th width="33%"><%=  Html.RouteLink("Routing Name", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "Name" }, new { title = "Sort By Name" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                
                foreach (var item in Model) { 
                 
                %>
                <tr>
                    <td><%= Html.Encode(item.PolicyRoutingSequenceNumber)%></td>
                    <td><%= Html.Encode(item.AirlineClassCode)%></td>
                    <td><%= Html.Encode(item.SupplierName)%></td>
                    <td><%= Html.Encode(item.Name) %></td>
                     <td><%= Html.RouteLink("View", "Main", new { id = item.AirlineClassCode, sCode = item.SupplierCode, rId = item.PolicyRoutingId, action = "View" }, new { title = "View" })%> </td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Edit", "Main", new { id = item.AirlineClassCode, sCode = item.SupplierCode, rId = item.PolicyRoutingId, action = "Edit" }, new { title = "Edit" })%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Delete", "Main", new { id = item.AirlineClassCode, sCode = item.SupplierCode, rId = item.PolicyRoutingId, action = "Delete" }, new { title = "Delete" })%><%} %></td>                
                </tr>
                <% 
                } 
                %>
                 <tr>
                <td colspan="7" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "AirlineClassCabin", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
             <tr> 
                 <td colspan="3" class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4">
			        <%if (ViewData["Access"] == "WriteAccess"){%>
                    <%= Html.RouteLink("Create Airline Class Cabin", "Main", new { action = "Create" }, new { @class = "red", title = "Create Airline Class Cabin" })%>
			        <%} %></td> 
		        </tr> 
        </table>    
        </div>
</div>
<script type="text/javascript">
$(document).ready(function() {
    $('#search').show();
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})

//Search
$('#btnSearch').click(function() {
    if ($('#filter').val() == "") {
        alert("No Search Text Entered");
        return false;
    }
    $("#frmSearch").attr("action", "/AirlineClassCabin.mvc/List");
    $("#frmSearch").submit();
});
 </script>
</asp:Content>
