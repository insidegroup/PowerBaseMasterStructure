<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceAccountVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Service Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Service Accounts</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
		<% using (Html.BeginForm(null, null, new { id = Model.ServiceAccount.ServiceAccountId }, FormMethod.Post, new { id = "form0", autocomplete = "off" }))
		   {%>
            <%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Service Account</th> 
		        </tr> 
                <tr>
                    <td>Service Account ID</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.ServiceAccountId, new { disabled = "disabled" })%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Service Account Name</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.ServiceAccountName, new { maxlength = "50" } )%> <span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ServiceAccount.ServiceAccountName)%>
						<label id="lblServiceAccountMsg"></label>
                    </td>
                </tr> 
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.LastName, new { maxlength = "50" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.LastName)%></td>
                </tr> 
				<tr>
                    <td>First Name</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.FirstName, new { maxlength = "50" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.FirstName)%></td>
                </tr> 
				<tr>
                    <td>Email</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.Email, new { maxlength = "100" } )%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.Email)%></td>
                </tr> 
				<tr>
                    <td>Is Active?</td>
                    <td><%= Html.CheckBox("ServiceAccount.IsActiveFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.IsActiveFlag)%></td>
                </tr>  
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td colspan="2">
						<% if(ViewData["ComplianceAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("ServiceAccount.CubaBookingAllowanceIndicator", Model.ServiceAccount.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.ServiceAccount.CubaBookingAllowanceIndicator.HasValue && Model.ServiceAccount.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("ServiceAccount.CubaBookingAllowanceIndicator", Model.ServiceAccount.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.ServiceAccount.CubaBookingAllowanceIndicator.HasValue && Model.ServiceAccount.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.ServiceAccount.CubaBookingAllowanceIndicator)%>
						<label id="lblCubaBookingAllowanceIndicatorMsg" for="CubaBookingAllowed" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Military and Government User</td>
                    <td colspan="2">
						<% if(ViewData["GDSGovernmentAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("ServiceAccount.MilitaryAndGovernmentUserFlag", Model.ServiceAccount.MilitaryAndGovernmentUserFlag ?? false, new { @checked = (Model.ServiceAccount.MilitaryAndGovernmentUserFlag.HasValue && Model.ServiceAccount.MilitaryAndGovernmentUserFlag.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("ServiceAccount.MilitaryAndGovernmentUserFlagDisabled", Model.ServiceAccount.MilitaryAndGovernmentUserFlag ?? false, new { @checked = (Model.ServiceAccount.MilitaryAndGovernmentUserFlag.HasValue && Model.ServiceAccount.MilitaryAndGovernmentUserFlag.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
							<%=Html.Hidden("ServiceAccount.MilitaryAndGovernmentUserFlag", Model.ServiceAccount.MilitaryAndGovernmentUserFlag.HasValue && Model.ServiceAccount.MilitaryAndGovernmentUserFlag.Value == true)%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => Model.ServiceAccount.MilitaryAndGovernmentUserFlag)%>
						<label id="lblMilitaryAndGovernmentUserFlagMsg" for="MilitaryAndGovernmentUserFlag" class="error"/>
                    </td>
                </tr>
				<tr>
                    <td>Robotic User</td>
                    <td>True</td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.RoboticUserFlag)%></td>
                </tr>
				
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.CWTManager, new { maxlength = "50" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.CWTManager)%></td>
                </tr> 
                <tr>
                    <td>User Type</td>
                    <td>Internal</td>
					<td><%= Html.ValidationMessageFor(model => model.ServiceAccount.ThirdPartyUserType)%></td>                    
                </tr> 
				<tr>
                    <td>Internal Remarks</td>
                    <td><%= Html.TextAreaFor(model => model.ServiceAccount.InternalRemark, new { maxlength = "1024" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.InternalRemark)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Service Account" title="Create Service Account" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ServiceAccount.RoboticUserFlag) %>
			<%= Html.HiddenFor(model => model.ServiceAccount.ThirdPartyUserType) %>
			<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ServiceAccount.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Account" })%> &gt;
Create
</asp:Content>