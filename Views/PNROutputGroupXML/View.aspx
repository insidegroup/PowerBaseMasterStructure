<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PNROutputGroupXMLItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - PNR Output Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">PNR Output Remark</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="2">View PNR Output Remark</th> 
		        </tr> 
		        <tr>
                    <td>Sequence</td>
                    <td><%= Html.Encode(Model.Sequence) %></td>
                </tr>
		        <tr>
                    <td>RemarkType</td>
                    <td><%= Html.Encode(Model.RemarkType) %></td>
                </tr>
                <tr>
                    <td>Bind</td>
                    <td><%= Html.Encode(Model.Bind)%></td>
                </tr>
                <tr>
                    <td>Qualifier</td>
                    <td><%= Html.Encode(Model.Qualifier)%></td>
                </tr>
                <tr>
                    <td>UpdateType</td>
                    <td><%= Html.Encode(Model.UpdateType)%></td>
                </tr>
                 <tr>
                    <td>GroupId</td>
                    <td><%= Html.Encode(Model.GroupId)%></td>
                </tr>
                 <tr>
                    <td>Value</td>
                    <td><%= Html.Encode(Model.Value)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="70%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_left"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_pnroutputs').click();
	     $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>


