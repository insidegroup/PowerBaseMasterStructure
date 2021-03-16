<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyGroups_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Groups (Deleted)</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="38%" class="row_header_left"><%= Html.RouteLink("PolicyGroupName", "ListMain", new { page = 1, sortField = "PolicyGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="25%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="11%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="9%" class="row_header_right">&nbsp;</th> 
		        </tr> 
		        </tr> 
                 <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.PolicyGroupName, 45))%></td>                    
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%if (item.HierarchyType == "ClientSubUnit") { %><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%><%} %></td>  
                    <td><%= Html.RouteLink("Items", "Default", new { action = "ListSubMenu", id = item.PolicyGroupId }, new { title = "List SubMenu" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.PolicyGroupId }, new { title = "View" })%> </td>
                      <td><%if (item.HierarchyType == "ClientSubUnit")
                          { %><%= Html.RouteLink("Hierarchy", "Default", new { id = item.PolicyGroupId, action = "LinkedClientSubUnits" }, new { title = "Hierarchy" })%><%} %></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.PolicyGroupId }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="8" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                  <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7"><%= Html.ActionLink("UnDeleted Policy Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted PolicyGroups" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
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
        $("#frmSearch").attr("action", "/PolicyGroup.mvc/ListDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>
