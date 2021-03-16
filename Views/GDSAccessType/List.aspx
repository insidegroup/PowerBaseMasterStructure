<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSAccessTypesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Type</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="35%" class="row_header_left">GDS</th>
			        <th width="54%">GDS Access Type</th>
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.GDSAccessTypes)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.GDSName) %></td>
	                <td><%= Html.Encode(item.GDSAccessTypeName) %></td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.GDSAccessTypeId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.GDSAccessTypeId })%>
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
							<% if (Model.GDSAccessTypes.HasPreviousPage){ %>
								<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "GDSAccessType", action = "List", page = (Model.GDSAccessTypes.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
							<%}%>
                        </div>
                        <div class="paging_right">
							<% if (Model.GDSAccessTypes.HasNextPage){ %>
								<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "GDSAccessType", action = "List", page = (Model.GDSAccessTypes.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
							<%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.GDSAccessTypes.TotalPages > 0){ %>Page <%=Model.GDSAccessTypes.PageIndex%> of <%=Model.GDSAccessTypes.TotalPages%><%} %></div>
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
							<%= Html.ActionLink("Create GDS Access Type", "Create", new {}, new { @class = "red", title = "Create GDS Access Type" })%>
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
