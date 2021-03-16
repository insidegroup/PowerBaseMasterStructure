<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTripTypes_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Trip Type Groups
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Types</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="54%" class="row_header_left"><%=  Html.RouteLink("Trip Type", "ListMain", new { controller = "TripType", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "TripTypeDescription" }, new { title = "Sort By Trip Type" })%></th> 
			        <th width="35%"><%=  Html.RouteLink("BackOffice Trip Type Code", "ListMain", new { controller = "TripType", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "BackOfficeTripTypeCode" }, new { title = "Sort By BackOffice TripType Code" })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.TripTypeDescription)%></td>
                    <td><%= Html.Encode(item.BackOfficeTripTypeCode)%></td>
                    <td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Edit", "Default", new { id = item.TripTypeId, action = "Edit" }, new { title = "Edit" })%>
						<% } %>
                    </td>
                    <td>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Delete", "Default", new { id = item.TripTypeId, action = "Delete" }, new { title = "Delete" })%>
						<% } %>
                    </td>
                   
                </tr>        
                <% 
                } 
                %>
                <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
								<%=  Html.RouteLink("<<Previous Page", "ListMain", new {action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
							<%}%>
                        </div>
                        <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
								<%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
							<%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="3">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Create Trip Type", "Main", new { action = "Create" }, new { @class = "red", title = "Create Trip Type" })%>
						<% } %>
			        </td>
                </tr> 
            </table>

    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_triptypegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    });

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/TripType.mvc/List");
        $("#frmSearch").submit();

    });
</script>
</asp:Content>
