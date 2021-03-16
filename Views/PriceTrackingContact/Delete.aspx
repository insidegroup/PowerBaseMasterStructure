<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PriceTrackingContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Contact</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Price Tracking Contact</th> 
		        </tr>  
                <tr>
					<td><label for="ContactTypeName">Contact Type</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.ContactType.ContactTypeName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="LastName">Last Name</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.LastName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="FirstName">First Name</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.FirstName)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="Email">Email</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.EmailAddress)%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="PriceTrackingContactUserTypeName">User Type</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.PriceTrackingContactUserType != null ? Model.PriceTrackingContact.PriceTrackingContactUserType.PriceTrackingContactUserTypeName : "")%></td>
					<td></td>
				</tr>
				<tr>
					<td><label for="PriceTrackingDashboardAccessTypeName">Access to Dashboard</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.PriceTrackingDashboardAccessFlag.HasValue? Model.PriceTrackingContact.PriceTrackingDashboardAccessFlag.Value.ToString() : "")%></td>
					<td></td>
				</tr>
                <tr>
					<td><label for="PriceTrackingEmailAlertTypeName">Receive email alerts (recommended None)</label></td>
					<td><%= Html.Encode(Model.PriceTrackingContact.PriceTrackingEmailAlertType != null ? Model.PriceTrackingContact.PriceTrackingEmailAlertType.PriceTrackingEmailAlertTypeName : "")%></td>
					<td></td>
				</tr>
				<tr>
					<td width="40%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="20%" class="row_footer_right"></td>
				</tr>
               <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    </td>
                    <td class="row_footer_blank_right" colspan="2">
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.PriceTrackingContact.PriceTrackingContactId) %>
								<%= Html.HiddenFor(model => model.PriceTrackingContact.VersionNumber) %>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingContact.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted" }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.RouteLink(Model.PriceTrackingContact.PriceTrackingSetupGroup.PriceTrackingSetupGroupName, "Main", new { controller = "PriceTrackingSetupGroup", action = "View", id = Model.PriceTrackingContact.PriceTrackingSetupGroupId }, new { title = Model.PriceTrackingContact.PriceTrackingSetupGroup.PriceTrackingSetupGroupName })%> &gt;
<%=Html.RouteLink("Price Tracking Contacts", "Main", new { controller = "PriceTrackingContact", action = "List", id = Model.PriceTrackingContact.PriceTrackingSetupGroupId }, new { title = "Price Tracking Contacts" })%> &gt;
Delete
</asp:Content>