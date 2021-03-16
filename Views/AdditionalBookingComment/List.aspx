<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.AdditionalBookingCommentsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Additional Booking Comments</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" style="table-layout: fixed;">
                <tr> 
                    <th width="30%" class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { controller = "AdditionalBookingComment", action = "List", page = 1, id = Model.BookingChannel.BookingChannelId, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Language" })%></th>
			        <th width="59%"><%= Html.RouteLink("Additional Booking Comment", "ListMain", new { controller = "AdditionalBookingComment", action = "List", page = 1, id = Model.BookingChannel.BookingChannelId, csu = Model.ClientSubUnit.ClientSubUnitGuid, sortField = "AdditionalBookingCommentDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Additional Booking Comment" })%></th> 
					<th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 	
		        </tr> 
                <% foreach (var item in Model.AdditionalBookingComments) { %>
					<tr>
						<td><%= Html.Encode(item.LanguageName)%></td>
						<td class="word-wrap"><%= Html.Encode(item.AdditionalBookingCommentDescription)%></td>
						<td>
							<%if (ViewData["Access"] == "WriteAccess"){ %>
								<%=  Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.AdditionalBookingCommentId, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Edit" })%>
							<%} %>
						</td>
						<td>
							<%if (ViewData["Access"] == "WriteAccess"){ %>
								<%=  Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.AdditionalBookingCommentId, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Delete" })%>
							<%} %>
						</td>     
					</tr>        
				<% } %>
                <tr>
					<td colspan="4" class="row_footer">
						<div class="paging_container">
							<div class="paging_left">
							  <% if (Model.AdditionalBookingComments.HasPreviousPage)
								 { %>
							<%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "BookingChannels", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.AdditionalBookingComments.PageIndex - 1) }, new { title = "Previous Page" })%>
							<%}%>
							</div>
							<div class="paging_right">
							 <% if (Model.AdditionalBookingComments.HasNextPage)
								{ %>
							<%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "BookingChannels", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.AdditionalBookingComments.PageIndex + 1) }, new { title = "Next Page" })%>
							<%}%> 
							</div>
							<div class="paging_centre"><%if (Model.AdditionalBookingComments.TotalPages > 0){ %>Page <%=Model.AdditionalBookingComments.PageIndex%> of <%=Model.AdditionalBookingComments.TotalPages%><%} %></div>
						</div>
					</td>
				</tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="1">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>  
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>
			        <td class="row_footer_blank_right" colspan="3">
						<%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Create Additional Booking Comment", "Create", new { id = Model.BookingChannel.BookingChannelId, csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Create Additional Booking Comment" })%> <%}%>
			        </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
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
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;

<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%= Html.RouteLink("Booking Channels", "Main", new { controller = "BookingChannel", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid} , new { title = "Booking Channels" })%> &gt; 
Additional Booking Comments
</asp:Content>
