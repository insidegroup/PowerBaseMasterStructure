<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Account</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
    
        <table cellpadding="0" cellspacing="0" width="100%"> 
	        <tr> 
		        <th class="row_header" colspan="3">Edit Client Account</th> 
	        </tr> 
            <tr>
                <td><label for="ClientAccountNumber">ClientAccount Number</label></td>
                <td><%= Html.TextBoxFor(model => model.ClientAccountNumber, new { maxlength = "20" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.ClientAccountNumber)%></td>
            </tr>  
            <tr>
                <td><label for="ClientAccountName">ClientAccount Name</label></td>
                <td><%= Html.TextBoxFor(model => model.ClientAccountName, new { maxlength = "150", style="width:220px" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.ClientAccountName)%>
                </td>
            </tr> 
            <tr>
                <td><label for="SourceSystemCode">Source System Code</label></td>
                <td><%= Html.TextBoxFor(model => model.SourceSystemCode, new { maxlength = "10" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.SourceSystemCode)%>
                </td>
            </tr> 
            <tr>
                <td><label for="GloryAccountName">Glory Account Name</label></td>
                <td><%= Html.TextBoxFor(model => model.GloryAccountName, new { maxlength = "50" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.GloryAccountName)%>
                </td>
            </tr> 
            <tr>
                <td><label for="ClientMasterCode">ClientMasterCode</label></td>
                <td><%= Html.TextBoxFor(model => model.ClientMasterCode, new { maxlength = "10" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.ClientMasterCode)%>
                </td>
            </tr> 
            <tr>
                <td><label for="EffectiveDate">Effective Date</label></td>
                <td><%= Html.EditorFor(model => model.EffectiveDate, new { maxlength = "12" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.EffectiveDate  )%>
                </td>
            </tr> 
             <tr>
                <td><label for="CountryName">Country</label></td>
                <td><%= Html.TextBoxFor(model => model.CountryName, new { maxlength="100"})%></td>
                <td>
                        <%= Html.ValidationMessageFor(model => model.CountryName)%>
                        <%= Html.Hidden("CountryCode")%>
                        <label id="lblCountryNameMsg"/>
                </td>
            </tr>   
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
           <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Client Account" title="Edit Client Account" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.VersionNumber)%>
<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientAccount.js")%>" type="text/javascript"></script>
</asp:Content>