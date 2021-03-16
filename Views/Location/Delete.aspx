<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.LocationDeleteVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Locations</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Location</th> 
		        </tr> 
                <tr>
                    <td>Location Name</td>
                    <td><%= Html.Encode(Model.Location.LocationName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.Location.CountryName)%></td>
                    <td></td>
                </tr>   
                <tr>
                    <td>Country Region</td>
                    <td><%= Html.Encode(Model.Location.CountryRegionName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.Encode(Model.Address.FirstAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Second Address Line</td>
                    <td><%= Html.Encode(Model.Address.SecondAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.Address.CityName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>State/Province</td>
                    <td><%= Html.Encode(Model.Address.StateProvinceName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.Address.PostalCode)%></td>
                    <td></td>
                </tr>
                <%
                bool hasAttachedItems = false;
                if(Model.SystemUsers.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached System Users</b> - You cannot delete this Location until all System Users are removed</td>
                </tr> 
                    <% foreach (var item in Model.SystemUsers) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.LastName %>,<%if(item.MiddleName!=""){%><% =item.MiddleName + " "%> <%}%><%: " " + item.FirstName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>
                <% if(Model.Teams.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Teams</b> - You cannot delete this Location until all Teams are removed</td>
                </tr> 
                    <% foreach (var item in Model.Teams) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.TeamName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.Contacts.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Contacts</b> - You cannot delete this Location until all Contacts are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.Contacts) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.ContactTypeName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                 <% if(Model.LinkedItems.Addresses.Count>0) { 
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Addresses</b></td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.Addresses) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.FirstAddressLine %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>


                <% if(Model.LinkedItems.CreditCards.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Credit Cards</b> - You cannot delete this Location until all Credit Cards are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.CreditCards) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.CreditCardHolderName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.ExternalSystemParameters.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached External System Parameters</b> - You cannot delete this Location until all External System Parameters are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.ExternalSystemParameters) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.ExternalSystemParameterValue %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.GDSAdditionalEntries.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached GDS Additional Entries</b> - You cannot delete this Location until all GDS Additional Entries are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.GDSAdditionalEntries) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.GDSAdditionalEntryValue %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.LocalOperatingHoursGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Local Operating Hours Groups</b> - You cannot delete this Location until all Local Operating Hours Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.LocalOperatingHoursGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.LocalOperatingHoursGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.PNROutputGroups.Count>0) { 
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached PNR Output Groups</b></td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.PNROutputGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.PNROutputGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.PolicyGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Policy Groups</b> - You cannot delete this Location until all Policy Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.PolicyGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.PolicyGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.PublicHolidayGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Public Holiday Groups</b> - You cannot delete this Location until all Public Holiday Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.PublicHolidayGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.PublicHolidayGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.QueueMinderGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Queue Minder Groups</b> - You cannot delete this Location until all Queue Minder Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.QueueMinderGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.QueueMinderGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.ServicingOptionGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Servicing Option Groups</b> - You cannot delete this Location until all Servicing Option Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.ServicingOptionGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.ServicingOptionGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.TicketQueueGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Ticket Queue Groups</b> - You cannot delete this Location until all Ticket Queue Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.TicketQueueGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.TicketQueueGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.TripTypeGroups.Count>0) { 
                       hasAttachedItems = true;
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Trip Type Groups</b> - You cannot delete this Location until all Trip Type Groups are removed</td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.TripTypeGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.TripTypeGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.ValidPseudoCityOrOfficeIds.Count>0) { 
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached Valid PseudoCity Or OfficeIds</b></td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.ValidPseudoCityOrOfficeIds) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.PsuedoCityOrOfficeIdDescription %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>

                <% if(Model.LinkedItems.WorkFlowGroups.Count>0) { 
                %>
                <tr>
                    <td></td>
                    <td colspan="2"><b>Attached WorkFlow Groups</b></td>
                </tr> 
                    <% foreach (var item in Model.LinkedItems.WorkFlowGroups) { %> 
                        <tr>
                            <td></td>
                            <td>
                                <%: item.WorkFlowGroupName %>
                            </td>
                            <td></td>
                        </tr>
    
                    <% }%>
                <% }%>
                <%if(hasAttachedItems){ %>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr> 
                <% } else{%>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2"><%=Html.RouteLink("Continue", "Default", new { action = "ConfirmDelete", id = Model.Location.LocationId }, new { @class = "red" })%></td>                
               </tr>
               <% }%>
            </table>
           

            
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_admin').click();
		$("tr:odd").addClass("row_odd");
	    $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Locations", "Main", new { controller = "Location", action = "List", }, new { title = "Locations" })%> &gt;
Delete Location &gt;
<%= Html.Encode(Model.Location.LocationName) %>
</asp:Content>