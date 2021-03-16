<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ApprovalItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Approval Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
     <div id="banner"><div id="banner_text">Approval Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Approval Items</th> 
		        </tr> 
                <tr>
                    <td>Approval Group</td>
                    <td><%= Html.Encode(Model.ApprovalItem.ApprovalGroup.ApprovalGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Approver Name</td>
                    <td><%= Html.Encode(Model.ApprovalItem.ApproverDescription)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Remark Text</td>
                    <td><%= Html.Encode(Model.ApprovalItem.RemarkText)%></td>
                    <td></td>
				</tr>                 

                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back", "List", new { id = Model.ApprovalItem.ApprovalGroupId }, new { @class = "red", title = "Back" })%></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ApprovalItem.ApprovalItemId)%>
						<%= Html.HiddenFor(model => model.ApprovalItem.ApprovalGroupId)%>
						<%= Html.HiddenFor(model => model.ApprovalItem.VersionNumber)%>
                    <%}%>
                    </td>
                </tr>
            </table>
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
