<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyHotelCapRateGroupItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Policy HotelCapRate Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Hotel Cap Rate Group Items</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="5%" class="row_header_left"><%=  Html.RouteLink("Seq.", "List", new{controller = "PolicyHotelCapRateGroupItem",action = "List",id = ViewData["PolicyGroupID"], page = 1,sortField = "Name",sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] },new { title = "Sequence" })%></th>
                	<th width="15%"><%=  Html.RouteLink("Location", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "PolicyLocationName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Location" })%></th>
                	<th width="12%"><%=  Html.RouteLink("Cap Rate", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "CapRate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Cap Rate" })%></th>
                	<th width="10%"><%=  Html.RouteLink("Currency", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "CurrencyCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Currency " })%></th>
                	<th width="14%"><%=  Html.RouteLink("Tax Inclusive?", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "TaxInclusiveFlag", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Currency " })%></th>
                	<th width="13%"><%=  Html.RouteLink("Enabled Date", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "EnabledDate", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Enabled Date" })%></th>
                	<th width="10%"><%=  Html.RouteLink("Advice", "List", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"], page = 1, sortField = "Advice", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Advice" })%></th>
			        <th width="4%"></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SequenceNumber)%></td>
                    <td><%= Html.Encode(item.PolicyLocationName)%></td>
					<td><%= Html.Encode(item.CapRate)%></td>
					<td><%= Html.Encode(item.CurrencyCode)%></td>
					<td><%= Html.Encode(item.TaxInclusiveFlag == true ? "Yes" : "No")%></td>
					<td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>
                    <td><%= Html.Encode(item.Advice > 0 ? "Yes" : "No")%></td>
                    <td>
                        <%= Html.ActionLink("Advice", "List", "HotelCapRateAdvice", new { id = item.PolicyHotelCapRateItemId }, null)%>
                    </td>
                    <td align="center"><%=  Html.ActionLink("View", "View", new { id = item.PolicyHotelCapRateItemId })%></td>
                    <td align="center">
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.PolicyHotelCapRateItemId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.PolicyHotelCapRateItemId })%>
                        <%} %>
                    </td>                  
                </tr>
                <% 
                } 
                %>
                 <tr>
                    <td colspan="11" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PolicyHotelCapRateGroupItem", action = "List", id = ViewData["PolicyGroupID"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
		        <tr> 
					<td class="row_footer_blank_left" colspan="4">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>&nbsp;
						<%= Html.RouteLink("Export", "Default", new { action = "Export", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Export" })%>
					</td>
			        <td class="row_footer_blank_right" colspan="7">
                        <%if (ViewData["ImportAccess"].ToString() == "WriteAccess"){ %>
                            <%= Html.RouteLink("Import", "Default", new { action = "ImportStep1", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Import" })%>
                        <% } %>
                        <%if (ViewData["Access"].ToString() == "WriteAccess"){ %>
                            <%= Html.RouteLink("Edit Sequencing","Default",  new { action="EditSequence", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Edit Sequencing" })%>
                            <%= Html.RouteLink("Create Policy Hotel Cap Rate Group Item", "Default", new { action = "Create", id = ViewData["PolicyGroupID"] }, new { @class = "red", title = "Create Policy Hotel Cap Rate Group Item" })%>
                        <% } %>
                    </td>
		         </tr>
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $('#search_wrapper').css('height', '32px');
        $("#frmSearch input[name='ft']").attr('name', 'id').val(<%=ViewData["PolicyGroupID"]%>);
    })
    $('#btnSearch').click(function () {
	    if ($('#filter').val() == "") {
		    alert("No Search Text Entered");
		    return false;
	    }
	    $("#frmSearch").attr("action", "/PolicyHotelCapRateGroupItem.mvc/List");
	    $("#frmSearch").submit();
    });
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(ViewData["PolicyGroupName"].ToString(), "Default", new { controller = "PolicyGroup", action = "View", id = ViewData["PolicyGroupId"] }, new { title = ViewData["PolicyGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = ViewData["PolicyGroupId"] }, new { title = "Items" })%> &gt;
Policy Hotel Cap Rate Group Items
</asp:Content>