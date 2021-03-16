<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemAirLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Policy Message Translations</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Policy Message</th> 
		    </tr> 
            <tr>
                <td>Policy Message Name</td>
                <td><%=Model.PolicyMessageGroupItemName%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Product</td>
                <td><%=Model.ProductName%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Policy Routing</td>
                <td><%=Model.PolicyRoutingName%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.PolicyMessageGroupItemLanguage.Language.LanguageName)%></td>                
                <td></td>
            </tr> 
              <tr>
                <td valign="top">Policy Message</td>
                <td><%=Model.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemTranslation%></td>                
                <td></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>             
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
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
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyMessageGroupItemName, "Default", new { controller = "PolicyMessageGroupItem" + Model.ProductName, action = "View", id = Model.PolicyMessageGroupItemId }, new { title = Model.PolicyMessageGroupItemName })%> &gt;
Policy Message
</asp:Content>