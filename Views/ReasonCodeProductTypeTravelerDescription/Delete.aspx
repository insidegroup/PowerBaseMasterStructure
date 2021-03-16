<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ReasonCodeProductTypeTravelerDescription>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Traveler Description</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Traveler Description</th> 
		    </tr> 
            <tr>
                <td><label for="ReasonCodeProductTypeDescription1">Reason Code Traveler Description</label></td>
                <td colspan="2"> <%= Html.Encode(ViewData["ReasonCodeProductTypeDescription"]) %></td>
            </tr> 
			<tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.LanguageName)%></td>
                <td></td>
            </tr>
            <tr>
                <td>Traveler Description</td>
                <td><%= Html.Encode(Model.ReasonCodeProductTypeTravelerDescription1)%></td>
                <td></td>
            </tr>   
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                <td class="row_footer_blank_right" colspan="2">
                <% using (Html.BeginForm()) { %>
                    <%= Html.AntiForgeryToken() %>
                    <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                    <%= Html.HiddenFor(model => model.ReasonCode) %>
                    <%= Html.HiddenFor(model => model.ReasonCodeTypeId) %>
                    <%= Html.HiddenFor(model => model.LanguageCode) %>
                    <%= Html.HiddenFor(model => model.ProductId) %>
                    <%= Html.HiddenFor(model => model.VersionNumber) %>
                <%}%>
                </td>                
            </tr>
        </table>       
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_reasoncodes').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Product Types", "Main", new { controller = "ReasonCodeProductType", action = "List", }, new { title = "Reason Code Product Types" })%> &gt;
<%=Html.RouteLink("Descriptions", "Main", new { controller = "ReasonCodeProductTypeDescription", action = "List", productId = Model.ProductId, reasonCode = Model.ReasonCode, reasonCodeTypeId = Model.ReasonCodeTypeId }, new { title = "Reason Code Product Types" })%>
</asp:Content>