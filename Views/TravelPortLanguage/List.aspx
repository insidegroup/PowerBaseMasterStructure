<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTravelPortLanguages_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Port Languages</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="26%" class="row_header_left"><%=  Html.RouteLink("Language", "ListMain", new { id = ViewData["TravelPortCode"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="31%"><%=  Html.RouteLink("Travel Port Type", "ListMain", new { id = ViewData["TravelPortCode"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "TravelPortTypeDescription" }, new { title = "Sort By Travel Port Type Description" })%></th>
                <th width="31%"><%=  Html.RouteLink("Travel Port Name", "ListMain", new { id = ViewData["TravelPortCode"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "TravelPortName" }, new { title = "Sort By Travel Port Name" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="8%" class="row_header_right">&nbsp;</th> 
			        
		    </tr> 
            <%
               
            foreach (var item in Model) { 
                  
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName)%></td>
                <td><%= Html.Encode(item.TravelPortTypeDescription)%></td>                   
                <td><%= Html.Encode(item.TravelPortName)%></td>
                <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, travelPortTypeId = item.TravelPortTypeId, id = item.TravelPortCode }, new { title = "Edit" })%><%} %> </td>
                <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, travelPortTypeId = item.TravelPortTypeId, id = item.TravelPortCode }, new { title = "Delete" })%><%} %></td>             
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "TravelPortLanguage", action = "List", id = ViewData["TravelPortCode"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "TravelPortLanguage", action = "List", id = ViewData["TravelPortCode"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
                
		    <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td colspan="4" class="row_footer_blank_right">
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Create Travel Port Language", "Main", new { action = "Create", id = ViewData["TravelPortCode"] }, new { title = "Create Travel Port Language", @class = "red" })%>
                    <% } %>
			    </td> 
		    </tr> 
		        
        </table>    
    </div>
</div>
<script type="text/javascript">
$(document).ready(function() {
	$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");	
	$('#menu_admin').click();	
})
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Travel Ports", "Main", new { controller = "TravelPort", action = "List", }, new { title = "TravelPorts" })%> &gt;
<%=Html.RouteLink(ViewData["TravelPortName"].ToString(), "Main", new { controller = "TravelPort", action = "ViewItem", id = ViewData["TravelPortCode"].ToString() }, new { title = ViewData["TravelPortName"] })%> &gt;
<%=Html.RouteLink("Travel Port Languages", "Default", new { controller = "TravelPortLanguage", action = "View", id = ViewData["TravelPortCode"].ToString() }, new { title = "TravelPort Languages" })%>
</asp:Content>