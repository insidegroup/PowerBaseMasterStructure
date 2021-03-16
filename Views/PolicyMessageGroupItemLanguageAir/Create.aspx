<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemAirLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Groups</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<link rel="stylesheet" type="text/css" href="/Scripts/MarkdownDeep/mdd_styles.css" />
<script src="<%=Url.Content("~/Scripts/MarkdownDeep/MarkdownDeepLib.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/ERD/PolicyMessageGroupItemLanguage.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message Translations</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create Policy Message</th> 
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
               <td><label for="PolicyMessageGroupItemLanguage_LanguageCode">Language</label></td>
                <td><%= Html.DropDownListFor(model => model.PolicyMessageGroupItemLanguage.LanguageCode, Model.PolicyMessageGroupItemLanguages, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemLanguage.LanguageCode)%></td>
            </tr> 
             <tr>
                <td colspan="2">
                    <div class="mdd_toolbar"></div>
                    <%= Html.TextAreaFor(model => model.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown, new { @class = "mdd_editor", @style = "width:500px;height:180px;border: 1px solid black;" })%>
                    <div class="mdd_resizer"></div>
                </td>
                <td> <%= Html.ValidationMessageFor(model => model.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemTranslationMarkdown)%> </td>
            </tr> 
            <tr>
                <td colspan="2" height="30"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td ><input type="submit" value="Create Policy Message" title="Create Policy Message" class="red"/></td>
            </tr>
            <tr>
                <td valign="top"><br />Preview</td>
                <td></td>
                <td></td>
            </tr> 
             <tr>
                <td colspan="2"><div class="mdd_preview" style="width:500px;height:360px;"></div></td>
                <td></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"></td>
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>
    <%= Html.HiddenFor(model => model.PolicyMessageGroupItemLanguage.PolicyMessageGroupItemId)%>
    <% } %>
    </div>
</div>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyMessageGroupItemName, "Default", new { controller = "PolicyMessageGroupItem" + Model.ProductName, action = "View", id = Model.PolicyMessageGroupItemId }, new { title = Model.PolicyMessageGroupItemName })%> &gt;
Policy Message
</asp:Content>