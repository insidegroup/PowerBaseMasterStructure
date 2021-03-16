<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ControlValueLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Control Value Translations</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete Control Value Translation</th> 
		        </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Translation</td>
                    <td><%= Html.Encode(Model.ControlValueTranslation)%></td>
                    <td></td>
                </tr>         
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.ControlValueId }, new { @class = "red", title = "Back To List" })%></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ControlValueId) %>
                        <%= Html.HiddenFor(model => model.LanguageCode) %>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
           
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_admin').click();
			$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

