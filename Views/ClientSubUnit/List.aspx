<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientTopUnitSubUnitsItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits</div></div>
        <div id="content">
            <div class="home_button">
				<%=Html.RouteLink("Home", "Main", new { controller = "ClientTopUnit", action = "List" }, new { title = "Home", @class = "red" })%>
			</div>
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="18%" class="row_header_left"><%=  Html.RouteLink("Name", "ListMain", new { controller = "ClientSubUnit", action = "List", page = 1, sortField = "ClientSubUnitName", sortOrder = ViewData["NewSortOrder"].ToString(), id= ViewData["ClientTopUnitGuid"]}, new { title = "Name" })%></th>
			        <th width="17%"><%=  Html.RouteLink("DisplayName", "ListMain", new { controller = "ClientSubUnit", action = "List", page = 1, sortField = "ClientSubUnitDisplayName", sortOrder = ViewData["NewSortOrder"].ToString(), id= ViewData["ClientTopUnitGuid"]}, new { title = "DisplayName" })%></th>
					<th width="12%">&nbsp;</th> 
			        <th width="10%">&nbsp;</th>
                    <th width="5%">&nbsp;</th>  
			        <th width="7%">&nbsp;</th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="6%">&nbsp;</th> 
					<th width="6%">&nbsp;</th>
					<th width="4%">&nbsp;</th> 
					<th width="3%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
					<td <%= Html.Raw(((item.ExpiryDate != null && item.ExpiryDate.Value <= DateTime.Now) || (item.ClientTopUnitExpiryDate != null && item.ClientTopUnitExpiryDate.Value <= DateTime.Now)) ? " class=\"expired\"" : String.Empty) %>><%= Html.Encode(item.ClientSubUnitName) %></td>
					<td><%= Html.Encode(item.ClientSubUnitDisplayName) %></td>
					<td><%= Html.RouteLink("Booking Channels", "Main", new { controller = "BookingChannel", action = "List", id = item.ClientSubUnitGuid} , new { title = "Booking Channels" })%></td>
					<td><%= Html.RouteLink("Traveller Types", "Main", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = item.ClientSubUnitGuid }, new { title = "TravelerTypes" })%> </td>
					<td><%= Html.RouteLink("Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = item.ClientSubUnitGuid }, new { title = "Client Accounts" })%></td>           
					<td><%= Html.RouteLink("Telephony", "Main", new { controller = "ClientSubUnitTelephony", action = "List", id = item.ClientSubUnitGuid }, new { title = "Telephony" })%> </td>
					<td><%= Html.RouteLink("Credit Cards", "Main", new { controller = "CreditCardClientSubUnit", action = "List", id = item.ClientSubUnitGuid }, new { title = "Credit Cards" })%></td>           
					<td><%= Html.RouteLink("CDR Link", "Main", new { controller = "ClientSubUnitCDR", action = "List", id = item.ClientSubUnitGuid }, new { title = "CDR Link" })%></td>           
					<td><%= Html.RouteLink("Contacts", "Main", new { controller = "Contact", action = "List", id = item.ClientSubUnitGuid }, new { title = "Contacts" })%></td>           
					<td><%= Html.RouteLink("CDRs", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = item.ClientSubUnitGuid }, new { title = "CDRs" })%></td>           
					<td><%= Html.RouteLink("NSI", "Main", new { controller = "PNRNameStatementInformation", action = "List", id = item.ClientSubUnitGuid }, new { title = "NSI" })%></td>           
					<td><%= Html.RouteLink("View", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = item.ClientSubUnitGuid }, new { title = "View Item" })%> </td>                     
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="12" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnit", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { controller = "ClientSubUnit", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = "Next Page" })%><%}%></div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>

        </table>    
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#search').show();
		$('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val("<%=ViewData["ClientTopUnitGuid"].ToString()%>");
	})

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/ClientSubUnit.mvc/List");
		$("#frmSearch").submit();

	});
 </script>

</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"] })%> &gt;
ClientSubUnits
</asp:Content>