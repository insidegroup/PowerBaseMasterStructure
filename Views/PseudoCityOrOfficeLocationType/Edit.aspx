<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeLocationTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Location Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Location Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "PseudoCityOrOfficeLocationType", action = "Edit", id = Model.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Pseudo City/Office ID Location Type</th> 
				</tr> 
		        <tr>
                    <td><label for="PseudoCityOrOfficeLocationType_PseudoCityOrOfficeLocationTypeName">Pseudo City/Office ID Location Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeName)%>
						<label id="lblPseudoCityOrOfficeLocationTypeMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Pseudo City/Office ID Location Type" title="Edit Pseudo City/Office ID Location Type" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PseudoCityOrOfficeLocationType.PseudoCityOrOfficeLocationTypeId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/PseudoCityOrOfficeLocationType.js")%>" type="text/javascript"></script>
</asp:Content>