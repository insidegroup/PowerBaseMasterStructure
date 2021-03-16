<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingClientSubUnitCreditCardsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting Groups - Credit Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups - Credit Cards</div></div>
    <div id="content">
        <table width="100%">
            <tr>
                <td valign="top" width="50%" style="padding:0">
                     <table class="tablesorter_other2">
                        <thead>
							<tr>
								<th colspan="6">Available Credit Cards</th>
							</tr>
							 <tr>
								<td>Product</td>
								<td>Supplier</td>
								<td>Card Holder</td>
								<td>Card Number</td>
								<td>Card Hierarchy</td>
								<td></td>
							</tr>
						</thead>
                        <% foreach (var item in Model.CreditCardsAvailable){ %>
							<tr>
								<td><%=Html.Encode(item.ProductName) %></td>
								<td><%=Html.Encode(item.SupplierName) %></td>
								<td><%=Html.Encode(item.CreditCardHolderName) %></td>
								<td><%=Html.Encode(item.MaskedCreditCardNumber) %></td>
								<td><%=Html.Encode(item.HierarchyItem) %></td>
								<td>
									<%if (item.HasWriteAccess){%>
										<%if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
											<a href="javascript:addRemoveClientSubUnitTravelerType('<%=item.CreditCardId %>', '<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
												<img src="../../images/add.png" alt="Add" />
											</a>
										<%} else if (item.HierarchyType == "TravelerType"){%>
											<a href="javascript:addRemoveTravelerType('<%=item.CreditCardId %>', '<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
												<img src="../../images/add.png" alt="Add" />
											</a>
										<%} else if (item.HierarchyType == "ClientAccount") {%>
											<a href="javascript:addRemoveClientAccount('<%=item.CreditCardId %>', '<%=item.ClientAccountNumber %>', '<%=item.SourceSystemCode %>')" title="Add this item to the Group">
												<img src="../../images/add.png" alt="Add" />
											</a>
										<%} else if (item.HierarchyType == "ClientSubUnit") {%>
											<a href="javascript:addRemoveClientSubUnit('<%=item.CreditCardId %>', '<%=item.HierarchyType %>', '<%=item.ClientSubUnitGuid %>')" title="Add this item to the Group">
												<img src="../../images/add.png" alt="Add" />
											</a>
										<%} else if (item.HierarchyType == "ClientTopUnit") {%>
											<a href="javascript:addRemoveClientTopUnit('<%=item.CreditCardId %>', '<%=item.HierarchyType %>', '<%=item.ClientTopUnitGuid %>')" title="Add this item to the Group">
												<img src="../../images/add.png" alt="Add" />
											</a>
										<%} %>
									<%}%>
								</td>
							</tr>
                        <% } %>
                    </table>   
                </td>
                <td valign="top" width="50%">
                    <table class="tablesorter_other2">
                        <thead>
							<tr>
								<th colspan="6">Current Linked Credit Cards</th>
							</tr>
							<tr>
								<td>Product</td>
								<td>Supplier</td>
								<td>Card Holder</td>
								<td>Card Number</td>
								<td>Card Hierarchy</td>
								<td></td>
							</tr>
                       </thead>
						<tbody>
							<% foreach (var item in Model.CreditCardsUnAvailable){ %>
							<tr>
								<td><%=Html.Encode(item.ProductName) %></td>
								<td><%=Html.Encode(item.SupplierName) %></td>
								<td><%=Html.Encode(item.CreditCardHolderName) %></td>
								<td><%=Html.Encode(item.MaskedCreditCardNumber) %></td>
								<td><%=Html.Encode(item.HierarchyItem) %></td>
								<td>
									<%if (item.HierarchyType == "ClientSubUnitTravelerType"){%>
										<a href="javascript:addRemoveClientSubUnitTravelerType('<%=item.CreditCardId %>', '<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
											<img src="../../images/remove.png" alt="Remove" />
										</a>
									<%} else if (item.HierarchyType == "TravelerType"){%>
										<a href="javascript:addRemoveTravelerType('<%=item.CreditCardId %>', '<%=item.ClientSubUnitGuid %>', '<%=item.TravelerTypeGuid %>')" title="Add this item to the Group">
											<img src="../../images/remove.png" alt="Remove" />
										</a>
									<%} else if (item.HierarchyType == "ClientAccount") {%>
										<a href="javascript:addRemoveClientAccount('<%=item.CreditCardId %>', '<%=item.ClientAccountNumber %>', '<%=item.SourceSystemCode %>')" title="Add this item to the Group">
											<img src="../../images/remove.png" alt="Remove" />
										</a>
									<%} else if (item.HierarchyType == "ClientSubUnit") {%>
										<a href="javascript:addRemoveClientSubUnit('<%=item.CreditCardId %>', '<%=item.HierarchyType %>', '<%=item.ClientSubUnitGuid %>')" title="Add this item to the Group">
											<img src="../../images/remove.png" alt="Remove" />
										</a>
									<%} else if (item.HierarchyType == "ClientTopUnit") {%>
										<a href="javascript:addRemoveClientTopUnit('<%=item.CreditCardId %>', '<%=item.HierarchyType %>', '<%=item.ClientTopUnitGuid %>')" title="Add this item to the Group">
											<img src="../../images/remove.png" alt="Remove" />
										</a>
									<%} %>
								</td>
							</tr>
							<% } %>
						</tbody>
                    </table>   
                </td>
            </tr>
        </table>
    </div>
</div>
<% using (Html.BeginForm("LinkedClientSubUnitCreditCards", "Meeting", FormMethod.Post, new { id = "hierarchyform"})) { %>
    <%= Html.AntiForgeryToken() %>
    <input type="hidden" id="CreditCardId" name="CreditCardId" value=""/>
	<input type="hidden" id="HierarchyType" name="HierarchyType" value=""/>
    <input type="hidden" id="HierarchyItem" name="HierarchyItem" value=""/>
    <input type="hidden" id="ClientTopUnitGuid" name="ClientTopUnitGuid" value=""/>
	<input type="hidden" id="ClientSubUnitGuid" name="ClientSubUnitGuid" value=""/>
    <input type="hidden" id="TravelerTypeGuid" name="TravelerTypeGuid" value=""/>
    <input type="hidden" id="SourceSystemCode" name="SourceSystemCode" value=""/>
	<input type="hidden" id="ClientAccountNumber" name="ClientAccountNumber" value=""/>
    <%= Html.HiddenFor(model => model.MeetingId) %>
<%}%>
<script src="<%=Url.Content("~/Scripts/ERD/MeetingCreditCardHierarchy.js")%>" type="text/javascript"></script>

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