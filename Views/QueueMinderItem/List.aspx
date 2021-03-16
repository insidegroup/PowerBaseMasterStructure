<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderItemsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="22%" class="row_header_left"><%= Html.RouteLink("Description", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "QueueMinderItemDescription", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Description" })%></th> 
			        <th width="16%"><%= Html.RouteLink("Type", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "QueueMinderTypeDescription", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Queue Minder Type" })%></th> 
			        <th width="10%"><%= Html.RouteLink("PCC/OID", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "PseudoCityOrOfficeId", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By PseudoCity Or OfficeId" })%></th> 
			        <th width="6%"><%= Html.RouteLink("Q No.", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "QueueNumber", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Queue  Number" })%></th> 
			        <th width="8%"><%= Html.RouteLink("Q Cat", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "QueueCategory", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Queue Category" })%></th> 
			        <th width="6%"><%= Html.RouteLink("GDS", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By GDS Name" })%></th> 
			        <th width="8%"><%= Html.RouteLink("Context", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = 1, sortField = "ContextName", sortOrder = ViewData["NewSortOrder"] }, new { title = "Sort By Context" })%></th> 
			        <th width="8%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.QueueMinderItems) { 
                %>
                <tr>
                    <td class="wrap-text"><%= Html.Encode(item.QueueMinderItemDescription) %></td>
                    <td class="wrap-text"><%= Html.Encode(item.QueueMinderTypeDescription)%></td>
                    <td><%= Html.Encode(item.PseudoCityOrOfficeId)%></td>
                    <td><%= Html.Encode(item.QueueNumber)%></td>
                    <td><%= Html.Encode(item.QueueCategory)%></td>
                    <td><%= Html.Encode(item.GDSName)%></td>
                    <td><%= Html.Encode(item.ContextName)%></td>
                    <td><%=  Html.RouteLink("Alt Desc", "Default", new { controller="QueueMinderItemLanguage", action = "List", id = item.QueueMinderItemId }, new { title = "Alt Desc" })%></td>
                    <td><%=  Html.RouteLink("View", "Default", new { action = "View", id = item.QueueMinderItemId }, new { title = "View" })%></td>
                    <td>
                        <%if ((bool)item.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.QueueMinderItemId }, new { title = "Edit" })%>
                        <%} %>
                    </td>

                    <td>
                        <%if ((bool)item.HasWriteAccess)
                          {%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.QueueMinderItemId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="11" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.QueueMinderItems.HasPreviousPage)
                                                        { %><%=  Html.RouteLink("<<Previous Page", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = (Model.QueueMinderItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"> <% if (Model.QueueMinderItems.HasNextPage)
                                                          { %><%=  Html.RouteLink("Next Page>>", "List", new { id = Model.QueueMinderGroup.QueueMinderGroupId, page = (Model.QueueMinderItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.QueueMinderItems.TotalPages > 0)
                                                         { %>Page <%=Model.QueueMinderItems.PageIndex%> of <%=Model.QueueMinderItems.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
		            <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="10">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
             <%= Html.RouteLink("Create Queue Minder Item", "Default", new { controller = "QueueMinderItem", action = "Create", id = Model.QueueMinderGroup.QueueMinderGroupId }, new { @class = "red", title = "Create Queue Minder Item" })%>
             <%} %></td>
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_ticketqueuegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderGroup.QueueMinderGroupName, "Default", new { controller = "FollowUpQueueGroup", action = "View", id = Model.QueueMinderGroup.QueueMinderGroupId }, new { title = Model.QueueMinderGroup.QueueMinderGroupName })%> &gt;
Queue Items
</asp:Content>