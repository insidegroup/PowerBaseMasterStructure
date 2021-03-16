<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text"><%=Html.Encode(Model.FeeTypeDisplayName)%>s (Orphaned) </div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
			    <th width="60%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientFeeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			    <th width="20%">&nbsp;</th> 
			    <th width="5%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
            foreach (var item in Model.ClientFeeGroupsOrphaned) { 
            %>
            <tr>
                <td colspan="2"><%= Html.Encode(CWTStringHelpers.TrimString(item.ClientFeeGroupName, 45))%></td>                    
                <td><%= Html.RouteLink("Items", "List", new { id = item.ClientFeeGroupId, controller="ClientFeeItem" }, new { title="Client Fee Items"})%></td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ClientFeeGroupId }, new { title = "View" })%> </td>
                <td>
                   <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientFeeGroupId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                        <%if(item.DeletedFlag){%>
                            <%=Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ClientFeeGroupId }, new { title = "UnDelete" })%>
                        <%}else{%>
                            <%=Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientFeeGroupId }, new { title = "Delete" })%>
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
                        <div class="paging_left"><% if (Model.ClientFeeGroupsOrphaned.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "ListOrphaned", page = (Model.ClientFeeGroupsOrphaned.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.ClientFeeGroupsOrphaned.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "ListOrphaned", page = (Model.ClientFeeGroupsOrphaned.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%></div>
                        <div class="paging_centre"><%if (Model.ClientFeeGroupsOrphaned.TotalPages > 0){ %>Page <%=Model.ClientFeeGroupsOrphaned.PageIndex%> of <%=Model.ClientFeeGroupsOrphaned.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr>   
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("UnDeleted " + Html.Encode(Model.FeeTypeDisplayName) + "s", "ListUnDeleted", new { ft = Model.FeeTypeId }, new { @class = "red", title = "UnDeleted " + Html.Encode(Model.FeeTypeDisplayName) + "s" })%></td> 
		    </tr> 
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $("#frmSearch input[name='ft']").val(<%=Model.FeeTypeId%>);
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ClientFeeGroup.mvc/ListOrphaned");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>