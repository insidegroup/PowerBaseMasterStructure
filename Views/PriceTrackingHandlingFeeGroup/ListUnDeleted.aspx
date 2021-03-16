<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPriceTrackingHandlingFeeGroups_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Groups</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="35%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page =1, sortField = "PriceTrackingHandlingFeeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="25%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="9%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "Multiple", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="10%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.PriceTrackingHandlingFeeGroupName, 40))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%><%} %></td>
                    <td><%= Html.RouteLink("Items", "List", new { id = item.PriceTrackingHandlingFeeGroupId, controller="PriceTrackingHandlingFeeItem" }, new { title="Price Tracking Group Items"})%></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.PriceTrackingHandlingFeeGroupId }, new { title = "View" })%> </td>
                    <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.RouteLink("Hierarchy", "Default", new { id = item.PriceTrackingHandlingFeeGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%><%} %></td>
                    <td>
                        <%if((bool)item.HasWriteAccess.Value == true){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.PriceTrackingHandlingFeeGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if((bool)item.HasWriteAccess.Value == true){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.PriceTrackingHandlingFeeGroupId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="8" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td colspan="7" class="row_footer_blank_right">
						<%= Html.ActionLink("Deleted Price Tracking Groups", "ListDeleted", null, new { @class = "red btn-small", title="Deleted Price Tracking Groups"})%>&nbsp;
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Price Tracking Group", "Create", null, new { @class = "red btn-small" ,title="Create Price Tracking Group" })%>
						<%} %>
                    </td>
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_pricetracking').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    });

    //Search
    $('#btnSearch').click(function() {
      if ($('#filter').val() == "") {
          alert("No Search Text Entered");
          return false;
      }
      $("#frmSearch").attr("action", "/PriceTrackingHandlingFeeGroup.mvc/ListUnDeleted");
      $("#frmSearch").submit();

  });
 </script>
</asp:Content>
