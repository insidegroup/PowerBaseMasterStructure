<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectApprovalGroups_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Approval Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Groups (Deleted)</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="40%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page =1, sortField = "ApprovalGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="25%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="14%"><%= Html.RouteLink("Enabled Date", "ListMain", new { page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ApprovalGroupName, 40))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%if(item.EnabledDate != null) { %>
							<%= Html.Encode(string.Format("{0:MMM dd, yyyy}", item.EnabledDate)) %>
						<% } %>
                    </td>                            
                    <td><%= Html.RouteLink("Items", "List", new { id = item.ApprovalGroupId, controller="ApprovalItem" }, new { title="CommissionableRoute Items"})%></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.ApprovalGroupId }, new { title = "View" })%> </td>
					<td>
                        <%if((bool)item.HasWriteAccess.Value == true){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.ApprovalGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if((bool)item.HasWriteAccess.Value == true){%>
                        <%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.ApprovalGroupId }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:window.print();" class="red btn-small" title="Print">Print</a> 
					</td>
					<td colspan="6" class="row_footer_blank_right">
						<%= Html.ActionLink("UnDeleted Approval Groups", "ListUNDeleted", null, new { @class = "red btn-small", title="UnDeleted Approval Groups"})%>&nbsp;
                    </td>
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_approvalgroups').click();
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
      $("#frmSearch").attr("action", "/ApprovalGroup.mvc/ListDeleted");
      $("#frmSearch").submit();

  });
 </script>
</asp:Content>
