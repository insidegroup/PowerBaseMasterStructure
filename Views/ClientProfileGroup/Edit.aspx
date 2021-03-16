<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profiles</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client Profiles</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "ClientProfileGroup", action = "Edit", id = Model.ClientProfileGroup.ClientProfileGroupId }, FormMethod.Post, new { id = "form0" }))
          {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Client Profile</th> 
		        </tr>
				<tr>
                    <td><label for="ClientProfileGroup_GroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ClientProfileGroup.UniqueName)%></td>
                </tr>
		        <tr>
                    <td><label for="ClientProfileGroup_GDS">GDS</label></td>
                    <td><%= Html.Encode(Model.ClientProfileGroup.ValidPseudoCityOrOfficeId.GDS.GDSName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_PseudoCityOrOfficeId">PCC/Office ID</label></td>
                    <td><%= Html.Encode(Model.ClientProfileGroup.PseudoCityOrOfficeId)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td><label for="ClientProfileGroup_BackOffice">Back-Office</label></td>
                    <td><%= Html.Encode(Model.ClientProfileGroup.BackOfficeSystem.BackOfficeSystemDescription)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="ClientProfileGroup_ClientProfileName">Profile Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientProfileGroup.ClientProfileGroupName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.ClientProfileGroup.ClientProfileGroupName)%>
						<label id="lblHierarchyItemMsg"/>
                    </td>
                </tr>
                <tr>
					<td><label for="ClientProfileGroup_HierarchyType">Hierarchy</label></td>
					<td><%= Html.Encode(Model.ClientProfileGroup.HierarchyType)%></td>
                    <td></td>
				</tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/><%= Html.Encode(Model.ClientProfileGroup.HierarchyType)%></td>
                    <td><%= Html.Encode(Model.ClientProfileGroup.HierarchyItem)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Client Profile" class="red" title="Edit Client Profile"/></td>
                </tr>
           </table>
            <%= Html.HiddenFor(model => model.ClientProfileGroup.VersionNumber)%>
            <%= Html.HiddenFor(model => model.ClientProfileGroup.ClientProfileGroupId)%>
            <%= Html.HiddenFor(model => model.ClientProfileGroup.ClientProfileGroupName)%>
			<%= Html.HiddenFor(model => model.ClientProfileGroup.UniqueName)%>
			<%= Html.HiddenFor(model => model.ClientProfileGroup.HierarchyType)%>
            <%= Html.HiddenFor(model => model.ClientProfileGroup.HierarchyCode)%>
            <%= Html.HiddenFor(model => model.ClientProfileGroup.HierarchyItem)%>
			<%= Html.HiddenFor(model => model.ClientProfileGroup.PseudoCityOrOfficeId)%>
			<%= Html.HiddenFor(model => model.ClientProfileGroup.GDSCode, new { id = "ClientProfileGroup_GDSCode" })%>
			<% } %>
        </div>
    </div>
    
<script src="<%=Url.Content("~/Scripts/ERD/ClientProfileGroup.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Profile Groups", "Main", new { controller = "ClientProfileGroup", action = "ListUnDeleted" }, new { title = "Client Profile Groups" })%> &gt;
<%=ViewData["UniqueName"]%>
</asp:Content>

