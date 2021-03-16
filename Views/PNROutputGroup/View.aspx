<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PNROutputGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - PNR Output Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">PNR Output Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View PNR Output Group</th> 
		        </tr> 
                <tr>
                    <td><label for="PNROutputGroupName">PNR Output Group Name</label></td>
                    <td><%= Html.Encode(Model.PNROutputGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
                    <td><%= Html.Encode(Model.InheritFromParentFlag)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td> <label for="TripType">Trip type</label></td>
                    <td><%if (Model.TripType != null)
                          {
                              Html.Encode(Model.TripType.TripTypeDescription);
                          }
                          else
                          {
                              %>None<%
                          }%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td> <label for="GDSCode">GDS</label></td>
                    <td><%= Html.Encode(Model.GDSName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td> <label for="PNROutputTypeID">PNR Output Type</label></td>
                    <td><%= Html.Encode(Model.PNROutputTypeName)%></td>
                    <td></td>
                </tr> 
                <% if (Model.IsMultipleHierarchy) { %>
                    <tr>
                        <td colspan="3">Current Linked Hierarchies</td>
                    </tr> 
                    <tr>
                        <td colspan="3"> 
                            <table cellpadding="0" cellspacing="4" border="0" width="100%"  class="hierarchyTable">
                                <tbody>
                                    <% foreach (KeyValuePair<string, List<CWTDesktopDatabase.Models.MultipleHierarchy>> item in Model.MultipleHierarchies) {%>
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
                        <td colspan="2"><%= Html.Encode(Model.HierarchyType)%></td>
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
                <% } %>
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
        //Navigation
        $('#menu_pnroutputs').click();
        $("#content > table > tbody > tr:odd").addClass("row_odd");
        $("#content > table > tbody > tr:even").addClass("row_even");
    })
 </script>
</asp:Content>