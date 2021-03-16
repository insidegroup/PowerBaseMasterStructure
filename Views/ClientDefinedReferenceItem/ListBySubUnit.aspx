<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedReferenceItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">CDRs</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="5%" class="row_header_left"><%= Html.RouteLink("Order", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "SequenceNumber", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Order" })%></th> 
		        <th width="11%"><%= Html.RouteLink("Display Name Alias", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "DisplayNameAlias", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Display Name Alias" })%></th> 
		        <th width="11%"><%= Html.RouteLink("Back Office Display Name", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "DisplayName", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Back Office Display Name" })%></th> 
		        <th width="8%"><%= Html.RouteLink("Entry Format", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "EntryFormat", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Entry Format" })%></th> 
		        <th width="6%"><%= Html.RouteLink("Mandatory", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MandatoryFlag", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Mandatory" })%></th> 
		        <th width="4%"><%= Html.RouteLink("Min", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MinLength", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Min" })%></th>
				<th width="4%"><%= Html.RouteLink("Max", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "MaxLength", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Max" })%></th> 
		        <th width="6%"><%= Html.RouteLink("Droplist", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TableDrivenFlag", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Droplist" })%></th> 
		        <th width="8%"><%= Html.RouteLink("Product", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "Product", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Product" })%></th> 
		        <th width="8%"><%= Html.RouteLink("Context", "ListMain", new { action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "Context", can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Context" })%></th> 
				<th width="8%">&nbsp;</th> 
				<th width="3%">&nbsp;</th>
				<th width="3%">&nbsp;</th> 
				<th width="3%">&nbsp;</th> 
		        <th width="12%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr class="bods_<%=item.BackOfficeDataSourceId %>">
                <td><%= Html.Encode(item.SequenceNumber) %></td>
                <td class="text-wrap"><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.DisplayNameAlias), 20)%></td>
				<td class="text-wrap"><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.DisplayName), 20) %></td>
				<td class="text-wrap"><%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Html.Encode(item.EntryFormat), 20) %></td>
				<td><%= Html.Encode(item.MandatoryFlag) %></td>
				<td><%= Html.Encode(item.MinLength) %></td>
				<td><%= Html.Encode(item.MaxLength) %></td>
				<td><%= Html.Encode(item.TableDrivenFlag) %></td>
 				<td><%= Html.Encode(item.Product) %></td>
				<td><%= Html.Encode(item.Context) %></td>
				<td><%= Html.RouteLink("PNR Format ", "Main", new { controller = "ClientDefinedReferenceItemPNROutput", action = "List", id = item.ClientDefinedReferenceItemId, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "PNR Format" })%></td>
                <td><%= Html.RouteLink("View ", "Main", new { action = "View", id = item.ClientDefinedReferenceItemId, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "View" })%></td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" && item.EditAccess == "True"){%>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientDefinedReferenceItemId, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Edit" })%>
					<% } %>
				</td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" && item.DeleteAccess == "True"){%>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientDefinedReferenceItemId, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Delete" })%>
					<% } %>
				</td>
				<td>
					<% if(item.TableDrivenFlag == true) { %>
						<%= Html.RouteLink("Drop List Values", "Main", new { action = "List", controller = "ClientDefinedReferenceItemValue", id = item.ClientDefinedReferenceItemId, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"].ToString(), ssc = ViewData["SourceSystemCode"].ToString() }, new { title = "Drop List Values" })%>
					<% } %>
				</td>
           </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="15" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage) {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { title = "Next Page" })%>
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
		        <td class="row_footer_blank_right" colspan="13">
					<% if(ViewData["CreateAccess"] == "CreateAccess" && ViewData["Access"] == "WriteAccess") { %>
						<%= Html.RouteLink("Edit Order", "Main", new { controller = "ClientDefinedReferenceItem", action="EditSequence", id = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { @class = "red", title = "Edit Order" })%>
						<%= Html.RouteLink("Create CDR", "Main", new { controller = "ClientDefinedReferenceItem", action="Create", id = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"]}, new { @class = "red", title = "Create CDR" })%>
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
        $('#breadcrumb').css('width', '780px')


    	//Search
        $('#search').show();
        $('#ft').attr('name', 'id')
			.after('<input type="hidden" name="can" value="<%=Html.Encode(ViewData["ClientAccountNumber"].ToString())%>"/>')
    		.after('<input type="hidden" name="ssc" value="<%=Html.Encode(ViewData["SourceSystemCode"].ToString())%>"/>');
        $("#frmSearch input[name='id']").val('<%=Html.Encode(ViewData["ClientSubUnitGuid"].ToString())%>');
    	$('#btnSearch').click(function () {
    		$("#frmSearch").attr("action", "/ClientDefinedReferenceItem.mvc/ListBySubUnit");
    		$("#frmSearch").submit();
    	});

    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title ="Client Accounts" })%> &gt;
<%=Html.RouteLink(ViewData["ClientAccountName"].ToString(), "Main", new { controller = "ClientAccount", action = "ViewItem", ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
CDRs
</asp:Content>