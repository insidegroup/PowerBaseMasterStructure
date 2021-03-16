<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelPortLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Port Languages</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
         <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Travel Port Language</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.TravelPortCodeTravelPortName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Type</td>
                    <td><%= Html.Encode(Model.TravelPortTypeDescription)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="TravelPortName">Name Translation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TravelPortName, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.TravelPortName)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Edit Travel Port Language" title="Edit Travel Port Language" class="red"/></td>
                </tr>
            </table>
            <%=Html.HiddenFor(model => model.TravelPortCode)%>
            <%=Html.HiddenFor(model => model.TravelPortTypeId)%>
            <%=Html.HiddenFor(model => model.LanguageCode)%>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/TravelPortLanguage.js")%>" type="text/javascript"></script>
</asp:Content>



