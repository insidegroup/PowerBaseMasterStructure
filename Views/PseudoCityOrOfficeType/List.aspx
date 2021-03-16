<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeTypesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Type
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Type</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="50%" class="row_header_left">Pseudo City/Office ID</th>		        
                    <th width="33%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.PseudoCityOrOfficeTypes)
                { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeTypeName) %></td>
                    <td align="right">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { id = item.PseudoCityOrOfficeTypeId})%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Delete", "Delete", new { id = item.PseudoCityOrOfficeTypeId})%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
					<td colspan="3" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
								<% if (Model.PseudoCityOrOfficeTypes.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "PseudoCityOrOfficeType", action = "List", page = (Model.PseudoCityOrOfficeTypes.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
								<%}%>
							</div>
							<div class="paging_right">
								<% if (Model.PseudoCityOrOfficeTypes.HasNextPage){ %>
									<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "PseudoCityOrOfficeType", action = "List", page = (Model.PseudoCityOrOfficeTypes.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
								<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.PseudoCityOrOfficeTypes.TotalPages > 0){ %>Page <%=Model.PseudoCityOrOfficeTypes.PageIndex%> of <%=Model.PseudoCityOrOfficeTypes.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="2">
						<%	if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Create Pseudo City/Office ID Type", "Create", new { }, new { @class = "red", title = "Create Pseudo City/Office ID Type" })%>
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
