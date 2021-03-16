<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderTypesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Type</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="29%" class="row_header_left">GDS Order Type</th>
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
					<th width="10%">3rd Party?</th>
					<th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.GDSOrderTypes) { %>
					<tr>
						<td><%= Html.Encode(item.GDSOrderTypeName) %></td>
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
						<td><%= Html.Encode(item.IsThirdPartyFlag.HasValue && item.IsThirdPartyFlag.Value == true ? "Yes" : "No") %></td>
						<td align="center">
							<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.GDSOrderTypeId })%>
							<%} %>
						</td>
						<td align="center">
							<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { id = item.GDSOrderTypeId })%>
							<%} %>
						</td>
					</tr>        
                <% } %>
				<tr>
					<td colspan="14" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.GDSOrderTypes.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "GDSOrderType", action = "List", page = (Model.GDSOrderTypes.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.GDSOrderTypes.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "GDSOrderType", action = "List", page = (Model.GDSOrderTypes.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.GDSOrderTypes.TotalPages > 0){ %>Page <%=Model.GDSOrderTypes.PageIndex%> of <%=Model.GDSOrderTypes.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="13">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create GDS Order Type", "Create", new {}, new { @class = "red", title = "Create GDS Order Type" })%>
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
