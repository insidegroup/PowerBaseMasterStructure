<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPolicyLocations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Locations</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="42%" class="row_header_left"><%=Html.RouteLink("Location Name", "ListMain", new { controller = "PolicyLocation", action = "List", page = 0, sortField = "PolicyLocationName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort By Name" })%></th>
			        <th width="30%"><%=Html.RouteLink("Location Value", "ListMain", new { controller = "PolicyLocation", action = "List", page = 0, sortField = "PolicyLocationValue", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Sort By Location Value" })%></th>
			        <th width="12%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.PolicyLocationName) %></td>
					<td><%= Html.Encode(item.PolicyLocationValue) %></td>
                    <td></td>
                    <td><%=  Html.RouteLink("View", "Default", new { id = item.PolicyLocationId, action = "View" }, new { title = "View" })%></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.RouteLink("Edit", "Default", new { id = item.PolicyLocationId, action="Edit" }, new { title="Edit"})%>
						<% } %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.RouteLink("Delete", "Default", new { id = item.PolicyLocationId, action = "Delete" }, new { title = "Delete" })%>
						<% } %>
                    </td>                  
                </tr>
                <% 
                } 
                %>
                 <tr>
                    <td colspan="6" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
									<%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
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
			        <td class="row_footer_blank_right" colspan="5">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
						<%= Html.ActionLink("Create Policy Location", "Create", null, new { @class = "red", title = "Create Policy Location" })%></td> 
		            <% } %>
		         </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
    })

    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/PolicyLocation.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>