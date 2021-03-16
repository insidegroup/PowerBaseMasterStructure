<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirOtherGroupItemLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Translations</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Translations</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
					 <th class="row_header" colspan="3">Edit Translation</th> 
					</tr> 
					<tr>
						<td><label for="PolicyAirOtherGroupItemLanguage_ColumnName">Label</label></td>
						<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.Label)%></td>
					</tr>	
					<tr>
						<td><label for="PolicyAirOtherGroupItemLanguage_LanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.PolicyAirOtherGroupItemLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyAirOtherGroupItemLanguage.LanguageCode)%></td>
					</tr> 
					<tr>
						<td><label for="PolicyAirOtherGroupItemLanguage_Label">Translation</label></td>
						<td><%= Html.TextArea("PolicyAirOtherGroupItemLanguage.Translation", System.Web.HttpUtility.HtmlDecode(Model.PolicyAirOtherGroupItemLanguage.Translation))%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyAirOtherGroupItemLanguage.Translation)%></td>
					</tr> 
     				<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left" colspan="2">
							<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
							<a href="javascript:window.print();" class="red" title="Print">Print</a>
						</td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Edit Translation" title="Edit Translation" class="red ck-save"/>
							<%= Html.HiddenFor(model => model.PolicyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId)%>
							<%= Html.HiddenFor(model => model.PolicyAirOtherGroupItemLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupId)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupName)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyType)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyItem)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
   <script type="text/javascript">
   		$(document).ready(function () {
   			$("tr:odd").addClass("row_odd");
   			$("tr:even").addClass("row_even");
   			$('#search').hide();
   			$('#search_wrapper').css('height', '24px');
   			$('#breadcrumb').css('width', '775px');
   			$('#breadcrumb').css('width', 'auto');
   			CKEDITOR.replace('PolicyAirOtherGroupItemLanguage_Translation');
   		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Other Group Items", "Default", new { controller = "PolicyAirOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Other Group Items" })%> &gt;
<%= Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
Translations &gt;
Edit
</asp:Content>