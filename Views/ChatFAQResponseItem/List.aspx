<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectChatFAQResponseItems_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Chat FAQ Response Items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat FAQ Response Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                	<th width="40%" class="row_header_left"><%= Html.RouteLink("Chat Message FAQ Description", "List", new { id = ViewData["ChatFAQResponseGroupId"], page = 1, sortField = "ChatFAQResponseItemDescription", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Chat Message FAQ Description" })%></th> 
                	<th width="40%" class="row_header_left"><%= Html.RouteLink("Chat FAQ Response", "List", new { id = ViewData["ChatFAQResponseGroupId"], page = 1, sortField = "ChatMessageFAQName", sortOrder = ViewData["NewSortOrder"], filter = "" }, new { title = "Sort By Chat FAQ Response" })%></th> 
			        <th width="10%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="6%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ChatMessageFAQName) %></td>
                    <td><%= Html.Encode(item.ChatFAQResponseItemDescription)%></td>
                    <td><%= Html.RouteLink("Translations", "List", new { controller = "ChatFAQResponseItemLanguage", id = item.ChatFAQResponseItemId }, new { title = "Translations" })%> </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ChatFAQResponseItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess")
                          {%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ChatFAQResponseItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["ChatFAQResponseGroupId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["ChatFAQResponseGroupId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>

		        <tr> 
		           <td class="row_footer_blank_left" colspan="2">
                        <a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                        <a href="javascript:window.print();" class="red" title="Print">Print</a>&nbsp;
		                <%= Html.RouteLink("Export", "Main", new { action="Export", id = ViewData["ChatFAQResponseGroupId"] }, new { @class = "red", title = "Export" })%>
                    </td>
			        <td class="row_footer_blank_right" colspan="3">
						<%if (ViewData["ImportAccess"] == "WriteAccess"){ %>
							<%= Html.RouteLink("Import", "Main", new { action = "ImportStep1", id = ViewData["ChatFAQResponseGroupId"] }, new { @class = "red", title = "Import" })%>&nbsp;
						<% } %>
						<%if (ViewData["Access"] == "WriteAccess"){ %>
    						<%= Html.RouteLink("Create", "Default", new { controller = "ChatFAQResponseItem", action = "Create", id = ViewData["ChatFAQResponseGroupId"] }, new { @class = "red", title = "Create" })%>
						<%} %>
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

         //Search
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val('<%=Html.Encode(ViewData["ChatFAQResponseGroupId"].ToString())%>');
   });

    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ChatFAQResponseItem.mvc/List");
        $("#frmSearch").submit();
    });
 </script>
 

</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ChatFAQResponseGroupName"].ToString(), "Default", new { controller = "ChatFAQResponseGroup", action = "View", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = ViewData["ChatFAQResponseGroupName"] })%> &gt;
Items
</asp:Content>