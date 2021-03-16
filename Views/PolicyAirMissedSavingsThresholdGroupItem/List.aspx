<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirMissedSavingsThresholdGroupItemsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Missed Savings Threshold Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="10%" class="row_header_left"><%=  Html.RouteLink("Amount", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "MissedThresholdAmount", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Amount" })%></th>
                    <th width="9%"><%=  Html.RouteLink("Currency", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "CurrencyCode", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Curreny" })%></th>
                    <th width="18%"><%=  Html.RouteLink("Routing", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "RoutingCode", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Routing" })%></th>
                    <th width="16%"><%=  Html.RouteLink("Travel Date From", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "TravelDateValidFrom", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Travel Date From" })%></th>
                    <th width="16%"><%=  Html.RouteLink("Travel Date To", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "TravelDateValidTo", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Travel Date To" })%></th>
                    <th width="10%"><%=  Html.RouteLink("Advice", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advice" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.PolicyAirMissedSavingsThresholdGroupItems) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.MissedThresholdAmount != null ? item.MissedThresholdAmount.Value.ToString("N2") : "")%></td>
                    <td><%= Html.Encode(item.CurrencyCode)%></td>
                    <td><%= Html.Encode(item.RoutingCode) %></td>
                    <td><%= Html.Encode(item.TravelDateValidFrom.HasValue ? item.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "")%></td>  
                    <td><%= Html.Encode(item.TravelDateValidTo.HasValue ? item.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "")%></td>  
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td>
                        <%= Html.RouteLink("Advice", "List", new { controller = "MissedSavingsAdvice", action = "List", id = item.PolicyAirMissedSavingsThresholdGroupItemId }, new { title = "Airline Advice" })%>
                    </td>
                    <td><%=  Html.RouteLink("View", "List", new { action = "View", id = item.PolicyAirMissedSavingsThresholdGroupItemId }, new { title = "View" })%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
                        <%=  Html.RouteLink("Edit", "List", new { action = "Edit", id = item.PolicyAirMissedSavingsThresholdGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (Model.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Delete", "List", new { action = "Delete", id = item.PolicyAirMissedSavingsThresholdGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="10" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.PolicyAirMissedSavingsThresholdGroupItems.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirMissedSavingsThresholdGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.PolicyAirMissedSavingsThresholdGroupItems.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyAirMissedSavingsThresholdGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyAirMissedSavingsThresholdGroupItems.TotalPages > 0)
                                                     { %>Page <%=Model.PolicyAirMissedSavingsThresholdGroupItems.PageIndex%> of <%=Model.PolicyAirMissedSavingsThresholdGroupItems.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="9">
			        <%if (Model.HasWriteAccess)
             { %>
			        <%= Html.RouteLink("Create Policy Air Missed Savings Threshold Group Item", "Default", new { action = "Create", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Air Missed Savings Threshold Group Item" })%>
			        <% } %> 
			        </td> 
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
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy Air Missed Savings Threshold Group Items
</asp:Content>