<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "GDSOrderType", action = "Edit", id = Model.GDSOrderType.GDSOrderTypeId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Order Type</th> 
				</tr> 
				<tr>
                    <td><label for="GDSOrderType_GDSOrderTypeName">GDS Order Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.GDSOrderType.GDSOrderTypeName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSOrderType.GDSOrderTypeName)%>
						<label id="lblGDSOrderTypeMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td>
						<label for="GDSOrderType_AbacusFlag" class="GDSOrderTypeLabel">Abacus (1B)</label>
						<label for="GDSOrderType_GDS" class="GDSOrderTypeLabelGDS">GDS <span class="error"> *</span></label>
                    </td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.AbacusFlagNullable, new { @Class = "checkbox" })%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSOrderType.AbacusFlag)%>
						<label id="lblGDSOrderTypeLabelGDSMsg" class="error">At least one GDS checkbox must be checked</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_AllGDSSystemsFlag" class="GDSOrderTypeLabel">All GDS Systems (ALL)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.AllGDSSystemsFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.AllGDSSystemsFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_AmadeusFlag" class="GDSOrderTypeLabel">Amadeus (1A)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.AmadeusFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.AmadeusFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_ApolloFlag" class="GDSOrderTypeLabel">Apollo (1V)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.ApolloFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.ApolloFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_EDSFlag" class="GDSOrderTypeLabel">EDS (1C)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.EDSFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.EDSFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_GalileoFlag" class="GDSOrderTypeLabel">Galileo (1V)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.GalileoFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.GalileoFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_RadixxFlag" class="GDSOrderTypeLabel">Radixx (1D)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.RadixxFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.RadixxFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_SabreFlag" class="GDSOrderTypeLabel">Sabre (1S)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.SabreFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.SabreFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_TravelskyFlag" class="GDSOrderTypeLabel">Travelsky (1E)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.TravelskyFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.TravelskyFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_WorldspanFlag" class="GDSOrderTypeLabel">Worldspan (1P)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.WorldspanFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.WorldspanFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderType_IsThirdPartyFlag">3<sup>rd</sup> Party?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderType.IsThirdPartyFlagNullable)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderType.IsThirdPartyFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GDS Order Type" title="Edit GDS Order Type" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.GDSOrderType.GDSOrderTypeId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSOrderType.js")%>" type="text/javascript"></script>
</asp:Content>