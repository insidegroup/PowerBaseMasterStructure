<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Hotel Property Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="10%" class="row_header_left"><%=  Html.RouteLink("Sequence", "List",new{controller = "PolicyHotelPropertyGroupItem",action = "List",id = ViewData["PolicyGroupID"], page = 1,sortField = "Name",sortOrder = ViewData["NewSortOrder"].ToString()},new { title = "SequenceNumber" })%></th>
                	<th width="55%"><%=  Html.RouteLink("Policy Hotel Status", "List", new { controller = "PolicyHotelPropertyGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicyHotelStatus", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Policy Location" })%></th>
			        <th width="20%"></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber)%></td>
                    <td><%= Html.Encode(item.PolicyHotelStatus)%></td>
                    <td><%= Html.ActionLink("Hotel Property Advice", "List", "HotelPropertyAdvice", new { id = item.PolicyHotelPropertyGroupItemId }, null)%></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.PolicyHotelPropertyGroupItemId })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.PolicyHotelPropertyGroupItemId })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.PolicyHotelPropertyGroupItemId })%>
                        <%} %>
                    </td>                  
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="3" class="row_footer_left">
                    <% if (Model.HasPreviousPage) { %>
                        <%=  Html.RouteLink("<<Previous Page", "List", new { controller = "PolicyHotelPropertyGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                    <% } %>
                    </td>
                    <td colspan="3" class="row_footer_right">
                    <% if (Model.HasNextPage) {  %>
                        <%=  Html.RouteLink("Next Page>>", "List", new { controller = "PolicyHotelPropertyGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                    <% } %> 
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="5">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.ActionLink("Create Policy Hotel Property Group Item", "Create", new { id = ViewData["PolicyGroupID"] }, new { @class = "red" , title="Create Policy Hotel Property Group Item"})%></td> 
		            <% } %>
		         </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Hotel Property Group Items
</asp:Content>