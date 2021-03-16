<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientTopUnits_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnits</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="20%" class="row_header_left"><%=  Html.RouteLink("Name", "ListMain", new { controller = "ClientTopUnit", action = "List", page = 1, sortField = "Name", sortOrder = ViewData["NewSortOrder"] }, new { title = "Name" })%></th>
                    <th width="8%"><%=  Html.RouteLink("Guid", "ListMain", new { controller = "ClientTopUnit", action = "List", page = 1, sortField = "ClientTopUnitGuid", sortOrder = ViewData["NewSortOrder"] }, new { title = "Guid" })%></th>
			        <th width="12%">&nbsp;</th> 
					<th width="14%">&nbsp;</th> 
			        <th width="11%">&nbsp;</th> 
                    <th width="13%">&nbsp;</th> 
                    <th width="9%">&nbsp;</th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="5%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td <%= Html.Raw(item.ExpiryDate != null && item.ExpiryDate.Value <= DateTime.Now ? " class=\"expired\"" : String.Empty) %>><%= Html.Encode(item.ClientTopUnitName) %></td>
                    <td><%= Html.Encode(item.ClientTopUnitGuid) %></td>
                    <td><%= Html.RouteLink("Client Details", "Main", new { controller = "ClientDetailClientTopUnit", action = "ListUnDeleted", id = item.ClientTopUnitGuid }, new { title = "View Details" })%> </td>
                    <td><%= Html.RouteLink("Client Locations", "Main", new { controller = "ClientTopUnitClientLocation", action = "List", id = item.ClientTopUnitGuid }, new { title = "Client Locations" })%> </td>
                    <td><%= Html.RouteLink("Credit Cards", "Main", new { controller = "CreditCardClientTopUnit", action = "List", id = item.ClientTopUnitGuid }, new { title = "Credit Cards" })%></td>           
                    <td><%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = item.ClientTopUnitGuid }, new { title = "ClientSubUnits" })%></td>           
                    <td><%= Html.RouteLink("Telephony", "Main", new { controller = "ClientTopUnitTelephony", action = "List", id = item.ClientTopUnitGuid }, new { title = "Telephony" })%> </td>
                    <td><%= Html.RouteLink("DP Code", "Main", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = item.ClientTopUnitGuid }, new { title = "DP Code" })%> </td>
                    <td><%=Html.RouteLink("View", "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = item.ClientTopUnitGuid }, new { title = "View" })%></td>           
                   </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="9" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                         <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientTopUnit", action = "List", page = (Model.PageIndex - 1), sortField = "Name", sortOrder = "0", filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientTopUnit", action = "List", page = (Model.PageIndex + 1), sortField = "Name", sortOrder = "0", filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
        </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#search').show();
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })



    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ClientTopUnit.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>