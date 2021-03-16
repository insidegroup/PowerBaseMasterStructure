<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectWorkFlowItems_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - WorkFlow Groups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">WorkFlow Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th style="width:10%" class="row_header_left">Sequence</th> 
			        <th style="width:78%">Form</th> 
			        <th style="width:12%">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.WorkFlowPanelDisplaySequence) %></td>
                    <td colspan="2"><%= Html.Encode(item.FormName) %></td>
                </tr>        
                <% 
                } 
                %>
               <tr>
                    <td colspan="3" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["WorkFlowGroupId"] , page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["WorkFlowGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>			        
                    <td class="row_footer_blank_right"></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_workflowgroups').click();
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
    $("#frmSearch").attr("action", "/WorkFlowItem.mvc/List/<%=ViewData["WorkFlowGroupId"] %>");
    $("#frmSearch").submit();

});
 </script>
</asp:Content>
