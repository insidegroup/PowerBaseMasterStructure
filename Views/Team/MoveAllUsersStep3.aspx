<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
    <div id="content">

    <% using (Html.BeginForm()) {%>
    <%= Html.AntiForgeryToken() %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
             <tr> 
		        <th class="row_header" colspan="3">Move Team Members : Step 3</th> 
	        </tr> 
          <tr>
                <td colspan="3">Press the 'Confirm' below to move all users from <%=ViewData["TeamName"] %> to <%=ViewData["NewTeamName"]%></td>
            </tr>
             <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
           <tr>
                <td class="row_footer_blank_left">&nbsp;</td>
                <td class="row_footer_blank_right" colspan="2">
                <%if(ViewData["Access"] == "WriteAccess"){
                       %><input type="submit" value="Confirm" title="Confirm" class="red"/><%} %></td>
            </tr>
        </table>
        <%= Html.Hidden("teamId", ViewData["TeamId"])%>
        <%= Html.Hidden("newTeamId", ViewData["NewTeamId"])%>
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



