<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Other Group Header</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Policy Other Group Header</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
						<th class="row_header" colspan="3">Edit Policy Other Group Header</th> 
					</tr> 
					<tr>
						<td><label for="PolicyOtherGroupHeader_ServiceTypeId">Service Type</label></td>
						<td><%= Html.Encode(Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderServiceType.PolicyOtherGroupHeaderServiceTypeDescription)%></td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeader_ProductId">Product</label></td>
						<td><%= Html.Encode(Model.PolicyOtherGroupHeader.Product.ProductName)%></td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeader_SubProductId">Sub Product</label></td>
						<td>
							<% if(Model.PolicyOtherGroupHeader.SubProduct != null) { %>
								<%= Html.Encode(Model.PolicyOtherGroupHeader.SubProduct.SubProductName)%>
							<% } else { %>
								&nbsp;
							<% } %>
						</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeader_Label">Label</label></td>
						<td><%= Html.TextBoxFor(model => model.PolicyOtherGroupHeader.Label, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.Label)%></td>
					</tr> 
					<tr>
						<td><label for="PolicyOtherGroupHeader_LanguageCode">Label Language</label></td>
						<td><%= Html.DropDownListFor(model => model.PolicyOtherGroupHeader.LabelLanguageCode, Model.Languages, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.LabelLanguageCode)%></td>
					</tr> 
					<tr>
						<td><label for="PolicyOtherGroupHeader_TableDefinitionsAttachedFlag">Table?</label></td>
						<td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.TableDefinitionsAttachedFlag)%></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.TableDefinitionsAttachedFlag)%></td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeader_TableName">Table Name</label></td>
						<td><%= Html.TextBoxFor(model => model.PolicyOtherGroupHeader.TableName, new { maxlength = "50" })%><span class="error" id="PolicyOtherGroupHeader_TableNameRequired"> *</span></td>
						<td>
							<%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.TableName)%>
							<span id="PolicyOtherGroupHeader_TableNameError" class="error"></span>
						</td>
					</tr> 
					<tr>
						<td><label for="PolicyOtherGroupHeader_TableNameLanguageCode">Table Language</label></td>
						<td><%= Html.DropDownListFor(model => model.PolicyOtherGroupHeader.TableNameLanguageCode, Model.Languages, "Please Select...")%></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.TableNameLanguageCode)%></td>
					</tr>
                    <tr>
                        <td><label for="PolicyOtherGroupHeader_RestrictAccessToAdminFlag">Restrict Access to Admin?</label></td>
                        <td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.RestrictAccessToAdminFlag)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.RestrictAccessToAdminFlag)%></td>
                    </tr>
                    <tr>
                        <td><label for="PolicyOtherGroupHeader_DisplayTopFlag">Display Top?</label></td>
                        <td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.DisplayTopFlag)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.DisplayTopFlag)%></td>
                    </tr>
                    <tr>
                        <td><label for="PolicyOtherGroupHeader_DisplayBottomFlag">Display Bottom?</label></td>
                        <td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.DisplayBottomFlag)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.DisplayBottomFlag)%></td>
                    </tr>
                    <tr>
                        <td><label for="PolicyOtherGroupHeader_DisplayRestrictedTranslationInPowerLibraryFlag">Display Restricted Translation in Power Library?</label></td>
                        <td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.DisplayRestrictedTranslationInPowerLibraryFlag)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.DisplayRestrictedTranslationInPowerLibraryFlag)%></td>
                    </tr>
                    <tr>
                        <td><label for="PolicyOtherGroupHeader_OnlineSensitiveDataFlag">Online Sensitive Data?</label></td>
                        <td><%= Html.CheckBoxFor(model => model.PolicyOtherGroupHeader.OnlineSensitiveDataFlag)%></td>
                        <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeader.OnlineSensitiveDataFlag)%></td>
                    </tr>
					<tr>
						<td width="40%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="20%" class="row_footer_right"></td>
					</tr>
				   <tr>
						<td class="row_footer_blank_left" colspan="2">
							<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
							<a href="javascript:window.print();" class="red" title="Print">Print</a>
						</td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Edit Policy Other Group Header" title="Edit Policy Other Group Header" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderServiceTypeId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.ProductId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.SubProductId)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
    
	<script src="<%=Url.Content("~/Scripts/ERD/PolicyOtherGroupHeader.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Headers", "Main", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { title = "Policy Other Group Headers" })%> &gt;
Edit &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.Label) %>
</asp:Content>