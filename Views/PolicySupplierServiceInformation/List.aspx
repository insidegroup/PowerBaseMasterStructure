<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicySupplierServiceInformations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Supplier Service Information</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="45%" class="row_header_left"><%=  Html.RouteLink("Value", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicySupplierServiceInformationValue", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Value" })%></th>
                	<th width="38%"><%=  Html.RouteLink("Description", "List", new { id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicySupplierServiceInformationTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Description" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PolicySupplierServiceInformationValue) %></td>
                    <td><%= Html.Encode(item.PolicySupplierServiceInformationTypeDescription) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { id = item.PolicySupplierServiceInformationId, action = "View" }, new { title = "View" })%> </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%= Html.RouteLink("Edit", "Default", new { id = item.PolicySupplierServiceInformationId, action = "Edit" }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%= Html.RouteLink("Delete", "Default", new { id = item.PolicySupplierServiceInformationId, action = "Delete" }, new { title = "Delete" })%>
                        <%} %>
                    </td>                  
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["PolicyGroupID"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["PolicyGroupID"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                <tr>
                   <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.ActionLink("Create Policy Supplier Service Information", "Create", new { id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Create PolicySupplierServiceInformation" })%></td> 
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
Policy Supplier Service Information
</asp:Content>