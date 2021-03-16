<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ChatFAQResponseItemLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Response Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Response Translations</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
            <tr> 
                <th class="row_header" colspan="3">Delete Response Translation</th> 
            </tr> 
            <tr>
                <td>Chat Message FAQ Description</td>
                <td><%= Html.Encode(Model.ChatFAQResponseItem.ChatMessageFAQName)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Response</td>
                <td><%= Html.Encode(Model.ChatFAQResponseItem.ChatFAQResponseItemDescription)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.LanguageName)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Translation</td>
                <td><%= Html.Encode(Model.ChatFAQResponseItemLanguageDescription)%></td>
                <td></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right" colspan="2">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.ChatFAQResponseItemId) %>
                    <%= Html.HiddenFor(model => model.VersionNumber) %>
                <%}%>
                </td>                
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_chatmessages').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
        $('#search_wrapper').css('height', 'auto');
})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ChatFAQResponseGroupName"].ToString(), "Default", new { controller = "ChatFAQResponseGroup", action = "View", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = ViewData["ChatFAQResponseGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ChatFAQResponseItem", action = "List", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.Encode(ViewData["ChatMessageFAQName"].ToString())%> &gt;
<%=Html.RouteLink("Translations", "Default", new { controller = "ChatFAQResponseItemLanguage", action = "List", id = ViewData["ChatFAQResponseItemId"].ToString() }, new { title = "Translations" })%> &gt;
Delete
</asp:Content>