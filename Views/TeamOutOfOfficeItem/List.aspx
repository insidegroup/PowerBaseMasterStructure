<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeamOutOfOfficeItems_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Out of Office Backup Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Out of Office Backup Teams</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="22%" class="row_header_left"><%= Html.RouteLink("Primary Team", "ListMain", new { page =1, sortField = "PrimaryTeamName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="22%"><%= Html.RouteLink("1st Backup Team", "ListMain", new { page = 1, sortField = "PrimaryBackupTeamName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="22%"><%= Html.RouteLink("2nd Backup Team", "ListMain", new { page = 1, sortField = "SecondaryBackupTeamName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
                    <th width="22%"><%= Html.RouteLink("3rd Backup Team", "ListMain", new { page = 1, sortField = "TertiaryBackupTeamName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="12%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PrimaryTeamName)%></td>
                    <td><%= Html.Encode(item.PrimaryBackupTeamName) %></td>
                    <td><%= Html.Encode(item.SecondaryBackupTeamName) %></td>
                    <td><%= Html.Encode(item.TertiaryBackupTeamName) %></td>
					<td>
                        <%if((bool)item.HasWriteAccess.Value == true){%>
                            <%=  Html.ActionLink("Edit", "Edit", new { id = item.TeamOutOfOfficeItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr>
                    <td class="row_footer_blank_left" colspan="2">
                        <a href="javascript:history.back();" class="red" title="Back">Back</a>
                        <a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td> 
			        <td class="row_footer_blank_right" colspan="3">&nbsp;</td>
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    	$('#menu_chatmessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').hide();
        $('#breadcrumb').css('width', 'auto');
  });
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Team Out of Office Groups", "Main", new { controller = "TeamOutOfOfficeGroup", action = "ListUnDeleted", }, new { title = "Team Out of Office Groups" })%> &gt;
<%=Html.Encode(ViewData["TeamOutOfOfficeGroupName"].ToString()) %> &gt;
Out of Office Backup Teams
</asp:Content>