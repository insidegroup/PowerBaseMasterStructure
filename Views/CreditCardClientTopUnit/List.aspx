<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitCreditCardsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Credit Cards</div></div>
    <div id="content">
		<div class="home_button">
			<%=Html.RouteLink("Home", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Home", @class = "red" })%>
		</div>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
		        <th width="18%" class="row_header_left"><%= Html.RouteLink("Card Type", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "CreditCardTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Type" })%></th>
				<th width="10%"><%= Html.RouteLink("Product", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Product" })%></th>
				<th width="10%"><%= Html.RouteLink("Supplier", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Supplier" })%></th>
				<th width="10%"><%= Html.RouteLink("Card Holder", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "CreditCardHolderName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Holder" })%></th> 
		        <th width="10%"><%= Html.RouteLink("Card Number", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "MaskedCreditCardNumber", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Card Number" })%></th> 
		        <th width="15%">CVV</th>
				<th width="15%"><%= Html.RouteLink("Hierarchy", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "HierarchyItem", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Hierarchy" })%></th> 
		        <th width="12%"><%= Html.RouteLink("Valid To", "ListMain", new { controller = "CreditCardClientTopUnit", action = "List", page = 1, filter = Request.QueryString["filter"], id = Model.ClientTopUnit.ClientTopUnitGuid, sortField = "CreditCardValidTo", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Valid To" })%></th> 
                <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
                    <th width="12%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
		            <th width="4%">&nbsp;</th> 
		            <th width="7%" class="row_header_right">&nbsp;</th> 
                <%}else{ %>
                    <th width="20%">&nbsp;</th> 
		            <th width="5%" class="row_header_right">&nbsp;</th> 
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
				<td><%= Html.Encode(item.HierarchyItem) %></td>
                <td><%= Html.Encode(item.CreditCardValidTo.ToString("MMM dd, yyyy"))%></td>
                <td></td>
                <td><%= Html.RouteLink("View", "Main", new { controller = "CreditCardClientTopUnit", action = "View", id = item.CreditCardId, ctu = item.ClientTopUnitGuid }, new { title = "View" })%> </td>
                <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
                <td>
                    
                        <%if (ViewData["Access"] == "WriteAccess"){%>
                            <%= Html.RouteLink("Edit", "Main", new { controller = "CreditCardClientTopUnit", action = "Edit", id = item.CreditCardId, ctu = item.ClientTopUnitGuid }, new { title = "Edit" })%>
                        <%} %>
                </td>
                 <td>
                        <%if (ViewData["Access"] == "WriteAccess" && (item.HierarchyType == "ClientTopUnit" || item.HierarchyType == "Country" || item.HierarchyType == "CountryRegion" || item.HierarchyType == "Location" || item.HierarchyType == "Team"))
						  {%>
                            <%= Html.RouteLink("Delete", "Main", new { controller = "CreditCardClientTopUnit", action = "Delete", id = item.CreditCardId, ctu = item.ClientTopUnitGuid }, new { title = "Delete" })%>
                        <%} %>
                    
                </td>
                <%} %>
            </tr>        
            <% 
            } 
            %>
            <tr>
                    <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td colspan="12" class="row_footer"><%}else{ %><td colspan="10" class="row_footer"><%} %>
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.CreditCards.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "Main", new { id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.CreditCards.PageIndex - 1), sortField = "Name", sortOrder = "1" }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.CreditCards.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "Main", new { id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.CreditCards.PageIndex + 1), sortField = "Name", sortOrder = "1" }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.CreditCards.TotalPages > 0){ %>Page <%=Model.CreditCards.PageIndex%> of <%=Model.CreditCards.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td class="row_footer_blank_right" colspan="11"><%}else{ %><td class="row_footer_blank_right" colspan="9"><%} %>
		        
		        <%if (ViewData["Access"] == "WriteAccess"){ %>
		        <%= Html.RouteLink("Add Credit Card", "Main", new { controller = "CreditCardClientTopUnit", action = "Create", ctu = Model.ClientTopUnit.ClientTopUnitGuid }, new { @class = "red", title = "Add Credit Card" })%>
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
		$('#search').show();
		$('.full-width #search_wrapper').css('height', '23px');
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val('<%=Html.Encode(Model.ClientTopUnit.ClientTopUnitGuid)%>');
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
Credit Cards
</asp:Content>