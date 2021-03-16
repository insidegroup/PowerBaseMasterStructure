<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeMaintenancesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Maintenance (Deleted)</div></div>
        <div id="content">
            <div class="home_button">
				<a href="/" class="red">Home</a>
			</div>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="10%" class="row_header_left"><%= Html.RouteLink("PCC/OID", "ListMain", new { page = 1, sortField = "PseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("IATA", "ListMain", new { page = 1, sortField = "IATANumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("Address", "ListMain", new { page = 1, sortField = "FirstAddressLine", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="5%"><%= Html.RouteLink("Country", "ListMain", new { page = 1, sortField = "CountryCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("PCC/OID Def Region", "ListMain", new { page = 1, sortField = "PseudoCityOrOfficeDefinedRegionName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("Active", "ListMain", new { page = 1, sortField = "ActiveFlag", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("PCC/OID Type", "ListMain", new { page = 1, sortField = "PseudoCityOrOfficeTypeName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("Internal Site Name", "ListMain", new { page = 1, sortField = "InternalSiteName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
                    <th width="4%">&nbsp;</th>
					<th width="4%">&nbsp;</th>
			        <th width="7%" class="row_header_right">&nbsp;</th> 
                </tr> 
                <%
                foreach (var item in Model.PseudoCityOrOfficeMaintenances)
                { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
                    <td><%= Html.Encode(item.IATANumber) %></td>
                    <td><%= Html.Encode(item.GDSName) %></td>
                    <td><%= Html.Encode(item.FirstAddressLine) %></td>
					<td><%= Html.Encode(item.CountryCode) %></td>
					<td><%= Html.Encode(item.PseudoCityOrOfficeDefinedRegionName) %></td>
					<td><%= Html.Encode(item.ActiveFlag) %></td>
					<td><%= Html.Encode(item.PseudoCityOrOfficeTypeName) %></td>
					<td><%= Html.Encode(item.InternalSiteName) %></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.PseudoCityOrOfficeMaintenanceId})%></td>
					<td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.PseudoCityOrOfficeMaintenanceId})%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.PseudoCityOrOfficeMaintenanceId})%>
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
								<% if (Model.PseudoCityOrOfficeMaintenances.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PseudoCityOrOfficeMaintenance", action = "ListDeleted", page = (Model.PseudoCityOrOfficeMaintenances.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.PseudoCityOrOfficeMaintenances.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PseudoCityOrOfficeMaintenance", action = "ListDeleted", page = (Model.PseudoCityOrOfficeMaintenances.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.PseudoCityOrOfficeMaintenances.TotalPages > 0){ %>Page <%=Model.PseudoCityOrOfficeMaintenances.PageIndex%> of <%=Model.PseudoCityOrOfficeMaintenances.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="10">
						<%= Html.ActionLink("UnDeleted PCC/OIDs", "ListUnDeleted", new { }, new { @class = "red", title = "UnDeleted PCC/OIDs" })%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_gdsmanagement').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

    	//Search
        $('#search').show();
        $('#btnSearch').click(function () {
        	if ($('#filter').val() == "") {
        		alert("No Search Text Entered");
        		return false;
        	}
        	$("#frmSearch").attr("action", "/PseudoCityOrOfficeMaintenance.mvc/ListDeleted");
        	$("#frmSearch").submit();
        });
    })
 </script>
</asp:Content>
