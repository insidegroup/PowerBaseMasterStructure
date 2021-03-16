<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingClientSubUnitCountriesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups - Hierarchy</div></div>
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
                            <td <% if(item.IsClientExpiredFlag == 1) { %>class="expired"<% } %>><%=Html.Encode(item.ClientSubUnitName) %></td>
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
						<tbody>
							<%
							foreach (var item in Model.ClientSubUnitsUnAvailable){ 
							%>
							<tr>
								<td <% if(item.IsClientExpiredFlag == 1) { %>class="expired"<% } %>><%=Html.Encode(item.ClientSubUnitName) %></td>
								<td><%=Html.Encode(item.CountryName) %></td>
								<td><%=Html.Encode(item.ClientSubUnitGuid) %></td>
								<td>
									<%if (Model.ClientSubUnitsUnAvailable.Count > 1 && item.HasWriteAccess) { %>
										<a href="javascript:addRemoveClientSubUnit('<%=item.ClientSubUnitGuid %>')" title="Remove this item from the Group">Remove</a>
									<%} %>
								</td>
							</tr>
							<%
							}
							%>
						</tbody>
                    </table>   
                </td>
            </tr>
        </table>
    </div>
</div>
<% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "hierarchyform"})) { %>
    <%= Html.AntiForgeryToken() %>
    <input type="hidden" id="ClientSubUnitGuid" name="ClientSubUnitGuid" value=""/>
    <%= Html.HiddenFor(model => model.MeetingId) %>
<%}%>
<script src="<%=Url.Content("~/Scripts/ERD/MeetingHierarchy.js")%>" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
 <link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.ClientTopUnitName)%> &gt;
<%=Html.Encode(Model.MeetingName)%> &gt;
Hierarchy
</asp:Content>