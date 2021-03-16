<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedReferenceItemValues_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">CDR - Drop List Values</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="45%" class="row_header_left"><%= Html.RouteLink("Value", "ListMain", new { action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "Value" }, new { title = "Value" })%></th> 
		        <th width="45%"><%= Html.RouteLink("Value Description", "ListMain", new { action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ValueDescription" }, new { title = "Value Description" })%></th> 
				<th width="5%">&nbsp;</th> 
		        <th width="5%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <% foreach (var item in Model) { %>
            <tr>
                <td><%= Html.Encode(item.Value) %></td>
				<td><%= Html.Encode(item.ValueDescription) %></td>
				<td>
                    <%if(ViewData["Access"] == "WriteAccess" && item.BackOfficeDataSourceId == 2){%>
						<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientDefinedReferenceItemValueId, csu = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = "Edit" })%>                     
                    <%}%>
                </td>
                <td>
                    <%if(ViewData["Access"] == "WriteAccess" && item.BackOfficeDataSourceId == 2){%>
                    <%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientDefinedReferenceItemValueId, csu = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = "Delete" })%>
                    <%}%>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientDefinedReferenceItemValue", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], csu = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientDefinedReferenceItemValue", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], csu = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="3">
					<%if(ViewData["Access"] == "WriteAccess" && ViewData["CreateAccess"] == "CreateAccess"){%>
						<%= Html.RouteLink("Create Drop List Item", "Main", new { controller = "ClientDefinedReferenceItemValue", action="Create", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { @class = "red", title = "Create Drop List Item" })%>
					<% } %>
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
        $('#breadcrumb').css('width', '780px')
        $('#search').show();
        $('#ft').attr('name', 'id');
        $("#frmSearch input[name='id']").val('<%=Html.Encode(ViewData["ClientDefinedReferenceItemId"].ToString())%>');
        $('#btnSearch').click(function () {
        	$("#frmSearch").attr("action", "/ClientDefinedReferenceItemValue.mvc/List");
        	$("#frmSearch").submit();
        });
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title ="Client Accounts" })%> &gt;
<%if (ViewData["ClientAccountName"] != null) { %>
	<%=Html.RouteLink(ViewData["ClientAccountName"].ToString(), "Main", new { controller = "ClientAccount", action = "ViewItem", ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<% } %>
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString()}, new { title = "CDRs" })%> &gt;
<% if(ViewData["ClientDefinedReferenceItemDisplayNameAlias"] != null && !string.IsNullOrEmpty(ViewData["ClientDefinedReferenceItemDisplayNameAlias"].ToString())) { %>
	<%=Html.RouteLink(ViewData["ClientDefinedReferenceItemDisplayNameAlias"].ToString(), "Main", new { controller = "ClientDefinedReferenceItem", action = "View", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"], ssc = ViewData["SourceSystemCode"].ToString(), can = ViewData["ClientAccountNumber"].ToString() }, new { title = ViewData["ClientDefinedReferenceItemDisplayNameAlias"].ToString() })%> &gt;
<% } %>
Drop List Values
</asp:Content>