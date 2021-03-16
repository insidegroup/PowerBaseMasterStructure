<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Rules Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client Rules Groups</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		        <tr> 
			        <th class="row_header" colspan="6">Create Rules Group</th> 
		        </tr> 
		         <tr>
                    <th><label for="ClientDefinedRuleGroup_ClientDefinedRuleGroupName">Rules Group Name</label></th>
                    <td><%= Html.TextBoxFor(model => model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName, new { maxlength = "100", autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName)%><label id="lblClientDefinedRuleGroupNameMsg"/></td>
                    <th><label for="ClientDefinedRuleGroup_EnabledFlag">Enabled?</label></th>
                    <td><%= Html.CheckBoxFor(model => model.ClientDefinedRuleGroup.EnabledFlag, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.EnabledFlag)%>
						<div class="home_button">
							<%= Html.ActionLink("Home", "ListUndeleted", "ClientDefinedRuleGroup", new { @Class = "red" })%>
						</div>
                    </td>
				</tr>  
                 <tr>
                    <th><label for="ClientDefinedRuleGroup_ClientDefinedRuleBusinessEntityCategory">Category</label></th>
                    <td><%= Html.DropDownListFor(model => model.ClientDefinedRuleGroup.Category, Model.ClientDefinedRuleBusinessEntityCategories,  "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.Category)%></td>
                    <th><label for="ClientDefinedRuleGroup_EnabledDate">Enabled Date</label></th>
                    <td><%= Html.EditorFor(model => model.ClientDefinedRuleGroup.EnabledDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.EnabledDate)%></td>
                </tr> 
                <tr>
					<th><label for="ClientDefinedRuleGroup_HierarchyType">Hierarchy Type</label></th>
					<td><%= Html.DropDownListFor(model => model.ClientDefinedRuleGroup.HierarchyType, Model.HierarchyTypes, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.HierarchyType)%></td>
					<th><label for="ClientDefinedRuleGroup_ExpiryDate">Expiration Date</label> </th>
                    <td> <%= Html.EditorFor(model => model.ClientDefinedRuleGroup.ExpiryDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.ExpiryDate)%></td>
                </tr>
                <tr>
                    <th><label id="lblHierarchyItem"/>Hierarchy Item</th>
                    <td> <%= Html.TextBoxFor(model => model.ClientDefinedRuleGroup.HierarchyItem, new { disabled="disabled",  size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.HierarchyCode)%>
						<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.SourceSystemCode)%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                    <th><label for="ClientDefinedRuleGroup_TripTypeID">Trip Type</label></th>
                    <td><%= Html.DropDownListFor(model => model.ClientDefinedRuleGroup.TripTypeId, Model.TripTypes, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.TripTypeId)%></td>
                </tr>  
				<tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientDefinedRuleGroup.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td colspan="4">
                        <%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.TravelerTypeGuid)%>
                        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.TravelerTypeGuid)%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
                <tr>
                    <td class="row_footer_left"></td>
                    <td colspan="4" class="row_footer_centre"></td>
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
						<th>Operator</th>
						<th>Logic Values</th>
						<th>&nbsp;</th>
					</tr>
				</thead>
				<tbody>
					<% foreach (var clientDefinedRuleGroupLogic in Model.ClientDefinedRuleGroupLogics) { %>
						<tr id="ClientDefinedRuleGroupLogicRow_1" class="clientDefinedRuleGroupLogicRow" valign="top">
							<td width="5%"><label id="ClientDefinedRuleGroupLogicRowLabel_1" class="ClientDefinedRuleGroupLogicRowLabel">1</label></td>
							<td width="35%">
								<%= Html.Hidden("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_1", "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
								<%= Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_1", "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%>
							</td>
							<td width="25%"><%= Html.DropDownList("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_1", Model.ClientDefinedRuleRelationalOperators, "Please Select...", new { autocomplete="off",  @Class = "ClientDefinedRuleRelationalOperatorId" })%></td>
							<td width="25%"><%= Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_1", null, new { @Class = "ClientDefinedRuleLogicItemValue" })%></td>
							<td width="10%" align="right">
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
							</td>
						</tr>
					<% }  %>
				</tbody>
			</table>

			<!--Business Rule Results-->
			<table  cellpadding="0" border="0" width="100%" cellspacing="0" class="business-rules-table">
				<thead>
					<tr class="business-rules-table-sub-header">
						<td colspan="5">Business Rule Results</td>
					</tr>
					<tr class="business-rules-table-header">
						<th>&nbsp;</th>
						<th>Results Description</th>
						<th>&nbsp;</th>
						<th>Results Values</th>
						<th>&nbsp;</th>
					</tr>
				</thead>
				<tbody>
					<% foreach (var clientDefinedRuleGroupResult in Model.ClientDefinedRuleGroupResults) { %>
						<tr id="ClientDefinedRuleGroupResultRow_1" class="clientDefinedRuleGroupResultRow" valign="top">
							<td width="5%">&nbsp</td>
							<td width="35%">
								<%= Html.Hidden("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_1", "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
								<%= Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_1","", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%><span class="error"> *</span>
							</td>
							<td width="25%">&nbsp</td>
							<td width="25%"><%= Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_1", null, new { autocomplete="off", @Class = "ClientDefinedRuleResultItemValue" }) %><span class="error"> *</span></td>
							<td width="10%" align="right">
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
							</td>
						</tr>
					<% }  %>
				</tbody>
			</table>
      
			<!--Business Rule Triggers-->
			<table  cellpadding="0" border="0" width="100%" cellspacing="0" class="business-rules-table">
			<thead>
				<tr class="business-rules-table-sub-header">
					<td colspan="5">Business Rule Triggers</td>
				</tr>
				<tr class="business-rules-table-header">
					<th>&nbsp;</th>
					<th>Workflow Trigger State</th>
					<th>&nbsp;</th>
					<th>Application Mode</th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var clientDefinedRuleGroupTrigger in Model.ClientDefinedRuleGroupTriggers) { %>
					<tr id="ClientDefinedRuleGroupTriggerRow_1" class="clientDefinedRuleGroupTriggerRow" valign="top">
						<td width="5%">&nbsp</td>
						<td width="35%"><%= Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_1", Model.ClientDefinedRuleWorkflowTriggerStates, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerStateId"})%><span class="error"> *</span></td>
						<td width="25%">&nbsp</td>
						<td width="25%"><%= Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_1", Model.ClientDefinedRuleWorkflowTriggerApplicationModes, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerApplicationModeId" })%><span class="error"> *</span>
						</td>
						<td width="10%" align="right">
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
							</td>
					</tr>
				<% }  %>
			</tbody>
		</table>

		<table cellpadding="0" border="0" width="100%" cellspacing="0">
			<tr>
				<td class="row_footer_blank_left" width="80%"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
				<td class="row_footer_blank_right" width="20%"><input type="submit" value="Create Rules Group" class="red" title="Create Rules Group"/></td>
			</tr>
		</table>
		
		<% } %>
    </div>
</div>
    
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedRuleGroupCreate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Client Rules Groups", "ListUndeleted", "ClientDefinedRuleGroup")%> &gt;
Create
</asp:Content>