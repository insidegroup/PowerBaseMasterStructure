<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ChatFAQResponseItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Chat FAQ Response Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat FAQ Response Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Chat FAQ Response Item</th> 
		        </tr> 
                <tr>
                    <td><label for="ChatMessageFAQId">Chat Message FAQ Description</label></td>
                    <td><%= Html.DropDownList("ChatMessageFAQId", ViewData["ChatMessageFAQs"] as SelectList, "Please Select...")%><span class="error"> *</span> </td>
                    <td><%= Html.ValidationMessageFor(model => model.ChatMessageFAQId)%></td>
                </tr>
                <tr>
                    <td><label for="LanguageCode">Language</label></td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td>
                        <%= Html.HiddenFor(model => model.LanguageCode)%>
                        <%= Html.ValidationMessageFor(model => model.LanguageCode)%>
                    </td>
                </tr>  
				<tr>
                    <td><label for="ChatFAQResponseItemDescription">Response</label></td>
                    <td><%= Html.TextAreaFor(model => model.ChatFAQResponseItemDescription, new { maxlength = "400" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ChatFAQResponseItemDescription)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Confirm Edit" title="Confirm Edit" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.ChatFAQResponseGroupId) %>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ChatFAQResponseItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Groups" })%> &gt;
<%=Html.RouteLink(ViewData["ChatFAQResponseGroupName"].ToString(), "Default", new { controller = "ChatFAQResponseGroup", action = "View", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = ViewData["ChatFAQResponseGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ChatFAQResponseItem", action = "List", id = ViewData["ChatFAQResponseGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.Encode(Model.ChatMessageFAQName)%>
</asp:Content>