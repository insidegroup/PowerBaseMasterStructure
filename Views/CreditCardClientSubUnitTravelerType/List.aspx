<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeCreditCardsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit TravelerType - Credit Cards</div></div>
    <div id="content">
        <div class="home_button">
			<%=Html.RouteLink("Home", "Main", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Home", @class = "red" })%>
		</div>
		<table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
		        <th width="16%" class="row_header_left"><%= Html.RouteLink("Card Type", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "CreditCardTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Type" })%></th>
				<th width="10%"><%= Html.RouteLink("Product", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Product" })%></th>
				<th width="17%"><%= Html.RouteLink("Supplier", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Supplier" })%></th>
				<th width="10%"><%= Html.RouteLink("Card Holder", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "CreditCardHolderName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Holder" })%></th> 
		        <th width="10%"><%= Html.RouteLink("Card Number", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "MaskedCreditCardNumber", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Number" })%></th> 
		        <th width="5%">CVV</th>
		        <th width="15%">Top Unit</th> 
		        <th width="10%"><%= Html.RouteLink("Valid To", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", page = 1, filter = Request.QueryString["filter"], tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "CreditCardValidTo", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Valid To" })%></th> 
		         <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
                    <th width="4%">&nbsp;</th> 
		            <th width="4%">&nbsp;</th> 
		            <th width="7%" class="row_header_right">&nbsp;</th> 
                <%}else{ %>
		            <th width="15%" class="row_header_right">&nbsp;</th> 
                <%} %>
	        </tr> 
            <%
            foreach (var item in Model.CreditCards) { 
            %>
            <tr>
                <td><%= Html.Encode(item.CreditCardTypeDescription) %></td>
				<td><%= Html.Encode(item.ProductName) %></td>
				<td><%= Html.Encode(item.SupplierName) %></td>
				<td><%= Html.Encode(item.CreditCardHolderName) %></td>
                <td><%= Html.Encode(item.MaskedCreditCardNumber) %></td>
                <td><%= Html.Encode(item.MaskedCVVNumber) %></td>
                <td><%= Html.Encode(item.ClientTopUnitName)%></td>
                <td><%= Html.Encode(item.CreditCardValidTo.ToString("MMM dd, yyyy"))%></td>
                 <td><%= Html.RouteLink("View", "Main", new { controller = "CreditCardClientSubUnitTravelerType", action = "View", id = item.CreditCardId }, new { title = "View" })%> </td>
                <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td>
                    <%if (ViewData["Access"] == "WriteAccess")
                      {%>
                        <%= Html.RouteLink("Edit", "Main", new { controller = "CreditCardClientSubUnitTravelerType", action = "Edit", id = item.CreditCardId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                 <td>
                    <%if (ViewData["Access"] == "WriteAccess")
                      {%>
                        <%= Html.RouteLink("Delete", "Main", new { controller = "CreditCardClientSubUnitTravelerType", action = "Delete", id = item.CreditCardId }, new { title = "Remove" })%>
                    <%} %>
                </td>
                <%} %>
 
            </tr>        
            <% 
            } 
            %>
            <tr>
                    <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td colspan="11" class="row_footer"><%}else{ %><td colspan="9" class="row_footer"><%} %>
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.CreditCards.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "List", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid, page = (Model.CreditCards.PageIndex - 1), sortField = "Name", sortOrder = "1" }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.CreditCards.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "CreditCardClientSubUnitTravelerType", action = "ListBy", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid, page = (Model.CreditCards.PageIndex - 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.CreditCards.TotalPages > 0){ %>Page <%=Model.CreditCards.PageIndex%> of <%=Model.CreditCards.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		         <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td class="row_footer_blank_right" colspan="10"><%}else{ %><td class="row_footer_blank_right" colspan="5"><%} %>
		        <%if (ViewData["Access"] == "WriteAccess"){ %>
		        <%= Html.RouteLink("Add Credit Card", "Main", new { controller = "CreditCardClientSubUnitTravelerType", action = "Create", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, new { @class = "red", title = "Add Credit Card" })%>
		        <% } %> 
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
        $('.full-width #search_wrapper').css('height', '23px');
        $('.full-width #search_wrapper #breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client TopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Traveller Types", "Main", new { controller = "ClientSubUnitTravelerType", action = "ListByClientSubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink(Model.TravelerType.TravelerTypeName, "Main", new { controller = "TravelerType", action = "ViewItem", id = Model.TravelerType.TravelerTypeGuid }, new { title = Model.TravelerType.TravelerTypeName })%> &gt;
CSU TT Credit Cards</asp:Content>