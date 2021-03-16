<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Optional Field Groups</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "OptionalFieldGroup", action = "Edit", id = Model.OptionalFieldGroup.OptionalFieldGroupId }, FormMethod.Post, new { id = "form0" }))
          {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Optional Field Group</th> 
		        </tr> 
		        <tr>
                    <td><label for="OptionalFieldGroup_OptionalFieldGroupName">Group Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.OptionalFieldGroup.OptionalFieldGroupName, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.OptionalFieldGroupName)%><label id="lblOptionalFieldGroupNameMsg"/></td>
                </tr>
                <tr>
                    <td><label for="OptionalFieldGroup_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.OptionalFieldGroup.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="OptionalFieldGroup_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.OptionalFieldGroup.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="OptionalFieldGroup_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.OptionalFieldGroup.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.ExpiryDate)%></td>
                </tr> 
				<tr>
					<td><label for="OptionalFieldGroup_HierarchyType">Hierarchy Type</label></td>
					<td><%= Html.DropDownListFor(model => model.OptionalFieldGroup.HierarchyType, Model.HierarchyTypes, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.HierarchyType)%></td>
					</tr>           
                <tr>
                    <td><label for="OptionalFieldGroup_HierarchyItem" id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldGroup.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.OptionalFieldGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.OptionalFieldGroup.HierarchyCode)%>
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
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Optional Field Group" class="red" title="Edit Optional Field Group"/></td>
                </tr>
           </table>
            <%= Html.HiddenFor(model => model.OptionalFieldGroup.VersionNumber)%>
            <%= Html.HiddenFor(model => model.OptionalFieldGroup.OptionalFieldGroupId)%>
            <%= Html.HiddenFor(model => model.OptionalFieldGroup.HierarchyType)%>
            <%= Html.HiddenFor(model => model.OptionalFieldGroup.HierarchyCode)%>
            <%= Html.HiddenFor(model => model.OptionalFieldGroup.HierarchyItem)%>
    <% } %>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#breadcrumb').css('width', 'auto');
	})
 </script>

<script src="<%=Url.Content("~/Scripts/ERD/OptionalFieldGroup.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.OptionalFieldGroup.OptionalFieldGroupName + " Groups", "Main", new { controller = "OptionalFieldGroup", action = "ListUnDeleted" }, new { title = Model.OptionalFieldGroup.OptionalFieldGroupName + " Groups" })%> &gt;
<%=Model.OptionalFieldGroup.OptionalFieldGroupName%>
</asp:Content>

