<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ApprovalItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Approval Item
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Approval Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
		<% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Approval Item</th> 
		        </tr> 
                <tr>
                    <td><label for="ApprovalItem_ApprovalGroup_ApprovalGroupName">Approval Group</label></td>
                    <td colspan="2"><%= Html.Encode(ViewData["ApprovalGroupName"])%></td>
                </tr>
				<tr>
					<td><label for="ApprovalItem_ApproverDescription">Approver Name</label></td>
					<td><%= Html.TextAreaFor(model => model.ApprovalItem.ApproverDescription, new { size = "100" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ApprovalItem.ApproverDescription)%></td>
				</tr> 
				<tr>
					<td><label for="ApprovalItem_RemarkText">Remark Text</label></td>
					<td><%= Html.TextAreaFor(model => model.ApprovalItem.RemarkText, new { size = "100" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ApprovalItem.RemarkText)%></td>
				</tr>
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Approval Item" title="Create Approval Item" class="red"/></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.ApprovalItem.ApprovalGroupId)%>

    <% } %>

   </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_approvalgroups').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Approval Item Groups", "Main", new { controller = "ApprovalGroup", action = "ListUnDeleted", id = ViewData["ApprovalGroupId"] }, new { title = "Approval Item Groups" })%> &gt;
<%=ViewData["ApprovalGroupName"]%> &gt;
Create Approval Items
</asp:Content>