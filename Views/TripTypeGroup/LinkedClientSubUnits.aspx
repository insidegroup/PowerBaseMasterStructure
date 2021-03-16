<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TripTypeGroupClientSubUnitCountriesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Trip Type Groups Hierarchy</div></div>
    <div id="content">
        <table>
            <tr>
                <td valign="top" width="50%">
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
                            <td class="wrap-text<% if(item.IsClientExpiredFlag == 1) { %> expired<% } %>"><%=Html.Encode(item.ClientSubUnitName) %></td>
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
                            <th colspan="4">Current Linked ClientSubUnits</th>
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
                            <td class="wrap-text<% if(item.IsClientExpiredFlag == 1) { %> expired<% } %>"><%=Html.Encode(item.ClientSubUnitName) %></td>
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
    <%= Html.HiddenFor(model => model.TripTypeGroupId) %>
<%}%>
<script src="<%=Url.Content("~/Scripts/ERD/TripTypeGroupHierarchy.js")%>" type="text/javascript"></script>
<link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Trip Type Groups", "Main", new { controller = "TripTypeGroup", action = "ListUnDeleted", }, new { title = "Trip Type Groups" })%> &gt;
<%=Html.RouteLink(Model.TripTypeGroupName, "Default", new { controller = "TripTypeGroup", action = "View", id = Model.TripTypeGroupId }, new { title = Model.TripTypeGroupName })%> &gt;
Hierarchy
</asp:Content>