<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.SystemUserGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "SystemUserGDSAccessRight", action = "Edit", id = Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Access Rights</th> 
		        </tr> 
				<tr>
                    <td><label for="SystemUserGDSAccessRight_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.SystemUserGDSAccessRight.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td><%= Html.DropDownListFor(model => model.SystemUserGDSAccessRight.PseudoCityOrOfficeId, Model.PseudoCityOrOfficeIds, "Please Select...", new { } )%> <span id="SystemUserGDSAccessRight_PseudoCityOrOfficeId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.PseudoCityOrOfficeId)%>
						<label id="lblSystemUserGDSAccessRightPseudoCityOrOfficeIdMsg" class="error">Please choose Home PCC/Office ID</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.SystemUserGDSAccessRight.GDSSignOnID, new { maxlength = "10" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.GDSSignOnID)%>
						<label id="lblSystemUserGDSAccessRightMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.SystemUserGDSAccessRight.GDSAccessTypeId, Model.GDSAccessTypes, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.GDSAccessTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td><%= Html.TextBoxFor(model => model.SystemUserGDSAccessRight.TAGTIDCertificate, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.TAGTIDCertificate)%> </td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_RequestId">Request ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.SystemUserGDSAccessRight.RequestId, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.RequestId)%> </td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.SystemUserGDSAccessRight.EnabledDate, new { })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.EnabledDate)%></td>
                </tr>
				<tr>
                    <td><label for="SystemUserGDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<%= Html.TextAreaFor(model => model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemark, new { maxlength = "1024" })%>
						<br />
						<%= Html.ValidationMessageFor(model => model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemark)%>
						<% if (Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks != null && Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks.Count > 0) { %>
							<br /><br />
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.SystemUserGDSAccessRightInternalRemark item in Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks) { %>
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
            <%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.SystemUserGDSAccessRightId) %>
			<%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.SystemUserGuid) %>
			<%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.VersionNumber) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUserGDSAccessRight.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink("System Users", "Main", new { controller = "SystemUser", action = "List", }, new { title = "System Users" })%> &gt;
<%= Html.Encode(Model.SystemUser.FirstName) %> <%= Html.Encode(Model.SystemUser.LastName) %>, <%= Html.Encode(Model.SystemUser.UserProfileIdentifier) %> &gt;
<%=Html.RouteLink("GDS Access Rights", "Main", new { controller = "SystemUserGDSAccessRight", action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid }, new { title = "GDS Access Rights" })%> &gt;
Edit
</asp:Content>