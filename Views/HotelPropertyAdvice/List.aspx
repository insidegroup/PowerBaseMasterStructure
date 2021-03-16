<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyHotelPropertyGroupItemHotelAdvice_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Hotel Property Advice</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="30%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = ViewData["PolicyHotelPropertyGroupItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                    <th width="55%"><%=  Html.RouteLink("Hotel Advice", "List", new { id = ViewData["PolicyHotelPropertyGroupItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "HotelAdvice" }, new { title = "Sort By Hotel Advice" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LanguageName) %></td>
                    <td><%= Html.Encode(item.HotelAdvice) %></td>
                    <td><%=  Html.RouteLink("View", "LanguageView", new { action = "View", languageCode = item.LanguageCode, id = item.PolicyHotelPropertyGroupItemId }, new { title = "View" })%></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                            <%=  Html.RouteLink("Edit", "LanguageView", new { action = "Edit", languageCode = item.LanguageCode, id = item.PolicyHotelPropertyGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                            <%=  Html.RouteLink("Delete", "LanguageView", new { action = "Delete", languageCode = item.LanguageCode, id = item.PolicyHotelPropertyGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["PolicyHotelPropertyGroupItemId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["PolicyHotelPropertyGroupItemId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
			        <td class="row_footer_blank_right" colspan="5">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Create Hotel Property Advice", "Default", new { action="Create", id = ViewData["PolicyHotelPropertyGroupItemId"] }, new { @class = "red", title = "Create Hotel Property Advice" })%>
			        <% } %> 
			        </td> 
		        </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_policies').click();
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
<%=Html.RouteLink("Policy Hotel Property Group Items", "Default", new { controller = "PolicyHotelPropertyGroupItem", action = "List", id = ViewData["PolicyGroupId"] }, new { title = "Policy Hotel Property Group Items" })%> &gt;
<%=Html.RouteLink("Item", "Default", new { controller = "PolicyHotelPropertyGroupItem", action = "View", id = ViewData["PolicyHotelPropertyGroupItemId"] }, new { title = "Item" })%> &gt;
Hotel Property Advice
</asp:Content>