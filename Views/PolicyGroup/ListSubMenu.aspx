<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyGroupSubMenuVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Group Items</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="2" border="0" width="100%" summary=""> 
            <tr> 
                <th class="row_header" width="100%"><strong>Policy Group Items</strong></th> 
            </tr> 
            <% foreach (CWTDesktopDatabase.Models.PolicyType item in Model.PolicyTypes) { 
				if(!string.IsNullOrEmpty(item.NavigationLinkPolicyTypeName) && !string.IsNullOrEmpty(item.PolicyTypeTableName)) {%>

				<tr> 
					<td><%= Html.RouteLink(item.NavigationLinkPolicyTypeName, "List", new { action = "List", controller = item.PolicyTypeTableName, id = Model.PolicyGroup.PolicyGroupId }, new { title = "View " + item.NavigationLinkPolicyTypeName })%> (<%=item.ItemCount %>)</td>
				</tr>  
			
				<% } 
			}%> 
        </table> 
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 80), "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
Items
</asp:Content>