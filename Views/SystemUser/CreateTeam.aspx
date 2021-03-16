<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUserTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - SystemUsers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">SystemUsers : Add Team</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Add SystemUser to Team</th> 
		        </tr> 
               
                <tr>
                    <td>SystemUserName</td>
                    <td><%= Html.Encode(Model.SystemUserName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Team</td>
                    <td><%= Html.DropDownList("TeamId", ViewData["Teams"] as SelectList, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TeamId)%></td>
                </tr> 
              
                <tr>
                    <td width="20%" class="row_footer_left"></td>
                    <td width="60%" class="row_footer"></td>
                    <td width="20%" class="row_footer_right" align="right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Add SystemUser to Team" title="Add SystemUser to Team" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.SystemUserGuid) %>
    <% } %>
        </div>
    </div>

 <script type="text/javascript">
     $(document).ready(function() {
         $('#menu_teams').click();
		   $("tr:odd").addClass("row_odd");
      $("tr:even").addClass("row_even");
     })
 </script>
</asp:Content>
