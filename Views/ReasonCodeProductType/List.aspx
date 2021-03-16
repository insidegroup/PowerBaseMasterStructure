<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeProductTypes_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Product Types</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="14%" class="row_header_left"><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title="Sort by Product"})%></th> 
			        <th width="12%"><%= Html.RouteLink("Reason Code", "ListMain", new { page = 1, sortField = "ReasonCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort by Reason Code" })%></th> 
                    <th width="42%"><%= Html.RouteLink("Reason Code Product Type", "ListMain", new { page = 1, sortField = "ReasonCodeProductTypeTravelerDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort by Reason Code Product Type Traveler Description" })%></th> 
                    <th width="18%">&nbsp;</th> 
			        <th width="14%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductName) %></td>
                    <td><%= Html.Encode(item.ReasonCode) %></td>
                    <td><%= Html.Encode(item.ReasonCodeTypeDescription) %></td>
                    <td><%= Html.RouteLink("Traveler Descriptions", "Main", new { controller = "ReasonCodeProductTypeTravelerDescription", action="List", productId = item.ProductId, reasonCode = item.ReasonCode, reasonCodeTypeId = item.ReasonCodeTypeId }, new { title = "Traveler Descriptions" })%></td>                    
                    <td><%= Html.RouteLink("Descriptions", "Main", new { controller = "ReasonCodeProductTypeDescription", action="List", productId = item.ProductId, reasonCode = item.ReasonCode, reasonCodeTypeId = item.ReasonCodeTypeId }, new { title = "Descriptions" })%></td>
                </tr>              
                <% 
                } 
                %>
                <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="4"></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_reasoncodes').click();
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ReasonCodeProductType.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>