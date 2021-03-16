<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TeamOutOfOfficeGroupImportStep3VM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Team Out of Office Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Out of Office</div></div>
    <div id="content">
     <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
			<table cellpadding="0" cellspacing="0" width="100%"> 
				<tr> 
					<th class="row_header" colspan="3">Team Out of Office Import Summary</th> 
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
					<td class="row_footer_blank_left" colspan="2"><a href="<%= Url.Action("ListUnDeleted","TeamOutOfOfficeGroup")%>" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right">
					</td>
				</tr>
			</table>
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
<%=Html.RouteLink("Team Out of Office Groups", "Main", new { controller = "TeamOutOfOfficeGroup", action = "ListUnDeleted", }, new { title = "Team Out of Office Groups" })%> &gt;
Import Team Out of Office &gt; 
Import Summary
</asp:Content>