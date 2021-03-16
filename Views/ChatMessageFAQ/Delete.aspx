<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ChatMessageFAQVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat Message FAQs</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Chat Message FAQ</th> 
		        </tr>    
				<tr>
					<td><label for="ChatMessageFAQId">FAQ ID</label></td>
					<td><%= Html.Encode(Model.ChatMessageFAQ.ChatMessageFAQId) %></td>
					<td></td>
				</tr> 
                <tr>
					<td><label for="ChatMessageFAQName">Chat Message FAQ</label></td>
					<td><%= Html.Encode(Model.ChatMessageFAQ.ChatMessageFAQName) %></td>
					<td></td>
				</tr>
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    </td>
                    <td class="row_footer_blank_right" colspan="2">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.ChatMessageFAQ.ChatMessageFAQId) %>
							<%= Html.HiddenFor(model => model.ChatMessageFAQ.VersionNumber) %>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
<script type="text/javascript">
$(document).ready(function() {
    $('#search').hide();
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Chat Message FAQs", "Main", new { controller = "ChatMessageFAQ", action = "List", }, new { title = "Chat Message FAQs" })%> &gt;
<%:Model.ChatMessageFAQ.ChatMessageFAQName%>
</asp:Content>