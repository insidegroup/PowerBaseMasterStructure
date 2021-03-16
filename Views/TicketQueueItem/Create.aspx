<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TicketQueueItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Ticket Queue Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Ticket Queue Item</th> 
		        </tr> 
                <tr>
                    <td><label for="TicketQueueGroupName">Ticket Queue Group Name</label></td>
                    <td><strong><%= Html.Label(Model.TicketQueueGroupName)%></strong></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="TicketQueueItemDescription">Ticket Queue Item Description</label></td>
                    <td><%= Html.TextBoxFor(model => model.TicketQueueItemDescription)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TicketQueueItemDescription)%></td>
                </tr> 
                 <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownList("GDSCode", ViewData["GDSs"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSCode)%></td>
                </tr> 
                <tr>
                    <td><label for="PseudoCityOrOfficeId">Pseudo City or Office Id</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeId, new { maxlength="10"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeId)%></td>
                </tr> 
                <tr>
                    <td><label for="QueueNumber">Queue Number</label></td>
                    <td><%= Html.TextBoxFor(model => model.QueueNumber, new { maxlength = "9"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueNumber)%></td>
                </tr> 
                <tr>
                    <td><label for="QueueCategory">Queue Category</label></td>
                    <td><%= Html.TextBoxFor(model => model.QueueCategory, new { maxlength="20"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.QueueCategory)%></td>
                </tr> 
                <tr>
                    <td> <label for="TicketTypeId">Ticket Type Id</label></td>
                    <td><%= Html.DropDownList("TicketTypeId", ViewData["TicketTypes"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TicketTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="TicketingFieldRemark">Ticketing Field Remark</label></td>
                    <td><%= Html.TextBoxFor(model => model.TicketingFieldRemark, new { maxlength="45"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TicketingFieldRemark)%></td>
                </tr> 
				<tr>
                    <td><label for="TicketingCommand">Ticketing Command</label></td>
                    <td><%= Html.TextBoxFor(model => model.TicketingCommand, new { maxlength="20"})%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TicketingCommand)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Ticket Queue Item" title="Create Ticket Queue Item" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.TicketQueueGroupId) %>    
    <% } %>

    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/TicketQueueItem.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Ticket Queue Groups", "Main", new { controller = "TicketQueueGroup", action = "ListUnDeleted", }, new { title = "Ticket Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.TicketQueueGroupName, "Default", new { controller = "TicketQueueGroup", action = "View", id = Model.TicketQueueGroupId }, new { title = Model.TicketQueueGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "TicketQueueItem", action = "List", id = Model.TicketQueueGroupId.ToString() }, new { title = "Items" })%>
</asp:Content>