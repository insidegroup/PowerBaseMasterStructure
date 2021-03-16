<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ChatFAQResponseItemImportStep1VM>" %>

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
					<td valign="top" colspan="3"<%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.IsValidData == false){%> style="color:red;"<%} %>>
						<%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.ReturnMessages != null) {
								foreach (string msg in Model.ImportStep2VM.ReturnMessages){
									Response.Write(msg); %>
							<br/><br/>
							<%}%>
						<%}%>
					</td>
				</tr>  
				<%if(Model != null && Model.ImportStep2VM  != null && Model.ImportStep2VM.IsValidData){%>
				<tr>
					<td valign="top" colspan="3">
					 <br/><br/>Upon selecting Submit, the data will be imported to the Chat FAQ Response Item Groups
					</td>
				</tr>  
				 <%}%>
				<tr>
					<td width="25%" class="row_footer_left"></td>
					<td width="50%" class="row_footer_centre"></td>
					<td width="25%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
						<%if (Model.ImportStep2VM.IsValidData == false)
														 {%>

						<input type="submit" value="Export" title="Export" class="red"/><%}%>
					</td>

					<td class="row_footer_blank_right"><%if (Model.ImportStep2VM.IsValidData)
														 {%><input type="submit" value="Submit" title="Submit" class="red"/>
					<%= Html.ActionLink("Cancel", "ListUnDeleted", "ChatFAQResponseGroup", null, new { @Class = "red" })%><%}%></td>
				</tr>
			</table>
			<%= Html.HiddenFor(model => model.ImportStep2VM.FileBytes)%>       
			<%= Html.HiddenFor(model => model.ImportStep2VM.IsValidData)%>
			<%= Html.HiddenFor(model => model.ChatFAQResponseGroupId) %>   
			<%= Html.Hidden("ImportStep2VM.ReturnMessages", Newtonsoft.Json.JsonConvert.SerializeObject(Model.ImportStep2VM.ReturnMessages))%> 
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