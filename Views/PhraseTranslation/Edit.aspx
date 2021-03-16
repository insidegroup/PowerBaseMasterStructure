<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PhraseTranslation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Phrase Translation</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Phrase Translation</th> 
		        </tr>  
                <tr>
                    <td>Phrase Name</td>
                    <td><%= Html.Encode(ViewData["PhraseName"].ToString())%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="LanguageName">Language Name</label></td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="PhraseName">Phrase Name Translation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PhraseTranslation1, new { maxlength = "100" })%></td>
                    <td> <%= Html.ValidationMessageFor(model => model.PhraseTranslation1)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Phrase Translation" title="Edit Phrase Translation" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.LanguageCode) %>
<%= Html.HiddenFor(model => model.PhraseId) %>
<%= Html.HiddenFor(model => model.VersionNumber) %>
    <% } %>
        </div>
    </div>

<script type="text/javascript">
$(document).ready(function() {
    $('#menu_admin').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');
})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Phrases", "Main", new { controller = "Phrase", action = "List", }, new { title = "Phrases" })%> &gt;
<%=Html.RouteLink(ViewData["PhraseName"].ToString(), "Main", new { controller = "Phrase", action = "ViewItem", id = ViewData["PhraseId"].ToString() }, new { title = ViewData["PhraseName"].ToString() })%> &gt;
Phrase Translations
</asp:Content>