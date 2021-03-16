<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.LocationLinkedItemsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% bool hasAttachedItems = false; %>
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Locations</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Location</th> 
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
                    <td>Location</td>
                    <td><%= Html.Encode(Model.Location.LocationName)%></td>
                    <td></td>
                </tr> 
                <%
if (Model.Contacts.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Contacts</b></td><td></td>
    </tr>
    <% foreach (var item in Model.Contacts){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.LastName %>, <%: item.FirstName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.CreditCards.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Credit Cards</b></td><td></td>
    </tr>
    <% foreach (var item in Model.CreditCards){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.CreditCardHolderName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.ExternalSystemParameters.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached External System Parameters</b></td><td></td>
    </tr>
    <% foreach (var item in Model.ExternalSystemParameters){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.ExternalSystemParameterValue %></td>
    </tr>  
    <% }%>
<%
}
if (Model.GDSAdditionalEntries.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached GDS Additional Entries</b></td><td></td>
    </tr>
    <% foreach (var item in Model.GDSAdditionalEntries){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.GDSAdditionalEntryValue %></td>
    </tr>  
    <% }%>
<%
}
if (Model.LocalOperatingHoursGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Local Operating Hours Groups</th<th></th>
    </tr>
    <% foreach (var item in Model.LocalOperatingHoursGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.LocalOperatingHoursGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PNROutputGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached PNR Output Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.PNROutputGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.PNROutputGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PolicyGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Policy Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.PolicyGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.PolicyGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PublicHolidayGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached PublicHolidayGroups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.PublicHolidayGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.PublicHolidayGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.QueueMinderGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Queue Minder Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.QueueMinderGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.QueueMinderGroupName %></td>
    </tr>  
    <% }%>
<%
}
    if (Model.ServicingOptionGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Servicing Option Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.ServicingOptionGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.ServicingOptionGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.Teams.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Teams</b></td><td></td>
    </tr>
    <% foreach (var item in Model.Teams){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.TeamName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.TicketQueueGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Ticket Queue Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.TicketQueueGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.TicketQueueGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.TripTypeGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Trip Type Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.TripTypeGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.TripTypeGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.ValidPseudoCityOrOfficeIds.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached Valid PseudoCity Or OfficeIds</b></td><td></td>
    </tr>
    <% foreach (var item in Model.ValidPseudoCityOrOfficeIds){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.PseudoCityOrOfficeId %></td>
    </tr>  
    <% }%>
<%
}
if (Model.WorkFlowGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <td></td><td><b>Attached WorkFlow Groups</b></td><td></td>
    </tr>
    <% foreach (var item in Model.WorkFlowGroups){ %> 
    <tr>
        <td></td><td colspan="2"><%: item.WorkFlowGroupName %></td>
    </tr>  
    <% }%>
<%
}
    if (!hasAttachedItems)
    {
    %><tr>
        <td></td><td colspan="2">There are no items attached to this Location</td>
    </tr>  <%
    }
%>
<tr>
    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
    <td class="row_footer_blank_right" colspan="2">
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
        <%= Html.HiddenFor(model => model.Location.VersionNumber)%>
    <%}%>
    </td>                
</tr>
           
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
Delete Location
</asp:Content>