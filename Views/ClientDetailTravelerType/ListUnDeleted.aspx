<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TravelerTypeClientDetailsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
   <div id="banner"><div id="banner_text">TravelerType - Client Details</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
             <tr> 
		        <th width="56%" class="row_header_left">Client Detail Name</th> 
		        <th width="12%">Enabled Date</th> 
		        <th width="10%"></th>  
		        <th width="5%">&nbsp;</th> 
		        <th width="4%">&nbsp;</th> 
		        <th width="4%">&nbsp;</th> 
		        <th width="7%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model.ClientDetails) { 
            %>
            <tr>
                <td><%= Html.Encode(item.ClientDetailName) %></td>
                <td><%= Html.Encode(item.EnabledDate.HasValue ? item.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>  
                <td></td>
                <td><%= Html.RouteLink("Items", "Main", new { controller = "ClientDetailTravelerType", action = "ListSubMenu", id = item.ClientDetailId, tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Items" })%> </td>                  
                <td><%= Html.RouteLink("View", "Main", new { controller = "ClientDetailTravelerType", action = "View", id = item.ClientDetailId, tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "View" })%> </td>
                <td>
                      <%if (ViewData["Access"] == "WriteAccess"){ %>
                    <%= Html.RouteLink("Edit", "Main", new { controller = "ClientDetailTravelerType", action = "Edit", id = item.ClientDetailId, tt=Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Edit" })%>                     
                    <%}%>
                </td>
                <td>
                      <%if (ViewData["Access"] == "WriteAccess"){ %>
                    <%= Html.RouteLink("Delete", "Main", new { controller = "ClientDetailTravelerType", action = "Delete", id = item.ClientDetailId, tt = Model.TravelerType.TravelerTypeGuid, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Delete" })%>
                    <%}%>
                </td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="7" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.ClientDetails.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientDetailTravelerType", action = "ListUnDeleted", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid, page = (Model.ClientDetails.PageIndex - 1), sortField = "ClientDetailName", sortOrder = "1" }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.ClientDetails.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientDetailTravelerType", action = "ListUnDeleted", csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid, page = (Model.ClientDetails.PageIndex + 1), sortField = "ClientDetailName", sortOrder = "1" }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.ClientDetails.TotalPages > 0){ %>Page <%=Model.ClientDetails.PageIndex%> of <%=Model.ClientDetails.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
		         <td class="row_footer_blank_right" colspan="6"><%= Html.ActionLink("Deleted Client Details", "ListDeleted", new { csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, new { @class = "red", title = "Deleted Client Details" })%>
			        <%if (ViewData["Access"] == "WriteAccess")
             { %>
			        &nbsp;<%= Html.ActionLink("Create Client Detail", "Create", new {csu = Model.ClientSubUnit.ClientSubUnitGuid, tt = Model.TravelerType.TravelerTypeGuid }, new { @class = "red", title = "Create Client Detail" })%>
			        <%} %></td> 
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
