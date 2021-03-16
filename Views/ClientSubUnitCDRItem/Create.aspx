<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitCDRItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link - Validate Values</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
	<% Html.EnableUnobtrusiveJavaScript(); %>
    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Create Validate Value</th> 
		    </tr>  
            <tr>
                <td valign="top" colspan="3">Validate CDR is <%= Html.Encode(Request.QueryString["RelatedToDisplayName"]) %></td>
            </tr> 
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReferenceItem_RelatedToValue">Validate Value</label></td>
                <td valign="top"> <%=Html.TextBoxFor(model => model.ClientSubUnitClientDefinedReferenceItem.RelatedToValue, new { maxlength="50" } )%> <span class="error">*</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.ClientSubUnitClientDefinedReferenceItem.RelatedToValue)%> </td>
            </tr> 
            <tr>
                <td width="25%" class="row_footer_left"></td>
                <td width="50%" class="row_footer_centre"></td>
                <td width="25%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Create Validate Value" title="Create Validate Value" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId) %>
    <% } %>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search_wrapper').css('height', '32px');
		$('#breadcrumb').css('width', 'auto');
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("CDR Links", "Main", new { controller="ClientSubUnitCDR", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDR Links" })%>
<%=Html.RouteLink(Model.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId.ToString(), "Main", new { controller="ClientSubUnitCDR", action = "View", id = Model.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId }, new { title = Model.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId.ToString() })%> &gt;
<%=Html.RouteLink("Validate Values", "Main", new { controller="ClientSubUnitCDRItem", action = "List", id = Model.ClientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId, RelatedToDisplayName = Request.QueryString["RelatedToDisplayName"], csu = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Validate Values" })%> &gt;
Create
</asp:Content>