<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupTransactionFeesVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Transaction Fee Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="24%" class="row_header_left"><%= Html.RouteLink("Description", "ListMain", new { page = 1, sortField = "TransactionFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="8%"><%= Html.RouteLink("Prod", "ListMain", new { page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="5%"><%= Html.RouteLink("Ind", "ListMain", new { page = 1, sortField = "TravelIndicator", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="8%"><%= Html.RouteLink("Origin", "ListMain", new { page = 1, sortField = "BookingOriginationCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="11%"><%= Html.RouteLink("Charge Type", "ListMain", new { page = 1, sortField = "ChargeTypeCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="11%"><%= Html.RouteLink("Trans Type", "ListMain", new { page = 1, sortField = "TransactionTypeCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="8%"><%= Html.RouteLink("Category", "ListMain", new { page = 1, sortField = "FeeCategory", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="7%"><%= Html.RouteLink("Amount/%", "ListMain", new { page = 1, sortField = "FeeAmount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ClientFeeGroupTransactionFees) { 
                %>
                <tr>
                    <td><%= Html.Encode(CWTStringHelpers.TrimString(item.TransactionFeeDescription,27)) %></td>                   
                    <td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.TravelIndicator)%></td>
                    <td><%= Html.Encode(item.BookingOriginationCode)%></td>
                    <td><%= Html.Encode(item.ChargeTypeCode)%></td>
                    <td><%= Html.Encode(item.TransactionTypeCode)%></td>
                    <td><%= Html.Encode(item.FeeCategory)%></td>
                     <%if (!string.IsNullOrEmpty(item.FeePercent.ToString())){%>
                        <td><%=item.FeePercent%> %</td>
                    <%} else {%>
                        <td><%= Html.Encode(item.FeeAmount)%> <%= Html.Encode(item.FeeCurrencyCode)%></td>
                   <%} %>

                     <%
                    var controllerName = "TransactionFeeClientFeeGroupCarHotel";
                if (item.ProductName == "Air")
                  {
                      controllerName = "TransactionFeeClientFeeGroupAir";
                  }
                  %>


                    <td><%= Html.RouteLink("View", "Main", new { controller = controllerName, action = "View", tid = item.TransactionFeeId, cid = item.ClientFeeGroupId }, new { title = "View" })%> </td>
                    <td><%if (Model.HasDomainWriteAccess){ %><%= Html.RouteLink("Edit", "Main", new { action = "Edit", tid = item.TransactionFeeId, cid = item.ClientFeeGroupId }, new { title = "Edit" })%><%}%></td>
                    <td><%if (Model.HasDomainWriteAccess)
                          { %><%= Html.RouteLink("Delete", "Main", new { controller = controllerName, action = "Delete", tid = item.TransactionFeeId, cid = item.ClientFeeGroupId }, new { title = "Delete" })%><%}%></td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="11" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientFeeGroupTransactionFees.HasPreviousPage)
                               { %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.ClientFeeGroupTransactionFees.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientFeeGroupTransactionFees.HasNextPage)
                               { %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.ClientFeeGroupTransactionFees.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId}, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientFeeGroupTransactionFees.TotalPages > 0)
                                                         { %>Page <%=Model.ClientFeeGroupTransactionFees.PageIndex%> of <%=Model.ClientFeeGroupTransactionFees.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="10">
			    <%= Html.RouteLink("Create Mid Office Transaction Fee Item", "Default", new { action = "Create", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { @class = "red", title = "Create Mid Office Transaction Fee Item" })%>
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
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val(<%=Model.ClientFeeGroup.ClientFeeGroupId%>);
	});

	//Search
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/TransactionFeeClientFeeGroup.mvc/List");
		$("#frmSearch").submit();
	});
 </script>
 </asp:Content>

  <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeTypeDisplayName + "s", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeTypeId }, new { title = "Client Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Mid Office Transaction Fee Items
</asp:Content>

