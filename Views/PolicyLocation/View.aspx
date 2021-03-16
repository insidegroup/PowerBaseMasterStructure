<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyLocation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
        <div id="banner"><div id="banner_text">Policy Locations</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Policy Location</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.PolicyLocationName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Global Location?</td>
                    <td><%= Html.Encode(Model.GlobalFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Location</td>
                    <td>
						<% if(!Model.GlobalFlag) { %>
							<%= Html.Encode(string.Format("{0} ({1}), {2}", Model.LocationName, Model.LocationCode, Model.ParentName))%>
						<% } else { %>
							<%= Html.Encode(Model.LocationName)%>
						<% } %>
                    </td>
                    <td></td>
                </tr> 
                <tr>
                    <td>TravelPort Type</td>
                    <td><%= Html.Encode(Model.TravelPortName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>TravelPort Name</td>
                    <td><%= Html.Encode(Model.TravelPortType)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
            </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

