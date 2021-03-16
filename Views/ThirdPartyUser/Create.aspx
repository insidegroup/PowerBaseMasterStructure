<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ThirdPartyUserVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Third Party Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Third Party Users</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
			<%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Third Party User</th> 
		        </tr> 
                <tr>
                    <td>Third Party User ID</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.TISUserId, new { disabled = "disabled" })%></td>
                    <td><label id="lblTISUserIdMsg" class="error">The request could not be processed. Please try again later</label></td>
                </tr> 
				<tr>
                    <td>Third Party User Name</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.ThirdPartyName, new { maxlength = "50" } )%> <span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.ThirdPartyName)%>
						<label id="lblThirdPartyUserMsg"></label>
                    </td>
                </tr> 
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.LastName, new { maxlength = "50" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.LastName)%></td>
                </tr> 
				<tr>
                    <td>First Name</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.FirstName, new { maxlength = "50" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.FirstName)%></td>
                </tr> 
				<tr>
                    <td>Email</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.Email, new { maxlength = "100" } )%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.Email)%></td>
                </tr> 
				<tr>
                    <td>Is Active?</td>
                    <td><%= Html.CheckBoxFor(model => model.ThirdPartyUser.IsActiveFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.IsActiveFlag)%></td>
                </tr>  
				<tr>
                    <td>Cuba Booking Allowed</td>
                    <td colspan="2">
						<% if(ViewData["ComplianceAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBoxFor(model => model.ThirdPartyUser.CubaBookingAllowanceIndicatorNonNullable)%>
						<% } else { %>
							<%=Html.CheckBox("ThirdPartyUser_CubaBookingAllowanceIndicatorDisabled", Model.ThirdPartyUser.CubaBookingAllowanceIndicator ?? false, new { @checked = (Model.ThirdPartyUser.CubaBookingAllowanceIndicator.HasValue && Model.ThirdPartyUser.CubaBookingAllowanceIndicator.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
							<%=Html.Hidden("ThirdPartyUser_CubaBookingAllowanceIndicator", Model.ThirdPartyUser.CubaBookingAllowanceIndicator.HasValue && Model.ThirdPartyUser.CubaBookingAllowanceIndicator.Value == true)%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.CubaBookingAllowanceIndicator)%>
						<label id="lblCubaBookingAllowanceIndicatorMsg" for="CubaBookingAllowed" class="error"/>
                    </td>
                </tr> 
				<tr>
                    <td>Military and Government User</td>
                    <td colspan="2">
						<% if(ViewData["GDSGovernmentAdministratorAccess"] == "WriteAccess") { %>
							<%=Html.CheckBoxFor(model => model.ThirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable)%>
						<% } else { %>
							<%=Html.CheckBox("ThirdPartyUser_MilitaryAndGovernmentUserFlagDisabled", Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag ?? false, new { @checked = (Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.HasValue && Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.Value == true) ? "checked" : "", disabled = true, autocomplete="off" })%>
							<%=Html.Hidden("ThirdPartyUser_MilitaryAndGovernmentUserFlag", Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.HasValue && Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag.Value == true)%>
						<% }  %>
                    	<%= Html.ValidationMessageFor(model => Model.ThirdPartyUser.MilitaryAndGovernmentUserFlag)%>
						<label id="lblMilitaryAndGovernmentUserFlagMsg" for="MilitaryAndGovernmentUserFlag" class="error"/>
                    </td>
                </tr>
				<tr>
                    <td>Robotic User</td>
                    <td><%= Html.CheckBoxFor(model => model.ThirdPartyUser.RoboticUserFlagNonNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.RoboticUserFlag)%></td>
                </tr>
				<tr>
                    <td>CWT Manager</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.CWTManager, new { maxlength = "100" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.CWTManager)%></td>
                </tr> 
				<tr>
                    <td>User Type</td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUser.ThirdPartyUserTypeId, Model.ThirdPartyUserTypes as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.ThirdPartyUserTypeId)%></td>
                </tr>
				<tr>
                    <td>Client TopUnit</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.ClientTopUnitName, new { disabled = "disabled" })%> <span id="ClientTopUnitGuidError" class="error"> *</span></td>
                    <td>
						<%= Html.HiddenFor(model => model.ThirdPartyUser.ClientTopUnitGuid)%>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.ClientTopUnitGuid)%>
                    </td>
                </tr> 
				<tr>
                    <td>Client SubUnit</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.ClientSubUnitName, new { disabled = "disabled" })%> <span  id="ClientSubUnitGuidError" class="error"> *</span></td>
                    <td>
						<%= Html.HiddenFor(model => model.ThirdPartyUser.ClientSubUnitGuid) %>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.ClientSubUnitGuid)%>
                    </td>
                </tr>
				<tr>
                    <td>Partner Name</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.PartnerName, new { disabled = "disabled" })%> <span class="error" id="PartnerIdError"> *</span></td>
                    <td>
						<span id="lblThirdPartyUser_PartnerName" class="error">Partner Name required</span>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.PartnerId)%>
						<%= Html.HiddenFor(model => model.ThirdPartyUser.PartnerId)%>
                    </td>
                </tr>
				<tr>
                    <td>Vendor Name</td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUser.GDSThirdPartyVendorId, Model.GDSThirdPartyVendors as SelectList, "Please Select...", new { disabled = "disabled" })%> <span class="error" id="GDSThirdPartyVendorIdError"> *</span></td>
                    <td>
                        <span id="lblThirdPartyUser_VendorName" class="error">Vendor Name required</span>
                        <%= Html.ValidationMessageFor(model => model.ThirdPartyUser.GDSThirdPartyVendorId)%>
                    </td>
                </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.FirstAddressLine, new { maxlength = "150" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.FirstAddressLine)%></td>
                </tr>
				<tr>
                    <td>Second Address Line</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.SecondAddressLine, new { maxlength = "150" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.SecondAddressLine)%></td>
                </tr>
				<tr>
                    <td>City</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.CityName, new { maxlength = "40" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.CityName)%></td>
                </tr>
				<tr>
                    <td>Postal Code</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.PostalCode, new { maxlength = "30" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.PostalCode)%></td>
                </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.CountryName)%> <span class="error"> *</span></td>
                    <td>
						<%= Html.HiddenFor(model => model.ThirdPartyUser.CountryCode)%>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.CountryCode)%>
						 <label id="lblCountryNameMsg"/>
                    </td>
                </tr>
				<tr>
                    <td>State/Province</td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUser.StateProvinceCode, Model.StateProvinces as SelectList, "Please Select...", new { disabled = "disabled" })%> <span id="StateProvinceError" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUser.StateProvinceCode)%>
						 <label id="lblStateProvinceCodeMsg"/>
                    </td>
                </tr>
				<tr>
                    <td>Phone Number</td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUser.Phone, new { maxlength = "20" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.Phone)%></td>
                </tr>
				<tr>
                    <td>Internal Remarks</td>
                    <td><%= Html.TextAreaFor(model => model.ThirdPartyUser.InternalRemark, new { maxlength = "1024" } )%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUser.InternalRemark)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Third Party User" title="Create Third Party User" class="red"/></td>
                </tr>
            </table>
			
			<% if( ConfigurationManager.AppSettings["TokenAuthenticationUrl"] != null) { %>
				<%= Html.Hidden("TokenAuthenticationUrl", ConfigurationManager.AppSettings["TokenAuthenticationUrl"].ToString()) %>
			<% } %>
			
			<% if( ConfigurationManager.AppSettings["TokenAuthorizationKey"] != null) { %>
				<%= Html.Hidden("TokenAuthorizationKey", ConfigurationManager.AppSettings["TokenAuthorizationKey"].ToString()) %>
			<% } %>

			<% if( ConfigurationManager.AppSettings["BrokerUrl"] != null) { %>
				<%= Html.Hidden("BrokerUrl", ConfigurationManager.AppSettings["BrokerUrl"].ToString()) %>
			<% } %>

			<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ThirdPartyUser_TIS.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/ERD/ThirdPartyUser.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Third Party Users", "Main", new { controller = "ThirdPartyUser", action = "ListUnDeleted", }, new { title = "Third Party Users" })%> &gt;
Create
</asp:Content>