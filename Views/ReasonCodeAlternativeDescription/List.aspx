<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeAlternativeDescriptions_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ReasonCodeGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
        <div id="banner"><div id="banner_text">Reason Code Item Alternative Descriptions</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="25%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = ViewData["ReasonCodeItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                    <th width="60%"><%=  Html.RouteLink("Alt Description", "List", new { id = ViewData["ReasonCodeItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "Name" }, new { title = "Sort By Alternative Description" })%></th>
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LanguageName)%></td>
                    <td><%= Html.Encode(item.ReasonCodeAlternativeDescription)%></td>
                    <td><%=  Html.RouteLink("View", "LanguageView", new { action = "View", languageCode = item.LanguageCode, id = item.ReasonCodeItemId }, new { title = "View Item" })%></td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                            <%=  Html.RouteLink("Edit", "LanguageView", new { action = "Edit", languageCode = item.LanguageCode, id = item.ReasonCodeItemId }, new { title = "Edit Item" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                            <%=  Html.RouteLink("Delete", "LanguageView", new { action = "Delete", languageCode = item.LanguageCode, id = item.ReasonCodeItemId }, new { title = "Delete Item" })%>
                        <%} %>
                    </td>             
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="5" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.HasPreviousPage){ %><%= Html.RouteLink("<<Previous Page", "List", new { id = ViewData["ReasonCodeItemId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.HasNextPage){  %><%= Html.RouteLink("Next Page>>>", "List", new { id = ViewData["ReasonCodeItemId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%></div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex+1%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>

		        <tr> 
		            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4">
			        <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.ActionLink("Add Description", "Create", new { id = ViewData["ReasonCodeItemId"] }, new { @class = "red", title = "Add Description" })%>
			        <% } %> 
			        </td> 
		        </tr> 
		        
            </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_reasoncodes').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px'); 
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Groups", "Main", new { controller = "ReasonCodeGroup", action = "ListUnDeleted", }, new { title = "Reason Code Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ReasonCodeGroupName"].ToString(), "Default", new { controller = "ReasonCodeGroup", action = "View", id = ViewData["ReasonCodeGroupId"].ToString() }, new { title = ViewData["ReasonCodeGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ReasonCodeItem", action = "List", id = ViewData["ReasonCodeGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.RouteLink(ViewData["ReasonCodeItem"].ToString(), "Default", new { controller = "ReasonCodeItem", action = "Edit", id = ViewData["ReasonCodeItemId"].ToString() }, new { title = ViewData["ReasonCodeItem"].ToString() })%> &gt;
Alternative Descriptions
</asp:Content>