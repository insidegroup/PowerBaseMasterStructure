<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyCountryGroupItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Country Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="10%" class="row_header_left"><%=  Html.RouteLink("Order", "List", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "SequenceNumber", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sequence" })%></th>
                	<th width="20%"><%=  Html.RouteLink("Country", "List", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Country" })%></th>
                    <th width="20%"><%=  Html.RouteLink("Status", "List", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicyCountryStatusDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Status" })%></th>
                	<th width="20%"><%=  Html.RouteLink("Enabled Date", "List", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Enabled Date" })%></th>
                	<th width="10%"><%=  Html.RouteLink("Advice", "List", new { controller = "PolicyCountryGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Advice" })%></th>
			        <th width="13%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                 <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber)%></td>
                    <td><%= Html.Encode(item.CountryName)%></td>
                    <td><%= Html.Encode(item.PolicyCountryStatusDescription)%></td>
                    <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>  
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>                  
                    <td><%= Html.RouteLink("Country Advice", "Default", new { controller = "CountryAdvice", action = "List", id = item.PolicyCountryGroupItemId }, new { title = "Country Advice" })%> </td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.PolicyCountryGroupItemId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.PolicyCountryGroupItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.PolicyCountryGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>                  
                </tr>
                <% 
                } 
                %>
                <tr>
                     <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { id = ViewData["PolicyGroupID"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { id = ViewData["PolicyGroupID"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0)
                                                     { %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
                </tr>
		        <tr> 
                  <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>&nbsp;
						<%= Html.RouteLink("Export", "Default", new { action = "Export", id = ViewData["PolicyGroupId"] }, new { @class = "red", title = "Export" })%>
					</td>
			        <td class="row_footer_blank_right" colspan="6">
			            <%if (ViewData["Access"] == "WriteAccess"){ %>
                            <%= Html.RouteLink("Import", "Main", new {  action="ImportStep1", id = ViewData["PolicyGroupId"] }, new { @class = "red", title = "Import" })%>
			                <%= Html.RouteLink("Edit Order", "Default", new { action = "EditSequence", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Edit Order" })%>
			                <%= Html.ActionLink("Create Policy Country Group Item", "Create", new { id = ViewData["PolicyGroupID"] }, new { @class = "red" , title="Create Policy Country Group Item"})%> 
		                <% } %>
                    </td>
		         </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val(<%=ViewData["PolicyGroupId"]%>);
    });

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/PolicyCountryGroupItem.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Country Group Items
</asp:Content>
