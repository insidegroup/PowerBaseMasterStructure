<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Client SubUnits</div></div>
        <div id="content">
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Remove Client SubUnit from Team</th> 
		        </tr> 
                <tr>
                    <td>Team</td>
                    <td><%= Model.TeamName%></td>
                    <td><%= Html.HiddenFor(model => model.TeamId)%></td>
                </tr>   
                 <tr>
                    <td>Client SubUnit</td>
                    <td><%= Model.ClientSubUnitName%></td>
                    <td><%= Html.HiddenFor(model => model.ClientSubUnitGuid)%></td>
                </tr>  
                 <tr>
                    <td>Include In Client DropList</td>
                    <td><%= Model.IncludeInClientDroplistFlag%></td>
                    <td></td>
                </tr>  
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
                        <input type="submit" value="Confirm Remove" title="Confirm Remove" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                        <%= Html.Hidden("From",Html.Encode(ViewData["from"].ToString())) %>
                    <%}%>
                    </td>                
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
