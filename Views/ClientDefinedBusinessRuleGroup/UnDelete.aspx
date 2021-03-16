<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Business Rules Group</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Business Rules Group</div></div>
     <div id="content">

        <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		    <tr> 
			    <th class="row_header" colspan="2">UnDelete Business Rules Group</th> 
		    </tr> 
		    <tr>
                <td><label for="ClientDefinedRuleGroup_ClientDefinedRuleGroupName">Group Name</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName) %></td>
            </tr>
			<tr>
                <td><label for="ClientDefinedRuleGroup_ClientDefinedRuleBusinessEntityCategory">Category</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.Category)%></td>
            </tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_EnabledFlag">Enabled?</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.EnabledFlag)%></td>
			</tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_EnabledDate">Enabled Date</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.EnabledDate.HasValue ? Model.ClientDefinedRuleGroup.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
            </tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_ExpiryDate">Expiration Date</label> </td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.ExpiryDate.HasValue ? Model.ClientDefinedRuleGroup.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
            </tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_TripTypeID">Trip Type</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.TripType != null ? Model.ClientDefinedRuleGroup.TripType.TripTypeDescription : "")%></td>
            </tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_EnabledFlag">Deleted?</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.DeletedFlag)%></td>
			</tr>
			<tr>
				<td><label for="ClientDefinedRuleGroup_EnabledFlag">Deleted Date/Time</label></td>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.DeletedDateTime)%></td>
			</tr>
            <tr>
                <td class="row_footer_left"></td>
                <td class="row_footer_right"></td>
            </tr>
			<tr>
				<td class="row_footer_blank_left" width="50%">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>
				</td>                    
				<td class="row_footer_blank_right" width="50%">
					<% using (Html.BeginForm()) { %>
						<%= Html.AntiForgeryToken() %>
						<input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
						<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.VersionNumber) %>
						<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId) %>
					<%}%>
				</td>
			</tr>
		</table>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#breadcrumb').css('width', 'auto');
		$('.full-width #search_wrapper').css('height', '22px');

		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");

	})
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Business Rules Group", "ListUndeleted", "ClientDefinedBusinessRuleGroup")%> &gt;
<%= Html.RouteLink(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName, "Default", new { action = "View", id = Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId }, new { title = "View" })%> &gt;
Item
</asp:Content>
