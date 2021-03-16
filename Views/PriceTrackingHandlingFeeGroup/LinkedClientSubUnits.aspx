<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PriceTrackingHandlingFeeGroupClientSubUnitCountriesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Price Tracking Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Price Tracking Groups Hierarchy</div></div>
    <div id="content">
   
        <table width="100%">
            <tr>
                <td  valign="top" width="50%">
                     <table class="tablesorter_other2">
                        <thead>
                        <tr>
                            <th colspan="4">Available ClientSubUnits</th>
                        </tr>
                        
                         <tr>
                           <td>SubUnit Name</td>
                            <td>Country</td>
                            <td>GUID</td>
                            <td></td>
                        </tr>
                       </thead>
                        <%
                        foreach (var item in Model.ClientSubUnitsAvailable){ 
                        %>
                        <tr>
                            <td<% if(item.IsClientExpiredFlag == 1) { %> class="expired"<% } %>><%=Html.Encode(item.ClientSubUnitName) %></td>
                            <td><%=Html.Encode(item.CountryName) %></td>
                            <td><%=Html.Encode(item.ClientSubUnitGuid) %></td>
                            <td><%if (item.HasWriteAccess){%><a href="javascript:addRemoveClientSubUnit('<%=item.ClientSubUnitGuid %>')" title="Add this item to the Group">Add</a><%}%></td>
                        </tr>
                        <%
                        }
                        %>
                    </table>   
                </td>
                <td valign="top" width="50%">
                    <table class="tablesorter_other2">
                        <thead>
                            <tr>
                                <th colspan="4">Current Linked ClientSubUnits</td>
                            </tr>
                             <tr>
                                <td>SubUnit Name</td>
                                <td>Country</td>
                                <td>GUID</td>
                                <td></td>
                            </tr>
                       </thead>
                        <%
                        foreach (var item in Model.ClientSubUnitsUnAvailable){ 
                        %>
                        <tr>
                            <td <% if(item.IsClientExpiredFlag == 1) { %>class="expired"<% } %>><%=Html.Encode(item.ClientSubUnitName) %></td>
                            <td><%=Html.Encode(item.CountryName) %></td>
                            <td><%=Html.Encode(item.ClientSubUnitGuid) %></td>
                            <td><%if (Model.ClientSubUnitsUnAvailable.Count > 1 && item.HasWriteAccess)
                                  { %><a href="javascript:addRemoveClientSubUnit('<%=item.ClientSubUnitGuid %>')" title="Remove this item from the Group">Remove</a><%} %></td>
                        </tr>
                        <%
                        }
                        %>
                    </table>   
                </td>
            </tr>
        </table>
    </div>
</div>
<% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "hierarchyform"})) { %>
    <%= Html.AntiForgeryToken() %>
    <input type="hidden" id="ClientSubUnitGuid" name="ClientSubUnitGuid" value=""/>
    <%= Html.HiddenFor(model => model.PriceTrackingHandlingFeeGroupId) %>
<%}%>
<script src="<%=Url.Content("~/Scripts/ERD/PriceTrackingHandlingFeeGroupHierarchy.js")%>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
 <link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Price Tracking Groups", "Main", new { controller = "PriceTrackingHandlingFeeGroup", action = "ListUnDeleted", }, new { title = "Price Tracking Groups" })%> &gt;
<%=Html.RouteLink(Model.PriceTrackingHandlingFeeGroupName, "Default", new { controller = "PriceTrackingHandlingFeeGroup", action = "View", id = Model.PriceTrackingHandlingFeeGroupId }, new { title = Model.PriceTrackingHandlingFeeGroupName })%> &gt;
Hierarchy
</asp:Content>