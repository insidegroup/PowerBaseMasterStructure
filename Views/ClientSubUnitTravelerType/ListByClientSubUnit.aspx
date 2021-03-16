<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client SubUnit Traveler Types</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th class="row_header_left">TravelerType Name</th>
			        <th class="row_header_right"></th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.TravelerTypes) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TravelerTypeName) %></td>
                    <td align="right"><%= Html.RouteLink("CSU TT Credit Cards", "Main", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = item.TravelerTypeGuid }, new { title = "Client SubUnit TravelerType Credit Cards" })%>&nbsp;       
                    <%= Html.RouteLink("TT Credit Cards", "Main", new { controller = "CreditCardTravelerType", action = "List", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = item.TravelerTypeGuid }, new { title = "TravelerType Credit Cards" })%>&nbsp;       
                    <%= Html.RouteLink("CSU TT Client Details", "Main", new { controller = "ClientDetailClientSubUnitTravelerType", action = "ListUnDeleted", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = item.TravelerTypeGuid }, new { title = "Client Details" })%>&nbsp;
                    <%= Html.RouteLink("TT Client Details", "Main", new { controller = "ClientDetailTravelerType", action = "ListUnDeleted", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = item.TravelerTypeGuid }, new { title = "Client Details" })%>&nbsp;
					<%= Html.RouteLink("TT Sponsor", "Main", new { controller = "TravelerTypeSponsor", action = "Edit", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = item.TravelerTypeGuid }, new { title = "TravelerType Sponsor" })%>&nbsp;         
                    <%= Html.RouteLink("View TT", "Main", new { controller = "TravelerType", action = "ViewItem", id = item.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "View Details" })%>&nbsp;</td>           
                </tr>
                <% 
                } 
                %>
                 <tr>
                <td colspan="2" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.TravelerTypes.HasPreviousPage)
                            { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.TravelerTypes.PageIndex - 1)}, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.TravelerTypes.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.TravelerTypes.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TravelerTypes.TotalPages > 0)
                                                     { %>Page <%=Model.TravelerTypes.PageIndex%> of <%=Model.TravelerTypes.TotalPages%><%} %></div>
                    </div>
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
    })
 </script>
</asp:Content>