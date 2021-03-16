<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPNRNameStatementInformationItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - Name Statement Information
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Name Statement Information</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="6%" class="row_header_left">GDS</th> 
		        <th width="8%">Delimiter 1</th> 
		        <th width="8%">NSI 1</th> 
		        <th width="8%">Delimiter 2</th> 
		        <th width="8%">NSI 2</th> 
		        <th width="8%">Delimiter 3</th> 
		        <th width="8%">NSI 3</th> 
		        <th width="8%">Delimiter 4</th> 
		        <th width="8%">NSI 4</th> 
		        <th width="8%">Delimiter 5</th> 
		        <th width="8%">NSI 5</th> 
		        <th width="8%">Delimiter 6</th> 
				<th width="3%">&nbsp;</th> 
				<th width="3%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.GDSName) %></td>
                <td><%= Html.Encode(item.Delimiter1)%></td>
				<td><%= Html.Encode(item.Field1_DisplayName) %></td>
                <td><%= Html.Encode(item.Delimiter2)%></td>
				<td><%= Html.Encode(item.Field2_DisplayName) %></td>
                <td><%= Html.Encode(item.Delimiter3)%></td>
				<td><%= Html.Encode(item.Field3_DisplayName) %></td>
                <td><%= Html.Encode(item.Delimiter4)%></td>
				<td><%= Html.Encode(item.Field4_DisplayName) %></td>
                <td><%= Html.Encode(item.Delimiter5)%></td>
				<td><%= Html.Encode(item.Field5_DisplayName) %></td>
                <td><%= Html.Encode(item.Delimiter6)%></td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" && item.HasWriteAccess == 1){%>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.PNRNameStatementInformationId, csu = item.ClientSubUnitGuid }, new { title = "Edit" })%>
					<% } %>
				</td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" && item.HasWriteAccess == 1){%>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.PNRNameStatementInformationId, csu = item.ClientSubUnitGuid }, new { title = "Delete" })%>
					<% } %>
				</td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="14" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage) {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left" colspan="2">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="12">
					<% if(ViewData["Access"] == "WriteAccess") { %>
						<%= Html.RouteLink("Create NSI", "Main", new { action="Create", id = ViewData["ClientSubUnitGuid"].ToString() }, new { @class = "red", title = "Create NSI" })%>
					<% } %>
		        </td> 
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');

    	//Search
        $('#search').hide();

    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
NSI
</asp:Content>