<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TransactionFeesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Transaction Fees</div></div>
    <div id="content">
         <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			        <th width="23%" class="row_header_left"><%= Html.RouteLink("Fee Description", "ListMain", new { page = 1, sortField = "TransactionFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="17%"><%= Html.RouteLink("Indicator", "ListMain", new { page = 1, sortField = "TravelIndicator", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="12%"><%= Html.RouteLink("Source", "ListMain", new { page = 1, sortField = "BookingSourceCode", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="9%"><%= Html.RouteLink("Origination", "ListMain", new { page = 1, sortField = "BookingOriginationCode", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="9%"><%= Html.RouteLink("Amt/Perc", "ListMain", new { page = 1, sortField = "FeePercent", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="9%"><%= Html.RouteLink("Currency", "ListMain", new { page = 1, sortField = "FeeCurrencyCode", sortOrder = ViewData["NewSortOrder"].ToString() })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model.TransactionFees) { 
            %>
            <tr>
                <td><%= Html.Encode(item.TransactionFeeDescription) %></td>
                <td><%= Html.Encode(item.TravelIndicator) %></td>
                <td><%= Html.Encode(item.BookingSourceCode) %></td>
                <td><%= Html.Encode(item.BookingOriginationCode) %></td>
                <%if (!string.IsNullOrEmpty(item.FeePercent.ToString())){%>
                    <td><%=item.FeePercent%>%</td>
                    <td></td>
                <%} else {%>
                    <td><%= Html.Encode(item.FeeAmount)%></td>
                    <td><%= Html.Encode(item.FeeCurrencyCode)%></td>
               <%} %>
                
                <%
                var controllerName = "TransactionFeeCarHotel"; 
                if (item.ProductId == 1)
                  {
                      controllerName = "TransactionFeeAir";
                  }
                  %>
                <td><%= Html.RouteLink("View", "Default", new { controller = controllerName, action = "View", id = item.TransactionFeeId }, new { title = "View" })%> </td>
                <td>
                    <%if (Model.HasWriteAccess)
                      {%>
                    <%= Html.RouteLink("Edit", "Default", new { controller = controllerName, action = "Edit", id = item.TransactionFeeId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (Model.HasWriteAccess)
                      {%>
                    <%= Html.RouteLink("Delete", "Default", new { controller = controllerName, action = "Delete", id = item.TransactionFeeId }, new { title = "Delete" })%>
                    <%} %>
                </td>        
                     
                </tr>
            <% 
            } 
            %>
             <tr>
                    <td colspan="9" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.TransactionFees.HasPreviousPage)
                                   { %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.TransactionFees.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.TransactionFees.HasNextPage)
                                   { %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { page = (Model.TransactionFees.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TransactionFees.TotalPages > 0)
                                                         { %>Page <%=Model.TransactionFees.PageIndex%> of <%=Model.TransactionFees.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
             <tr> 
                 <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="7">
		        <%if (Model.HasWriteAccess)
            {%>
		        <%= Html.RouteLink("Create Transaction Fee", "Main", new { action = "SelectProduct" }, new { @class = "red", title = "Create Transaction Fee" })%>
		        <% } %>
		        </td> 
	        </tr> 
        </table>    
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/TransactionFee.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>
