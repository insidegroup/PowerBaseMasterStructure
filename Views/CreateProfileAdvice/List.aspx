<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientSubUnitCreateProfileAdviceLanguages_v1Result>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - CreateProfile Advice</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="33%" class="row_header_left"><%=Html.ActionLink("Language", "List", new { controller = "CreateProfileAdvice", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                    <th width="50%"><%=Html.ActionLink("CreateProfile Advice", "List", new { controller = "CreateProfileAdvice", id = ViewData["ClientSubUnitGuid"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "CreateProfileAdvice" }, new { title = "Sort By CreateProfile Advice" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
               
                foreach (var item in Model) { 
                  
                %>
                <tr>
                    <td><%= Html.Encode(item.LanguageName)%></td>
                    <td><%= Html.Encode(item.CreateProfileAdvice)%></td>
                    <td align="center"><%=  Html.RouteLink("View", "Main", new { action = "ViewItem", languageCode = item.LanguageCode, id = item.ClientSubUnitGuid }, new { title = "View Item" })%></td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                            <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, id = item.ClientSubUnitGuid }, new { title = "Edit Item" })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                            <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, id = item.ClientSubUnitGuid }, new { title = "Delete Item" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td class="row_footer_left">
                    <% if (Model.HasPreviousPage) { %>
                        <%= Html.RouteLink("<<Previous Page", "List", new { id = ViewData["ClientSubUnitGuid"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                    <% } %>
                    </td>
                    <td colspan="4" class="row_footer_right">
                    <% if (Model.HasNextPage) {  %>
                        <%= Html.RouteLink("Next Page>>>", "List", new { id = ViewData["ClientSubUnitGuid"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                    <% } %> 
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td colspan="4" class="row_footer_blank_right" colspan="3">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.ActionLink("Create CreateProfile Advice", "Create", new { id = ViewData["ClientSubUnitGuid"] }, new { @class = "red", title = "Create CreateProfile Advice" })%>
			        <% } %> 
			        </td> 
		        </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
