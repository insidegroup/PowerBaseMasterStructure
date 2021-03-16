<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectApprovalItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Approval Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="40%" class="row_header_left"><%=  Html.RouteLink("Approver Name", "ListMain", new { controller = "ApprovalItem", action = "List", id = ViewData["ApprovalGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ApproverDescription" }, new { title = "Approver Name" })%></th>
                    <th width="40%"><%= Html.RouteLink("Remark Text", "ListMain", new { controller = "ApprovalItem", action = "List", id = ViewData["ApprovalGroupId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "RemarkText" }, new { title = "Remark Text" })%></th>
			        <th width="3%">&nbsp;</th> 
			        <th width="3%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) {  
                %>
                <tr>
                    <td><%= Html.Encode(item.ApproverDescription)%></td>
                    <td><%= Html.Encode(item.RemarkText) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { id = item.ApprovalItemId, action = "View" }, new { title = "View" })%></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { id = item.ApprovalItemId, action = "Edit" }, new { title = "Edit" })%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Delete", "Default", new { id = item.ApprovalItemId, action = "Delete" }, new { title = "Delete" })%><%} %></td>                
                </tr>
                <% 
                } 
                %>
                 <tr>
                    <td colspan="11" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = ViewData["ApprovalGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { id = ViewData["ApprovalGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> 
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
					<td class="row_footer_blank_right" colspan="10">
						<%if (ViewData["Access"] == "WriteAccess"){%>
							<%= Html.RouteLink("Create Approval Item", "Main", new { id = ViewData["ApprovalGroupId"], action = "Create" }, new { @class = "red", title = "Create Approval Item" })%>
						<%} %>
			        </td> 
		        </tr> 
			</table>    
        </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#search').show();
        $('#menu_approvalgroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=ViewData["ApprovalGroupId"]%>);
    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ApprovalItem.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Approval Item Groups", "Main", new { controller = "ApprovalGroup", action = "ListUnDeleted", }, new { title = "Approval Item Groups" })%> &gt;
<%=ViewData["ApprovalGroupName"]%> &gt;
Approval Items
</asp:Content>