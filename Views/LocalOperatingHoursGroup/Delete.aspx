<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.LocalOperatingHoursGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Local Operating Hours Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		  <tr> 
			        <th class="row_header" colspan="3">Delete Local Operating Hours Group</th> 
		        </tr> 
                <tr>
                    <td>Local Operating Hours Group Name</td>
                    <td><%= Html.Encode(Model.LocalOperatingHoursGroupName)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Hierarchy Type</td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                    <tr>
                        <td>Client Subunit Name</td>
                        <td><%= Html.Encode(Model.ClientSubUnitName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                        <td></td>
                    </tr> 
                    <tr>
                        <td>Traveler Type Name</td>
                        <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                        <td></td>
                    </tr> 
                <% } else if (Model.HierarchyType == "TravelerType"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.TravelerTypeName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                    </tr> 
               <% } else if (Model.HierarchyType == "ClientSubUnit"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                    </tr> 
                <% } else if (Model.HierarchyType == "ClientAccount"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.HierarchyCode)%>, <%= Html.Encode(Model.SourceSystemCode)%></td>
                    </tr> 
                <%} else { %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td><%= Html.Encode(Model.HierarchyItem != null ? Model.HierarchyItem : "")%></td>
                        <td></td>
                    </tr> 
                <% } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                   
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" class="red"/>
                         <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
       $(document).ready(function() {
        $('#menu_localoperatinghours').click();
		$("tr:odd").addClass("row_odd");
$("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

