<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientAccountClientDetailVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ClientDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Detail</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "ClientDetailClientAccount", action = "Edit",id = Model.ClientDetail.ClientDetailId }, FormMethod.Post, new { id = "form0" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Client Detail</th> 
		        </tr> 
		         <tr>
                    <td><label for="ClientDetail_ClientDetailName">Client Detail Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientDetail.ClientDetailName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.ClientDetailName)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_WebsiteAddress">Website Address</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientDetail.WebsiteAddress, new { maxlength = "100" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.WebsiteAddress)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_Logo">Logo</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientDetail.Logo, new { maxlength = "50" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.Logo)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_InheritFromParentFlag">Inherit From Parent Flag</label></td>
                   <td><%= Html.CheckBoxFor(model => model.ClientDetail.InheritFromParentFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.InheritFromParentFlag)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_EnabledFlag">Enabled</label></td>
                    <td><%= Html.CheckBoxFor(model => model.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.EnabledFlag)%></td>
                </tr>  
                 <tr>
                    <td><label for="ClientDetail_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.ClientDetail.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ClientDetail.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientDetail_TripTypeId">Trip Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientDetail.TripTypeId, Model.TripTypes, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetail.TripTypeId)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Client Detail" title="Edit Client Detail" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientAccount.ClientAccountNumber) %>
            <%= Html.HiddenFor(model => model.ClientAccount.SourceSystemCode)%>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId )%>
            <%= Html.HiddenFor(model => model.ClientDetail.VersionNumber )%>
            <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientDetail.js")%>" type="text/javascript"></script>
</asp:Content>


