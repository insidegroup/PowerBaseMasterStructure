<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeDefinedRegionsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Defined Region
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Defined Region</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="25%" class="row_header_left">Global Region</th>
					<th width="35%" class="row_header_left">Pseudo City/Office ID Defined Region</th>
                    <th width="33%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.PseudoCityOrOfficeDefinedRegions)
                { 
                %>
                <tr>
                    <td><%= Html.Encode(item.GlobalRegionName) %></td>
					<td><%= Html.Encode(item.PseudoCityOrOfficeDefinedRegionName) %></td>
                    <td align="right">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.PseudoCityOrOfficeDefinedRegionId})%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { id = item.PseudoCityOrOfficeDefinedRegionId})%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
					<td colspan="4" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.PseudoCityOrOfficeDefinedRegions.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PseudoCityOrOfficeDefinedRegion", action = "List", page = (Model.PseudoCityOrOfficeDefinedRegions.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.PseudoCityOrOfficeDefinedRegions.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PseudoCityOrOfficeDefinedRegion", action = "List", page = (Model.PseudoCityOrOfficeDefinedRegions.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.PseudoCityOrOfficeDefinedRegions.TotalPages > 0){ %>Page <%=Model.PseudoCityOrOfficeDefinedRegions.PageIndex%> of <%=Model.PseudoCityOrOfficeDefinedRegions.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="3">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Pseudo City/Office ID Defined Region", "Create", new { }, new { @class = "red", title = "Create Pseudo City/Office ID Defined Region" })%>
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

    	//Search
        $('#search').hide();
    })
 </script>
</asp:Content>
