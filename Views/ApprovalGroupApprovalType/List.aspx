<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectApprovalGroupApprovalTypes_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Types</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="20%" class="row_header_left">Approval Type ID</th> 
			        <th width="69%">Approval Type</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ApprovalGroupApprovalTypeId)%></td>
                    <td><%= Html.Encode(item.ApprovalGroupApprovalTypeDescription)%></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.ApprovalGroupApprovalTypeId })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.ApprovalGroupApprovalTypeId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ApprovalGroupApprovalType", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ApprovalGroupApprovalType", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            <tr> 
                <td class="row_footer_blank_left">
                    <a href="javascript:history.back();" class="red" title="Back">Back</a> 
                    <a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
                <td class="row_footer_blank_right" colspan="3">
                    <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Create", "Create", null, new { @class = "red", title = "Create" })%><%} %>
                </td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
