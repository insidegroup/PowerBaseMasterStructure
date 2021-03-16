<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientFeeLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Fee Alternate Descriptions</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create Alternate Description</th> 
		    </tr>  
            <tr>
                <td><label for="LanguageCode">Language Code</label></td>
                <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
            </tr> 
                <tr>
                <td><label for="ClientFeeLanguageDescription">Alternate Description</label></td>
                <td> <%= Html.TextBoxFor(model => model.ClientFeeLanguageDescription, new {maxlength="50"})%><span class="error"> *</span></td>
                <td> <%= Html.ValidationMessageFor(model => model.ClientFeeLanguageDescription)%> </td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Create Alternate Description" title="Create Alternate Description" class="red"/></td>
            </tr>
        </table>
    <%= Html.HiddenFor(model => model.ClientFeeId) %>
    <% } %>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Fees", "Main", new { controller = "ClientFee", action = "List", }, new { title = "ClientFees" })%> &gt;
<%=Html.RouteLink(Model.ClientFee.ClientFeeDescription, "Default", new { controller = "ClientFee", action = "View", id = Model.ClientFee.ClientFeeId }, new { title = Model.ClientFee.ClientFeeDescription })%> &gt;
Alternate Descriptions
</asp:Content>
