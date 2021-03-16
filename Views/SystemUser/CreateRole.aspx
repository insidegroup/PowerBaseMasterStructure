<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.AdministratorRoleHierarchyLevelType>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Roles</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Add Role</th> 
		        </tr> 
               
                <tr>
                    <td>User</td>
                    <td><%= ViewData["SystemUserName"]%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Role</td>
                    <td><%= Html.DropDownList("AdministratorRoleHierarchyLevelTypeName", ViewData["Roles"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.AdministratorRoleHierarchyLevelTypeName)%></td>
                </tr> 
              
                <tr>
                    <td width="20%" class="row_footer_left"></td>
                    <td width="60%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right" align="right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank"></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Add Role" title="Add Role" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.SystemUserGuid) %>
            <%= Html.HiddenFor(model => model.AdministratorRoleId) %>
            <%= Html.HiddenFor(model => model.HierarchyLevelTypeId) %>
    <% } %>
        </div>
    </div>

<script src="<%=Url.Content("~/Scripts/ERD/SystemUserRole.js")%>" type="text/javascript"></script>
</asp:Content>
