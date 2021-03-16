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
			        <th class="row_header" colspan="3">View Control Value Translation</th> 
		        </tr> 
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td><%= Html.LabelFor(model => model.ControlValueTranslation)%> </td>
                    <td><%= Html.Encode(Model.ControlValueTranslation)%></td>
                    <td></td>
                </tr>
              
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right"></td>
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

