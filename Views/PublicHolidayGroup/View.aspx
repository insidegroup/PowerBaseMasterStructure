<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PublicHolidayGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - PublicHolidayGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">View Public Holiday Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View Public Holiday Group</th> 
		        </tr> 
                <tr>
                    <td>Public Holiday Group Name</td>
                    <td><%= Html.Encode(Model.PublicHolidayGroupName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.ToString("MMM dd, yyyy"))%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
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
                        <td colspan="2"><%= Html.Encode(Model.ClientSubUnitName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                    </tr> 
                    <tr>
                        <td>Traveler Type Name</td>
                        <td colspan="2"><%= Html.Encode(Model.TravelerTypeName)%></td>
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
                        <td colspan="2"><%= Html.Encode(Model.HierarchyItem != null ? Model.HierarchyItem : "")%></td>
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
    $(document).ready(function() {
    $('#menu_publicholidays').click();
	$("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

