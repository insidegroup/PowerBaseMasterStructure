<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileItemsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Builder</asp:Content>
<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
	<script src="<%=Url.Content("~/Scripts/ERD/ClientProfileItem.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Profile Builder</div>
		</div>
		<div id="content">
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td width="15%">Client Top</td>
					<td width="15%"><%=Html.Encode(ViewData["ClientTopUnitName"]) %></td>
					<td width="15%">GDS</td>
					<td width="15%"><%=Html.Encode(Model.ClientProfileGroup.GDSCode) %><%=Html.HiddenFor(model => Model.ClientProfileGroup.GDSCode) %></td>
					<td width="15%">Last Published</td>
					<td width="20%"><%=Html.Encode(Model.ClientProfileGroup.LastRevisionPublishDate) %></td>
				</tr>
				<tr>
					<td>Client SubUnit</td>
					<td><%=Html.Encode(Model.ClientProfileGroupHierarchyItem) %></td>
					<td>PCC/Office Id</td>
					<td><%=Html.Encode(Model.ClientProfileGroup.PseudoCityOrOfficeId) %></td>
					<td>Last Saved</td>
					<td><%=Html.Encode(Model.ClientProfileGroup.LastUpdateTimestamp) %></td>
				</tr>
				<tr>
					<td>Profile Hierarchy</td>
					<td><%=Html.Encode(Model.ClientProfileGroup.HierarchyType) %></td>
					<td>Profile Name</td>
					<td><%=Html.Encode(Model.ClientProfileGroup.ClientProfileGroupName) %></td>
					<td colspan="2" align="left">
						<div class="home_button">
							<a href="<%=Url.Content("~/ClientProfileGroup.mvc/ListUnDeleted")%>" class="red">Home</a>
						</div>
					</td>
				</tr>
				<tr>
					<td colspan="6">
						<%= Html.ActionLink("Client Profiles", "ListUnDeleted", "ClientProfileGroup", null, new { title = "Client Profiles" })%> &gt;
						<%=Html.RouteLink(Model.ClientProfileGroup.UniqueName, "Default", new { controller = "ClientProfileGroup", action = "Edit", id = Model.ClientProfileGroup.ClientProfileGroupId }, new { title = Model.ClientProfileGroup.UniqueName }) %> &gt; 
						Client Profile Builder
					</td>
				</tr>
			</table>

			<div id="tabs">
				<ul>
					<li><a href="#tabs-1" id="clientDetails">Client Details</a></li>
					<li><a href="#tabs-2" id="midOffice">Mid-Office</a></li>
					<li><a href="#tabs-3" id="backOffice">Back-Office</a></li>
					<li><a href="#tabs-4" id="airRailLandPolicy">Air/Rail/Land Policy</a></li>
					<li><a href="#tabs-5" id="itinerary">Itinerary</a></li>
					<li><a href="#tabs-6" id="24hours">24 Hours</a></li>
					<li><a href="#tabs-7" id="amadeusTPM">Amadeus TPM</a></li>
				</ul>
				
				<% /* ClientProfileItemsClientDetails */ %>
				<div id="tabs-1">
					<div id="tabs-1Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form0" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsClientDetails.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileDataElementName)%></span></span>
									<span id="ClientProfileItemsClientDetails_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsClientDetails[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsClientDetails[i].ClientProfileItem.InheritedMoveStatusFlag== true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsClientDetails[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsClientDetails[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsClientDetails_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsClientDetails[i].ClientProfileItem.InheritedGDSCommandFormat== true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsClientDetails_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsClientDetails[i].ClientProfileItem.InheritedRemark== true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsClientDetails[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsClientDetails[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsClientDetails_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsClientDetails[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsClientDetails[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count0" id="count0" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome0" value="0" />
						<br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

				<% /* ClientProfileItemsMidOffice */ %>
				<div id="tabs-2">
					<div id="tabs-2Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form1" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsMidOffice.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItemsMidOffice_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsMidOffice[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsMidOffice[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsMidOffice[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsMidOffice[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsMidOffice_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsMidOffice[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsMidOffice_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsMidOffice[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsMidOffice[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsMidOffice[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsMidOffice_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsMidOffice[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsMidOffice[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count1" id="count1" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome1" value="0" />
                        <br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

				<% /* ClientProfileItemsBackOffice */ %>
				<div id="tabs-3">
					<div id="tabs-3Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form2" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsBackOffice.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItemsBackOffice_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsBackOffice[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsBackOffice[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsBackOffice[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsBackOffice[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsBackOffice_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsBackOffice[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsBackOffice_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsBackOffice[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsBackOffice[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsBackOffice[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsBackOffice_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsBackOffice[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsBackOffice[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count2" id="count2" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome2" value="0" />
                        <br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

				<% /* ClientProfileItemsAirRailPolicy */ %>
				<div id="tabs-4">
					<div id="tabs-4Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form3" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsAirRailPolicy.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItemsAirRailPolicy_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsAirRailPolicy[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsAirRailPolicy[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsAirRailPolicy_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsAirRailPolicy_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsAirRailPolicy_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsAirRailPolicy[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count3" id="count3" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome3" value="0" />
                        <br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

				<% /* ClientProfileItemsItinerary */ %>
				<div id="tabs-5">
					<div id="tabs-5Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form4" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsItinerary.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItemsItinerary_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsItinerary[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsItinerary[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsItinerary[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsItinerary[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsItinerary_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsItinerary[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsItinerary_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsItinerary[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsItinerary[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsItinerary[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsItinerary[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsItinerary_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsItinerary[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsItinerary[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count4" id="count4" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome4" value="0" />
                        <br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>				

				<% /* ClientProfileItems24Hours */ %>
				<div id="tabs-6">
					<div id="tabs-6Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form5" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItems24Hours.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItems24Hours_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItems24Hours[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItems24Hours[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItems24Hours[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItems24Hours[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItems24Hours_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItems24Hours[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItems24Hours_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItems24Hours[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItems24Hours[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItems24Hours[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItems24Hours[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItems24Hours_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItems24Hours[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItems24Hours[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count5" id="count5" value="<%=i%>" />
                        <input type="hidden" name="redirectToHome" id="redirectToHome5" value="0" />
						<br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

				<% /* ClientProfileItemsAmadeusTPM */ %>
				<div id="tabs-7">
					<div id="tabs-7Content">
						<%using (Html.BeginForm("List", "ClientProfileItem", FormMethod.Post, new { @id = "form6" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="30%"></td>
								<td width="15%">Move Status</td>
								<td width="20%">Format</td>
								<td width="20%">Remark</td>
								<td width="15%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileItemsAmadeusTPM.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileDataElementName)%></span>
									<span id="ClientProfileItemsAmadeusTPM_ClientProfileItem_ClientProfileDataElementName<%=i %>" class="required">
										<% if (Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.MandatoryFlag == true) { %>*<% } %>
									</span>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileGroupId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileAdminItemId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.MandatoryFlag)%>
								</td>
								<td>
									<% if (Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.InheritedMoveStatusFlag == true){ %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsAmadeusTPM[i].ClientProfileMoveStatuses, "Please Select...", new { @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileMoveStatusId) %>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ClientProfileMoveStatusId, Model.ClientProfileItemsAmadeusTPM[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
									<span id="ClientProfileItemsAmadeusTPM_ClientProfileItem_ClientProfileMoveStatusId<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.InheritedGDSCommandFormat == true){ %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20", @disabled = "disabled"})%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.GDSCommandFormat) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.GDSCommandFormat,  new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsAmadeusTPM_ClientProfileItem_GDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if (Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.InheritedRemark == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ToolTip, @disabled = "disabled" })%>
										<%= Html.HiddenFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.Remark) %>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.Remark, new { maxlength = "512", @title = Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.ToolTip })%>
									<% } %>
									<br />
									<span id="ClientProfileItemsAmadeusTPM_ClientProfileItem_Remark<%=i %>"></span>
								</td>
								<td title="<%= Html.Encode(Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.SourceItem)%>"><%= Html.Encode(Model.ClientProfileItemsAmadeusTPM[i].ClientProfileItem.SourceName)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<%= Html.HiddenFor(model => Model.ClientProfileGroup.LastUpdateTimestamp)%>
						<input type="hidden" name="count6" id="count6" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome6" value="0" />
                        <br />
						<a href="#" class="add-remark red">Add New Remark</a>
						<% } %>
					</div>
				</div>

			</div>
			<%if(ViewData["clientProfileText"] != null && !string.IsNullOrEmpty(ViewData["clientProfileText"].ToString())) { %>
				<textarea rows="20" cols="20" readonly="readonly" id="GDS-window"><%=ViewData["clientProfileText"] %></textarea>
			<% } %>
			<input type="hidden" name="dataChanged" id="dataChanged" value="0" />
		</div>
	</div>
</asp:Content>
