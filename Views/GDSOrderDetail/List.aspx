<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderDetailsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Detail
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Detail</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="39%" class="row_header_left">GDS Order Detail</th>
					<th width="5%">1A</th>
					<th width="5%">1B</th>
					<th width="5%">1C</th>
					<th width="5%">1D</th>
					<th width="5%">1E</th>
					<th width="5%">1G</th>
					<th width="5%">1P</th>
					<th width="5%">1S</th>
					<th width="5%">1V</th>
					<th width="5%">ALL</th>
					<th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.GDSOrderDetails) { %>
					<tr>
						<td><%= Html.Encode(item.GDSOrderDetailName) %></td>
						<td><%= Html.Encode(item.AmadeusFlag.HasValue && item.AmadeusFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.AbacusFlag.HasValue && item.AbacusFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.EDSFlag.HasValue && item.EDSFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.RadixxFlag.HasValue && item.RadixxFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.TravelskyFlag.HasValue && item.TravelskyFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.GalileoFlag.HasValue && item.GalileoFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.WorldspanFlag.HasValue && item.WorldspanFlag.Value == true ? "Yes" : "No") %></td>					
						<td><%= Html.Encode(item.SabreFlag.HasValue && item.SabreFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.ApolloFlag.HasValue && item.ApolloFlag.Value == true ? "Yes" : "No") %></td>
						<td><%= Html.Encode(item.AllGDSSystemsFlag.HasValue && item.AllGDSSystemsFlag.Value == true ? "Yes" : "No") %></td>
						<td align="center">
							<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.GDSOrderDetailId })%>
							<%} %>
						</td>
						<td align="center">
							<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { id = item.GDSOrderDetailId })%>
							<%} %>
						</td>
					</tr>        
                <% } %>
                <tr>
					<td colspan="13" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.GDSOrderDetails.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "GDSOrderDetail", action = "List", page = (Model.GDSOrderDetails.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.GDSOrderDetails.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "GDSOrderDetail", action = "List", page = (Model.GDSOrderDetails.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.GDSOrderDetails.TotalPages > 0){ %>Page <%=Model.GDSOrderDetails.PageIndex%> of <%=Model.GDSOrderDetails.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="12">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create GDS Order Detail", "Create", new {}, new { @class = "red", title = "Create GDS Order Detail" })%>
						<%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_admin, #menu_admin_gdsmanagement').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

    	//Search
        $('#search').hide();
    })
 </script>
</asp:Content>
