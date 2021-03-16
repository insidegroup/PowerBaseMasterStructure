<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeAddressesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Address
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Address</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="17%" class="row_header_left"><%= Html.RouteLink("First Address Line", "ListMain", new { page = 1, sortField = "FirstAddressLine", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="17%"><%= Html.RouteLink("Second Address Line", "ListMain", new { page = 1, sortField = "SecondAddressLine", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("City", "ListMain", new { page = 1, sortField = "CityName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="19%"><%= Html.RouteLink("State/Province", "ListMain", new { page = 1, sortField = "StateProvinceName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="16%"><%= Html.RouteLink("Postal Code", "ListMain", new { page = 1, sortField = "PostalCode", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
					<th width="10%"><%= Html.RouteLink("Country", "ListMain", new { page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
                    <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
                </tr> 
                <%
                foreach (var item in Model.PseudoCityOrOfficeAddresses)
                { 
                %>
                <tr>
                    <td class="wrap-text"><%= Html.Encode(item.FirstAddressLine) %></td>
                    <td class="wrap-text"><%= Html.Encode(item.SecondAddressLine) %></td>
                    <td class="wrap-text"><%= Html.Encode(item.CityName) %></td>
                    <td class="wrap-text"><%= Html.Encode(item.StateProvinceName) %></td>
					<td class="wrap-text"><%= Html.Encode(item.PostalCode) %></td>
					<td class="wrap-text"><%= Html.Encode(item.CountryName) %></td>
                    <td align="right">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.PseudoCityOrOfficeAddressId})%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { id = item.PseudoCityOrOfficeAddressId})%>
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
								<% if (Model.PseudoCityOrOfficeAddresses.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PseudoCityOrOfficeAddress", action = "List", page = (Model.PseudoCityOrOfficeAddresses.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.PseudoCityOrOfficeAddresses.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PseudoCityOrOfficeAddress", action = "List", page = (Model.PseudoCityOrOfficeAddresses.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.PseudoCityOrOfficeAddresses.TotalPages > 0){ %>Page <%=Model.PseudoCityOrOfficeAddresses.PageIndex%> of <%=Model.PseudoCityOrOfficeAddresses.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="7">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Pseudo City/Office ID Address", "Create", new { }, new { @class = "red", title = "Create Pseudo City/Office ID Address" })%>
						<%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_gdsmanagement').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

    	//Search
        $('#search').show();
        $('#btnSearch').click(function () {
        	if ($('#filter').val() == "") {
        		alert("No Search Text Entered");
        		return false;
        	}
        	$("#frmSearch").attr("action", "/PseudoCityOrOfficeAddress.mvc/List");
        	$("#frmSearch").submit();
        });
    })
 </script>
</asp:Content>
