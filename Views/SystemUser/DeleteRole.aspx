<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.AdministratorRoleHierarchyLevelTypeSystemUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
        <div id="banner"><div id="banner_text">System Users</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete Role</th> 
		        </tr> 
               
                <tr>
                    <td>User</td>
                    <td><%=Model.SystemUserName%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Role</td>
                    <td colspan="2"><%=Model.AdministratorRoleHierarchyLevelTypeName%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Removal" title="Confirm Removal" class="red"/>
                        <%= Html.HiddenFor(model => model.SystemUserGuid) %>
                        <%= Html.HiddenFor(model => model.AdministratorRoleId) %>
                        <%= Html.HiddenFor(model => model.HierarchyLevelTypeId) %>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
            
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_teams').click();
	  $("tr:odd").addClass("row_odd");
      $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

