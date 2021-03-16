<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceFundsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Service Funds</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Service Funds</div></div>
        <div id="content">
		<div class="home_button">
			<a href="/" class="red">Home</a>
		</div>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 			        					
					<th class="row_header_left"><%= Html.RouteLink("Client TopUnit Name", "ListMain", new { page = 1, sortField = "ClientTopUnitName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("ClientTopGuid", "ListMain", new { page = 1, sortField = "ClientTopUnitGuid", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("PCC/OID", "ListMain", new { page = 1, sortField = "ServiceFundPseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Status", "ListMain", new { page = 1, sortField = "FundUseStatus", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Queue", "ListMain", new { page = 1, sortField = "ServiceFundQueue", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Account No.", "ListMain", new { page = 1, sortField = "ClientAccountNumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Supplier", "ListMain", new { page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Minimum", "ListMain", new { page = 1, sortField = "ServiceFundMinimumValue", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th><%= Html.RouteLink("Channel", "ListMain", new { page = 1, sortField = "ChannelType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
		        </tr> 
                <%
                foreach (var item in Model.ServiceFunds) { 
                %>
                <tr>
                    <td width="24%" <%= Html.Raw(item.ClientTopUnitExpiryDate != null && item.ClientTopUnitExpiryDate <= DateTime.Now ? " class=\"expired\"" : String.Empty) %>><%= Html.Encode(item.ClientTopUnitName) %></td>
                    <td width="9%"><%= Html.Encode(item.ClientTopUnitGuid) %></td>
					<td width="4%"><%= Html.Encode(item.GDSCode) %></td>
					<td width="7%"><%= Html.Encode(item.ServiceFundPseudoCityOrOfficeId) %></td>
					<td width="6%"><%= Html.Encode(item.FundUseStatus) %></td>
					<td width="6%"><%= Html.Encode(item.ServiceFundQueue) %></td>
					<td width="9%"><%= Html.Encode(item.ClientAccountNumber) %></td>
					<td width="10%"><%= Html.Encode(item.SupplierName) %></td>
					<td width="7%"><%= Html.Encode(item.ServiceFundMinimumValue.ToString("0.####")) %></td>
					<td width="7%"><%= Html.Encode(item.ServiceFundChannelTypeName) %></td>
					<td width="4%">
						<%=  Html.RouteLink("View", "Default", new { action = "View", id = item.ServiceFundId }, new { title = "Edit" })%>
					</td>
					<td width="3%">
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ServiceFundId }, new { title = "Edit" })%>
                        <%} %>
					</td>
					<td width="4%">
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ServiceFundId }, new { title = "Delete" })%>
                        <%} %>
					</td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="13" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ServiceFunds.HasPreviousPage){ %>
								<%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.ServiceFunds.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ServiceFunds.HasNextPage){ %>
								<%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.ServiceFunds.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ServiceFunds.TotalPages > 0) { %>Page <%=Model.ServiceFunds.PageIndex%> of <%=Model.ServiceFunds.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="3">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="10">
						<%if (Model.HasWriteAccess){ %>
							<%= Html.RouteLink("Create", "Main", new { action = "Create" }, new { @class = "red", title = "Create" })%>
						<% } %>
					</td>  
				</tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
	$(document).ready(function () {
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#menu_admin').click();
		$('#search').show();
		$('#search_wrapper').css('height', '24px');
	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/ServiceFund.mvc/List");
		$("#frmSearch").submit();
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
Service Funds
</asp:Content>