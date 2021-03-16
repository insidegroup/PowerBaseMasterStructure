<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.BookingChannelsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Booking Channels</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
                    <th width="11%" class="row_header_left"><%= Html.RouteLink("Channel", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "BookingChannelTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Booking Channel" })%></th>
			        <th width="10%"><%= Html.RouteLink("Product", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "ProductChannelTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Product" })%></th> 
			        <th width="10%"><%= Html.RouteLink("GDS", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "GDS" })%></th> 
			        <th width="10%"><%= Html.RouteLink("Desktop Used", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "DesktopUsedTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Desktop Used" })%></th> 
			        <th width="15%"><%= Html.RouteLink("Booking Pseudo City/Office ID", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "BookingPseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Booking Pseudo City/Office ID" })%></th> 
			        <th width="15%"><%= Html.RouteLink("Ticketing Pseudo City/Office ID", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "TicketingPseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Ticketing Pseudo City/Office ID" })%></th> 
					<th width="10%"><%= Html.RouteLink("Comments", "ListMain", new { controller = "BookingChannel", action = "List", page = 1, id = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "Comments", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Comments" })%></th> 
                    <th width="4%">&nbsp;</th>
					<th width="4%">&nbsp;</th>
					<th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 	
		        </tr> 
                <% foreach (var item in Model.BookingChannels) { %>
					<tr>
						<td><%= Html.Encode(item.BookingChannelTypeDescription)%></td>
						<td><%= Html.Encode(item.ProductChannelTypeDescription)%></td>
						<td><%= Html.Encode(item.GDSName) %></td>
						<td><%= Html.Encode(item.DesktopUsedTypeDescription)%></td>
						<td><%= Html.Encode(item.BookingPseudoCityOrOfficeId) %></td>
						<td><%= Html.Encode(item.TicketingPseudoCityOrOfficeId) %></td>
						<td><%= Html.Encode(item.Comments) %></td>
						<td><%= Html.RouteLink("Translations", "Main", new { action = "List", id = item.BookingChannelId, csu = Model.ClientSubUnit.ClientSubUnitGuid, controller = "AdditionalBookingComment" }, new { title = "Translations" })%></td>
						<td><%= Html.RouteLink("View", "Main", new { action = "View", id = item.BookingChannelId }, new { title = "View" })%></td>
						<td>
							<%if (ViewData["Access"] == "WriteAccess"){ %>
								<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.BookingChannelId }, new { title = "Edit" })%>
							<%} %>
						</td>
						<td>
							<%if (ViewData["Access"] == "WriteAccess"){ %>
								<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.BookingChannelId }, new { title = "Delete" })%>
							<%} %>
						</td>     
					</tr>        
				<% } %>
                <tr>
					<td colspan="11" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
							  <% if (Model.BookingChannels.HasPreviousPage)
								 { %>
							<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "BookingChannel", action = "List", page = (Model.BookingChannels.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Previous Page" })%>
							<%}%>
							</div>
							<div class="paging_right">
							 <% if (Model.BookingChannels.HasNextPage)
								{ %>
							<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "BookingChannel", action = "List", page = (Model.BookingChannels.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Next Page" })%>
							<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.BookingChannels.TotalPages > 0){ %>Page <%=Model.BookingChannels.PageIndex%> of <%=Model.BookingChannels.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
                        &nbsp;
                        <a href="javascript:window.print();" class="red" title="Print">Print</a>
                        &nbsp;
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
			                <%= Html.RouteLink("Export", "Main", new {  action="Export", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Export" })%>
			            <% } %>
                    </td>
			        <td class="row_footer_blank_right" colspan="11">
					    <%if (ViewData["Access"] == "WriteAccess"){ %>
			                <%= Html.RouteLink("Import", "Main", new {  action="ImportStep1", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Import" })%>
			            <% } %>
                        &nbsp;
                        <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Create Booking Channel", "Create", new { id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Create Booking Channel" })%> <%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

    	//Search
        $('#search').show();
        $('#frmSearch').submit(function () {
        	if ($('#filter').val() == "") {
        		alert("No Search Text Entered");
        		return false;
        	}
        	$('<input>').attr({ type: 'hidden', id: 'id', name: 'id', value: '<%=Model.ClientSubUnit.ClientSubUnitGuid%>' }).appendTo('#frmSearch');
            $("#frmSearch").attr("action", "/BookingChannel.mvc/List");
        });
    	//Search
    	$('#btnSearch').click(function () {
    		$("#frmSearch").submit();

    	});
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;

<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
Booking Channels
</asp:Content>
