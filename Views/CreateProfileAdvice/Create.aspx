<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitCreateProfileAdvice>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - CreateProfile Advice</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create CreateProfileAdvice</th> 
		        </tr>  
                <tr>
                    <td> <%= Html.LabelFor(model => model.LanguageCode)%></td>
                    <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
                </tr> 
                 <tr>
                    <td><%= Html.LabelFor(model => model.CreateProfileAdvice)%> </td>
                    <td> <%= Html.TextBoxFor(model => model.CreateProfileAdvice, new { maxlength="200"})%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CreateProfileAdvice)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create Profile Advice" title="Create Profile Advice" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.ClientSubUnitGuid) %>
    <% } %>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

