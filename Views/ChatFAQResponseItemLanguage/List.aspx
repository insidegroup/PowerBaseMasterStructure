<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectChatFAQResponseItemLanguages_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Response Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Response Translations</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="33%" class="row_header_left"><%=  Html.RouteLink("Language", "ListMain", new { id = ViewData["ChatFAQResponseItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="52%"><%=  Html.RouteLink("Translation", "ListMain", new { id = ViewData["ChatFAQResponseItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "ChatFAQResponseItemLanguageDescription" }, new { title = "Sort By Translation" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th>
		    </tr> 
            <%
            foreach (var item in Model) {
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName) %></td>
                <td><%= Html.Encode(item.ChatFAQResponseItemLanguageDescription) %></td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, id = item.ChatFAQResponseItemId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, id = item.ChatFAQResponseItemId }, new { title = "Delete" })%>
                    <%} %>
                </td>             
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["ChatFAQResponseItemId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["ChatFAQResponseItemId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left">
                    <a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    <a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
			    <td class="row_footer_blank_right" colspan="3">
			    <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Create", "Main", new { action = "Create", id = ViewData["ChatFAQResponseItemId"] }, new { @class = "red", title = "Create" })%>
			    <% } %> 
			    </td>  
		    </tr> 	        
        </table>    
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_chatmessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
        $('#search_wrapper').css('height', 'auto');
    })
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ChatFAQResponseGroupName"].ToString(), "Default", new { controller = "ChatFAQResponseGroup", action = "View", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = ViewData["ChatFAQResponseGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ChatFAQResponseItem", action = "List", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.Encode(ViewData["ChatMessageFAQName"].ToString())%> &gt;
Translations
</asp:Content>