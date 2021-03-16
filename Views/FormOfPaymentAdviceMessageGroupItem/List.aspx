<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItems_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - FOP Advice Message Group Items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">FOP Advice Message Group Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="8%" class="row_header_left"><%= Html.RouteLink("Product", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "ProductName", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Product Name" })%></th> 
                	<th width="19%"><%= Html.RouteLink("Supplier", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "SupplierName", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Supplier Name" })%></th> 
                	<th width="8%"><%= Html.RouteLink("Country", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Country Name" })%></th> 
                	<th width="15%"><%= Html.RouteLink("Travel Indicator", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "TravelIndicator", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Travel Indicator" })%></th> 
                	<th width="10%"><%= Html.RouteLink("FOP Type", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "FormOfPaymentTypeDescription", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Supplier Name" })%></th> 
                	<th width="20%"><%= Html.RouteLink("FOP Advice Message", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = 1, sortField = "FormOfPaymentAdviceMessage", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "FOP Advice Message" })%></th> 
			        <th width="10%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ProductName) %></td>
                    <td><%= Html.Encode(item.SupplierName) %></td>
                    <td><%= Html.Encode(item.CountryCode) %></td>
                    <td><%= Html.Encode(item.TravelIndicator) %></td>
                    <td><%= Html.Encode(item.FormOfPaymentTypeDescription) %></td>
                    <td class="wrap-text"><%= Html.Encode(item.FormOfPaymentAdviceMessage)%></td>
                    <td><%= Html.RouteLink("Translations", "List", new { controller = "FormOfPaymentAdviceMessageGroupItemTranslation", id = item.FormOfPaymentAdviceMessageGroupItemId }, new { title = "Translations" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value)
                            {%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.FormOfPaymentAdviceMessageGroupItemId }, new { title = "Edit" })%>
                        <%
                            } %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value)
                          {%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.FormOfPaymentAdviceMessageGroupItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="9" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupID"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>

		        <tr> 
		           <td class="row_footer_blank_left" colspan="2">
                        <a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                        <a href="javascript:window.print();" class="red" title="Print">Print</a>&nbsp;
                    </td>
			        <td class="row_footer_blank_right" colspan="7">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
    						<%= Html.RouteLink("Create", "Default", new { controller = "FormOfPaymentAdviceMessageGroupItem", action = "Create", id = ViewData["FormOfPaymentAdviceMessageGroupID"] }, new { @class = "red", title = "Create" })%>
						<%} %>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_fopadvicemessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

         //Search
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val('<%=Html.Encode(ViewData["FormOfPaymentAdviceMessageGroupID"].ToString())%>');
   });

    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/FormOfPaymentAdviceMessageGroupItem.mvc/List");
        $("#frmSearch").submit();
    });
 </script>
 

</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "FormOfPaymentAdviceMessageGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["FormOfPaymentAdviceMessageGroupName"].ToString(), "Default", new { controller = "FormOfPaymentAdviceMessageGroup", action = "View", id = ViewData["FormOfPaymentAdviceMessageGroupID"].ToString() }, new { title = ViewData["FormOfPaymentAdviceMessageGroupName"] })%> &gt;
Items
</asp:Content>