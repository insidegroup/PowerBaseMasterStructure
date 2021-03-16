<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeGroups_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Groups (Deleted)</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="40%" class="row_header_left"><%= Html.RouteLink("Reason Code Group Name", "ListMain", new { page =1, sortField = "ReasonCodeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="14%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="9%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ReasonCodeGroupName, 40))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%></td>                    
                    <td><%= Html.RouteLink("Items", "List", new { id = item.ReasonCodeGroupId, controller="ReasonCodeItem" }, new { title="ReasonCode Items"})%></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.ReasonCodeGroupId }, new { title = "View" })%> </td>
                     <td><%if (item.HierarchyType == "ClientSubUnit")
                          { %><%= Html.RouteLink("Hierarchy", "Default", new { id = item.ReasonCodeGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%><%} %></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.ReasonCodeGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.ReasonCodeGroupId }, new { title = "UnDelete" })%>
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
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7"><%= Html.ActionLink("UnDeleted ReasonCode Groups", "ListUndeleted", null, new { @class = "red" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_reasoncodes').click();
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
      $("#frmSearch").attr("action", "/ReasonCodeGroup.mvc/ListDeleted");
      $("#frmSearch").submit();

  });
 </script>
</asp:Content>
