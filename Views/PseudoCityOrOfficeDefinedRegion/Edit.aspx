<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeDefinedRegionVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Defined Region
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Defined Region</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PseudoCityOrOfficeDefinedRegion", action = "Edit", id = Model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Pseudo City/Office ID Defined Region</th> 
				</tr> 
		        <tr>
                    <td><label for="PseudoCityOrOfficeDefinedRegion_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.PseudoCityOrOfficeDefinedRegion.GlobalRegionCode, Model.GlobalRegions as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeDefinedRegion.GlobalRegionCode)%></td>
                </tr>
				<tr>
                    <td><label for="PseudoCityOrOfficeDefinedRegion_PseudoCityOrOfficeDefinedRegionName">Pseudo City/Office ID Defined Region</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionName)%>
						<label id="lblPseudoCityOrOfficeDefinedRegionMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Pseudo City/Office ID Defined Region" title="Edit Pseudo City/Office ID Defined Region" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PseudoCityOrOfficeDefinedRegion.PseudoCityOrOfficeDefinedRegionId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PseudoCityOrOfficeDefinedRegion.js")%>" type="text/javascript"></script>
</asp:Content>