<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectServicingOptionItems_v1Result>>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Servicing Option Groups
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Servicing Option Items</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="10%" class="row_header_left"><%= Html.RouteLink("Order", "List", new { id = ViewData["servicingOptionGroupId"], page = 1, sortField = "ServicingOptionItemSequence", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
		        <th width="35%"><%= Html.RouteLink("Name", "List", new { id = ViewData["servicingOptionGroupId"], page = 1, sortField = "ServicingOptionName", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
		        <th width="15%">Value</th> 
				<th width="15%">Parameters</th> 
		        <th width="10%">GDS</th> 
		        <th width="4%">&nbsp;</th> 
		        <th width="4%">&nbsp;</th> 
		        <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><% if (item.ServicingOptionItemSequence!=0) { %>
						<%:item.ServicingOptionItemSequence%>
					<% } %>
                </td>
                <td><%= Html.Encode(item.ServicingOptionName)%></td>
                <td><%=CWTStringHelpers.TrimString(Html.Encode(item.ServicingOptionItemValue), 30)%></td>
 				<td><%= Html.Encode(item.Parameters)%></td>
				<td><%= Html.Encode(item.GDSName)%></td>
                <td><%= Html.RouteLink("View", "Default", new { id = item.ServicingOptionItemId, action="View" }, new { title = "View" })%> </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Edit", "Default", new { id = item.ServicingOptionItemId, action = "Edit" }, new { title = "Edit" })%><%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Delete", "Default", new { id = item.ServicingOptionItemId, action = "Delete" }, new { title = "Delete" })%><%} %>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "List", new { page = (Model.PageIndex - 1), id = ViewData["ServicingOptionGroupId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "List", new { page = (Model.PageIndex + 1), id = ViewData["ServicingOptionGroupId"], sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%></div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
		        <td class="row_footer_blank_right" colspan="7">
		        <%if (ViewData["Access"] == "WriteAccess"){%>
		            <%= Html.ActionLink("Edit Order", "SelectServicingOptionToOrder", new { id = ViewData["ServicingOptionGroupId"] }, new { @class = "red", title = "Edit Order" })%>&nbsp;
		            <%= Html.ActionLink("Create Servicing Option Item", "Create", new { id = ViewData["ServicingOptionGroupId"] }, new { @class = "red", title = "Create Servicing Option Item" })%></td> 
		        <%}%>
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_servicingoptions').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Servicing Option Groups", "Main", new { controller = "ServicingOptionGroup", action = "ListUnDeleted", }, new { title = "Servicing Option Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ServicingOptionGroupName"].ToString(), "Default", new { controller = "ServicingOptionGroup", action = "View", id = ViewData["ServicingOptionGroupId"] }, new { title = ViewData["ServicingOptionGroupName"].ToString() })%> &gt;
Items
</asp:Content>