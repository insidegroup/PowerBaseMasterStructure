<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Business Rules Group</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Business Rules Group</div></div>
     <div id="content">

        <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		    <tr> 
			    <th class="row_header" colspan="5">Delete Rules Group</th> 
		    </tr> 
		        <tr>
                <th><label for="ClientDefinedRuleGroup_ClientDefinedRuleGroupName">Rules Group Name</label></th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName) %></td>
                <th><label for="ClientDefinedRuleGroup_EnabledFlag">Enabled?</label></th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.EnabledFlag)%></td>
                <td>
					<div class="home_button">
						<%= Html.ActionLink("Home", "ListUndeleted", "ClientDefinedBusinessRuleGroup", null, new { @Class = "red" })%>
					</div>
                </td>
			</tr>  
                <tr>
                <th><label for="ClientDefinedRuleGroup_ClientDefinedRuleBusinessEntityCategory">Category</label></th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.Category)%></td>
                <th><label for="ClientDefinedRuleGroup_EnabledDate">Enabled Date</label></th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.EnabledDate.HasValue ? Model.ClientDefinedRuleGroup.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td>&nbsp;</td>
            </tr> 
            <tr>
				<td>&nbsp;</td>
                <td>&nbsp;</td>
				<th><label for="ClientDefinedRuleGroup_ExpiryDate">Expiration Date</label> </th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.ExpiryDate.HasValue ? Model.ClientDefinedRuleGroup.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
				<td>&nbsp;</td>
                <td>&nbsp;</td>
				<th><label for="ClientDefinedRuleGroup_TripTypeID">Trip Type</label></th>
                <td><%= Html.Encode(Model.ClientDefinedRuleGroup.TripType != null ? Model.ClientDefinedRuleGroup.TripType.TripTypeDescription : "None")%></td>
                <td>&nbsp;</td>
            </tr>  
            <tr>
                <td class="row_footer_left"></td>
                <td colspan="3" class="row_footer_centre"></td>
                <td class="row_footer_right"></td>
            </tr>
		</table>

		<!--Business Rule Conditions-->
		<table cellpadding="0" border="0" width="100%" cellspacing="0" class="business-rules-table">
			<thead>
				<tr class="business-rules-table-sub-header">
					<td colspan="5">Business Rule Conditions</td>
				</tr>
				<tr class="business-rules-table-header">
					<th>Order</th>
					<th>Conditions Description</th>
					<th>Rule Name</th>
					<th>Operator</th>
					<th>Logic Values</th>
				</tr>
			</thead>
			<tbody>
				<% 
					foreach (var clientDefinedRuleGroupLogic in Model.ClientDefinedRuleGroupLogics) { %>
					<tr id="ClientDefinedRuleGroupLogicRow_<% =Html.Encode(clientDefinedRuleGroupLogic.LogicSequenceNumber) %>" class="clientDefinedRuleGroupLogicRow" valign="top">
						<td width="5%"><%=Html.Encode(clientDefinedRuleGroupLogic.LogicSequenceNumber) %></td>
						<td width="25%"><%= Html.Encode(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntity.BusinessEntityDescription) %></td>
						<td width="20%"><%= Html.Encode(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntity.BusinessEntityName) %></td>
						<td width="25%"><%= Html.Encode(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperator.RelationalOperatorName) %></td>
						<td width="25%"><%= Html.Encode(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue) %></td>
					</tr>
					<%
				}  %>
			</tbody>
		</table>

		<!--Business Rule Results-->
		<table  cellpadding="0" border="0" width="100%" cellspacing="0" class="business-rules-table">
			<thead>
				<tr class="business-rules-table-sub-header">
					<td colspan="3">Business Rule Results</td>
				</tr>
				<tr class="business-rules-table-header">
					<th width="5%">&nbsp;</th>
					<th>Results Description</th>
					<th>Results Values</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var clientDefinedRuleGroupResult in Model.ClientDefinedRuleGroupResults){ %>
					<tr class="clientDefinedRuleResultItemRow" valign="top">
						<td width="5%">&nbsp;</td>
						<td width="70%">
							<% if (clientDefinedRuleGroupResult.ClientDefinedRuleResultItem != null && clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntity != null) {
                                var clientDefinedRuleBusinessEntity = clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntity; %>
                                <% if (clientDefinedRuleBusinessEntity.BusinessEntityDescription != null && !string.IsNullOrEmpty(clientDefinedRuleBusinessEntity.BusinessEntityDescription)) { %>
                                    <%= Html.Encode(clientDefinedRuleBusinessEntity.BusinessEntityDescription) %>
                                <% } else if (clientDefinedRuleBusinessEntity.BusinessEntityName != null && !string.IsNullOrEmpty(clientDefinedRuleBusinessEntity.BusinessEntityName)) { %>
                                    <%= Html.Encode(clientDefinedRuleBusinessEntity.BusinessEntityName) %>
                                <% } %>
                            <% } %>
						</td>
						<td width="25%"><%= Html.Encode(clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue) %></td>
					</tr>
				<% }  %>
			</tbody>
		</table>
      
		<!--Business Rule Triggers-->
		<table  cellpadding="0" border="0" width="100%" cellspacing="0" class="business-rules-table">
			<thead>
				<tr class="business-rules-table-sub-header">
					<td colspan="3">Business Rule Triggers</td>
				</tr>
				<tr class="business-rules-table-header">
					<th width="5%">&nbsp;</th>
					<th>Workflow Trigger State</th>
					<th>Application Mode</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var clientDefinedRuleGroupTrigger in Model.ClientDefinedRuleGroupTriggers) { %>
					<tr class="clientDefinedRuleWorkflowTriggerRow" valign="top">
						<td width="5%">&nbsp;</td>
						<td width="45%"><%= Html.Encode(clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerState.ClientDefinedRuleWorkflowTriggerStateName)%></td>
						<td width="50%"><%= Html.Encode(clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationMode.ClientDefinedRuleWorkflowTriggerApplicationModeName)%></td>
					</tr>
				<% }  %>
			</tbody>
		</table>

		<table cellpadding="0" border="0" width="100%" cellspacing="0">
			<tr>
				<td class="row_footer_blank_left" width="80%">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>
				</td>                    
				<td class="row_footer_blank_right" width="20%">
					<% using (Html.BeginForm()) { %>
						<%= Html.AntiForgeryToken() %>
						<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
						<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.VersionNumber) %>
						<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId) %>
					<%}%>
				</td>
			</tr>
		</table>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedRuleGroupCreate.js")%>" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#breadcrumb').css('width', 'auto');
			$('.full-width #search_wrapper').css('height', '22px');
		})
	 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Business Rules Group", "ListUndeleted", "ClientDefinedBusinessRuleGroup")%> &gt;
<%= Html.RouteLink(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName, "Default", new { action = "View", id = Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId }, new { title = "View" })%> &gt;
Items
</asp:Content>