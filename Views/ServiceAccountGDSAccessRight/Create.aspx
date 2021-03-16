<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceAccountGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create GDS Access Rights</th> 
		        </tr> 
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.ServiceAccountGDSAccessRight.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td><%= Html.DropDownListFor(model => model.ServiceAccountGDSAccessRight.PseudoCityOrOfficeId, Enumerable.Empty<SelectListItem>(), "Please Select...", new { } )%> <span id="ServiceAccountGDSAccessRight_PseudoCityOrOfficeId_Error" class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.PseudoCityOrOfficeId)%>
						<label id="lblServiceAccountGDSAccessRightPseudoCityOrOfficeIdMsg" class="error">Please choose Home PCC/Office ID</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccountGDSAccessRight.GDSSignOnID, new { maxlength = "10" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.GDSSignOnID)%>
						<label id="lblServiceAccountGDSAccessRightMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ServiceAccountGDSAccessRight.GDSAccessTypeId, Enumerable.Empty<SelectListItem>(), "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.GDSAccessTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccountGDSAccessRight.TAGTIDCertificate, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.TAGTIDCertificate)%> </td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_RequestId">Request ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccountGDSAccessRight.RequestId, new { maxlength = "20" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.RequestId)%> </td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.TextBoxFor(model => model.ServiceAccountGDSAccessRight.EnabledDate, new { })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.EnabledDate)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td><%= Html.TextAreaFor(model => model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemark, new { maxlength = "1024" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemark)%> </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create GDS Access Rights" title="Create GDS Access Rights" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ServiceAccountGDSAccessRight.ServiceAccountId) %>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ServiceAccountGDSAccessRight.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts" })%> &gt;
<%= Html.Encode(Model.ServiceAccount.ServiceAccountName)%>, <%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%> &gt;
<%=Html.RouteLink("GDS Access Rights", "Main", new { controller = "ServiceAccountGDSAccessRight", action = "ListUnDeleted", id = Model.ServiceAccount.ServiceAccountId }, new { title = "GDS Access Rights" })%> &gt;
Create
</asp:Content>