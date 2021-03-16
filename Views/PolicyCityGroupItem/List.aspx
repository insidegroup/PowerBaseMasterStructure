<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyCityGroupItemsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy City Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="5%" class="row_header_left"><%=  Html.RouteLink("Order", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "SequenceNumber", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Order" })%></th>
                    <th width="20%"><%=  Html.RouteLink("City", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Name" })%></th>
                    <th width="13%"><%=  Html.RouteLink("Status", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "PolicyCityStatusDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Status" })%></th>
                    <th width="19%"><%=  Html.RouteLink("Inherit From Parent", "List", new { id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "InheritFromParentFlag", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Status" })%></th>
                    <th width="12%"><%=  Html.RouteLink("Enabled Date", "List", new {id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Enabled Date" })%></th>
                    <th width="10%"><%=  Html.RouteLink("Advice", "List", new {id = Model.PolicyGroup.PolicyGroupId, page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Advice" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.PolicyCityGroupItems) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber )%></td>
                    <td><%= Html.Encode(item.Name)%></td>
                    <td><%= Html.Encode(item.PolicyCityStatusDescription) %></td>
                    <td><%= Html.Encode(item.InheritFromParentFlag) %></td>
                    <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td>
                        <%= Html.RouteLink("Advice", "List", new { controller = "CityAdvice", action = "List", id = item.PolicyCityGroupItemId }, new { title = "Airline Advice" })%>
                    </td>
                    <td><%=  Html.RouteLink("View", "List", new { action="View", id = item.PolicyCityGroupItemId }, new  { title="View"})%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
                        <%=  Html.RouteLink("Edit", "List", new { action = "Edit", id = item.PolicyCityGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (Model.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Delete", "List", new { action = "Delete", id = item.PolicyCityGroupItemId }, new { title = "Delete" })%>
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
                         <% if (Model.PolicyCityGroupItems.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyCityGroupItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.PolicyCityGroupItems.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { id = Model.PolicyGroup.PolicyGroupId, page = (Model.PolicyCityGroupItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyCityGroupItems.TotalPages > 0)
                                                     { %>Page <%=Model.PolicyCityGroupItems.PageIndex%> of <%=Model.PolicyCityGroupItems.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2">
                        <a href="javascript:window.print();" class="red" title="Print">Print</a>
                         &nbsp;
                        <%if (Model.HasWriteAccess){ %>
			                <%= Html.RouteLink("Export", "Main", new {  action="Export", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Export" })%>
			            <% } %>
                    </td>
			        <td class="row_footer_blank_right" colspan="8">
                    <%if (Model.HasWriteAccess){ %>
			                <%= Html.RouteLink("Import", "Main", new {  action="ImportStep1", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Import" })%>
			            <% } %>
                        &nbsp;
			        <%if (Model.HasWriteAccess)
             { %><%= Html.RouteLink("Edit Order", "Default", new { action = "EditSequence", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Edit Order" })%>
			        <%= Html.RouteLink("Create Policy City Group Item", "Default", new { action = "Create", id = Model.PolicyGroup.PolicyGroupId }, new { @class = "red", title = "Create Policy City Group Item" })%>
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
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=Model.PolicyGroup.PolicyGroupId%>);
    });

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/PolicyCityGroupItem.mvc/List");
		$("#frmSearch").submit();
	});
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
Policy City Group Items
</asp:Content>