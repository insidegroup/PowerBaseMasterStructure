<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
    <div id="content">

    <% using (Html.BeginForm("MoveCopyusersStep2","Team",FormMethod.Post, new { id = "form0" })){%>
    <%= Html.AntiForgeryToken() %>
        <table cellpadding="0" cellspacing="0" width="100%"> 
             <tr> 
		        <th class="row_header" colspan="3">Move/Copy Team Members : Step 2</th> 
	        </tr> 
           <tr>
                <td colspan="3">Move/Copy members from <%=Html.Encode(ViewData["TeamName"])%></td>
            </tr>
          <tr>
                <td colspan="3">Move/Copy members to <%=Html.Encode(ViewData["NewTeamName"])%></td>
            </tr>
          <tr>
                <td><label for="CopyAll">Copy All members</label></td>
                <td><input name="membersToCopy" type="radio" value="CopyAll" /></td>
                <td>This will copy all members from <%=Html.Encode(ViewData["TeamName"])%> to <%=Html.Encode(ViewData["NewTeamName"])%></td>
            </tr>
            <tr>
                <td><label for="CopySome">Copy Some members</label></td>
                <td><input name="membersToCopy" type="radio" value="CopySome" /></td>
                <td>This will allow you to select members to copy from <%=Html.Encode(ViewData["TeamName"])%> to <%=Html.Encode(ViewData["NewTeamName"])%></td>
            </tr>  
          <tr>
                <td><label for="MoveAll">Move All members</label></td>
                <td><input name="membersToCopy" type="radio" value="MoveAll" /></td>
                <td>This will move all members from <%=Html.Encode(ViewData["TeamName"])%> to <%=Html.Encode(ViewData["NewTeamName"])%></td>
            </tr>
            <tr>
                <td><label for="MoveSome">Move Some members</label></td>
                <td><input name="membersToCopy" type="radio" value="MoveSome" /></td>
                <td>This will allow you to select members to move from <%=Html.Encode(ViewData["TeamName"])%> to <%=Html.Encode(ViewData["NewTeamName"])%></td>
            </tr>  
             <tr>
                <td></td>
                <td colspan="2"><label id="lblErrorMsg" class="field-validation-error"></label></td>
            </tr>  
            <tr>
                <td width="20%" class="row_footer_left"></td>
                <td width="15%" class="row_footer_centre"></td>
                <td width="65%" class="row_footer_right"></td>
            </tr>
           <tr>
                <td class="row_footer_blank_left">&nbsp;</td>
                <td class="row_footer_blank_right" colspan="2">
                <%if(ViewData["Access"] == "WriteAccess"){%>
                <input type="submit" value="Continue" title="Continue" class="red"/>
                 <%}%></td>
            </tr>
        </table>
        <%= Html.Hidden("TeamId", ViewData["TeamId"])%>
        <%= Html.Hidden("NewTeamId", ViewData["NewTeamId"])%>
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

        if (!$("input[@name='membersToCopy']:checked").val()) {
            $('#lblErrorMsg').text("Please select an option");
            return false;
        }
    });
    $(function() {
        $('input[type="radio"]').click(function() {
            if ($(this).is(':checked')) {
                $('#lblErrorMsg').text("");
            }
        });
    });
     
</script>
</asp:Content>



