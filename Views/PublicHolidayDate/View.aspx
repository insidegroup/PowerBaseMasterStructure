<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PublicHolidayGroupHolidayDate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Public Holiday Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Public Holiday Date</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Public Holiday Date</th> 
		        </tr> 
                <tr>
                    <td><label for="PublicHolidayGroupName">Public Holiday Group Name</label></td>
                    <td><%= Html.Encode(Model.PublicHolidayGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="PublicHolidayDate">Public Holiday Date</label></td>
                    <td><%= Html.Encode(String.Format("{0:dd MMMM yyyy}", Model.PublicHolidayDate1)) %></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="PublicHolidayDescription">Public Holiday Description</label></td>
                    <td><%= Html.Encode(Model.PublicHolidayDescription)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                 <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_publicholidays').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
})
 </script>
</asp:Content>

