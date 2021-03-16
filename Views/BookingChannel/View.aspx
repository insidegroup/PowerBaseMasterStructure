<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.BookingChannelVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Booking Channel</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
            <tr> 
                <th class="row_header" colspan="3">View Booking Channel</th> 
            </tr> 
            <tr>
                <td><label for="BookingChannel_BookingChannelType">Booking Channel</label></td>
                <td colspan="2">
					<%= Html.Encode(Model.BookingChannel.BookingChannelType != null ? Model.BookingChannel.BookingChannelType.BookingChannelTypeDescription.ToString() : "") %>
				</td>
            </tr>
			<tr>
                <td><label for="BookingChannel_BookingChannelType">Channel Product</label></td>
                <td colspan="2">
					<%= Html.Encode(Model.BookingChannel.ProductChannelType != null ? Model.BookingChannel.ProductChannelType.ProductChannelTypeDescription.ToString() : "") %>
				</td>
            </tr>
			<tr>
                <td><label for="BookingChannel_GDSCode">GDS</label></td>
                 <td colspan="2"><%= Html.Encode(Model.GDS.GDSName) %></td>
            </tr>
			<tr>
                <td><label for="BookingChannel_BookingChannelType">Desktop Used</label></td>
                <td colspan="2">
					<%= Html.Encode(Model.BookingChannel.DesktopUsedType != null ? Model.BookingChannel.DesktopUsedType.DesktopUsedTypeDescription.ToString() : "") %>
                </td>
            </tr>
			<tr>
                <td><label for="BookingChannel_BookingPseudoCityOrOfficeId">Booking Pseudo City/Office ID</label></td>
                 <td colspan="2"><%= Html.Encode(Model.BookingChannel.BookingPseudoCityOrOfficeId) %></td>
            </tr>
			<tr>
                <td><label for="BookingChannel_TicketingPseudoCityOrOfficeId">Ticketing Pseudo City/Office ID</label></td>
                 <td colspan="2"><%= Html.Encode(Model.BookingChannel.TicketingPseudoCityOrOfficeId) %></td>
            </tr> 
			<tr>
                <td><label for="BookingChannel_ContentBooked">Content Booked</label></td>
                <td colspan="2"><%= Html.Encode(Model.ContentBookedItemsList)%></td>
            </tr>    
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right" colspan="2"></td>                
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
View Booking Channel
</asp:Content>