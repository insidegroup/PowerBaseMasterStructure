<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.BookingChannelVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Booking Channel</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Booking Channel</th> 
		        </tr> 
				<tr>
                    <td width="30%"><label for="BookingChannel_BookingChannel">Booking Channel</label></td>
                    <td width="40%"><%= Html.DropDownListFor(model => model.BookingChannel.BookingChannelTypeId, Model.BookingChannelTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td width="30%"><%= Html.ValidationMessageFor(model => model.BookingChannel.BookingChannelTypeId)%></td>
                </tr>
				<tr>
                    <td width="30%"><label for="BookingChannel_ChannelProductTypeId">Channel Product</label></td>
                    <td width="40%"><%= Html.DropDownListFor(model => model.BookingChannel.ProductChannelTypeId, Model.ProductChannelTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td width="30%"><%= Html.ValidationMessageFor(model => model.BookingChannel.ProductChannelTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="BookingChannel_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.BookingChannel.GDSCode, Model.GDSList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.BookingChannel.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="BookingChannelDesktopUsedId">Desktop Used</label></td>
                    <td><%= Html.DropDownListFor(model => model.BookingChannel.DesktopUsedTypeId, Model.DesktopUsedTypes, "Please Select...")%><span id="BookingChannelDesktopUsedIdError" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.BookingChannel.DesktopUsedTypeId)%>
						<label id="lblValidDesktopUsedIdMessage"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="BookingChannel_BookingPseudoCityOrOfficeId">Booking Pseudo City/Office ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.BookingChannel.BookingPseudoCityOrOfficeId, new { maxlength = "9" })%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.BookingChannel.BookingPseudoCityOrOfficeId)%>
						<label id="lblValidBookingPseudoCityOrOfficeIdMessage"/>
                    </td>
                </tr>
				<tr>
                    <td><label for="BookingChannel_TicketingPseudoCityOrOfficeId">Ticketing Pseudo City/Office ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.BookingChannel.TicketingPseudoCityOrOfficeId, new { maxlength = "9" })%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.BookingChannel.TicketingPseudoCityOrOfficeId)%>
						<label id="lblValidTicketingPseudoCityOrOfficeIdMessage"/>
                    </td>
                </tr>
				<tr valign="top">
                    <td><label for="BookingChannel_ContentBookedItems">Content Booked</label></td>
                    <td valign="top" class="listbox">
						<%= Html.ListBoxFor(model => model.ContentBookedItems, Model.Products, "Please Select...")%>
						<span> Hold CNTRL to select multiple</span>
                    </td>
                    <td><%= Html.ValidationMessageFor(model => model.ContentBookedItems)%></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Booking Channel" class="red" title="Create Booking Channel"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.BookingChannel.ClientSubUnit.ClientSubUnitGuid) %>
			<%= Html.HiddenFor(model => model.BookingChannel.ClientSubUnitGuid) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/BookingChannel.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Model.ClientSubUnit.ClientSubUnitName%>
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%= Html.RouteLink("Booking Channels", "Main", new { controller = "BookingChannel", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid} , new { title = "Booking Channels" })%> &gt; 
Create Booking Channel
</asp:Content>