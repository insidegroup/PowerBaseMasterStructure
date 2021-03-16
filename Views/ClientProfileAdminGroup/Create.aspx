<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileAdminGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Administration</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
    
    <script src="<%=Url.Content("~/Scripts/ERD/ClientProfileAdminGroup.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client Profile Administration</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Profile Administration Group</th> 
		        </tr> 
                 <tr>
                    <td>Group Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.ClientProfileAdminGroup.ClientProfileGroupName)%><label id="lblClientProfileAdminGroupNameMsg"/></td>
                </tr>  
		        <tr>
                    <td><label for="ClientProfileAdminGroup_GDS">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientProfileAdminGroup.GDSCode, Model.GDSs, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientProfileAdminGroup.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileAdminGroup_BackOffice">Back Office</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientProfileAdminGroup.BackOfficeSytemId, Model.BackOffices, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientProfileAdminGroup.BackOfficeSytemId)%></td>
                </tr>
                <tr>
					<td><label for="ClientProfileAdminGroup_HierarchyType">Hierarchy</label></td>
					<td><%= Html.DropDownListFor(model => model.ClientProfileAdminGroup.HierarchyType, Model.HierarchyTypes, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientProfileAdminGroup.HierarchyType)%></td>
				</tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td> <%= Html.TextBoxFor(model => model.ClientProfileAdminGroup.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientProfileAdminGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.ClientProfileAdminGroup.HierarchyCode)%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Client Profile Administration Group" class="red" title="Create Client Profile Administration Group"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ClientProfileAdminGroup.ClientProfileAdminGroupId)%>
		<% } %>
        </div>
    </div>
</asp:Content>


