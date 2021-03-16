<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelPortLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Port Languages</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Travel Port Language</th> 
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
                    <td>Name Translation</td>
                    <td><%= Html.Encode(Model.TravelPortName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.TravelPortCode)%>
                        <%= Html.HiddenFor(model => model.LanguageCode) %>
                        <%= Html.HiddenFor(model => model.TravelPortTypeId) %>
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

