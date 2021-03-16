<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTravelerTypeESCInformationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit TravelerType - Client Detail ESC Information</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create ESC Information</th> 
		        </tr> 
                <tr>
                    <td><label for="ClientDetailESCInformation_ESCInformation">ESC Information</label></td>
                    <td><%= Html.TextAreaFor(model => model.ClientDetailESCInformation.ESCInformation, new { maxlength = "255" })%><span class="error"> *</span><br /><span id="countBO"></span>&nbsp; characters remaining</td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDetailESCInformation.ESCInformation)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create ESC Information" class="red" title="Create ESC Information"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientDetail.ClientDetailId) %>
    <% } %>


    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ESCInformation.js")%>" type="text/javascript"></script>
</asp:Content>


