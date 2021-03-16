<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeAddressesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">ClientSubUnit TravelerType - Client Detail Addresses</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="50%" class="row_header_left">Address</th> 
			        <th width="35%">City</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.Addresses)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.FirstAddressLine)%></td>
                    <td><%= Html.Encode(item.CityName) %></td>
                    <td align="center"><%= Html.ActionLink("View", "View", new { id = item.AddressId })%> </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.AddressId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.AddressId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.Addresses.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitAddress", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.Addresses.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.Addresses.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitAddress", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.Addresses.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.Addresses.TotalPages > 0){ %>Page <%=Model.Addresses.PageIndex%> of <%=Model.Addresses.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7"><%= Html.ActionLink("Create Address", "Create", new { id = Model.ClientDetail.ClientDetailId }, new { @class = "red", title = "Create Address" })%></td> 
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
