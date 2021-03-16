<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPageClientSubUnitClientAccounts_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Accounts</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="20%" class="row_header_left">Account Name</th> 
		        <th width="12%">Account No.</th> 
		        <th width="8%">SSC</th> 
		        <th width="8%">Default?</th> 
		        <th width="11%">&nbsp;</th> 
		        <th width="12%">&nbsp;</th> 
		        <th width="17%">&nbsp;</th>
				<th width="4%">&nbsp;</th> 
		        <th width="4%">&nbsp;</th> 
		        <th width="4%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.ClientAccountName) %></td>
                <td><%= Html.Encode(item.ClientAccountNumber) %></td>
                <td><%= Html.Encode(item.SourceSystemCode)%></td>
                <td><%= Html.Encode(item.DefaultFlag ? "True" : "False")%></td>
                <td><%= Html.RouteLink("Credit Cards", "Main", new { controller = "CreditCardClientAccount", action = "List", can = item.ClientAccountNumber, ssc = item.SourceSystemCode, csu = ViewData["ClientSubUnitGuid"] }, new { title = "View Credit Cards" })%> </td>
                <td><%= Html.RouteLink("Client Details", "Main", new { controller = "ClientDetailClientAccount", action = "ListUnDeleted", can = item.ClientAccountNumber, ssc = item.SourceSystemCode }, new { title = "View Details" })%> </td>
                <td><%= Html.RouteLink("View Client Account", "Main", new { controller = "ClientAccount", action = "ViewItem", can = item.ClientAccountNumber, ssc = item.SourceSystemCode}, new { title = "View Client Account" })%> </td>
				<td><%= Html.RouteLink("CDRs", "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"], can = item.ClientAccountNumber, ssc = item.SourceSystemCode}, new { title = "CDRs" })%> </td>
				<td>
                    <%if(item.HasWriteAccess.Value){%>
                    <%= Html.RouteLink("Edit", "Main", new { controller = "ClientSubUnitClientAccount", action = "Edit", can = item.ClientAccountNumber, ssc = item.SourceSystemCode, csu = ViewData["ClientSubUnitGuid"] }, new { title = "View" })%>                     
                    <%}%>
                </td>
                <td>
                    <%if(item.HasWriteAccess.Value){%>
                    <%= Html.RouteLink("Remove", "Main", new { controller = "ClientSubUnitClientAccount", action = "Delete", can = item.ClientAccountNumber, ssc = item.SourceSystemCode, clientSubUnitId = ViewData["ClientSubUnitGuid"] }, new { title = "View" })%>
                    <%}%>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="10" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = "AccountName", sortOrder = "1" }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = "AccountName", sortOrder = "1" }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		        <td class="row_footer_blank_right" colspan="9">
					<% if (ViewData["Access"].ToString() == "WriteAccess"){ %>
						<%= Html.RouteLink("Add Client Account", "Main", new { controller = "ClientSubUnitClientAccount", action="CreateAccount", id = ViewData["ClientSubUnitGuid"].ToString()}, new { @class = "red", title = "Add Client Account" })%>
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
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
Client Accounts
</asp:Content>