<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ThirdPartyUserGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View GDS Access Rights</th> 
		        </tr> 
                <tr>
                    <td><label for="GDSAccessRight_GDSCode">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.PseudoCityOrOfficeId)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.GDSSignOnID)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.GDSAccessType.GDSAccessTypeName)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.TAGTIDCertificate)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_RequestId">Request ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.RequestId)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.EnabledDate.HasValue ? Model.ThirdPartyUserGDSAccessRight.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_DeletedDateTime">Deleted Date Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ThirdPartyUserGDSAccessRight.DeletedDateTime.HasValue ? Model.ThirdPartyUserGDSAccessRight.DeletedDateTime.Value.ToShortDateString() : "No Deleted Date")%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<% if (Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemarks != null && Model.ThirdPartyUserGDSAccessRight.ThirdPartyUserGDSAccessRightInternalRemarks.Count > 0) { %>
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
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a> 
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
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
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("Third Party Users", "Main", new { controller = "ThirdPartyUser", action = "ListUnDeleted", }, new { title = "Third Party Users" })%> &gt;
<%= Html.Encode(Model.ThirdPartyUser.ThirdPartyName)%>, <%= Html.Encode(Model.ThirdPartyUser.TISUserId)%> &gt;
<%=Html.RouteLink("GDS Access Rights", "Main", new { controller = "ThirdPartyUserGDSAccessRight", action = "ListUnDeleted", id = Model.ThirdPartyUser.ThirdPartyUserId }, new { title = "GDS Access Rights" })%> &gt;
View
</asp:Content>