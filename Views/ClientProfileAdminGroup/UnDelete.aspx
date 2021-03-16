<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileAdminGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - UnDelete Client Profile Administration Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">UnDelete Client Profile Administration Groups</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Client Profile Administration Group</th> 
		        </tr> 
		        <tr>
                    <td><label for="ClientProfileAdminGroup_GroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.ClientProfileGroupName)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_GDS">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_BackOffice">Back Office</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.BackOfficeSystem.BackOfficeSystemDescription)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_InheritFromParentFlag">Inherit From Parent</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.InheritFromParentFlag)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.DeletedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_DeletedDateTime">Deleted Date/Time</label></td>
                    <td colspan="2"><%= (Model.ClientProfileAdminGroup.DeletedDateTime != null) ? Html.Encode(Model.ClientProfileAdminGroup.DeletedDateTime) : "No Deleted Date"%></td>
                </tr>
                <tr>
                    <td><label for="ClientProfileAdminGroup_Hierarchy Type">Hierarchy Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.HierarchyType)%></td>
                </tr>
                <tr>
					<td><label for="ClientProfileAdminGroup_HierarchyItem">Hierarchy Value</label></td>
					<td colspan="2"><%= Html.Encode(Model.ClientProfileAdminGroup.HierarchyItem)%></td>
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
							<input type="submit" value="Confirm UnDelete" title="Confirm UNDelete" class="red"/>
							<%= Html.HiddenFor(model => model.ClientProfileAdminGroup.VersionNumber)%>
							<%= Html.HiddenFor(model => model.ClientProfileAdminGroup.ClientProfileAdminGroupId)%>
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
<%=Html.RouteLink("Client Profile Admininistration Groups", "Main", new { controller = "ClientProfileAdminGroup", action = "ListDeleted" }, new { title = "Client Profile Admininistration Groups" })%> &gt;
<%=Model.ClientProfileAdminGroup.ClientProfileGroupName%>
</asp:Content>
