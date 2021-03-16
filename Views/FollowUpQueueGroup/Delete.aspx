<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.QueueMinderGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Group</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Follow Up Queue Group</th> 
		        </tr> 
                <tr>
                    <td>Follow Up Queue Group Name</td>
                    <td><%= Html.Encode(Model.QueueMinderGroupName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Inherit From Parent?</td>
                    <td><%= Html.Encode(Model.InheritFromParentFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Trip Type</td>
                    <td><%= Html.Encode(Model.TripType)%></td>
                    <td></td>
                </tr> 
               <tr>
                    <td>Deleted?</td>
                    <td><%= Html.Encode(Model.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Deleted Date/Time</td>
                    <td><%= Html.Encode(Model.DeletedDateTime)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="MeetingId">Meeting</label> </td>
                    <td><%= Html.Encode((Model.Meeting != null) ? Model.Meeting.MeetingDisplayName : "")%></td>
                    <td></td>
                </tr>
                <% if (Model.IsMultipleHierarchy && Model.ClientSubUnitsHierarchy != null) { %>
                    <tr>
                        <td><label for="HierarchyType">Hierarchy Type</label></td>
                        <td colspan="2">ClientSubUnit</td>
                    </tr>
                    <tr>
                        <td>Client Top Unit Name</td>
                        <td colspan="2"><%= Html.Encode(!string.IsNullOrEmpty(Model.ClientTopUnitName) ? Model.ClientTopUnitName : "")%></td>
                    </tr> 
                    <tr>
                        <td>Client Sub Unit Names</td>
                        <td colspan="2"> 
                            <table cellpadding="4" cellspacing="0" border="0" width="100%" class="hierarchyTable">
                                <tbody>
                                    <% foreach (CWTDesktopDatabase.Models.ClientSubUnit clientSubUnit in Model.ClientSubUnitsHierarchy) {%>
                                        <tr>
                                            <td width="50%"><%= Html.Raw(clientSubUnit.ClientSubUnitName) %></td>
                                            <td width="50%"><%= Html.Raw(clientSubUnit.Country.CountryName) %></td>
                                        </tr>
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
                   <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_ticketqueuegroups').click();
        $("#content > table > tbody > tr:odd").addClass("row_odd");
        $("#content > table > tbody > tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Model.QueueMinderGroupName%>
</asp:Content>