<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientProfileAdminItemsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Profile Administration Items</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
	<script src="<%=Url.Content("~/Scripts/ERD/ClientProfileAdminItem.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Profile Administration Items</div>
		</div>
		<div id="content">
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr>
					<td>Group Name</td>
					<td><%=Html.Encode(Model.ClientProfileAdminGroupClientProfileGroupName) %></td>
					<td>GDS</td>
					<td>
						<%=Html.Encode(Model.ClientProfileAdminGroupGDSName) %>
						<%=Html.HiddenFor(model => model.ClientProfileAdminGroupGDSCode) %>
					</td>
					<td>
						<div class="home_button">
							<a href="<%=Url.Content("~/ClientProfileAdminGroup.mvc/ListUnDeleted")%>" class="red">Home</a>
						</div>
					</td>
				</tr>
				<tr>
					<td>Hierarchy</td>
					<td><%=Html.Encode(Model.ClientProfileAdminGroupHierarchyItem) %></td>
					<td>Back Office</td>
					<td><%=Html.Encode(Model.ClientProfileAdminGroupBackOfficeSystemDescription) %></td>
				</tr>
			</table>

			<!-- Display each panel using tabs and show all ClientProfileAdminItems relating to that panel -->
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
				<div id="tabs-1">
					<div id="tabs-1Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form0" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsClientDetails.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsClientDetails[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsClientDetails[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsClientDetails_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsClientDetails[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<input type="hidden" name="count0" id="count0" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome0" value="0" />
						<% } %>
					</div>
				</div>
				<div id="tabs-2">
					<div id="tabs-2Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form1" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsMidOffice.Count; i++){ %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsMidOffice[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsMidOffice[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsMidOffice_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsMidOffice[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<input type="hidden" name="count1" id="count1" value="<%=i%>" />
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<input type="hidden" name="redirectToHome" id="redirectToHome1" value="0" />
						<% } %>
					</div>
				</div>
				<div id="tabs-3">
					<div id="tabs-3Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form2" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsBackOffice.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsBackOffice[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsBackOffice[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsBackOffice_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsBackOffice[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<input type="hidden" name="count2" id="count2" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome2" value="0" />
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<% } %>
					</div>
				</div>
				<div id="tabs-4">
					<div id="tabs-4Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form3" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsAirRailPolicy.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAirRailPolicy_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsAirRailPolicy[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<input type="hidden" name="count3" id="count3" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome3" value="0" />
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<% } %>
					</div>
				</div>
				<div id="tabs-5">
					<div id="tabs-5Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form4" })){%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsItinerary.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsItinerary[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsItinerary[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsItinerary_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %><br />
									<span id="ClientProfileAdminItemsItinerary_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsItinerary[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<input type="hidden" name="count4" id="count4" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome4" value="0" />
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<% } %>
					</div>
				</div>
				<div id="tabs-6">
					<div id="tabs-6Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form5" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItems24Hours.Count; i++) { %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItems24Hours[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItems24Hours[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItems24Hours_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItems24Hours_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItems24Hours[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>

						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<input type="hidden" name="count5" id="count5" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome5" value="0" />
						<% } %>
					</div>
				</div>
				<div id="tabs-7">
					<div id="tabs-7Content">
						<%using (Html.BeginForm("List", "ClientProfileAdminItem", FormMethod.Post, new { @id = "form6" })) {%>
						<% Html.EnableClientValidation(); %>
						<input type="submit" value="Save" class="red" />
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td width="64%"></td>
								<td width="8%">Mandatory</td>
								<td width="5%">Move Status</td>
								<td width="5%">Format</td>
								<td width="5%">Remark</td>
								<td width="5%">Tooltip</td>
								<td width="8%">Hierarchy</td>
							</tr>
							<%
							int i = 0;
							for (i = 0; i < Model.ClientProfileAdminItemsAmadeusTPM.Count; i++){ %>
							<tr>
								<td>
									<span class="clientProfileDataElementName"><%= Html.Encode(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ClientProfileDataElementName)%></span>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ClientProfileDataElementId)%>
									<%= Html.HiddenFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ClientProfileAdminItemId)%>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.MandatoryFlag, new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.CheckBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.MandatoryFlag)%>
									<% } %>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileMoveStatuses, "Please Select...", new {  @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.DropDownListFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ClientProfileMoveStatusId, Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileMoveStatuses, "Please Select...")%>
									<% } %>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.DefaultGDSCommandFormat, new { maxlength = "20" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultGDSCommandFormat<%=i %>"></span>
								</td>
								<td class="uppercase">
									<% if(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.DefaultRemark, new { maxlength = "512" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_DefaultRemark<%=i %>"></span>
								</td>
								<td>
									<% if(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.InheritedFlag == true) { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256", @disabled = "disabled"})%>
									<% } else { %>
										<%= Html.TextBoxFor(model => Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.ToolTip, new { maxlength = "256" })%>
									<% } %>
									<br />
									<span id="ClientProfileAdminItemsAmadeusTPM_ClientProfileAdminItem_ToolTip<%=i %>"></span>
								</td>
								<td><%= Html.Encode(Model.ClientProfileAdminItemsAmadeusTPM[i].ClientProfileAdminItem.Source)%></td>
							</tr>
							<% } %>
						</table>
						<input type="hidden" name="count6" id="count6" value="<%=i%>" />
						<input type="hidden" name="redirectToHome" id="redirectToHome6" value="0" />
						<%= Html.HiddenFor(model => Model.ClientProfileAdminGroupId)%>
						<%= Html.HiddenFor(model => Model.ClientProfilePanelId)%>
						<% } %>
					</div>
				</div>

			</div>
			<input type="hidden" name="dataChanged" id="dataChanged" value="0" />
		</div>
	</div>
</asp:Content>
