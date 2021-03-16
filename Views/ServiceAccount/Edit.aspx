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
		<%using (Html.BeginRouteForm("Default", new { controller = "ServiceAccount", action = "Edit", id = Model.ServiceAccount.ServiceAccountId }, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit ServiceAccount</th> 
		        </tr> 
                <tr>
                    <td>Service Account ID</td>
                    <td><%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%></td>
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
                    <td><%= Html.CheckBox("ServiceAccount.IsActiveFlag", Model.ServiceAccount.IsActiveFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.IsActiveFlag)%></td>
                </tr>  
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td colspan="2">
						<% if(ViewData["ComplianceAdministratorAccess"].ToString() == "WriteAccess") { %>
							<%=Html.CheckBoxFor(model => model.ServiceAccount.CubaBookingAllowanceIndicatorNonNullable)%>
						<% } else { %>
							<%=Html.CheckBox("ServiceAccount_CubaBookingAllowanceIndicatorNonNullableDisabled", Model.ServiceAccount.CubaBookingAllowanceIndicatorNonNullable, new { @disabled="disabled" } ) %>
                            <%=Html.Hidden("ServiceAccount_CubaBookingAllowanceIndicatorNonNullable", Model.ServiceAccount.CubaBookingAllowanceIndicatorNonNullable) %>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.ServiceAccount.CubaBookingAllowanceIndicator)%>
						<label id="lblCubaBookingAllowanceIndicatorMsg" for="CubaBookingAllowed" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Military and Government User</td>
                    <td colspan="2">
						<% if(ViewData["GDSGovernmentAdministratorAccess"].ToString() == "WriteAccess") { %>
							<%=Html.CheckBoxFor(model => model.ServiceAccount.MilitaryAndGovernmentUserFlagNonNullable)%>
						<% } else { %>
							<%=Html.CheckBox("ServiceAccount_MilitaryAndGovernmentUserFlagNonNullableDisabled", Model.ServiceAccount.MilitaryAndGovernmentUserFlagNonNullable, new { @disabled="disabled" } ) %>
                            <%=Html.Hidden("ServiceAccount_MilitaryAndGovernmentUserFlagNonNullable", Model.ServiceAccount.MilitaryAndGovernmentUserFlagNonNullable) %>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => Model.ServiceAccount.MilitaryAndGovernmentUserFlag)%>
						<label id="lblMilitaryAndGovernmentUserFlagMsg" for="MilitaryAndGovernmentUserFlag" class="error"/>
                    </td>
                </tr>
                <tr>
                    <td>Robotic User</td>
                    <td>True</td>
                    <td></td>
                </tr>
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccount.CWTManager, new { maxlength = "100" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccount.CWTManager)%></td>
                </tr> 
				<tr>
                    <td>User Type</td>
                    <td>Internal</td>
					<td></td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUser_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<%= Html.TextAreaFor(model => model.ServiceAccount.InternalRemark, new { maxlength = "1024" })%>
						<br />
						<%= Html.ValidationMessageFor(model => model.ServiceAccount.InternalRemark)%>
						<% if (Model.ServiceAccount.ServiceAccountInternalRemarks != null && Model.ServiceAccount.ServiceAccountInternalRemarks.Count > 0) { %>
							<br /><br />
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ServiceAccountInternalRemark item in Model.ServiceAccount.ServiceAccountInternalRemarks) { %>
									<dt><%= Html.Encode(item.CreationTimestamp.Value.ToString("yyyy-MM-dd")) %></dt>
									<dd><%= Html.Encode(item.InternalRemark) %></dd>
								<% } %>
							</dl>
						<% } %>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Service Account" title="Edit Service Account" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ServiceAccount.ServiceAccountId)%>			
			<%= Html.HiddenFor(model => model.ServiceAccount.RoboticUserFlag) %>
			<%= Html.HiddenFor(model => model.ServiceAccount.ThirdPartyUserType) %>
			<%= Html.HiddenFor(model => model.ServiceAccount.DeletedFlag) %>
			<%= Html.HiddenFor(model => model.ServiceAccount.VersionNumber) %>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ServiceAccount.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<% if(Model.ServiceAccount.DeletedFlag == true) { %>
	<%=Html.RouteLink("Service Accounts (Deleted)", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts (Deleted)" })%> &gt;
<% } else { %>
	<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts" })%> &gt;
<% } %>>
<%= Html.Encode(Model.ServiceAccount.ServiceAccountName)%>, <%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%> &gt;
Edit
</asp:Content>