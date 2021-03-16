<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSRequestTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Request Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Request Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "GDSRequestType", action = "Edit", id = Model.GDSRequestType.GDSRequestTypeId }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit GDS Request Type</th> 
				</tr> 
		        <tr>
                    <td><label for="GDSRequestType_GDSRequestType1">GDS Request Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.GDSRequestType.GDSRequestTypeName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSRequestType.GDSRequestTypeName)%>
						<label id="lblGDSRequestTypeMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit GDS Request Type" title="Edit GDS Request Type" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.GDSRequestType.GDSRequestTypeId) %>
		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSRequestType.js")%>" type="text/javascript"></script>
</asp:Content>