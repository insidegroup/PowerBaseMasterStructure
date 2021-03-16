<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.SystemUserGDSAccessRightVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Rights</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">UnDelete GDS Access Rights</th> 
		        </tr> 
               <tr>
                    <td><label for="GDSAccessRight_GDSCode">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_PseudoCityOrOfficeId">Home PCC/Office ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.PseudoCityOrOfficeId)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_GDSSignOnID">GDS Sign On ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.GDSSignOnID)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_GDSAccessTypeId">GDS Access Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.GDSAccessType != null ? Model.SystemUserGDSAccessRight.GDSAccessType.GDSAccessTypeName : "")%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_TAGTIDCertificate">TA/GTID/Certificate</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.TAGTIDCertificate)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_RequestId">Request ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.RequestId)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_EnabledDate">Enabled Date</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.EnabledDate.HasValue ? Model.SystemUserGDSAccessRight.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_DeletedDateTime">Deleted Date Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.SystemUserGDSAccessRight.DeletedDateTime.HasValue ? Model.SystemUserGDSAccessRight.DeletedDateTime.Value.ToString("G") : "No Deleted Date")%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessRight_InternalRemarks">Internal Remarks</label></td>
                    <td colspan="2">
						<% if (Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks != null && Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks.Count > 0) { %>
							<dl class="InternalRemarks">
								<% foreach (CWTDesktopDatabase.Models.SystemUserGDSAccessRightInternalRemark item in Model.SystemUserGDSAccessRight.SystemUserGDSAccessRightInternalRemarks){ %>
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
							<input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
							<%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.SystemUserGDSAccessRightId) %>
							<%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.SystemUserGuid) %>
							<%= Html.HiddenFor(model => model.SystemUserGDSAccessRight.VersionNumber) %>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_teams').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Teams", "Main", new { controller = "Team", action = "List", }, new { title = "Teams" })%> &gt;
<%=Html.RouteLink("System Users", "Main", new { controller = "SystemUser", action = "List", }, new { title = "System Users" })%> &gt;
<%= Html.Encode(Model.SystemUser.FirstName) %> <%= Html.Encode(Model.SystemUser.LastName) %>, <%= Html.Encode(Model.SystemUser.UserProfileIdentifier) %> &gt;
<%=Html.RouteLink("GDS Access Rights (Deleted)", "Main", new { controller = "SystemUserGDSAccessRight", action = "ListUnDeleted", id = Model.SystemUser.SystemUserGuid }, new { title = "GDS Access Rights" })%> &gt;
UnDelete
</asp:Content>