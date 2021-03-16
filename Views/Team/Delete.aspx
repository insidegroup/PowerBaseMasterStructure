<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Team>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Teams
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
        <div id="banner"><div id="banner_text">Teams</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete Team</th> 
		        </tr> 
                  <tr>
                    <td>Team Name</td>
                    <td><%= Html.Encode(Model.TeamName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Team Email</td>
                    <td><%= Html.Encode(Model.TeamEmail)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Team Phone Number</td>
                    <td><%= Html.Encode(Model.TeamPhoneNumber)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Team City Code</td>
                    <td><%= Html.Encode(Model.CityCode)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Team Queue</td>
                    <td><%= Html.Encode(Model.TeamQueue)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>TeamType</td>
                    <td><%= Html.Encode(Model.TeamTypeDescription)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td><%= Html.LabelFor(model => model.HierarchyType)%> </td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                
                <tr>
                    <td><%= Html.LabelFor(model => model.ClientSubUnitName)%> </td>
                    <td><%= Html.Encode(Model.ClientSubUnitName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><%= Html.LabelFor(model => model.TravelerTypeName)%> </td>
                    <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                    <td></td>
                </tr> 
                <%}else{ %>
                <tr>
                    <td><%= Html.Label(Model.HierarchyType)%> </td>
                    <td><%= Html.Encode(Model.HierarchyItem)%></td>
                    <td></td>
                </tr> 
                <%}%>  
                <% if (Model.SystemUsers.Count > 0 || Model.ClientSubUnits.Count > 0)
                   {
                       if (Model.SystemUsers.Count > 0)
                       {%>
                        <tr>
                            <td colspan="3"><b>Attached System Users</b> - You cannot delete this Team until all System Users are removed</td>
                        </tr> 
                        <% foreach (var item in Model.SystemUsers)
                           { %> 
                            <tr>
                                <td></td>
                                <td>
                                    <%: item.LastName%>,<%if (item.MiddleName != "")
                                                           {%><% =item.MiddleName + " "%> <%}%><%: " " + item.FirstName%>
                                </td>
                                <td></td>
                            </tr>
    
                        <% }
                       }
                       if (Model.ClientSubUnits.Count > 0)
                       {%>
                        <tr>
                            <td colspan="3"><b>Attached Client Sub Units</b> - You cannot delete this Team until all Client Sub Units are removed</td>
                        </tr> 
                        <% foreach (var item in Model.ClientSubUnits) { %> 
                            <tr>
                                <td></td>
                                <td>
                                    <%: item.ClientSubUnitName %>
                                </td>
                                <td></td>
                            </tr>
    
                        <% }
                           
                       }%>
                     <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr> 
                <% } else{%>
                 <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><%=Html.RouteLink("Continue", "Default", new { action = "ConfirmDelete", id = Model.TeamId }, new { @class = "red" })%></td>                
               </tr>
               <% }%>
               
            </table>
            
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_teams').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

