<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PriceTrackingContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Contact</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Price Tracking Contact</th> 
		        </tr>
				<tr>
                    <td><label for="ContactTypeId">Contact Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingContact.ContactTypeId, Model.ContactTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.ContactTypeId)%></td>
                </tr>
				<tr>
					<td><label for="LastName">Last Name</label></td>
					<td><%= Html.TextBoxFor(model => model.PriceTrackingContact.LastName, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.LastName)%></td>
				</tr>
				<tr>
					<td><label for="FirstName">First Name</label></td>
					<td><%= Html.TextBoxFor(model => model.PriceTrackingContact.FirstName, new { maxlength = "50" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.FirstName)%></td>
				</tr>
				<tr>
					<td><label for="EmailAddress">Email</label></td>
					<td><%= Html.TextBoxFor(model => model.PriceTrackingContact.EmailAddress, new { maxlength = "100" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.EmailAddress)%></td>
				</tr>
                <tr>
                    <td><label for="PriceTrackingContactUserTypeId">User Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingContact.PriceTrackingContactUserTypeId, Model.PriceTrackingContactUserTypes as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.PriceTrackingContactUserTypeId)%></td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingDashboardAccessFlag">Access to Dashboard</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingContact.PriceTrackingDashboardAccessFlagSelectedValue, Model.PriceTrackingDashboardAccessTypes as SelectList, "Please Select...")%></td>
                    <td>
                        <%= Html.HiddenFor(model => model.PriceTrackingContact.PriceTrackingDashboardAccessFlag)%>
                        <%= Html.ValidationMessageFor(model => model.PriceTrackingContact.PriceTrackingDashboardAccessFlag)%>
                    </td>
                </tr>
                <tr>
                    <td><label for="PriceTrackingEmailAlertTypeId">Receive email alerts (recommended None)</label></td>
                    <td><%= Html.DropDownListFor(model => model.PriceTrackingContact.PriceTrackingEmailAlertTypeId, Model.PriceTrackingEmailAlertTypes as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PriceTrackingContact.PriceTrackingEmailAlertTypeId)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Price Tracking Contact" title="Create Price Tracking Contact" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PriceTrackingContact.PriceTrackingSetupGroupId) %>
		 <% } %>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingContact.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Setups", "Main", new { controller = "PriceTrackingSetupGroup", action = "ListUnDeleted" }, new { title = "Price Tracking Setups" })%> &gt;
<%=Html.RouteLink(Model.PriceTrackingContact.PriceTrackingSetupGroup.PriceTrackingSetupGroupName, "Main", new { controller = "PriceTrackingSetupGroup", action = "View", id = Model.PriceTrackingContact.PriceTrackingSetupGroupId }, new { title = Model.PriceTrackingContact.PriceTrackingSetupGroup.PriceTrackingSetupGroupName })%> &gt;
<%=Html.RouteLink("Price Tracking Contacts", "Main", new { controller = "PriceTrackingContact", action = "List", id = Model.PriceTrackingContact.PriceTrackingSetupGroupId }, new { title = "Price Tracking Contacts" })%> &gt;
Create
</asp:Content>