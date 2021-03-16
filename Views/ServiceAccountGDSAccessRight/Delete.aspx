<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServiceAccountGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GDS Access Rights</th> 
		        </tr> 
               <tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSCode">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.PseudoCityOrOfficeId)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.GDSSignOnID)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.GDSAccessType.GDSAccessTypeName)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.TAGTIDCertificate)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_RequestId">Request ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.RequestId)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.EnabledDate.HasValue ? Model.ServiceAccountGDSAccessRight.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_DeletedDateTime">Deleted Date Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ServiceAccountGDSAccessRight.DeletedDateTime.HasValue ? Model.ServiceAccountGDSAccessRight.DeletedDateTime.Value.ToString("G") : "No Deleted Date")%></td>
                </tr>
				<tr>
                    <td><label for="ServiceAccountGDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<% if (Model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemarks != null && Model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemarks.Count > 0) { %>
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.ServiceAccountGDSAccessRightInternalRemark item in Model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightInternalRemarks) { %>
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
                    <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
                    </td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.ServiceAccountGDSAccessRight.ServiceAccountGDSAccessRightId) %>
							<%= Html.HiddenFor(model => model.ServiceAccountGDSAccessRight.ServiceAccountId) %>
							<%= Html.HiddenFor(model => model.ServiceAccountGDSAccessRight.VersionNumber) %>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Service Accounts", "Main", new { controller = "ServiceAccount", action = "ListUnDeleted", }, new { title = "Service Accounts" })%> &gt;
<%= Html.Encode(Model.ServiceAccount.ServiceAccountName)%>, <%= Html.Encode(Model.ServiceAccount.ServiceAccountId)%> &gt;
<%=Html.RouteLink("GDS Access Rights", "Main", new { controller = "ServiceAccountGDSAccessRight", action = "ListUnDeleted", id = Model.ServiceAccount.ServiceAccountId }, new { title = "GDS Access Rights" })%> &gt;
Delete
</asp:Content>