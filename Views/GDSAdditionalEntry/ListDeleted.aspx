<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectGDSAdditionalEntries_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Additional Entries
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Additional Entries (Deleted)</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="41%" class="row_header_left"><%= Html.RouteLink("Value", "ListMain", new { page = 1, sortField = "GDSAdditionalEntryValue", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="10%"></th>
			        <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="12%"><%= Html.RouteLink("Enabled Date", "ListMain", new { page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="9%" class="row_header_right">&nbsp;</th> 
		        </tr> 
               <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.GDSAdditionalEntryValue, 60))%></td>
                    <td></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>                    
                   <td><%= Html.ActionLink("View", "View", new { id = item.GDSAdditionalEntryId }, new { title="View"})%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.GDSAdditionalEntryId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.GDSAdditionalEntryId }, new { title = "UnDelete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
              <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListDeleted", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
		            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="6"><%= Html.ActionLink("UnDeleted GDS Additional Entries", "ListUnDeleted", null, new { @class = "red", title = "UnDeleted GDS Additional Entries" })%></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_gdsadditionalentries').click();
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
        $("#frmSearch").attr("action", "/GDSAdditionalEntry.mvc/ListDeleted");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>