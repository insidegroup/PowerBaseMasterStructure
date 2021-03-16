<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.PaginatedList<CWTDesktopDatabase.Models.fnDesktopDataAdmin_SelectSystemUserGDSs_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - System Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">System User GDS</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="43%" class="row_header_left">GDS</th> 
			        <th width="10%">Default?</th> 
			        <th width="20%">Home PCC/Office ID</th> 
			        <th width="15%">GDS Sign On ID</th> 
			        <th width="4%"></th> 
			        <th width="8%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.GDSName) %></td>
                    <td>
                        <%if (item.DefaultGDS == true) {%>
                            Yes
                        <%} else {%>
                           No
                        <% } %>
                    </td>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeId) %></td>
                    <td><%= Html.Encode(item.GDSSignOn) %></td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Edit", "Edit", new { gdscode = item.GDSCode, id = item.SystemUserGuid }, new { title = "Edit" })%>
						<% } %> 
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
							<%=  Html.ActionLink("Remove", "Delete", new { gdscode = item.GDSCode, id = item.SystemUserGuid }, new { title = "Delete" })%>
						<% } %> 
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="3" class="row_footer_left">
						<% if (Model.HasPreviousPage) { %>
							<%= Html.RouteLink("<<Previous Page", "List", new { page = (Model.PageIndex - 1), id = ViewData["SystemUserGuid"] }, new { title = "Previous Page" })%>
						<% } %>
                    </td>
                    <td colspan="3" class="row_footer_right">
						<% if (Model.HasNextPage) {  %>
							<%= Html.RouteLink("Next Page>>>", "List", new { page = (Model.PageIndex + 1), id = ViewData["SystemUserGuid"] }, new { title = "Next Page" })%>
						<% } %> 
                    </td>
                </tr>
		        <tr> 
		            <td class="row_footer_blank_left" colspan="3"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="3">
						<%if (ViewData["Access"] == "WriteAccess"){ %>
							<%= Html.ActionLink("Add GDS", "Create", new { id = ViewData["SystemUserGuid"] }, new { @class = "red", title = "Add GDS" })%>
			            <% } %> 
			        </td> 
		        </tr> 
            </table>
        </div> </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_teams').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
