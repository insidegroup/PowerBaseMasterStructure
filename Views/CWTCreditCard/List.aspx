<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.CWTCreditCardsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Credit Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">CWT Credit Cards</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
               <tr> 
		        <th width="19%" class="row_header_left">Client TopUnit</th> 
		        <th width="15%">Card Holder</th> 
		        <th width="15%">Card Number</th> 
		        <th width="12%">Valid To</th> 
		         <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
                    <th width="10%">Hierarchy</th>
					<th width="18%">Hierarchy Item</th> 
                    <th width="4%">&nbsp;</th> 
		            <th width="7%" class="row_header_right">&nbsp;</th> 
                <%}else{ %>
                    <th width="10%">Hierarchy</th>
				    <th width="10%">Hierarchy Item</th>
		            <th width="5%" class="row_header_right">&nbsp;</th> 
                <%} %>
	        </tr> 
                <%
                    foreach (var item in Model.CreditCards)
                    { 
                %>
                <tr>
					<td><%= Html.Encode(item.ClientTopUnitName) %></td>
                    <td><%= Html.Encode(item.CreditCardHolderName) %></td>
                    <td><%= Html.Encode(item.MaskedCreditCardNumber) %></td>
                    <td><%= Html.Encode(item.CreditCardValidTo.ToString("MMM yyyy"))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
					<td><%= Html.Encode(item.HierarchyItem) %></td>
                    <td><%= Html.ActionLink("View", "View", new { id = item.CreditCardId })%> </td>
                     <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
                     <td>
                        <%if (ViewData["Access"] == "WriteAccess")   {%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.CreditCardId })%>
                        <%} %>
                    </td>
                    <%} %>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %><td colspan="8" class="row_footer"><%}else{ %><td colspan="7" class="row_footer"><%} %>
                        <div class="paging_container">
                            <div class="paging_left">
								<% if (Model.CreditCards.HasPreviousPage) { %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.CreditCards.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = (ViewData["CurrentSortOrder"] != null) ? ViewData["CurrentSortOrder"].ToString() : "", filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
								<%}%>
                            </div>
                            <div class="paging_right">
								<% if (Model.CreditCards.HasNextPage) {  %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.CreditCards.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = (ViewData["CurrentSortOrder"] != null) ? ViewData["CurrentSortOrder"].ToString() : "", filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
								<%}%>
                            </div>
                            <div class="paging_centre">
								<%if (Model.CreditCards.TotalPages > 0) { %>
									Page <%=Model.CreditCards.PageIndex%> of <%=Model.CreditCards.TotalPages%>
								<%} %>
                            </div>
                        </div>
                    </td>
                </tr>
		        <tr>
					<td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
					<%if(Model.CreditCardBehavior.CanChangeCreditCardsFlag){ %>
						<td class="row_footer_blank_right" colspan="7"></td>
					<%}else{ %>
						<td class="row_footer_blank_right" colspan="6"></td>
					<%} %>
		        </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_creditcards').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //$('#search').show();
    })
 </script>
 </asp:Content>