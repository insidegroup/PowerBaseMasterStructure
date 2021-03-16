<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeSupplierProductsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit TravelerType - Client Detail Product Suppliers</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="30%" class="row_header_left">Product</th> 
			        <th width="63%">Supplier</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.SupplierProducts)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = Model.ClientDetail.ClientDetailId, sc = item.SupplierCode, pid = item.ProductId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.SupplierProducts.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitTravelerTypeSupplierProduct", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.SupplierProducts.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.SupplierProducts.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitTravelerTypeSupplierProduct", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.SupplierProducts.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.SupplierProducts.TotalPages > 0){ %>Page <%=Model.SupplierProducts.PageIndex%> of <%=Model.SupplierProducts.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Create Product Supplier", "Create", new { id = Model.ClientDetail.ClientDetailId }, new { @class = "red", title = "Create Product Supplier" })%></td> 
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
