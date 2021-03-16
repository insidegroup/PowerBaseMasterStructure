<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text"><%= Html.Encode(Model.FeeTypeDisplayName)%>s</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">UnDelete Client <%= Html.Encode(Model.FeeTypeDisplayName)%></th> 
		        </tr> 
                  <tr>
                     <td>Group Name</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.ClientFeeGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Display Name</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.DisplayName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.EnabledFlag)%></td>
                    <td></td>
                </tr>  
               <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.EnabledDate.HasValue ? Model.ClientFeeGroup.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.ExpiryDate.HasValue ? Model.ClientFeeGroup.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Trip Type</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.TripType)%></td>
                    <td></td>
                </tr> 
               <tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Deleted Date/Time</td>
                    <td><%= Html.Encode(Model.ClientFeeGroup.DeletedDateTime)%></td>
                    <td></td>
                </tr> 
                <% if (Model.ClientFeeGroup.IsMultipleHierarchy) { %>
                    <tr>
                        <td colspan="3">Current Linked Hierarchies</td>
                    </tr> 
                    <tr>
                        <td colspan="3"> 
                            <table cellpadding="0" cellspacing="4" border="0" width="100%"  class="hierarchyTable">
                                <tbody>
                                    <% foreach (KeyValuePair<string, List<CWTDesktopDatabase.Models.MultipleHierarchy>> item in Model.ClientFeeGroup.MultipleHierarchies) {%>
                                        <% if(item.Value.Count > 0) { %>
                                            <tr>
                                                <td width="10%">&nbsp;</td>
                                                <td width="20%"><%= Html.Raw(item.Key) %></td>
                                                <td width="70%">
                                                    <% foreach (CWTDesktopDatabase.Models.MultipleHierarchy value in item.Value) {%>
                                                        <%= Html.Raw(!string.IsNullOrEmpty(value.Name) ? value.Name : "") %><%= Html.Raw(!string.IsNullOrEmpty(value.ParentName) ? ", " + value.ParentName : "") %><%= Html.Raw(!string.IsNullOrEmpty(value.GrandParentName) ? ", " + value.GrandParentName : "") %><br />
                                                    <% } %>
                                                </td>
                                            </tr>
                                        <% } %>
                                    <% } %>
                                </tbody>
                            </table>
                        </td>
                    </tr> 
                <% } else { %>
                    <tr>
                        <td><label for="HierarchyType">Hierarchy Type</label> </td>
                        <td colspan="2"><%= Html.Encode(Model.ClientFeeGroup.HierarchyType)%></td>
                    </tr>
                    <% if (Model.ClientFeeGroup.HierarchyType == "ClientSubUnitTravelerType"){ %>
                            <tr>
                                <td>Client Subunit Name</td>
                                <td><%= Html.Encode(Model.ClientFeeGroup.ClientSubUnitName)%>, <%= Html.Encode(Model.ClientFeeGroup.ClientTopUnitName) %></td>
                                <td></td>
                            </tr> 
                            <tr>
                                <td>Traveler Type Name</td>
                                <td><%= Html.Encode(Model.ClientFeeGroup.TravelerTypeName)%></td>
                                <td></td>
                            </tr> 
                    <% } else if (Model.ClientFeeGroup.HierarchyType == "ClientSubUnit"){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.ClientFeeGroup.HierarchyItem)%>, <%= Html.Encode(Model.ClientFeeGroup.ClientTopUnitName) %></td>
                        </tr> 
                    <% } else if (Model.ClientFeeGroup.HierarchyType == "ClientAccount"){ %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td colspan="2"><%= Html.Encode(Model.ClientFeeGroup.HierarchyItem)%>, <%= Html.Encode(Model.ClientFeeGroup.HierarchyCode)%>, <%= Html.Encode(Model.ClientFeeGroup.SourceSystemCode)%></td>
                        </tr> 
                    <%} else { %>
                        <tr>
                            <td>Hierarchy Item</td>
                            <td><%= Html.Encode(Model.ClientFeeGroup.HierarchyItem != null ? Model.ClientFeeGroup.HierarchyItem : "")%></td>
                            <td></td>
                        </tr> 
                    <% } %>
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
                        <input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
                        <%= Html.HiddenFor(model => model.ClientFeeGroup.ClientFeeGroupId)%>
                        <%= Html.HiddenFor(model => model.ClientFeeGroup.VersionNumber)%>
                    <%}%>
                    </td>
               </tr>
            </table>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clientfeegroups').click();
        $("#content > table > tbody > tr:odd").addClass("row_odd");
        $("#content > table > tbody > tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

