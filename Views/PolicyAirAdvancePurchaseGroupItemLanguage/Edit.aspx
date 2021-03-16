<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemLanguageVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Air Advance Purchase Advice</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Air Advance Purchase Advice</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
					 <th class="row_header" colspan="3">Edit Air Advance Purchase Advice</th> 
					</tr> 
					<tr>
						<td><label for="PolicyAirParameterGroupItemLanguage_LanguageCode">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.PolicyAirParameterGroupItemLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItemLanguage.LanguageCode)%></td>
					</tr> 
					<tr>
						<td><label for="PolicyAirParameterGroupItemLanguage_Label">Translation</label></td>
						<td><%= Html.TextAreaFor(model => Model.PolicyAirParameterGroupItemLanguage.Translation, new { maxlength = "255" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyAirParameterGroupItemLanguage.Translation)%></td>
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
							<input type="submit" value="Edit Air Advance Purchase Advice" title="Edit Air Advance Purchase Advice" class="red ck-save"/>
							<%= Html.HiddenFor(model => model.PolicyAirParameterGroupItemLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId)%>
							<%= Html.HiddenFor(model => model.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId)%>
							<%= Html.HiddenFor(model => model.PolicyAirParameterGroupItem.PolicyGroupName)%>
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
   			$('#search_wrapper').css('height', '32px');
   			$('#breadcrumb').css('width', '775px');
   			$('#breadcrumb').css('width', 'auto');
   		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Time Window Group Items", "Default", new { controller = "PolicyAirTimeWindowGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Time Window Group Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)) %> &gt;
Air Advance Purchase Advice &gt;
<%= Html.Encode(Model.PolicyAirParameterGroupItem.PolicyRouting.FromCode) %> to <%= Html.Encode(Model.PolicyAirParameterGroupItem.PolicyRouting.ToCode) %>
</asp:Content>