<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientSummaryClientTopUnits_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Summary</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr> 
		        <th width="34%" class="row_header_left">Client TopUnit</th> 
		        <th width="66%">Client SubUnit</th> 
	        </tr> 
            <%
                foreach (var item in Model) { 
                %>
            <tr>
                <td>
                <% if (item.ClientTopUnitName != null){ %>
                    <%= Html.RouteLink(Html.Encode(item.ClientTopUnitName), "ListMain", new { controller = "ClientSummary", action = "ClientSubUnitsMatrix", id = item.ClientTopUnitGuid, filter = Request.QueryString["filter"] }, new { title = "View TopUnit's SubUnits" })%>
                <%}%>
                </td>
                <td>
                <% if (item.ClientSubUnitName != null){ %>
                    <%= Html.RouteLink(Html.Encode(item.ClientSubUnitName), "ListMain", new { controller = "ClientSummary", action = "ClientAccountsMatrix", id = item.ClientSubUnitGuid, filter = Request.QueryString["filter"] }, new { title = "View SubUnit's ClientAccounts" })%>
                <%}%>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="2" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                            <%= Html.RouteLink("<<Previous Page", "Main", new { page = (Model.PageIndex - 1), filter= Request.QueryString["filter"] })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage){  %>
                            <%= Html.RouteLink("Next Page>>>", "Main", new { page = (Model.PageIndex + 1),  filter = Request.QueryString["filter"] })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div> 
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
        $('#search').show();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");

    })

    //Search
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ClientSummary.mvc/SearchResults");
        $("#frmSearch").submit();

    });
 </script>
 </asp:Content>
 
  <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Summary Search", "Main", new { controller = "ClientSummary", action = "SearchForm", }, new { title = "Client Summary Search" })%> &gt;
Results
</asp:Content>