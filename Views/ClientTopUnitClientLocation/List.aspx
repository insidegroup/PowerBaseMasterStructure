<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitClientLocationsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Locations
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Locations</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="20%" class="row_header_left"><%=Html.RouteLink("City", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CityName" }, new { title = "City" })%></th>
			        <th width="20%"><%=Html.RouteLink("Client Location Name", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "AddressLocationName" }, new { title = "Client Location Name" })%></th>
					<th width="15%"><%=Html.RouteLink("First Address Line", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "FirstAddressLine" }, new { title = "First Address Line" })%></th>
					<th width="15%"><%=Html.RouteLink("Postal Code", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PostalCode" }, new { title = "Postal Code" })%></th>
					<th width="15%"><%=Html.RouteLink("Country", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "CountryName" }, new { title = "Country" })%></th>
			        <th width="15%"><%=Html.RouteLink("Ranking", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "Ranking" }, new { title = "Ranking" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.ClientLocations)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.CityName) %></td>
                    <td><%= Html.Encode(item.AddressLocationName) %></td>
					<td><%= Html.Encode(item.FirstAddressLine) %></td>
					<td><%= Html.Encode(item.PostalCode) %></td>
					<td><%= Html.Encode(item.CountryName) %></td>
                    <td><%= Html.Encode(item.Ranking) %></td>
                    <td align="center"><%= Html.ActionLink("View", "View", new { id = item.ClientTopUnitGuid, addressId = item.AddressId })%> </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.ClientTopUnitGuid, addressId = item.AddressId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.ClientTopUnitGuid, addressId = item.AddressId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
							<% if (Model.ClientLocations.HasPreviousPage){ %>
								<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", page = (Model.ClientLocations.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = Model.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = "Previous Page" })%>
							<%}%>
                        </div>
                        <div class="paging_right">
							<% if (Model.ClientLocations.HasNextPage){ %>
								<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientTopUnitClientLocation", action = "List", page = (Model.ClientLocations.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString(), id = Model.ClientTopUnit.ClientTopUnitGuid.ToString() }, new { title = "Next Page" })%>
							<%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.ClientLocations.TotalPages > 0){ %>Page <%=Model.ClientLocations.PageIndex%> of <%=Model.ClientLocations.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left">
                        <a href="javascript:window.print();" class="red" title="Print">Print</a> 
                        <%= Html.ActionLink("Export", "Export", new { id = Model.ClientTopUnit.ClientTopUnitGuid }, new { @class = "red", title = "Export" })%>
                    </td>
			        <td class="row_footer_blank_right" colspan="8">
						<%if (ViewData["ImportAccess"].ToString() == "WriteAccess"){ %>
                            <%= Html.ActionLink("Import", "ImportStep1", new { id = Model.ClientTopUnit.ClientTopUnitGuid }, new { @class = "red", title = "Import" })%>
                        <% } %>
                        <%	if (ViewData["CreateAccess"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Client Location", "Create", new { id = Model.ClientTopUnit.ClientTopUnitGuid }, new { @class = "red", title = "Create Contact" })%>
						<%}%>
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
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val('<%=Html.Encode(Model.ClientTopUnit.ClientTopUnitGuid)%>');

    	$('#btnSearch').click(function () {
        	if ($('#filter').val() == "") {
        		alert("No Search Text Entered");
        		return false;
        	}
        	$("#frmSearch").attr("action", "/ClientTopUnitClientLocation.mvc/List");
        	$("#frmSearch").submit();

        });
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
Client Locations
</asp:Content>