<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSThirdPartyVendorsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Third Party Vendor
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Third Party Vendor</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="49%" class="row_header_left">Third Party Vendor Name</th>
			        <th width="44%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.GDSThirdPartyVendors) { %>
                <tr>
                    <td><%= Html.Encode(item.GDSThirdPartyVendorName) %></td>
                    <td align="right">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.GDSThirdPartyVendorId})%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.GDSThirdPartyVendorId})%>
                        <%} %>
                    </td>
                </tr>        
                <% } %>
                <tr>
					<td colspan="3" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.GDSThirdPartyVendors.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "GDSThirdPartyVendor", action = "List", page = (Model.GDSThirdPartyVendors.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.GDSThirdPartyVendors.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "GDSThirdPartyVendor", action = "List", page = (Model.GDSThirdPartyVendors.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.GDSThirdPartyVendors.TotalPages > 0){ %>Page <%=Model.GDSThirdPartyVendors.PageIndex%> of <%=Model.GDSThirdPartyVendors.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="2">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Third Party Vendor", "Create", null, new { @class = "red", title = "Create Third Party Vendor" })%>
						<%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_admin, #menu_admin_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').hide();
	});
 </script>
</asp:Content>
