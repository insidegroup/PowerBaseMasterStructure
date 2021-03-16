<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.IATAsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - IATA
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">IATA (Deleted)</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="35%" class="row_header_left"><%= Html.RouteLink("IATA", "ListMain", new { page = 1, sortField = "IATANumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
			        <th width="54%"><%= Html.RouteLink("Branch or GL String", "ListMain", new { page = 1, sortField = "IATABranchOrGLString", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.IATAs)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.IATANumber) %></td>
	                <td><%= Html.Encode(item.IATABranchOrGLString) %></td>
                    <td align="center">&nbsp;</td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("UnDelete", "UnDelete", new { id = item.IATAId })%>
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
							<% if (Model.IATAs.HasPreviousPage){ %>
								<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "IATA", action = "List", page = (Model.IATAs.PageIndex - 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
							<%}%>
                        </div>
                        <div class="paging_right">
							<% if (Model.IATAs.HasNextPage){ %>
								<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "IATA", action = "List", page = (Model.IATAs.PageIndex + 1), sortField = ViewData["CurrentSortField"].ToString(), sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
							<%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.IATAs.TotalPages > 0){ %>Page <%=Model.IATAs.PageIndex%> of <%=Model.IATAs.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="3">
						<%= Html.ActionLink("UnDeleted IATAs", "ListUnDeleted", new {}, new { @class = "red", title = "UnDeleted IATAs" })%>
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
