<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ReasonCodeProductTypeTravelerDescription>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Traveler Description</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete = "off" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
				<tr> 
					<th class="row_header" colspan="3">Create Traveler Description</th>  
				</tr>  
				<tr>
					<td><label for="ReasonCodeProductTypeDescription1">Reason Code Traveler Description</label></td>
					<td colspan="2"> <%= Html.Encode(ViewData["ReasonCodeProductTypeDescription"]) %></td>
				</tr> 
				<tr>
					<td><label for="LanguageCode">Language</label></td>
					<td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...", new { autocomplete = "off" } )%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
				</tr> 
				<tr>
					<td><label for="ReasonCodeProductTypeTravelerDescription1">Traveler Description</label></td>
					<td> <%= Html.TextBoxFor(model => model.ReasonCodeProductTypeTravelerDescription1, new { maxlength="50", autocomplete = "off" })%><span class="error"> *</span></td>
					<td> <%= Html.ValidationMessageFor(model => model.ReasonCodeProductTypeTravelerDescription1)%> </td>
				</tr> 
				<tr>
					<td width="30%" class="row_footer_left"></td>
					<td width="40%" class="row_footer_centre"></td>
					<td width="30%" class="row_footer_right"></td>
				</tr>
				<tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
					<td class="row_footer_blank_right"><input type="submit" value="Create Traveler Description" title="Create Traveler Description" class="red"/></td>
				</tr>
			</table>
			<%= Html.HiddenFor(model => model.ReasonCode) %>
			<%= Html.HiddenFor(model => model.ProductId) %>
			<%= Html.HiddenFor(model => model.ReasonCodeTypeId) %>
		<% } %>
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
<%=ViewData["ReasonCodeItem"].ToString()%> &gt;
Descriptions
</asp:Content>