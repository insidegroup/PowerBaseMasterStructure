<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ThirdPartyUserGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "ThirdPartyUserGDSAccessRight", action = "Edit", id = Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Access Rights</th> 
		        </tr> 
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUserGDSAccessRight.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUserGDSAccessRight.PseudoCityOrOfficeId, Model.PseudoCityOrOfficeIds, "Please Select...", new { } )%> <span id="ThirdPartyUserGDSAccessRight_PseudoCityOrOfficeId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.PseudoCityOrOfficeId)%>
						<label id="lblThirdPartyUserGDSAccessRightPseudoCityOrOfficeIdMsg" class="error">Please choose Home PCC/Office ID</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUserGDSAccessRight.GDSSignOnID, new { maxlength = "10" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.GDSSignOnID)%>
						<label id="lblThirdPartyUserGDSAccessRightMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ThirdPartyUserGDSAccessRight.GDSAccessTypeId, Model.GDSAccessTypes, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.GDSAccessTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUserGDSAccessRight.TAGTIDCertificate, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.TAGTIDCertificate)%> </td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_RequestId">Request ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ThirdPartyUserGDSAccessRight.RequestId, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.RequestId)%> </td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.ThirdPartyUserGDSAccessRight.EnabledDate, new { })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.EnabledDate)%></td>
                </tr>
				<tr>
                    <td><label for="ThirdPartyUserGDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<%= Html.TextAreaFor(model => model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemark, new { maxlength = "1024" })%>
						<br />
						<%= Html.ValidationMessageFor(model => model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemark)%>
						<% if (Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemarks != null && Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemarks.Count > 0) { %>
							<br /><br />
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ThirdPartyUserGDSAccessRightInternalRemark item in Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemarks) { %>
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
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GDS Access Rights" title="Edit GDS Access Rights" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightId) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUserGDSAccessRight.ThirdPartyUserId) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUserGDSAccessRight.VersionNumber) %>

			<% if( ConfigurationManager.AppSettings["TokenAuthenticationUrl"] != null) { %>
				<%= Html.Hidden("TokenAuthenticationUrl", ConfigurationManager.AppSettings["TokenAuthenticationUrl"].ToString()) %>
			<% } %>
			
			<% if( ConfigurationManager.AppSettings["TokenAuthorizationKey"] != null) { %>
				<%= Html.Hidden("TokenAuthorizationKey", ConfigurationManager.AppSettings["TokenAuthorizationKey"].ToString()) %>
			<% } %>

			<% if( ConfigurationManager.AppSettings["BrokerUrl"] != null) { %>
				<%= Html.Hidden("BrokerUrl", ConfigurationManager.AppSettings["BrokerUrl"].ToString()) %>
			<% } %>
			
			<%= Html.HiddenFor(model => model.ThirdPartyUser.ThirdPartyUserId) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.TISUserId) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.ThirdPartyName) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.ThirdPartyUserTypeId) %>
		
			<%= Html.HiddenFor(model => model.ThirdPartyUser.FirstName) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.LastName) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.Email) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.CWTManager) %>
		
			<%= Html.HiddenFor(model => model.ThirdPartyUser.FirstAddressLine) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.SecondAddressLine) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.CityName) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.StateProvinceCode) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.PostalCode) %>
			
			<%= Html.HiddenFor(model => model.ThirdPartyUser.MilitaryAndGovernmentUserFlagNonNullable) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.RoboticUserFlagNonNullable) %>
			<%= Html.HiddenFor(model => model.ThirdPartyUser.IsActiveFlagNonNullable) %>
			
			<% if (Model.Entitlements != null && Model.Entitlements.Count > 0) { %>
				<%=Html.Hidden("ThirdPartyUser_Entitlements", Newtonsoft.Json.JsonConvert.SerializeObject(Model.Entitlements)) %>
			<% } %>

			<% if (Model.ThirdPartyUser.ThirdPartyUserInternalRemarks != null && Model.ThirdPartyUser.ThirdPartyUserInternalRemarks.Count > 0){ %>
				<%=Html.Hidden("ThirdPartyUser_InternalRemarks", Newtonsoft.Json.JsonConvert.SerializeObject(Model.ThirdPartyUser.ThirdPartyUserInternalRemarks)) %>
			<% } %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ThirdPartyUser_TIS.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/ERD/ThirdPartyUserGDSAccessRight.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Third Party Users", "Main", new { controller = "ThirdPartyUser", action = "ListUnDeleted", }, new { title = "Third Party Users" })%> &gt;
<%= Html.Encode(Model.ThirdPartyUser.ThirdPartyName)%>, <%= Html.Encode(Model.ThirdPartyUser.ThirdPartyUserId)%> &gt;
<%=Html.RouteLink("GDS Access Rights", "Main", new { controller = "ThirdPartyUserGDSAccessRight", action = "ListUnDeleted", id = Model.ThirdPartyUser.ThirdPartyUserId }, new { title = "GDS Access Rights" })%> &gt;
Edit
</asp:Content>