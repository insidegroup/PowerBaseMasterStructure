<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client Accounts – CDRs</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Create CDR</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.DisplayNameAlias, new { maxlength = 50 } )%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.DisplayNameAlias)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_DisplayName">Back Office Display Name</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.DisplayName, new { maxlength = 50 })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.DisplayName)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_EntryFormat">Entry Format</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.EntryFormat, new { maxlength = 50 } )%></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.EntryFormat)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_MandatoryFlag">Mandatory?</label></td>
						<td><%= Html.CheckBoxFor(model => model.ClientDefinedReferenceItem.MandatoryFlagNullable)%></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MandatoryFlagNullable)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_MinLength">Min Length</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.MinLength, new { @Value = "0" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MinLength)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_MaxLength">Max Length</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.MaxLength)%><span class="error"> *</span></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MaxLength)%>
							<span class="error" id="ClientDefinedReferenceItem_MinLengthError"></span><br />
							<span class="error" id="ClientDefinedReferenceItem_EntryFormatError"></span>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_Droplist">Drop List?</label></td>
						<td><%= Html.CheckBoxFor(model => model.ClientDefinedReferenceItem.TableDrivenFlagNullable)%></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.TableDrivenFlagNullable)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_OnlineDefaultValue">Online Default Value</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.OnlineDefaultValue, new { maxlength = 50 } )%></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.OnlineDefaultValue)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_ClientDefinedReferenceItemProductIds">Product</label></td>
						<td valign="top" class="listbox listbox-large">
							<%= Html.ListBoxFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemProductIds, Model.Products, "Please Select...")%>
							<span> Hold CNTRL to select multiple</span>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemProductIds)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_ClientDefinedReferenceItemContextIds">Context</label></td>
						<td valign="top" class="listbox listbox-large">
							<%= Html.ListBoxFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemContextIds, Model.Contexts, "Please Select...")%>
							<span> Hold CNTRL to select multiple</span>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemContextIds)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2"><input type="submit" value="Create CDR" title="Create CDR" class="red" /></td>
					</tr>
				</table>
				<%=Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItem.ClientSubUnitGuid) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItem.ClientAccountNumber)	%>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItem.SourceSystemCode) %>
			<% } %>
		</div>
	</div>
	<script type="text/javascript" src="../../Scripts/ERD/ClientDefinedReferenceItem.js"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("Client Accounts", "Main", new { controller = "ClientSubUnitClientAccount", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Client Accounts" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitClientAccountClientAccountName"].ToString(), "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = ViewData["ClientSubUnitClientAccountClientAccountName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid, ssc = Model.ClientDefinedReferenceItem.SourceSystemCode, can = Model.ClientDefinedReferenceItem.ClientAccountNumber}, new { title = "CDRs" })%> &gt;
Create CDR
</asp:Content>
