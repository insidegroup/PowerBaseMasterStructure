<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupHierarchySearchVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Rules Groups
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Rules Groups Hierarchy</div></div>
    <% using (Html.BeginForm("HierarchySearch", null, FormMethod.Post, new { id = "hierarchysearchform", autocomplete = "off" }))
       { %>
    <%= Html.AntiForgeryToken() %>
    <div id="content">
     <table width="100%">
		<tr> 
			<th class="row_header" colspan="3">Add/Remove Client Rules Group</th> 
		</tr> 
        <tr>
		<td valign="top" width="50%">Hierarchy Level:<%= Html.DropDownListFor(model => model.FilterHierarchySearchProperty, Model.HierarchyPropertyOptions, "Please Select...")%><span class="error"> *</span>
			<br />
			<div id="errorMessageSearchProperty" style="color: red"></div>
		</td>
		<td valign="top" width="40%">
			<div class="filter-search">
				Find:
				<%=Html.TextBox("FilterHierarchySearchText", Model.FilterHierarchySearchText, new { autocomplete = "off", @Class ="filterHierarchySearchText" })%>
			</div>
			<div class="csu-search">
				<span class="csu-search-item">Client SubUnit </span><%=Html.TextBox("FilterHierarchyCSUSearchText", Model.FilterHierarchyCSUSearchText, new { autocomplete = "off", @Class ="filterHierarchyCSUSearchText" })%><span class="error"> *</span><br />
				<span class="csu-search-item">TravelerType </span><%=Html.TextBox("FilterHierarchyTTSearchText", Model.FilterHierarchyTTSearchText, new { autocomplete = "off", @Class ="filterHierarchyTTSearchText" })%><span class="error"> *</span>
			</div>
			<br />
			<div id="errorMessageSearchText" style="color: red">
				<%if (Model.AvailableHierarchies != null){ %>
					<%if (Model.AvailableHierarchies.Count == 0){ %>No results match your search criteria<% }%>
				<% }%>
			</div>
		</td>
		<td valign="top" width="10%">
			<input type="submit" value="Search" title="Search" class="red" />
		</td>
	</tr>
    </table>
      <%= Html.HiddenFor(model => model.GroupId)%>
    <%}%>
        <table width="100%">
            <tr>
                <td  valign="top" width="50%">
                    <table  width="100%" id="clientTable1" class="tablesorter_other2" cellspacing="0">
                        <thead>
                        <tr>
                            <th colspan="4">Available <%=Html.Raw(Model.AvailableHierarchyTypeDisplayName) %></th>
                        </tr>
                         <tr>
                            <th>Name</th>
                            <th>Level</th>
                            <th>ID</th>
                            <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
                        </tr>
                       </thead>
                        <%
                            if (Model.AvailableHierarchies != null)
                            {
                                foreach (var item in Model.AvailableHierarchies)
                                { 
                        %>
                        <tr>
                            <td <% if(item.IsClientExpiredFlag == 1) { %>class="expired"<% } %>>
								<% if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
									<%=Html.Encode(item.ClientSubUnitName)%> / <%=Html.Encode(item.ClientSubUnitGuid)%>
								<% } else{ %>
									<%=Html.Encode(CWTStringHelpers.TrimString(item.HierarchyName, 25))%>
								<% } %>
                            </td>
                            <td><%=Html.Encode(CWTStringHelpers.ShortHierarchyType(item.HierarchyType))%></td>
                            <td><% if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
									<%=Html.Encode(item.TravelerTypeName)%> / <%=Html.Encode(item.TravelerTypeGuid)%>
								<% } else if (item.HierarchyType == "ClientAccount")
                                  {%>
                                      <%=Html.Encode(item.SourceSystemCode)%>-<%=Html.Encode(item.ClientAccountNumber)%>
                                 <% }
                                  else
                                  {%>
                                     <%=Html.Encode(item.HierarchyCode)%>
                                 <% }%>
                            </td>
                            <td><%if (Model.HasWriteAccess){ %>
									<%if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
										<a href="javascript:addRemoveClientSubUnitTravelerType('<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
											<img src="../../images/add.png" alt="Add" />
										</a>
									<%} else if (item.HierarchyType == "TravelerType"){%>
										<a href="javascript:addRemoveTravelerType('<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
											<img src="../../images/add.png" alt="Add" />
										</a>
									<%} else if (item.HierarchyType == "ClientAccount") {%>
										<a href="javascript:addRemoveClientAccount('<%=item.ClientAccountNumber %>', '<%=item.SourceSystemCode %>')" title="Add this item to the Group">
											<img src="../../images/add.png" alt="Add" />
										</a>
									<%} else {%>
										<a href="javascript:addRemoveHierarchy('<%=item.HierarchyType %>', '<%=item.HierarchyCode %>')" title="Add this item to the Group">
											<img src="../../images/add.png" alt="Add" />
										</a>
									<%} %>
								<%} %>
                            </td>
                        </tr>
                        <%
                                }
                            }
                        %>
                    </table>   
                </td>
                <td valign="top" width="50%">
                    <table width="100%" id="clientTable2" class="tablesorter_other2" cellspacing="0">
                        <thead>
                        <tr>
                            <th colspan="4">Current Linked Hierarchies</th>
                        </tr>
                         <tr>
                            <th>Name</th>
                            <th>Level</th>
                            <th>ID</th>
                            <td style="background-color:#007886;border: 0;font-size: 10pt;padding: 4px;color:#000000;color:#FFF;"></td>
                        </tr>
                       </thead>
                        <%
                        foreach (var item in Model.LinkedHierarchies){ 
                        %>
                        <tr>
                            <td <% if(item.IsClientExpiredFlag == 1) { %>class="expired"<% } %>>
								<% if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
									<%=Html.Encode(item.ClientSubUnitName)%> / <%=Html.Encode(item.ClientSubUnitGuid)%>
								<% } else{ %>
									<%=Html.Encode(CWTStringHelpers.TrimString(item.HierarchyName,25)) %>
								<% } %>
                            </td>
                            <td><%=Html.Encode(CWTStringHelpers.ShortHierarchyType(item.HierarchyType))%></td>
                            <td><% if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
									<%=Html.Encode(item.TravelerTypeName)%> / <%=Html.Encode(item.TravelerTypeGuid)%>
								<% } else if (item.HierarchyType == "ClientAccount"){%>
                                      <%=Html.Encode(item.SourceSystemCode)%>-<%=Html.Encode(item.ClientAccountNumber)%>
                                 <% }else{%>
                                     <%=Html.Encode(item.HierarchyCode) %>
                                 <% }%></td>
                            <td><%if (Model.HasWriteAccess && Model.LinkedHierarchiesTotal > 1)
                                  { %>
                                  <%if (item.HierarchyType == "ClientSubUnitTravelerType")
                                    {%>
                                  <a href="javascript:addRemoveClientSubUnitTravelerType('<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Remove this item from the Group"><img src="../../images/remove.png" alt="Remove" /></a>
                                  <%}else if (item.HierarchyType == "ClientAccount")
                                    {%>
                                  <a href="javascript:addRemoveClientAccount('<%=item.ClientAccountNumber %>', '<%=item.SourceSystemCode %>')" title="Remove this item from the Group"><img src="../../images/remove.png" alt="Remove" /></a>
                                  <%}else
                                    {%>
                                  <a href="javascript:addRemoveHierarchy('<%=item.HierarchyType %>', '<%=item.HierarchyCode %>')" title="Remove this item from the Group"><img src="../../images/remove.png" alt="Remove" /></a>
                                  <%} %>
                                  <%} %></td>
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
<% using (Html.BeginForm("AddRemoveHierarchy", null, FormMethod.Post, new { id = "hierarchyform"})) { %>
    <%= Html.AntiForgeryToken() %>
    <input type="hidden" id="HierarchyType" name="HierarchyType" value=""/>
    <input type="hidden" id="HierarchyItem" name="HierarchyItem" value=""/>
    <input type="hidden" id="FilterHierarchySearchText" name="FilterHierarchySearchText" value=""/>
    <input type="hidden" id="FilterHierarchySearchProperty" name="FilterHierarchySearchProperty" value=""/>
    <input type="hidden" id="FilterHierarchyCSUSearchText" name="FilterHierarchyCSUSearchText" value=""/>
    <input type="hidden" id="FilterHierarchyTTSearchText" name="FilterHierarchyTTSearchText" value=""/>
    <input type="hidden" id="HierarchyCode" name="HierarchyCode" value=""/>
    <input type="hidden" id="TravelerTypeGuid" name="TravelerTypeGuid" value=""/>
    <input type="hidden" id="ClientSubUnitGuid" name="ClientSubUnitGuid" value=""/>
    <input type="hidden" id="SourceSystemCode" name="SourceSystemCode" value=""/>
    <%= Html.HiddenFor(model => model.GroupId)%>
    <%= Html.HiddenFor(model => model.GroupType)%>
<%}%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
 <link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedRuleGroupHierarchy.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Client Rules Groups", "ListUndeleted", "ClientDefinedRuleGroup")%> &gt;
<%= Html.RouteLink(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName, "Default", new { action = "View", id = Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId }, new { title = "View" })%>
</asp:Content>