<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeItemsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Fee Items</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="51%" class="row_header_left"><%= Html.RouteLink("Fee Description", "ListMain", new { page = 1, sortField = "ClientFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="17%"><%= Html.RouteLink("Output Description", "ListMain", new { page = 1, sortField = "OutputDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="17%"><%= Html.RouteLink("Output Format", "ListMain", new { page = 1, sortField = "OutputFormat", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ClientFeeItems) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ClientFeeDescription)%></td>                    
                    <td><%= Html.Encode(item.OutputDescription)%></td>
                    <td><%= Html.Encode(item.OutputFormat) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ClientFeeItemId }, new { title = "View" })%> </td>
                    <td><%if (Model.HasWriteAccess){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientFeeItemId }, new { title = "Edit" })%><%}%></td>
                    <td><%if (Model.HasWriteAccess){ %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientFeeItemId }, new { title = "Delete" })%><%}%></td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="6" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.ClientFeeItems.HasPreviousPage)
                               { %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.ClientFeeItems.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.ClientFeeItems.HasNextPage)
                               { %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.ClientFeeItems.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], id = Model.ClientFeeGroup.ClientFeeGroupId}, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.ClientFeeItems.TotalPages > 0)
                                                         { %>Page <%=Model.ClientFeeItems.PageIndex%> of <%=Model.ClientFeeItems.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="5">
			    <%= Html.RouteLink("Create Client Fee Item", "Main", new { action = "Create", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { @class = "red", title = "Create Client Fee" })%>
			    </td>  
		    </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })

 </script>
 </asp:Content>

  <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeTypeDisplayName + "s", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeType.FeeTypeId }, new { title = "Client Fee Groups" })%> &gt;
<%=Html.RouteLink(Model.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Client Fee Items
</asp:Content>

