<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ExternalNameVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - External Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">External Name</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create External Name</th> 
		        </tr> 
		       <tr>
                    <td><label for="Address_LocationName">External Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ExternalName.ExternalName1, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExternalName.ExternalName1)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create External Name" title="Create External Name" class="red"/></td>
                </tr>
            </table>
		<% } %>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#menu_admin, #menu_admin_gdsmanagement').click();
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#search').hide();
		});
	</script>
</asp:Content>