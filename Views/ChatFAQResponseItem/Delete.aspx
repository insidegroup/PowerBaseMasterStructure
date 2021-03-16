<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ChatFAQResponseItem>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Chat FAQ Response Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat FAQ Response Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Chat FAQ Response Item</th> 
		        </tr> 
                 <tr>
                    <td>Chat Message FAQ Description</td>
                    <td><%= Html.Encode(Model.ChatMessageFAQName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Response</td>
                    <td class="wrap-text"><%= Html.Encode(Model.ChatFAQResponseItemDescription)%></td>
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
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ChatFAQResponseGroupName"].ToString(), "Default", new { controller = "ChatFAQResponseGroup", action = "View", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = ViewData["ChatFAQResponseGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ChatFAQResponseItem", action = "List", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.Encode(Model.ChatMessageFAQName)%>
</asp:Content>