<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedRuleGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Rules Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Client  Rules Groups</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0" class="main-table"> 
		        <tr> 
			        <th class="row_header" colspan="6">Edit Rules Group</th> 
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
					<% if(Model.ClientDefinedRuleGroup.IsMultipleHierarchy) { %>	
						<th><label for="ClientDefinedRuleGroup_HierarchyType">Hierarchy</label></th>
						<td>Multiple</td>
					<% } else { %>
						<th><label for="ClientDefinedRuleGroup_HierarchyType">Hierarchy Type</label></th>
						<td><%= Html.Encode(Model.ClientDefinedRuleGroup.HierarchyType) %></td>
					<% } %>
					<td>&nbsp;</td>
					<th><label for="ClientDefinedRuleGroup_ExpiryDate">Expiration Date</label> </th>
                    <td><%= Html.EditorFor(model => model.ClientDefinedRuleGroup.ExpiryDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.ExpiryDate)%></td>
                </tr>
                <tr>
					<% if(Model.ClientDefinedRuleGroup.IsMultipleHierarchy) { %>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					<% } else { %>
						<th><label id="lblHierarchyItem"/>Hierarchy Item</th>
						<td> <%= Html.Encode(Model.ClientDefinedRuleGroup.HierarchyItem)%></td>
					<% } %>
					<td>&nbsp;</td>
					<th><label for="ClientDefinedRuleGroup_TripTypeID">Trip Type</label></th>
                    <td><%= Html.DropDownListFor(model => model.ClientDefinedRuleGroup.TripTypeId, Model.TripTypes, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientDefinedRuleGroup.TripTypeId)%></td>
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
					<% 
						var counter = 1;
						foreach (var clientDefinedRuleGroupLogic in Model.ClientDefinedRuleGroupLogics) { %>
						<tr id="ClientDefinedRuleGroupLogicRow_<% =Html.Encode(counter) %>" class="clientDefinedRuleGroupLogicRow" valign="top">
							<td width="5%"><label id="ClientDefinedRuleGroupLogicRowLabel_1" class="ClientDefinedRuleGroupLogicRowLabel"><%=Html.Encode(counter) %></label></td>
							<td width="35%">
								<% if(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem != null && clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId != 0) { %>
									<%= Html.Hidden("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_" + counter, clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId, new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
									<%= Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_" + counter, clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityDescription, new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%>
								<% } else { %>
									<%= Html.Hidden("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_" + counter, "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
									<%= Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_" + counter, "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%>
								<% } %>
							</td>
							<td width="25%">
								<% if(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem != null && clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId != 0) { %>
									<%= Html.DropDownList("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_" + counter, clientDefinedRuleGroupLogic.ClientDefinedRuleRelationalOperators, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleRelationalOperatorId" })%>
								<% } else { %>
									<% =Html.DropDownList("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_" + counter, Model.ClientDefinedRuleRelationalOperators, "Please Select...", new { autocomplete="off",  @Class = "ClientDefinedRuleRelationalOperatorId" })%>
								<% } %>
							</td>
							<td width="25%">
								<% if(clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem != null && clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue != null) { %>
									<% =Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_" + counter, clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue,  new { @Class = "ClientDefinedRuleLogicItemValue" }) %>
							   <% } else { %>
									<% =Html.TextBox("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_" + counter, null, new { @Class = "ClientDefinedRuleLogicItemValue" })%>
							   <% } %>
							</td>
							<td width="10%" align="right">
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
							</td>
						</tr>
						<% counter++; 
					}  %>
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
					<% 
						counter = 1; 
						foreach (var clientDefinedRuleGroupResult in Model.ClientDefinedRuleGroupResults){ %>
						<tr id="ClientDefinedRuleGroupResultRow_<%= Html.Encode(counter) %>" class="clientDefinedRuleGroupResultRow" valign="top">
							<td width="5%">&nbsp</td>
							<td width="35%">
								<% if(clientDefinedRuleGroupResult.ClientDefinedRuleResultItem != null && clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId != 0) { %>
									<%= Html.Hidden("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_" + counter, clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId, new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
									<%= Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_" + counter, clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityDescription, new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%><span class="error"> *</span>
								<% } else { %>
									<% =Html.Hidden("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_" + counter, "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityId" })%>
									<% =Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_" + counter, "", new { autocomplete="off", @Class = "ClientDefinedRuleBusinessEntityName" })%><span class="error"> *</span>
								<% } %>
							</td>
							<td width="25%">&nbsp</td>
							<td width="25%">
								<% if(clientDefinedRuleGroupResult.ClientDefinedRuleResultItem != null && clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue != null) { %>
									<%= Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_" + counter, clientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue, new { autocomplete = "off", @Class = "ClientDefinedRuleResultItemValue" })%><span class="error"> *</span>
								<% } else { %>
									<% =Html.TextBox("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_" + counter, null, new { autocomplete="off", @Class = "ClientDefinedRuleResultItemValue" })%><span class="error"> *</span>
								<% } %>
							</td>
							<td width="10%" align="right">
								<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
								<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
							</td>
						</tr>
					<% counter++;
					}  %>
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
				<% 
					counter = 1; 
					foreach (var clientDefinedRuleGroupTrigger in Model.ClientDefinedRuleGroupTriggers) { %>
					<tr id="ClientDefinedRuleGroupTriggerRow_<%= Html.Encode(counter) %>" class="clientDefinedRuleGroupTriggerRow" valign="top">
						<td width="5%">&nbsp</td>
						<td width="35%">
							<% if(clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger != null && clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId != null) { %>
								<%= Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_" + counter, clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTriggerStates, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerStateId" })%><span class="error"> *</span>
							<% } else { %>
								<% =Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_" + counter, Model.ClientDefinedRuleWorkflowTriggerStates, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerStateId" })%><span class="error"> *</span>
							<% } %>
						</td>
						<td width="25%">&nbsp</td>
						<td width="25%">
							<% if(clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger != null && clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId != null) { %>
								<%= Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_" + counter, clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTriggerApplicationModes, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerApplicationModeId" })%><span class="error"> *</span>
							<% } else { %>
								<% =Html.DropDownList("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_" + counter, Model.ClientDefinedRuleWorkflowTriggerApplicationModes, "Please Select...", new { autocomplete="off", @Class = "ClientDefinedRuleWorkflowTriggerApplicationModeId" })%><span class="error"> *</span>
							<% } %>
						</td>
						<td width="10%" align="right">
							<a href="#" class="btn btn-add" title="Add"><img src="../../Images/Common/add.png" alt="Add" /></a> 
							<a href="#" class="btn btn-remove" title="Remove"><img src="../../Images/Common/minus.png" alt="Remove" /></a> 
						</td>
					</tr>
				<% counter++;
				} %>
			</tbody>
		</table>

		<table cellpadding="0" border="0" width="100%" cellspacing="0">
			<tr>
				<td class="row_footer_blank_left" width="50%"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
				<td class="row_footer_blank_right" width="50%">
                    <%= Html.RouteLink("Edit Order","Default", new { controller ="ClientDefinedRuleGroupLogic", action = "EditSequence", id = Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId }, new { @Class = "red", title = "Edit Order" })%>&nbsp;&nbsp;&nbsp;
					<input type="submit" value="Edit Rules Group" class="red" title="Edit Rules Group" style="height: 18px; line-height: 13px; position: relative; top: 3px;"/>
				</td>
			</tr>
		</table>

		<%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.VersionNumber)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.SourceSystemCode)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.HierarchyType)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.HierarchyCode)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.HierarchyItem)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.TravelerTypeGuid)%>
        <%= Html.HiddenFor(model => model.ClientDefinedRuleGroup.IsMultipleHierarchy)%>

		<% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/ClientDefinedRuleGroupCreate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%= Html.ActionLink("Client Rules Groups", "ListUndeleted", "ClientDefinedRuleGroup")%> &gt;
<%= Html.RouteLink(Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupName, "Default", new { action = "View", id = Model.ClientDefinedRuleGroup.ClientDefinedRuleGroupId }, new { title = "View" })%> &gt;
Items
</asp:Content>