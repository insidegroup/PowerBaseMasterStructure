<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientDefinedReferenceItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - CDRs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="contentarea">
		<div id="banner">
			<div id="banner_text">Client SubUnit – CDRs</div>
		</div>
		<div id="content">
			<% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<%= Html.ValidationSummary(true) %>
				<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<th class="row_header" colspan="3">Edit CDR</th>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_DisplayNameAlias">Display Name Alias</label></td>
						<td><%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.DisplayNameAlias, new { maxlength = 50 } )%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.DisplayNameAlias)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_DisplayName">Back Office Display Name</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.DisplayName)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.DisplayName)%>
							<% } else { %>
								<%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.DisplayName, new { maxlength = 50 })%><span class="error"> *</span>
							<% } %>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.DisplayName)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_EntryFormat">Entry Format</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.EntryFormat)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.EntryFormat)%>
							<% } else { %>
								<%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.EntryFormat, new { maxlength = 50 } )%>
							<% } %>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.EntryFormat)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_Mandatory">Mandatory?</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.MandatoryFlagNullable)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.MandatoryFlagNullable)%>
							<% } else { %>
								<%= Html.CheckBoxFor(model => model.ClientDefinedReferenceItem.MandatoryFlagNullable)%>
							<% } %>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MandatoryFlagNullable)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_MinLength">Min Length</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.MinLength)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.MinLength)%>
							<% } else { %>
								<%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.MinLength)%><span class="error"> *</span>
							<% } %>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MinLength)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_MaxLength">Max Length</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.MaxLength)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.MaxLength)%>
							<% } else { %>
								<%= Html.TextBoxFor(model => model.ClientDefinedReferenceItem.MaxLength)%><span class="error"> *</span>
							<% } %>
						</td>
						<td>
							<%= Html.ValidationMessageFor(model => model.ClientDefinedReferenceItem.MaxLength)%>
							<span class="error" id="ClientDefinedReferenceItem_MinLengthError"></span><br />
							<span class="error" id="ClientDefinedReferenceItem_EntryFormatError"></span>
						</td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_Droplist">Drop List?</label></td>
						<td>
							<% if(Model.ClientDefinedReferenceItem.BackOfficeDataSourceId == 0) {%>
								<%= Html.Encode(Model.ClientDefinedReferenceItem.TableDrivenFlagNullable)%>
								<%= Html.HiddenFor(model => model.ClientDefinedReferenceItem.TableDrivenFlagNullable)%>
							<% } else { %>
								<%= Html.CheckBoxFor(model => model.ClientDefinedReferenceItem.TableDrivenFlagNullable)%>
							<% } %>
						</td>
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
							<%= Html.ListBoxFor(model => model.ProductList, Model.Products, "Please Select...")%>
							<span> Hold CNTRL to select multiple</span>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ProductList)%></td>
					</tr>
					<tr>
						<td><label for="ClientDefinedReferenceItem_ClientDefinedReferenceItemContextIds">Context</label></td>
						<td valign="top" class="listbox listbox-large">
							<%= Html.ListBoxFor(model => model.ContextList, Model.Contexts, "Please Select...")%>
							<span> Hold CNTRL to select multiple</span>
						</td>
						<td><%= Html.ValidationMessageFor(model => model.ContextList)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
						<td class="row_footer_blank_right" colspan="2">
							<input type="submit" value="Edit CDR" title="Edit CDR" class="red" />
						</td>
					</tr>
				</table>	
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItem.VersionNumber) %>
				<%=Html.HiddenFor(model => model.ClientDefinedReferenceItem.ClientDefinedReferenceItemId) %>
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
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List" }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("CDRs", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDRs" })%> &gt;
Edit CDR
</asp:Content>
