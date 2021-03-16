<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ControlValue>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Control Values</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete ControlValue</th> 
		        </tr> 
                 <tr>
                    <td>ControlValue</td>
                    <td><%= Html.Encode(Model.ControlValue1)%></td>
                    <td></td>
                </tr>   
                <tr>
                    <td>Property</td>
                    <td><%= Html.Encode(Model.ControlPropertyDescription)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.ControlName)%></td>
                    <td></td>
                </tr> 
               <tr>
                    <td>Default Value?</td>
                    <td><%= Html.Encode(Model.DefaultValueFlag)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td class="row_footer_blank_left" ><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
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

