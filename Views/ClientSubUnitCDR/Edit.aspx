<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitCDRVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/ClientSubUnitClientDefinedReference.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Edit CDR Link</th> 
		    </tr> 
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReference_Value">CDR Value</label></td>
                <td valign="top"> <%= Html.TextBoxFor(model => model.ClientSubUnitClientDefinedReference.Value, new { maxlength = "50" })%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.ClientSubUnitClientDefinedReference.Value)%><label id="lblClientSubUnitClientDefinedReferenceMsg"></label></td>
            </tr> 
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReference_ClientAccountNumberSourceSystemCode">Account</label></td>
                <td valign="top"> <%=Html.DropDownListFor(model => model.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode, Model.ClientAccountSelectList, "Please Select...")%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.ClientSubUnitClientDefinedReference.ClientAccountNumberSourceSystemCode)%> </td>
            </tr> 
            <tr>
                <td valign="top"><label for="ClientSubUnitClientDefinedReference_CreditCardId">Credit Card</label></td>
                <td valign="top"> <%=Html.DropDownListFor(model => model.ClientSubUnitClientDefinedReference.CreditCardId, Model.CreditCardSelectList, "Please Select...")%><span class="error" style="vertical-align:top"> *</span></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.ClientSubUnitClientDefinedReference.CreditCardId)%> </td>
            </tr> 
            <tr>
                <td valign="top"><label for="CreditCardValidTo">Credit Card Valid To</label></td>
                <td valign="top"> <%=Html.TextBoxFor(model => model.CreditCardValidTo, new { @readonly="readonly" })%></td>
                <td valign="top"> <%= Html.ValidationMessageFor(model => model.CreditCardValidTo)%> </td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                <td class="row_footer_blank_right"><input type="submit" value="Edit CDR Link" class="red" title="Edit CDR Link"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
        <%= Html.HiddenFor(model => model.ClientDefinedReferenceItemId) %>
        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReference.ClientAccountNumber) %>
        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId) %>
        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReference.VersionNumber) %>


    <% } %>


    </div>
</div>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("CDR Links", "Main", new {action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDR Links" })%>

</asp:Content>

