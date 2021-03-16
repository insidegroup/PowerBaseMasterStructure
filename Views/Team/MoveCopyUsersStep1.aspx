<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Team>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
    <div id="content">

    <% using (Html.BeginForm("MoveCopyusersStep1","Team",FormMethod.Post, new { id = "form0" }))
       {%>
    <%= Html.AntiForgeryToken()%>
        <table cellpadding="0" cellspacing="0" width="100%"> 
             <tr> 
		        <th class="row_header" colspan="3">Move/Copy Team Members : Step 1</th> 
	        </tr> 
          <tr>
                <td>Move/Copy members from</td>
                <td><%=Html.Encode(Model.TeamName)%></td>
                <td></td>
            </tr>
          <tr>
                <td><label for="NewTeamId">Move/Copy members to</label></td>
                    <td><%= Html.DropDownList("NewTeamId", ViewData["TeamList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><label id="lblNewTeam" class="field-validation-error"></label></td>
                </tr> 
  
             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
           <tr>
                <td class="row_footer_blank_left">&nbsp;</td>
                <td class="row_footer_blank_right" colspan="2">
                <%if (ViewData["Access"] == "WriteAccess")
                  {
                       %><input type="submit" value="Continue" title="Continue" class="red"/><%} %></td>
            </tr>
        </table>
        <%= Html.Hidden("teamId", ViewData["TeamId"])%>
<% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_teams').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })

    $('#form0').submit(function() {

        if ($('#NewTeamId').val() == "") {
            $('#lblNewTeam').text("Please select a Team");
            return false;
        }
    });

    //NewTeam Disable/Enable OnChange
    $("#NewTeamId").change(function() {
    $("#lblNewTeam").text("");
    });
</script>
</asp:Content>



