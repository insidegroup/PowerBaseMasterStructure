<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
    <div id="content">

        <table cellpadding="0" cellspacing="0" width="100%"> 
             <tr> 
		        <th class="row_header" colspan="3">Move/Copy Team Members : Completed</th> 
	        </tr> 
          <tr>
                <td><%=Html.Encode(ViewData["Message"])%></td>
            </tr>

             <tr>
                    <td width="50%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer_right"></td>
                </tr>
           <tr>
                <td class="row_footer_blank_left">&nbsp;</td>
                <td class="row_footer_blank_right"></td>
            </tr>
        </table>

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



