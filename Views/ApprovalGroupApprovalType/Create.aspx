<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ApprovalGroupApprovalType>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Type</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>

            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr> 
                    <th class="row_header" colspan="3">Create Approval Type</th> 
                </tr> 
                <tr>
                    <td><label for="ApprovalGroupApprovalTypeId">Approval Type ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.ApprovalGroupApprovalTypeId, new { maxlength = "3", @Value = "" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ApprovalGroupApprovalTypeId)%>
                        <label id="lblApprovalGroupIdMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="ApprovalGroupApprovalTypeDescription">Approval Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.ApprovalGroupApprovalTypeDescription, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ApprovalGroupApprovalTypeDescription)%>
                        <label id="lblApprovalGroupNameMsg"/>
                    </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Approval Type" title="Create Approval Type" class="red"/></td>
                </tr>
            </table>
        <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ApprovalGroupApprovalType.js")%>" type="text/javascript"></script>
</asp:Content>