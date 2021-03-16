<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ContactType>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Contact Types</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Contact Type</th> 
		        </tr> 
                <tr>
                    <td>Contact Type</td>
                    <td><%= Html.Encode(Model.ContactTypeName)%></td>
                    <td></td>
                </tr> 
                 <%if (!ViewData.ModelState.IsValid) { %>
                <%if (ViewData.ModelState["Exception"].Errors.Count > 0 ){ %>
                <tr>
                    <td></td>
                    <td colspan="2"><span class="error"><% =ViewData.ModelState["Exception"].Errors[0].ErrorMessage %></span></td>
                </tr> 
                <% } %>
                <% } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
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
    $(document).ready(function () {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

