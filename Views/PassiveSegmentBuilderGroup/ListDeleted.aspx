<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ProductGroupsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Passive Segment Builder</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Passive Segment Builder Products Groups (Deleted)</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="34%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ProductGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="15%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="34%"><%= Html.RouteLink("Hierarchy Item", "ListMain", new { page = 1, sortField = "HierarchyItem", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			       <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="9%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ProductGroups) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductGroupName) %></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.HierarchyItem) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ProductGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ProductGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ProductGroupId }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>              
                <% 
                } 
                %>
                <tr>
                <td colspan="6" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.ProductGroups.HasPreviousPage)
                                                    { %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.ProductGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.ProductGroups.HasNextPage)
                                                      { %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.ProductGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.ProductGroups.TotalPages > 0)
                                                     { %>Page <%=Model.ProductGroups.PageIndex%> of <%=Model.ProductGroups.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("UnDeleted PSB Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Passive Segment Builder Products Groups" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_passivesegmentbuilder').click();
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/PassiveSegmentBuilderGroup.mvc/ListDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>