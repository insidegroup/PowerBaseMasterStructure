<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirParameterGroupItemLanguageVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Air Advance Purchase Advice</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Air Advance Purchase Advice</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Air Advance Purchase Advice</th> 
		        </tr> 
				<tr>
                    <td><label for="PolicyAirParameterGroupItemColumnNameLanguages_Language">Language</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyAirParameterGroupItemLanguage.Language.LanguageName) %></td>
                </tr>
				<tr>
                    <td><label for="PolicyAirParameterGroupItemColumnNameLanguage_Translation">Air Advance Purchase Advice</label></td>
                    <td colspan="2" class="linkify"><%= System.Web.HttpUtility.HtmlDecode(Model.PolicyAirParameterGroupItemLanguage.Translation)%></td>
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
                    <td class="row_footer_blank_right">&nbsp;</td>
                </tr>
           </table>
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
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnViewd", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroup.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroup.PolicyGroupId }, new { title = Model.PolicyGroup.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "PolicyGroup", action = "ListSubMenu", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Policy Air Time Window Group Items", "Default", new { controller = "PolicyAirTimeWindowGroupItem", action = "List", id = Model.PolicyGroup.PolicyGroupId }, new { title = "Policy Air Time Window Group Items" })%> &gt;
<%= Html.Encode(CWTStringHelpers.TrimString(Model.PolicyGroup.PolicyGroupName, 60)) %> &gt;
Air Advance Purchase Advice &gt;
<%= Html.Encode(Model.PolicyAirParameterGroupItem.PolicyRouting.FromCode) %> to <%= Html.Encode(Model.PolicyAirParameterGroupItem.PolicyRouting.ToCode) %>
</asp:Content>