<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ChatFAQResponseItemImportStep3VM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Chat FAQ Response Items
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Chat FAQ Response Items</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			<table cellpadding="0" cellspacing="0" width="100%"> 
				<tr> 
					<th class="row_header" colspan="3">Chat FAQ Response Items Import Summary</th> 
				</tr>  
				<tr>
					<td valign="top" colspan="3">
						<% if (Model != null && Model.ReturnMessages != null && Model.ReturnMessages.Count > 0){ %>
							<%foreach (string msg in Model.ReturnMessages){ 
								Response.Write(msg); %> <br/><br/>
							<%}%>
						<% } else { %>
							<p style="color:red;">The import of the selected data file has failed.<br />Please try again or contact your administrator</p>
						<% } %>
					</td>
				</tr>  
				<tr>
					<td width="25%" class="row_footer_left"></td>
					<td width="50%" class="row_footer_centre"></td>
					<td width="25%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2"><a href="<%= Url.Action("List", "ChatFAQResponseItem", new { id = Model.ChatFAQResponseGroup.ChatFAQResponseGroupId })%>" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right">
					</td>
				</tr>
			</table>
			<%= Html.HiddenFor(model => model.ChatFAQResponseGroupId) %>  
		<% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
    	$('#menu_chatmessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
    })
 </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Chat FAQ Response Item Groups", "Main", new { controller = "ChatFAQResponseGroup", action = "ListUnDeleted", }, new { title = "Chat FAQ Response Item Groups" })%> &gt;
<%=Html.RouteLink(Model.ChatFAQResponseGroup.ChatFAQResponseGroupName, "Main", new { controller = "ChatFAQResponseGroup", action = "View", id = Model.ChatFAQResponseGroup.ChatFAQResponseGroupId }, new { title = Model.ChatFAQResponseGroup.ChatFAQResponseGroupName })%> &gt; 
<%=Html.RouteLink("Items", "Main", new { controller = "ChatFAQResponseItem", action = "List", id = Model.ChatFAQResponseGroup.ChatFAQResponseGroupId }, new { title = "Items" })%> &gt; 
Import Summary
</asp:Content>