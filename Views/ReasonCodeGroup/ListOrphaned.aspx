<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeGroupsOrphaned_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - ReasonCode Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Groups (Orphaned)</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
			    <th width="60%" class="row_header_left"><%= Html.RouteLink("Reason Code Group Name", "ListMain", new { page = 1, sortField = "ReasonCodeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			    <th width="20%">&nbsp;</th> 
			    <th width="5%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td colspan="2"><%= Html.Encode(CWTStringHelpers.TrimString(item.ReasonCodeGroupName, 45))%></td>                    
                <td><%= Html.RouteLink("Items", "List", new { id = item.ReasonCodeGroupId, controller="ReasonCodeItem" }, new { title="ReasonCode Items"})%></td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ReasonCodeGroupId }, new { title = "View" })%> </td>
                <td>
                   <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ReasonCodeGroupId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                        <%if(item.DeletedFlag){%>
                            <%=Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ReasonCodeGroupId }, new { title = "UnDelete" })%>
                        <%}else{%>
                            <%=Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ReasonCodeGroupId }, new { title = "Delete" })%>
                        <%} %>
                    <%} %>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="6" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListOrphaned", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListOrphaned", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%></div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr>   
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("UnDeleted Reason Code Groups", "ListUnDeleted", null, new { @class = "red", title="UnDeleted Reason Code Groups" })%></td> 
		    </tr> 
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_reasoncodes').click();
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
        $("#frmSearch").attr("action", "/ReasonCodeGroup.mvc/ListOrphaned");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>