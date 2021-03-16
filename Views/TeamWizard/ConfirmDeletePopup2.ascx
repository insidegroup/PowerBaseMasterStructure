<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.TeamLinkedItemsVM>" %>


   <h2>Team Details</h2>
    <% bool hasAttachedItems = false; %>
 
<div id="divClientSubUnit">
<table width="100%" class="tablesorter_other3">
<%
    if (Model.Addresses.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Addresses</th>
    </tr>
    <% foreach (var item in Model.Addresses)
       { %> 
    <tr>
        <td><%: item.FirstAddressLine %>, <%: item.CityName %>, <%: item.CountryName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.Contacts.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Contacts</th>
    </tr>
    <% foreach (var item in Model.Contacts){ %> 
    <tr>
        <td><%: item.LastName %>, <%: item.FirstName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.CreditCards.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Credit Cards</th>
    </tr>
    <% foreach (var item in Model.CreditCards){ %> 
    <tr>
        <td><%: item.CreditCardHolderName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.ExternalSystemParameters.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached External System Parameters</th>
    </tr>
    <% foreach (var item in Model.ExternalSystemParameters){ %> 
    <tr>
        <td><%: item.ExternalSystemParameterValue %></td>
    </tr>  
    <% }%>
<%
}
    if (Model.ExternalSystemLogins.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached External System Logins</th>
    </tr>
    <% foreach (var item in Model.ExternalSystemLogins)
       { %> 
    <tr>
        <td><%: item.ExternalSystemLoginName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.GDSAdditionalEntries.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached GDS Additional Entries</th>
    </tr>
    <% foreach (var item in Model.GDSAdditionalEntries){ %> 
    <tr>
        <td><%: item.GDSAdditionalEntryValue %></td>
    </tr>  
    <% }%>
<%
}
if (Model.LocalOperatingHoursGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Local Operating Hours Groups</th>
    </tr>
    <% foreach (var item in Model.LocalOperatingHoursGroups){ %> 
    <tr>
        <td><%: item.LocalOperatingHoursGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PNROutputGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached PNR Output Groups</th>
    </tr>
    <% foreach (var item in Model.PNROutputGroups){ %> 
    <tr>
        <td><%: item.PNROutputGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PolicyGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Policy Groups</th>
    </tr>
    <% foreach (var item in Model.PolicyGroups){ %> 
    <tr>
        <td><%: item.PolicyGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.PublicHolidayGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached PublicHolidayGroups</th>
    </tr>
    <% foreach (var item in Model.PublicHolidayGroups){ %> 
    <tr>
        <td><%: item.PublicHolidayGroupName %></td>
    </tr>  
    <% }%>
<%
}
if (Model.QueueMinderGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Queue Minder Groups</th>
    </tr>
    <% foreach (var item in Model.QueueMinderGroups){ %> 
    <tr>
        <td><%: item.QueueMinderGroupName %></td>
    </tr>  
    <% }%>
<%
}
    if (Model.ServicingOptionGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Servicing Option Groups</th>
    </tr>
    <% foreach (var item in Model.ServicingOptionGroups){ %> 
    <tr>
        <td><%: item.ServicingOptionGroupName %></td>
    </tr>  
    <% }%>
<%
}
    if (Model.TicketQueueGroups.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Ticket Queue Groups</th>
    </tr>
    <% foreach (var item in Model.TicketQueueGroups)
       { %> 
    <tr>
        <td><%: item.TicketQueueGroupName%></td>
    </tr>  
    <% }%>
<%
}
if (Model.ValidPseudoCityOrOfficeIds.Count > 0){ 
    hasAttachedItems = true;
%>
    <tr>
        <th>Attached Valid PseudoCity Or OfficeIds</th>
    </tr>
    <% foreach (var item in Model.ValidPseudoCityOrOfficeIds){ %> 
    <tr>
        <td><%: item.PseudoCityOrOfficeId %></td>
    </tr>  
    <% }%>
<%
}
    if (!hasAttachedItems)
    {
    %><tr>
        <td>There are no other items attached to this Team</td>
    </tr>  <%
    }
%>


</table>
</div>
    


