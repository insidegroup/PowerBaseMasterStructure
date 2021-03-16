<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ClientSummary.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientSummaryClientSubUnitClientAccounts_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Summary - Client Accounts Matrix</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" border="0" style="width:2250px;">
            <tr> 
		        <th style="width:300px;">ClientAccount</th> 
		        <th style="width:150px;">ClientDetail</th> 
		        <th style="width:150px;">CreditCard</th> 
		        
		        <th style="width:150px;">ClientFeeGroup</th> 
		        <th style="width:150px;">ExternalSystemParameter</th> 
		        <th style="width:150px;">GDSAdditionalEntry</th> 
		        
		        <th style="width:150px;">LocalOperatingHoursGroup</th> 
		        <th style="width:150px;">PNROutputGroup</th> 
		        <th style="width:150px;">PolicyGroup</th> 
		        
		        <th style="width:150px;">PublicHolidayGroup</th> 
		        <th style="width:150px;">ReasonCodeGroup</th> 
		        <th style="width:150px;">ServicingOptionGroup</th> 
		        
		        <th style="width:150px;">TicketQueueGroup</th> 
		        <th style="width:150px;">TripTypeGroup</th> 
		        <th style="width:150px;">WorkFlowGroup</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td>
                <% if (item.ClientSubUnitName != null){ %>
                    <%= Html.RouteLink(Html.Encode(item.ClientAccountName), "Main", new { controller = "ClientAccount", action = "ViewItem", can = item.ClientAccountNumber, ssc = item.SourceSystemCode, csu = ViewData["ClientSubUnitGuid"]}, new { title = "View Client Account" })%>
                <%}%>
                </td>
                <td>
                <% if (item.ClientDetailId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.ClientDetailName), "Main", new { controller = "ClientDetailClientAccount", action = "ListUnDeleted", can = item.ClientAccountNumber, ssc = item.SourceSystemCode }, new { title = "View ClientDetail" })%>
                <%}%>
                </td>
                <td>
                
                    <%= Html.RouteLink("Credit Cards", "Main", new { controller = "CreditCardClientAccount", action = "List", can = item.ClientAccountNumber, ssc = item.SourceSystemCode, csu = ViewData["ClientSubUnitGuid"] }, new { title = "View ClientDetail" })%>
                
                </td>
                <td>
                <% if (item.ClientFeeGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.ClientFeeGroupName), "Default", new { controller = "ClientFeeGroup", action = "View", id = item.ClientFeeGroupId }, new { title = "View ClientFeeGroup" })%>
                <%}%>
                </td>
                <td>
                <% if (item.ExternalSystemParameterId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.ExternalSystemParameterValue), "Default", new { controller = "ExternalSystemParameter", action = "View", id = item.ExternalSystemParameterId }, new { title = "View ExternalSystemParameter" })%>
                <%}%>
                </td>
                <td>
                <% if (item.GDSAdditionalEntryId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.GDSAdditionalEntryValue), "Default", new { controller = "GDSAdditionalEntry", action = "View", id = item.GDSAdditionalEntryId }, new { title = "View GDSAdditionalEntry" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.LocalOperatingHoursGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.LocalOperatingHoursGroupName), "Default", new { controller = "LocalOperatingHoursGroup", action = "View", id = item.LocalOperatingHoursGroupId }, new { title = "View LocalOperatingHoursGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.PNROutputGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.PNROutputGroupName), "Default", new { controller = "PNROutputGroup", action = "View", id = item.PNROutputGroupId }, new { title = "View PNROutputGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.PolicyGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.PolicyGroupName), "Default", new { controller = "PolicyGroup", action = "View", id = item.PolicyGroupId }, new { title = "View PolicyGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.PublicHolidayGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.PublicHolidayGroupName), "Default", new { controller = "PublicHolidayGroup", action = "View", id = item.PublicHolidayGroupId }, new { title = "View PublicHolidayGroupI" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.ReasonCodeGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.ReasonCodeGroupName), "Default", new { controller = "ReasonCodeGroup", action = "View", id = item.ReasonCodeGroupId }, new { title = "View ReasonCodeGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.ServicingOptionGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.ServicingOptionGroupName), "Default", new { controller = "ServicingOptionGroup", action = "View", id = item.ServicingOptionGroupId }, new { title = "View ServicingOptionGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.TicketQueueGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.TicketQueueGroupName), "Default", new { controller = "TicketQueueGroup", action = "View", id = item.TicketQueueGroupId }, new { title = "View TicketQueueGroup" })%>
                <%}%>
                </td>
                 <td>
                <% if (item.TripTypeGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.TripTypeGroupName), "Default", new { controller = "TripTypeGroup", action = "View", id = item.TripTypeGroupId }, new { title = "View TripTypeGroup" })%>
                <%}%>
                </td>
                <td>
                <% if (item.WorkFlowGroupId != null)
                   { %>
                    <%= Html.RouteLink(Html.Encode(item.WorkFlowGroupName), "Default", new { controller = "WorkFlowGroup", action = "View", id = item.WorkFlowGroupId }, new { title = "View WorkFlowGroup" })%>
                <%}%>
                </td>
            </tr>        
            <% 
            } 
            %>
        </table>
    </div>
     <div class="paging_container">
        <div class="paging_left">
         <% if (Model.HasPreviousPage){ %>
            <%= Html.RouteLink("<<Previous Page", "Main", new { page = (Model.PageIndex - 1), filter = Request.QueryString["filter"], id = ViewData["ClientTopUnitGuid"] })%>
        <%}%>
        </div>
        <div class="paging_right">
        <% if (Model.HasNextPage){  %>
            <%= Html.RouteLink("Next Page>>>", "Main", new { page = (Model.PageIndex + 1), filter = Request.QueryString["filter"], id = ViewData["ClientTopUnitGuid"] })%>
        <%}%> 
        </div>
        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
    </div>
</div>  
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
 </asp:Content>
 
 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Summary Search", "Main", new { controller = "ClientSummary", action = "SearchForm", }, new { title = "Client Summary Search" })%> &gt;
<%=Html.RouteLink("Results", "Main", new { controller = "ClientSummary", action = "ByClientTopUnit", filter = Request.QueryString["filter"] }, new { title = "Search Results" })%> &gt;
Client TopUnit:<%=ViewData["ClientTopUnitName"].ToString()%> &gt;
Client SubUnit:<%=Html.Encode(ViewData["ClientSubUnitName"].ToString())%>
</asp:Content>