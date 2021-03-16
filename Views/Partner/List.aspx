<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PartnersVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Partners
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Partners</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
					<th width="40%" class="row_header_left"><%= Html.RouteLink("Partner Name", "ListMain", new { page = 1, sortField = "PartnerName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
					<th width="40%"><%= Html.RouteLink("Country Name", "ListMain", new { page = 1, sortField = "CountryName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="13%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% foreach (var item in Model.Partners) { %>
                <tr>
                    <td><%= Html.Encode(item.PartnerName) %></td>
					<td><%= Html.Encode(item.CountryName) %></td>
                    <td align="right">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.PartnerId})%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.PartnerId})%>
                        <%} %>
                    </td>
                </tr>        
                <% } %>
                <tr>
					<td colspan="4" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.Partners.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "Partner", action = "List", page = (Model.Partners.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.Partners.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "Partner", action = "List", page = (Model.Partners.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.Partners.TotalPages > 0){ %>Page <%=Model.Partners.PageIndex%> of <%=Model.Partners.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="2">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Partner", "Create", null, new { @class = "red", title = "Create Partner" })%>
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
