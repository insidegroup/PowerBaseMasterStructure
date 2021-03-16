<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUser</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm(null, null, new { id = Model.SystemUserGuid }, FormMethod.Post, new { autocomplete="off" })) {%>
            <%= Html.AntiForgeryToken() %>
			<%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit SystemUser</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.LastName)%>, <%= Html.Encode(Model.FirstName)%> <%= Html.Encode(Model.MiddleName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Is Active?</td>
                    <td><%= Html.Encode(Model.IsActiveFlag)%></td>
                    <td></td>
                </tr>  
                 
                <tr>
                    <td>Login Identifier</td>
                    <td><%= Html.Encode(Model.SystemUserLoginIdentifier)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Profile ID</td>
                    <td><%= Html.Encode(Model.UserProfileIdentifier)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Location</td>
                    <td><%= Html.TextBoxFor(model => model.LocationName, new { autocomplete = "off" })%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.LocationName)%>
						<%= Html.Hidden("LocationId")%>
						<label id="lblLocationNameMsg"/>
                    </td>
                </tr> 
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td colspan="2">
						<% if(ViewData["ComplianceAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("CubaBookingAllowanceIndicator", Model.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.CubaBookingAllowanceIndicator.HasValue && Model.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("CubaBookingAllowanceIndicator", Model.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.CubaBookingAllowanceIndicator.HasValue && Model.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.CubaBookingAllowanceIndicator)%>
						<label id="lblCubaBookingAllowanceIndicatorMsg" for="CubaBookingAllowed" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Military and Government User</td>
                    <td colspan="2">
						<% if(ViewData["GDSGovernmentAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("MilitaryAndGovernmentUserFlag", Model.MilitaryAndGovernmentUserFlag ?? false, new { @checked = (Model.MilitaryAndGovernmentUserFlag.HasValue && Model.MilitaryAndGovernmentUserFlag.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("MilitaryAndGovernmentUserFlagDisabled", Model.MilitaryAndGovernmentUserFlag ?? false, new { @checked = (Model.MilitaryAndGovernmentUserFlag.HasValue && Model.MilitaryAndGovernmentUserFlag.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
							<%=Html.Hidden("MilitaryAndGovernmentUserFlag", Model.MilitaryAndGovernmentUserFlag.HasValue && Model.MilitaryAndGovernmentUserFlag.Value == true)%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.MilitaryAndGovernmentUserFlag)%>
						<label id="lblMilitaryAndGovernmentUserFlagMsg" for="MilitaryAndGovernmentUserFlag" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Default Profile?</td>
                    <td colspan="2">
						<%=Html.CheckBox("DefaultProfileIdentifier", Model.DefaultProfileIdentifier ?? false, new { @checked = (Model.DefaultProfileIdentifier.HasValue && Model.DefaultProfileIdentifier.Value == true) ? "checked" : "", autocomplete="off" })%>
                    	<%= Html.ValidationMessageFor(model => model.DefaultProfileIdentifier)%>
						<label id="lblDefaultProfileIdentifierMsg" for="DefaultProfile" class="error"/>
                    </td>
                </tr
                <tr>
                    <td>Restricted?</td>
                    <td colspan="2">
						<% if(ViewData["RestrictedSystemUserAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("RestrictedFlag", Model.RestrictedFlag ?? false, new { @checked = (Model.RestrictedFlag.HasValue && Model.RestrictedFlag.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("RestrictedFlagDisabled", Model.RestrictedFlag ?? false, new { @checked = (Model.RestrictedFlag.HasValue && Model.RestrictedFlag.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
                            <%=Html.Hidden("RestrictedFlag", (Model.RestrictedFlag.HasValue && Model.RestrictedFlag.Value == true) ? true : false) %>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.RestrictedFlag)%>
						<label id="lblRestrictedFlagMsg" for="RestrictedFlag" class="error"/>
                    </td>
                </tr>
                <tr>
                    <td>Online User?</td>
                    <td colspan="2">
						<% if(ViewData["OnlineUserAccess"] == "WriteAccess") { %>
							<%=Html.CheckBox("OnlineUserFlag", Model.OnlineUserFlag ?? false, new { @checked = (Model.OnlineUserFlag.HasValue && Model.OnlineUserFlag.Value == true) ? "checked" : "", autocomplete="off" })%>
						<% } else { %>
							<%=Html.CheckBox("OnlineUserFlagDisabled", Model.OnlineUserFlag ?? false, new { @checked = (Model.OnlineUserFlag.HasValue && Model.OnlineUserFlag.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
                            <%=Html.Hidden("OnlineUserFlag", (Model.OnlineUserFlag.HasValue && Model.OnlineUserFlag.Value == true) ? true : false) %>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.OnlineUserFlag)%>
						<label id="lblOnlineUserFlagMsg" for="OnlineUserFlag" class="error"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit SystemUser" title="SystemUser" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.VersionNumber) %>
			<%= Html.HiddenFor(model => model.SystemUserGuid, new { id = "SystemUserGuid"})%>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUser.js")%>" type="text/javascript"></script>
</asp:Content>


