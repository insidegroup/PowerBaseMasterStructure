<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Messages</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">GDS Response Messages</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">View GDS Response Message</th> 
		        </tr> 
		        <tr>
                    <td><label for="GDSEndWarningConfiguration_GDSName">GDS Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSEndWarningConfiguration.GDS.GDSName)%></td>
                </tr>
				<tr>
                    <td><label for="GDSEndWarningConfiguration_IdentifyingWarningMessage">GDS Response Message</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSEndWarningConfiguration.IdentifyingWarningMessage)%></td>
                </tr>
				<tr>
                    <td><label for="GDSEndWarningConfiguration_GDSEndWarningBehaviorType_GDSEndWarningBehaviorTypeDescription">Behaviour</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSEndWarningConfiguration.GDSEndWarningBehaviorType.GDSEndWarningBehaviorTypeDescription)%></td>
                </tr> 
				<% foreach(var automatedCommand in Model.GDSEndWarningConfiguration.AutomatedCommands) { %>
				<tr>
                    <td><label for="GDSEndWarningConfiguration_AutomatedCommands">Automated Command <%= automatedCommand.CommandExecutionSequenceNumber %></label></td>
                    <td colspan="2"><%= Html.Encode(automatedCommand.CommandText)%></td>
                </tr>
				<% } %>
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
		$(document).ready(function () {
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#menu_admin').click();
		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("GDS Response Messages", "Main", new { controller = "GDSEndWarningConfiguration", action = "List" }, new { title = "GDS Response Messages" })%> &gt;
<%= CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(ViewData["WarningMessage"].ToString(), 30) %> &gt;
View GDS Response Message
</asp:Content>