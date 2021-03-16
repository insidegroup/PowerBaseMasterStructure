<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Summary Search</div></div>
    <div id="content">

    <% using (Html.BeginForm(null, null, FormMethod.Post, new { autocomplete = "off" })) {%>
    <%= Html.AntiForgeryToken() %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
            <tr> 
		        <th class="row_header" colspan="3">Search</th> 
	        </tr> 
            <tr>
                <td><label for="Filter">Search on Client TopUnit</label></td>
                <td><%= Html.TextBox("Filter", "", new { maxlength="50", autocomplete = "off" })%></td>
                <td></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
           <tr>
                <td class="row_footer_blank_left">&nbsp;</td>
                <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Search" title="Search" class="red"/></td>
            </tr>
        </table>
<% } %>
    </div>
</div>
<script type="text/javascript">
$(document).ready(function() {
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})

function searchClick() {
    var category = $("#searchList").val();
    var searchText = $("#txtSearch").val();
    var queryString = "SearchText=" + searchText + "&Category=" + category;
    $.post("/ClientSummary/List", queryString, callBackSearch, "_default");
}
</script>
</asp:Content>



