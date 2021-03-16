<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputItems_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnits - CDRs - PNR Formats
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - CDRs - PNR Formats</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="10%" class="row_header_left"><%= Html.RouteLink("GDS", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSName", }, new { title = "GDS" })%></th> 
		        <th width="20%"><%= Html.RouteLink("Remark Type", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "PNROutputRemarkTypeDescription", }, new { title = "Remark Type" })%></th> 
		        <th width="20%"><%= Html.RouteLink("Remark Qualifier", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "GDSRemarkQualifier", }, new { title = "Remark Qualifier" })%></th> 
		        <th width="21%"><%= Html.RouteLink("Remark", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "DefaultRemark", }, new { title = "Remark" })%></th> 
		        <th width="20"><%= Html.RouteLink("Language", "ListMain", new { action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LanguageName", }, new { title = "Language" })%></th> 
				<th width="3%">&nbsp;</th>
				<th width="3%">&nbsp;</th> 
				<th width="3%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.GDSName) %></td>
                <td><%= Html.Encode(item.PNROutputRemarkTypeName)%></td>
				<td><%= Html.Encode(item.GDSRemarkQualifier) %></td>
				<td><%= Html.Encode(item.DefaultRemark) %></td>
				<td><%= Html.Encode(item.LanguageName) %></td>
				<td><%= Html.RouteLink("Translations ", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutputLanguage", action = "List", id = item.ClientDefinedReferenceItemPNROutputId,  csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "Translations" })%></td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" ){%>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientDefinedReferenceItemPNROutputId, csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "Edit" })%>
					<% } %>
				</td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess"){%>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientDefinedReferenceItemPNROutputId, csu = ViewData["ClientSubUnitGuid"].ToString()}, new { title = "Delete" })%>
					<% } %>
				</td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage) {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left" colspan="2">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="6">
					<%= Html.RouteLink("Create PNR Format", "Main", new { action="Create", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString()}, new { @class = "red", title = "Create PNR Format" })%>
		        </td> 
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');

    	//Search
        $('#search').hide();

    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink(ViewData["DisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "View", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["DisplayNameAlias"].ToString() })%> &gt;
PNR Format
</asp:Content>