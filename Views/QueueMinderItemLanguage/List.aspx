<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderItemLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Alternate Descriptions</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr> 
			<th width="33%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = Model.QueueMinderItem.QueueMinderItemId, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
            <th width="52%"><%=  Html.RouteLink("Description", "List", new { id = Model.QueueMinderItem.QueueMinderItemId, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "QueueMinderItemLanguageDescription" }, new { title = "Sort By Description" })%></th>
			<th width="4%">&nbsp;</th> 
			<th width="4%">&nbsp;</th> 
			<th width="7%" class="row_header_right">&nbsp;</th> 		        
		</tr> 
        <%
        foreach (var item in Model.QueueMinderItemLanguages) { 
        %>
        <tr>
            <td><%= Html.Encode(item.LanguageName)%></td>
            <td><%= Html.Encode(item.QueueMinderItemLanguageItineraryDescription)%></td>
            <td><%=  Html.RouteLink("View", "LanguageView", new { action = "View", languageCode = item.LanguageCode, id = Model.QueueMinderItem.QueueMinderItemId }, new { title = "View" })%></td>
            <td><%if(Model.HasWriteAccess){ %><%=  Html.RouteLink("Edit", "LanguageView", new { action = "Edit", languageCode = item.LanguageCode, id = Model.QueueMinderItem.QueueMinderItemId }, new { title = "Edit" })%><%} %></td>
            <td><%if(Model.HasWriteAccess){ %><%=  Html.RouteLink("Delete", "LanguageView", new { action = "Delete", languageCode = item.LanguageCode, id = Model.QueueMinderItem.QueueMinderItemId }, new { title = "Delete" })%><%} %></td>             
        </tr>
        <% 
        } 
        %>
        <tr>
            <td colspan="5" class="row_footer">
                <div class="paging_container">
                    <div class="paging_left"><% if (Model.QueueMinderItemLanguages.HasPreviousPage)
                                                { %><%=  Html.RouteLink("<<Previous Page", "List", new { id = Model.QueueMinderItem.QueueMinderItemId, page = (Model.QueueMinderItemLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                    <div class="paging_right"> <% if (Model.QueueMinderItemLanguages.HasNextPage)
                                                  { %><%=  Html.RouteLink("Next Page>>", "List", new { id = Model.QueueMinderItem.QueueMinderItemId, page = (Model.QueueMinderItemLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                    <div class="paging_centre"><%if (Model.QueueMinderItemLanguages.TotalPages > 0)
                                                 { %>Page <%=Model.QueueMinderItemLanguages.PageIndex%> of <%=Model.QueueMinderItemLanguages.TotalPages%><%} %></div>
                </div>
            </td>
        </tr>
		    <tr> 
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="4"><%if(Model.HasWriteAccess){ %>
			    <%= Html.RouteLink("Create Alternative Description", "Default", new { action = "Create", id = Model.QueueMinderItem.QueueMinderItemId }, new { @class = "red", title = "Create Alternative Description" })%>
			    <%} %></td> 
		    </tr> 	        
        </table>    
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_ticketqueuegroups').click();
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName, "Default", new { controller = "FollowUpQueueGroup", action = "View", id = Model.QueueMinderItem.QueueMinderGroupId }, new { title = Model.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName })%> &gt;
<%=Html.RouteLink("Queue Minder Items", "Default", new { controller = "QueueMinderItem", action = "List", id = Model.QueueMinderItem.QueueMinderGroupId }, new { title = "Queue Minder Items" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderItem.QueueMinderItemDescription, "Default", new { controller = "QueueMinderItem", action = "View", id = Model.QueueMinderItem.QueueMinderItemId }, new { title = Model.QueueMinderItem.QueueMinderItemDescription })%>
</asp:Content>