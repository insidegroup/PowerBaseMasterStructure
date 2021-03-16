<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectGDSOrders_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Orders
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
	<style type="text/css">
		select { 
			width: 100%; 
			max-width: 153px; 
			padding: 0;
		}
		.datepicker-cell input {
			width: 120px;
		}

		.ui-datepicker-trigger {
			position: relative;
			top: 4px;
			left: 2px;
		}
		.content {
		    margin-left: 10px;
			width: 980px;
			padding-top: 10px;
		    padding-right: 10px;
			padding-bottom: 10px;
		}
		#divSearch td {
			padding: 0;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
	<div class="content">
		<%using (Html.BeginForm(null, null, FormMethod.Get, new { id = "form0" })){ %>
			<div id="divSearch">
				<table class="" cellspacing="0" width="100%">
					<tr>
						<td colspan="8"><strong>Search Multiple Criteria</strong></td>
					</tr>
					<tr> 
						<td width="10%"><label for="GDSOrderId">Order Number</label></td>
						<td width="17%"><%= Html.TextBox("GDSOrderId", ViewData["GDSOrderId"], new { maxlength = "8" } ) %></td>
						<td width="10%"><label for="PseudoCityOrOfficeId">PCC/OID</label></td>
						<td width="17%"><%= Html.TextBox("PseudoCityOrOfficeId", ViewData["PseudoCityOrOfficeId"], new { maxlength = "50" } ) %></td>
						<td width="12%"><label for="Analyst">Analyst</label></td>
						<td width="17%"><%= Html.DropDownList("Analyst", ViewData["Analysts"] as SelectList, "Please Select...") %></td>
						<td width="7%"><label for="GDSCode">GDS</label></td>
						<td width="10%"><%= Html.DropDownList("GDSCode", ViewData["GDSs"] as SelectList, "Please Select...") %></td>
					</tr>
					<tr> 
						<td><label for="GDSOrderStatusId">Order Status</label></td>
						<td><%= Html.DropDownList("GDSOrderStatusId", ViewData["OrderStatuses"] as SelectList, "Please Select...") %></td>
						<td><label for="GDSOrderTypeId">Order Type</label></td>
						<td><%= Html.DropDownList("GDSOrderTypeId", ViewData["OrderTypes"] as SelectList, "Please Select...") %></td>
						<td><label for="InternalSiteName">Internal Site Name</label></td>
						<td><%= Html.TextBox("InternalSiteName", ViewData["InternalSiteName"], new { maxlength = "50" } ) %></td>
						<td colspan="2">&nbsp;</td>
					</tr>
					<tr> 
						<td><label for="TicketNumber">Ticket Number</label></td>
						<td><%= Html.TextBox("TicketNumber", ViewData["TicketNumber"], new { maxlength = "50" } ) %></td>
						<td><label for="GDSOrderDateTimeStart">Begin Date</label></td>
						<td class="datepicker-cell"><%= Html.TextBox("GDSOrderDateTimeStart") %></td>
						<td><label for="GDSOrderDateTimeEnd">End Date</label></td>
						<td class="datepicker-cell"><%= Html.TextBox("GDSOrderDateTimeEnd") %></td>
						<td colspan="2" align="right"><span id="SearchButton"><small>Search >> </small></span></td>
					</tr>
					<tr id="lastSearch">
						<td colspan="7">
							<span class="error"></span>
						</td>
					</tr>
				</table>
			</div>
		<% } %>
	</div>
	<div id="banner"><div id="banner_text">GDS Orders</div></div>
	<div id="content">	
		<% if (TempData["GDSOrder_Success"] != null) { %>
			<%= Html.Raw(TempData["GDSOrder_Success"].ToString()) %>
		<% } %>
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0" border="0" id="mainTable">
            <tr> 
                <th width="8%" class="row_header_left"><%=  Html.RouteLink("Order", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(),  filter = Request.QueryString["filter"], sortField = "GDSOrderId", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Order" })%></th>
                <th width="7%"><%= Html.RouteLink("GDS", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSCode",  GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "GDS" } )%></th>
                <th width="8%"><%= Html.RouteLink("PCC/OID", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PseudoCityOrOfficeId", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "PCC/OID" })%></th>
                <th width="10%"><%= Html.RouteLink("Order Analyst", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "OrderAnalystName", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Order Analyst" })%></th>
				<th width="9%"><%= Html.RouteLink("Order Status", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSOrderStatusName", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Order Status" })%></th>
                <th width="9%"><%= Html.RouteLink("Order Type", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSOrderTypeName", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Order Type" })%></th>
                <th width="14%"><%= Html.RouteLink("Internal Site Name", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "InternalSiteName", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Internal Site Name" })%></th>
                <th width="11%"><%= Html.RouteLink("Ticket Number", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TicketNumber", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Ticket Number" })%></th>
                <th width="12%"><%= Html.RouteLink("Order Date", "ListMain", new { controller = "GDSOrder", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSOrderDateTime", GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { title = "Order Date" })%></th>
		        <th width="4%">&nbsp;</th> 
				<th width="4%">&nbsp;</th> 
				<th width="4%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.GDSOrderId)%></td>
                <td><%= Html.Encode(item.GDSCode)%></td>
                <td><%= Html.Encode(item.PseudoCityOrOfficeId)%></td>
                <td><%= Html.Encode(item.OrderAnalystName)%></td>
                <td><%= Html.Encode(item.GDSOrderStatusName)%></td>
                <td><%= Html.Encode(item.GDSOrderTypeName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.InternalSiteName)%></td>
                <td class="wrap-text"><%= Html.Encode(item.TicketNumber)%></td>
                <td><%= Html.Encode(item.GDSOrderDateTime.ToString("MMM dd, yyyy"))%></td>
                <td><%= Html.RouteLink("Email", "Main", new { id = item.GDSOrderId, action = "Email" }, new { title = "Email" })%> </td>
				<td><%= Html.RouteLink("View", "Main", new { id = item.GDSOrderId, action = "ViewItem" }, new { title = "View" })%> </td>        
				<td>
					<%if (item.HasWriteAccess == true){ %>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.GDSOrderId }, new { title = "Edit" })%>
					<%} %>
				</td>
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="12" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"]  }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"]  }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>   
            <tr> 
                <td class="row_footer_blank_left" colspan="6">
					<a href="javascript:history.back();" class="red" title="Back">Back</a> 
					<a href="javascript:window.print();" class="red" title="Print">Print</a> 
					<%= Html.ActionLink("Export", "Export", new { GDSOrderId = Request.QueryString["GDSOrderId"], PseudoCityOrOfficeId = Request.QueryString["PseudoCityOrOfficeId"], Analyst = Request.QueryString["Analyst"], GDSCode = Request.QueryString["GDSCode"], GDSOrderStatusId = Request.QueryString["GDSOrderStatusId"], GDSOrderTypeId = Request.QueryString["GDSOrderTypeId"], InternalSiteName = Request.QueryString["InternalSiteName"], TicketNumber = Request.QueryString["TicketNumber"], GDSOrderDateTimeStart = Request.QueryString["GDSOrderDateTimeStart"], GDSOrderDateTimeEnd = Request.QueryString["GDSOrderDateTimeEnd"] }, new { @class = "red", title = "Export" })%>
                </td>
		        <td class="row_footer_blank_right" colspan="6">
					<%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create GDS Order", "Create", new { }, new { @class = "red", title = "Create GDS Order" })%>
					<% } %>
		        </td> 
	        </tr> 
        </table>    
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSOrderList.js")%>" type="text/javascript"></script>
</asp:Content>
