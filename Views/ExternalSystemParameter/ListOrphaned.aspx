<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectExternalSystemParametersOrphaned_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - External System Parameters</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">External System Parameters (Orphaned)</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
			    <th width="60%" class="row_header_left"><%= Html.RouteLink("External System Parameter Name", "ListMain", new { page = 1, sortField = "Value", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
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
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ExternalSystemParameterValue, 60))%></td>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.ExternalSystemParameterTypeName, 20))%></td>                    
                <td><%= Html.RouteLink("Items", "List", new { id = item.ExternalSystemParameterId, controller = "ServicingOptionItem" }, new { title="External System Parameter Items"})%></td>
                <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ExternalSystemParameterId }, new { title = "View" })%> </td>
                <td>
                   <%if(item.HasWriteAccess.Value){%>
                    <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ExternalSystemParameterId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                        <%if(item.DeletedFlag){%>
                            <%=Html.RouteLink("UnDelete", "Default", new { action = "UnDelete", id = item.ExternalSystemParameterId }, new { title = "UnDelete" })%>
                        <%}else{%>
                            <%=Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ExternalSystemParameterId }, new { title = "Delete" })%>
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
			    <td class="row_footer_blank_right" colspan="5"><%= Html.ActionLink("UnDeleted External System Parameters", "ListUnDeleted", null, new { @class = "red", title="UnDeleted External System Parameters" })%></td> 
		    </tr> 
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_externalsystemparameters').click();
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
        $("#frmSearch").attr("action", "/ExternalSystemParameter.mvc/ListOrphaned");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>