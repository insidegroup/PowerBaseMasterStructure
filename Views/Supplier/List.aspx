<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectSuppliers_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Supplier</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="30%" class="row_header_left"><%= Html.RouteLink("Supplier Name", "ListMain", new { page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="30%"><%= Html.RouteLink("Supplier Code", "ListMain", new { page = 1, sortField = "SupplierCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="29%"><%= Html.RouteLink("Product", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
					<th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SupplierName)%></td>                    
                    <td><%= Html.Encode(item.SupplierCode) %></td>
                    <td><%= Html.Encode(item.ProductName) %></td>
					<td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.SupplierCode, productId = item.ProductId }, new { title = "Edit" })%>
						<%} %>
					</td>
					<td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.SupplierCode, productId = item.ProductId }, new { title = "Delete" })%>
						<%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:window.print();" class="red" title="Print">Print</a> 
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="4">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Supplier", "Create", new { }, new { @class = "red", title = "Create Supplier" })%>
						<% } %>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_admin').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').show();
	});

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/Supplier.mvc/List");
        $("#frmSearch").submit();
    });
 </script>
 </asp:Content>