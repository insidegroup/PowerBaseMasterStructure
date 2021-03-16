<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ProductGroupsVM>" %><%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Passive Segment Builder</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Passive Segment Builder Products Groups (Orphaned)</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="36%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ProductGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="15%">Hierarchy</th> 
			        <th width="35%">Hierarchy Item</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ProductGroupsOrphaned) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductGroupName) %></td>
                    <td></td>
                    <td></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ProductGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ProductGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ProductGroupId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>              
                <% 
                } 
                %>
                <tr>
                <td colspan="6" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.ProductGroupsOrphaned.HasPreviousPage)
                                                    { %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListUnDeleted", page = (Model.ProductGroupsOrphaned.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.ProductGroupsOrphaned.HasNextPage)
                                                      { %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListUnDeleted", page = (Model.ProductGroupsOrphaned.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.ProductGroupsOrphaned.TotalPages > 0)
                                                     { %>Page <%=Model.ProductGroups.PageIndex%> of <%=Model.ProductGroupsOrphaned.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>		    <tr>   
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("UnDeleted PSB Groups", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted Passive Segment Builder Products Groups" })%></td> 
		    </tr> 
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_passivesegmentbuilder').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/PassiveSegmentBuilderGroup.mvc/ListOrphaned");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>