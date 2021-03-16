<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeItems_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="6%" class="row_header_left"><%= Html.RouteLink("Display Order", "List", new { id = ViewData["ReasonCodeGroupId"], page = 1, sortField = "DisplayOrder", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Display Order" })%></th> 
                	<th width="12%" class="row_header_left"><%= Html.RouteLink("Reason Code", "List", new { id = ViewData["ReasonCodeGroupId"], page = 1, sortField = "ReasonCode", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Reason Code" })%></th> 
                	<th width="23%" class="row_header_left"><%= Html.RouteLink("Reason Code Type", "List", new { id = ViewData["ReasonCodeGroupId"], page = 1, sortField = "ReasonCodeTypeDescription", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Reason Code Type" })%></th> 
                	<th width="10%" class="row_header_left"><%= Html.RouteLink("Product", "List", new { id = ViewData["ReasonCodeGroupId"], page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Product" })%></th> 
                	<th width="8%" class="row_header_left"><%= Html.RouteLink("Traveler Facing", "List", new { id = ViewData["ReasonCodeGroupId"], page = 1, sortField = "TravelerFacingFlag", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Traveler Facing" })%></th> 
			        <th width="18%">&nbsp;</th> 
			        <th width="13%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.DisplayOrder) %></td>
                    <td><%= Html.Encode(item.ReasonCode)%></td>
                    <td><%= Html.Encode(item.ReasonCodeTypeDescription)%></td>
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.TravelerFacingFlag)%></td>
                    <td><%= Html.RouteLink("Traveler  Descriptions", "List", new { controller = "ReasonCodeTravelerDescription", id = item.ReasonCodeItemId }, new { title = "Traveler Descriptions" })%> </td>
                    <td><%= Html.RouteLink("Alt Descriptions", "List", new { controller = "ReasonCodeAlternativeDescription", id = item.ReasonCodeItemId }, new { title = "Alternative Descriptions" })%> </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ReasonCodeItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ReasonCodeItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="9" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["ReasonCodeGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["ReasonCodeGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>

		        <tr> 
		           <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
			        <td class="row_footer_blank_right" colspan="7">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
                    <%= Html.RouteLink("Edit Order", "Default", new { controller = "ReasonCodeItem", action = "EditSequenceTypeSelection", id = ViewData["ReasonCodeGroupId"] }, new { @class = "red", title = "Edit Order" })%>
			        <%= Html.RouteLink("Create Reason Code Item", "Default", new { controller = "ReasonCodeItem", action = "Create", id = ViewData["ReasonCodeGroupId"] }, new { @class = "red", title = "Create Reason Code Item" })%>
            <%} %></td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_reasoncodes').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    })
 </script>
 

</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Groups", "Main", new { controller = "ReasonCodeGroup", action = "ListUnDeleted", }, new { title = "Reason Code Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ReasonCodeGroupName"].ToString(), "Default", new { controller = "ReasonCodeGroup", action = "View", id = ViewData["ReasonCodeGroupId"].ToString() }, new { title = ViewData["ClientTopUnitName"] })%> &gt;
Items
</asp:Content>