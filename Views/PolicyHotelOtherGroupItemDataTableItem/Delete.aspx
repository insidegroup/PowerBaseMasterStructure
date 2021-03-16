<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyHotelOtherGroupItemDataTableItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Data Item</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Table Data Item</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Table Data Item</th> 
		        </tr> 
				<tr>
					<th>Column Name</th>
					<th colspan="2">Table Data</th>
				</tr>	
				<% foreach(var item in Model.PolicyHotelOtherGroupItemDataTableItems.OrderBy(x => x.PolicyOtherGroupHeaderColumnName.DisplayOrder)) { %>
				<tr>
					<td><label for="PolicyHotelOtherGroupItemLanguage_Label"><% =Html.Encode(item.PolicyOtherGroupHeaderColumnName.ColumnName) %></label></td>
					<td colspan="2" class="preserve-whitespace linkify"><%= System.Web.HttpUtility.HtmlDecode(item.TableDataItem)%></td>
				</tr> 
				<% } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Delete Table Data Item" title="Delete Table Data Item" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyHotelOtherGroupItemDataTableRow.PolicyHotelOtherGroupItemDataTableRowId)%>
							<%= Html.HiddenFor(model => model.PolicyHotelOtherGroupItemDataTableRow.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupId)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupName)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyType)%>
							<%= Html.HiddenFor(model => model.PolicyGroup.HierarchyItem)%>
						<%}%>
                    </td>
                </tr>
           </table>
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
   	});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Hotel Other Group Items", "Default", new { controller = "PolicyHotelOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Hotel Other Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.Label, "Default", new { controller = "PolicyHotelOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.Label })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.TableName, "Default", new { controller = "PolicyHotelOtherGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyOtherGroupHeader.TableName })%> &gt;
<%=Html.RouteLink("Table Data", "Default", new { controller = "PolicyHotelOtherGroupItemDataTableItem", action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = Model.PolicyGroup.PolicyGroupId }, new { title = "Table Data" })%> &gt;
Delete
</asp:Content>