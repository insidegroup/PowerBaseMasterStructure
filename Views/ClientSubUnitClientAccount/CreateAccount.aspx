
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitClientAccountsVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="headerContent" runat="server">
	<link href="<%=Url.Content("~/Style/tablesorter/blue/style.css")%>" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery.tablesorter.js")%>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnit - Client Accounts</div>
		</div>
		<div id="content">
			<div id="divSearch">
				<table class="" cellspacing="0" width="100%">
					<thead>
						<tr>
							<th>Search field:</th>
							<th>
								<select id="ClientAccountFilterField1">
									<option value="PleaseSelect">Please select...</option>
									<option value="ClientAccountName">Client Account Name</option>
									<option value="ClientAccountNumber">Client Account Number</option>
									<option value="CountryName">Country</option>
									<option value="SourceSystemCode">Source System Code</option>
								</select>
								<br />
								<input type="text" value="" id="ClientAccountFilter1" />
							</th>
							<th>
								<select id="ClientAccountFilterField2">
									<option value="PleaseSelect">Please select...</option>
									<option value="ClientAccountName">Client Account Name</option>
									<option value="ClientAccountNumber">Client Account Number</option>
									<option value="CountryName">Country</option>
									<option value="SourceSystemCode">Source System Code</option>
								</select>
								<br />
								<input type="text" value="" id="ClientAccountFilter2" />
							</th>
							<th>
								<select id="ClientAccountFilterField3">
									<option value="PleaseSelect">Please select...</option>
									<option value="ClientAccountName">Client Account Name</option>
									<option value="ClientAccountNumber">Client Account Number</option>
									<option value="CountryName">Country</option>
									<option value="SourceSystemCode">Source System Code</option>
								</select>
								<br />
								<input type="text" value="" id="ClientAccountFilter3" />
							</th>

							<th><span id="clientAccountsSearchButton"><small>Search >> </small></span></th>
							<th><span id="lastSearchAccount"></span></th>
							<th>&nbsp;</th>
						</tr>
					</thead>
				</table>
			</div>

			<table class="tablesorter_other" cellspacing="0">
				<tr>
					<td>
						<div id="ClientAccountSearchResults"></div>
					</td>
					<td>
						<table id="currentAccountsTable" cellspacing="0" class="tablesorter_other2">
							<thead>
								<tr>
									<td colspan="6">Current Accounts</td>
								</tr>
								<tr>
									<th>Account Name</th>
									<th>Acc No</th>
									<th title="System Source Code">SSC</th>
									<th>Country</th>
									<td style="background-color: #007886; border: 0; font-size: 10pt; padding: 4px; color: #000000; color: #FFF;"></td>
								</tr>

							</thead>
							<tbody>
								<% 
									var count = Model.ClientAccounts.Count;
									foreach (var item in Model.ClientAccounts)
									{
								%>
								<tr id="<%: item.ClientAccountNumber %>" ssc="<%: item.SourceSystemCode %>" combinedaccssc="<%: item.ClientAccountNumber + item.SourceSystemCode %>" accountstatus="Current" versionnumber="<%: item.VersionNumber %>">
									<td><%: item.ClientAccountName %></td>
									<td><%: item.ClientAccountNumber %></td>
									<td><%: item.SourceSystemCode %></td>
									<td><%: item.CountryName %></td>
									<td>
										<img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" border="0" /></td>
								</tr>
								<%
								}
							%>
							</tbody>
						</table>
					</td>
				</tr>
			</table>
			
			<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr> 
					<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
				</tr>
			</table>

			<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>

			<input type="hidden" id="RemovedClientAccounts"/>
			<input type="hidden" id="AddedClientAccounts"/>

			<div style="visibility:hidden" id="hiddenTables">
				<table id="hiddenRemovedClientAccountsTable"></table>
				<table id="hiddenAddedClientAccountsTable"></table>
			</div>
				
		</div>
	</div>
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/ERD/ClientSubUnitClientAccountCreateAccount.js")%>"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "View", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Client Accounts" })%> &gt;
Edit Client Account
</asp:Content>