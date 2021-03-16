<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profiles (Deleted)</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client Profiles (Deleted)</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">UnDelete Client Profile</th> 
		        </tr> 
		        <tr>
                    <td><label for="ClientProfileGroup_GroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.UniqueName)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_GDS">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.ValidPseudoCityOrOfficeId.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_PseudoCityOrOfficeId">PCC/Office ID</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.PseudoCityOrOfficeId)%></td>
                </tr> 
				<tr>
                    <td><label for="ClientProfileGroup_BackOffice">Back Office</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.BackOfficeSystem.BackOfficeSystemDescription)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_ClientProfileName">Profile Name</label></td>
                    <td><%= Html.Encode(Model.ClientProfileGroup.ClientProfileGroupName)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_DeletedDateTime">Deleted Date/Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.DeletedDateTime)%></td>
                </tr>
                <tr>
					<td><label for="ClientProfileGroup_HierarchyType">Hierarchy</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.HierarchyType)%></td>
				</tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/><%= Html.Encode(Model.ClientProfileGroup.HierarchyType)%></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.HierarchyItem)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
							<%= Html.HiddenFor(model => model.ClientProfileGroup.VersionNumber)%>
							<%= Html.HiddenFor(model => model.ClientProfileGroup.ClientProfileGroupId)%>
						<%}%>
                    </td>
                </tr>
           </table>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function () {
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
		});
	</script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Profile Groups", "Main", new { controller = "ClientProfileGroup", action = "ListUnDeleted" }, new { title = "Client Profile Groups" })%> &gt;
<%=ViewData["UniqueName"]%>
</asp:Content>

