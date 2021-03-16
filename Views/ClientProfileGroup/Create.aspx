<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client Profiles</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Client Profile</th> 
		        </tr> 
		        <tr>
                    <td><label for="ClientProfileGroup_GDS">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientProfileGroup.GDSCode, Model.GDSs, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientProfileGroup.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_PseudoCityOrOfficeId">PCC/Office ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientProfileGroup.PseudoCityOrOfficeId, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ClientProfileGroup.PseudoCityOrOfficeId)%>
						<label id="lblValidPccGDSMessage"/>
                    </td>
                </tr> 
				<tr>
                    <td><label for="ClientProfileGroup_BackOffice">Back Office</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientProfileGroup.BackOfficeSytemId, Model.BackOffices, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientProfileGroup.BackOfficeSytemId)%></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_ClientProfileName">Profile Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientProfileGroup.ClientProfileGroupName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientProfileGroup.ClientProfileGroupName)%></td>
                </tr>
                <tr>
					<td><label for="ClientProfileGroup_HierarchyType">Hierarchy Type</label></td>
					<td><%= Html.DropDownListFor(model => model.ClientProfileGroup.HierarchyType, Model.HierarchyTypes, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientProfileGroup.HierarchyType)%></td>
				</tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td> <%= Html.TextBoxFor(model => model.ClientProfileGroup.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientProfileGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.ClientProfileGroup.HierarchyCode)%>
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
                    <td class="row_footer_blank_right"><input type="submit" value="Create Client Profile" class="red" title="Create Client Profile"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.ClientProfileGroup.UniqueName)%>
		<% } %>
        </div>
    </div>
    
	<script src="<%=Url.Content("~/Scripts/ERD/ClientProfileGroup.js")%>" type="text/javascript"></script>
</asp:Content>


