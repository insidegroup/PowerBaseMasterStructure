<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectControlValueLanguages_v1Result>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Control Value Translations</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="30%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = ViewData["ControlValueId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                    <th width="55%"><%=  Html.RouteLink("Translation", "List", new { id = ViewData["ControlValueId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "ControlValueTranslation" }, new { title = "Sort By Translation" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LanguageName)%></td>
                    <td><%= Html.Encode(item.ControlValueTranslation)%></td>
                    <td><%=  Html.RouteLink("View", "LanguageView", new { action = "View", languageCode = item.LanguageCode, id = item.ControlValueId }, new { title = "View" })%></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%=  Html.RouteLink("Edit", "LanguageView", new { action = "Edit", languageCode = item.LanguageCode, id = item.ControlValueId }, new { title = "Edit" })%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){ %><%=  Html.RouteLink("Delete", "LanguageView", new { action = "Delete", languageCode = item.LanguageCode, id = item.ControlValueId }, new { title = "Delete" })%><%} %></td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "List", new { controller = "ControlValueTranslation", action = "List", id = ViewData["ControlValueId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "List", new { controller = "ControlValueTranslation", action = "List", id = ViewData["ControlValueId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="4">
                    <%if (ViewData["Access"] == "WriteAccess"){%>
                    <%= Html.ActionLink("Create Translation", "Create", new { id = ViewData["ControlValueId"] }, new { @class = "red", title = "Create Translation" })%>
                    <%} %></td> 
		        </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_admin').click();
		$("tr:odd").addClass("row_odd");
	    $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
