<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.FormOfPaymentAdviceMessageGroupItemTranslation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - FOP Advice Message Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">FOP Advice Message Translations</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create FOP Advice Message Translation</th> 
		        </tr>  
                <tr>
                    <td>FOP Advice Message</td>
                    <td colspan="2"><%= Html.Encode(Model.FormOfPaymentAdviceMessageGroupItem.FormOfPaymentAdviceMessage)%></td>
                </tr>
                <tr>
                    <td><label for="LanguageCode">Language</label></td>
                    <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
                </tr> 
                <tr>
                    <td><label for="FormOfPaymentAdviceMessageTranslation">Translation</label></td>
                    <td> <%= Html.TextAreaFor(model => model.FormOfPaymentAdviceMessageTranslation, new {maxlength="255"})%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.FormOfPaymentAdviceMessageTranslation)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Confirm Create" title="Confirm Create" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.FormOfPaymentAdviceMessageGroupItemId) %>
        <% } %>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_fopadvicemessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
        $('#search_wrapper').css('height', 'auto');
    })
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("FOP Advice Message Groups", "Main", new { controller = "FormOfPaymentAdviceMessageGroup", action = "ListUnDeleted", }, new { title = "FOP Advice Message Groups" })%> &gt;
<%=Html.RouteLink(ViewData["FormOfPaymentAdviceMessageGroupName"].ToString(), "Default", new { controller = "FormOfPaymentAdviceMessageGroup", action = "View", id = ViewData["FormOfPaymentAdviceMessageGroupId"].ToString() }, new { title = ViewData["FormOfPaymentAdviceMessageGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "FormOfPaymentAdviceMessageGroupItem", action = "List", id = ViewData["FormOfPaymentAdviceMessageGroupId"].ToString() }, new { title = "Items" })%> &gt;
<%=Html.RouteLink("Translations", "Default", new { controller = "FormOfPaymentAdviceMessageGroupItemTranslation", action = "List", id = ViewData["FormOfPaymentAdviceMessageGroupId"].ToString() }, new { title = "Translations" })%> &gt;
Create
</asp:Content>