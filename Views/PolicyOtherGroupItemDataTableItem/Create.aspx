<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupItemDataTableItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Data Items</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Table Data Items</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Table Data Item</th> 
		        </tr> 
				<tr>
					<th>Column Name</th>
					<th colspan="2">Table Data</th>
				</tr>
				<% int counter = 1;
				foreach(var item in Model.PolicyOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.DisplayOrder)) { %>
				<tr>
                    <td><label for="PolicyOtherGroupItemLanguage_Label"><% =Html.Encode(item.PolicyOtherGroupHeaderColumnName.ColumnName) %></label></td>
                    <td><%= Html.TextArea("PolicyOtherGroupHeaderColumnNameId_" + item.PolicyOtherGroupHeaderColumnNameId.ToString(), null, new { 
							id="PolicyOtherGroupHeaderColumnNameId_" + item.PolicyOtherGroupHeaderColumnNameId, @class="PolicyOtherGroupItemDataTableItem" })%>
                    </td>
					<td>
						<%= Html.ValidationMessage("PolicyOtherGroupHeaderColumnNameId_" + item.PolicyOtherGroupHeaderColumnNameId.ToString())%>
						<% if(counter == 1) { %>
							<span class="error" id="PolicyOtherGroupItemDataTableItemErrorMessage"></span>
						<% } %>
					</td>
                </tr> 
				<% counter++; } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Table Data Item" class="red ck-save" title="Create Table Data Item"/></td>
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
    
	<script src="<%=Url.Content("~/Scripts/ERD/PolicyOtherGroupItemDataTableItem.js")%>" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			<% foreach(var item in Model.PolicyOtherGroupItemDataTableItems) { %>
				CKEDITOR.replace('<%="PolicyOtherGroupHeaderColumnNameId_" + item.PolicyOtherGroupHeaderColumnNameId.ToString()%>');
			<% } %>
		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Other Group Items", "Default", new { controller = "PolicyOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Other Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.Label, "Default", new { controller = "PolicyOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.Label })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.TableName, "Default", new { controller = "PolicyOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.TableName })%> &gt;
<%=Html.RouteLink("Table Data", "Default", new { controller = "PolicyOtherGroupItemDataTableItem", action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { title = "Table Data" })%> &gt;
Create
</asp:Content>