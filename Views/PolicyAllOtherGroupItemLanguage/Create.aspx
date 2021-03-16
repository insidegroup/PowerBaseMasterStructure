<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAllOtherGroupItemLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Translations</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Translations</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Translation</th> 
		        </tr> 
				<tr>
					<td><label for="PolicyAllOtherGroupItemLanguage_ColumnName">Label</label></td>
					<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.Label)%></td>
                </tr>	
				<tr>
                    <td><label for="PolicyAllOtherGroupItemLanguage_LanguageCode">Language</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAllOtherGroupItemLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAllOtherGroupItemLanguage.LanguageCode)%></td>
                </tr> 
				<tr>
                    <td><label for="PolicyAllOtherGroupItemLanguage_Label">Translation</label></td>
                    <td><%= Html.TextAreaFor(model => model.PolicyAllOtherGroupItemLanguage.Translation)%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAllOtherGroupItemLanguage.Translation)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Translation" class="red ck-save" title="Create Translation"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
			<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupId)%>
			<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupName)%>
			<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyType)%>
			<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyItem)%>
		<% } %>
        </div>
    </div>
   <script type="text/javascript">
   		$(document).ready(function () {
   			$("tr:odd").addClass("row_odd");
   			$("tr:even").addClass("row_even");
   			$('#search_wrapper').css('height', '24px');
   			$('#breadcrumb').css('width', '775px');
   			$('#search').hide();
   			$('#breadcrumb').css('width', 'auto');
			CKEDITOR.replace('PolicyAllOtherGroupItemLanguage_Translation');
   		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy All Other Group Items", "Default", new { controller = "PolicyAllOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy All Other Group Items" })%> &gt;
<%= Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
Translations &gt;
Create
</asp:Content>