<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitTeam>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Team Client SubUnits</div></div>
         <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Edit Client SubUnit Team</th> 
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
                    <td><label for="IncludeInClientDroplistFlag">Include In Client DropList</label></td>
                    <td><%= Html.CheckBoxFor(model => model.IncludeInClientDroplistFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.IncludeInClientDroplistFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                        <input type="submit" value="Edit Client SubUnit Team" title="Edit Client SubUnit Team" class="red"/>
                       
                    </td>                
               </tr>
            </table>
             <%= Html.HiddenFor(model => model.VersionNumber) %>
                        <%= Html.Hidden("From",Html.Encode(ViewData["from"].ToString())) %>
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
