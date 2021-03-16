<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TicketQueueItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Ticket Queue Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Ticket Queue Item</th> 
		        </tr> 
                <tr>
                    <td>Ticket Queue Group Name</td>
                    <td><%= Html.Encode(Model.TicketQueueGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Ticket Queue Item Description</td>
                    <td><%= Html.Encode(Model.TicketQueueItemDescription)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>GDS</td>
                    <td><%= Html.Encode(Model.GDSName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Pseudo City or Office Id</td>
                    <td><%= Html.Encode(Model.PseudoCityOrOfficeId)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Queue Number</td>
                    <td><%= Html.Encode(Model.QueueNumber)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Queue Category</td>
                    <td><%= Html.Encode(Model.QueueCategory)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Ticket Type Description</td>
                    <td><%= Html.Encode(Model.TicketTypeDescription)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Ticketing Command</td>
                    <td><%= Html.Encode(Model.TicketingCommand)%></td>
					<td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%}%>
                    </td>                
               </tr>
            </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_ticketqueuegroups').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Ticket Queue Groups", "Main", new { controller = "TicketQueueGroup", action = "ListUnDeleted", }, new { title = "Ticket Queue Groups" })%> &gt;
<%=Html.RouteLink(ViewData["TicketQueueGroupName"].ToString(), "Default", new { controller = "TicketQueueGroup", action = "View", id = ViewData["TicketQueueGroupId"].ToString() }, new { title = ViewData["TicketQueueGroupName"] })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "TicketQueueItem", action = "List", id = ViewData["TicketQueueGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.Encode(Model.TicketQueueItemDescription) %>
</asp:Content>
