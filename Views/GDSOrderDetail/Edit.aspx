<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSOrderDetailVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Order Detail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Order Detail</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "GDSOrderDetail", action = "Edit", id = Model.GDSOrderDetail.GDSOrderDetailId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Order Detail</th> 
				</tr> 
				<tr>
                    <td><label for="GDSOrderDetail_GDSOrderDetailName">GDS Order Detail</label></td>
                    <td><%= Html.TextBoxFor(model => model.GDSOrderDetail.GDSOrderDetailName, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSOrderDetail.GDSOrderDetailName)%>
						<label id="lblGDSOrderDetailMsg"></label>
                    </td>
                </tr>
				<tr>
                    <td>
						<label for="GDSOrderDetail_AbacusFlag" class="GDSOrderDetailLabel">Abacus (1B)</label>
						<label for="GDSOrderDetail_GDS" class="GDSOrderDetailLabelGDS">GDS <span class="error"> *</span></label>
                    </td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.AbacusFlagNullable, new { @Class = "checkbox" })%></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSOrderDetail.AbacusFlag)%>
						<label id="lblGDSOrderDetailLabelGDSMsg" class="error">At least one GDS checkbox must be checked</label>
                    </td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_AllGDSSystemsFlag" class="GDSOrderDetailLabel">All GDS Systems (ALL)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.AllGDSSystemsFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.AllGDSSystemsFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_AmadeusFlag" class="GDSOrderDetailLabel">Amadeus (1A)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.AmadeusFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.AmadeusFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_ApolloFlag" class="GDSOrderDetailLabel">Apollo (1V)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.ApolloFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.ApolloFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_EDSFlag" class="GDSOrderDetailLabel">EDS (1C)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.EDSFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.EDSFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_GalileoFlag" class="GDSOrderDetailLabel">Galileo (1V)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.GalileoFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.GalileoFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_RadixxFlag" class="GDSOrderDetailLabel">Radixx (1D)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.RadixxFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.RadixxFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_SabreFlag" class="GDSOrderDetailLabel">Sabre (1S)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.SabreFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.SabreFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_TravelskyFlag" class="GDSOrderDetailLabel">Travelsky (1E)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.TravelskyFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.TravelskyFlag)%></td>
                </tr>
				<tr>
                    <td><label for="GDSOrderDetail_WorldspanFlag" class="GDSOrderDetailLabel">Worldspan (1P)</label></td>
                    <td><%= Html.CheckBoxFor(model => model.GDSOrderDetail.WorldspanFlagNullable, new { @Class = "checkbox" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSOrderDetail.WorldspanFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GDS Order Detail" title="Edit GDS Order Detail" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.GDSOrderDetail.GDSOrderDetailId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSOrderDetail.js")%>" type="text/javascript"></script>
</asp:Content>