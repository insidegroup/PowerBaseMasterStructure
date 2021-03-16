<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Policy Other Group Header</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Policy Other Group Header</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Policy Other Group Header</th> 
		        </tr> 
		        <tr>
                    <td><label for="PolicyOtherGroupHeader_PolicyOtherGroupHeaderServiceType_PolicyOtherGroupHeaderServiceTypeDescription">Service Type</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderServiceType.PolicyOtherGroupHeaderServiceTypeDescription)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_ProductName">Product</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.Product.ProductName)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_SubProductName">Sub Product</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.SubProduct != null ? Model.PolicyOtherGroupHeader.SubProduct.SubProductName : "")%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_Label">Label</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.Label)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_LabelLanguage">Label Language</label></td>
                    <td colspan="2"><%= Html.Encode("English (United Kingdom)")%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_TableDefinitionsAttachedFlag">Table?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableDefinitionsAttachedFlag)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_TableName">Table Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableName)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeader_TableNameLanguage">Table Name Language</label></td>
                    <td colspan="2"><%= Html.Encode("English (United Kingdom)")%></td>
                </tr>
                <tr>
                    <td><label for="PolicyOtherGroupHeader_RestrictAccessToAdminFlag">Restrict Access to Admin?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.RestrictAccessToAdminFlag)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyOtherGroupHeader_DisplayTopFlag">Display Top?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.DisplayTopFlag)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyOtherGroupHeader_DisplayBottomFlag">Display Bottom?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.DisplayBottomFlag)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyOtherGroupHeader_DisplayRestrictedTranslationInPowerLibraryFlag">Display Restricted Translation in Power Library?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.DisplayRestrictedTranslationInPowerLibraryFlag)%></td>
                </tr>
                <tr>
                    <td><label for="PolicyOtherGroupHeader_OnlineSensitiveDataFlag">Online Sensitive Data?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.OnlineSensitiveDataFlag)%></td>
                </tr>
                <tr>
                    <td width="40%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Delete Policy Other Group Header" title="Delete Policy Other Group Header" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
						<%}%>
                    </td>
                </tr>
           </table>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function () {
			$("tr:odd").addClass("row_odd");
			$("tr:even").addClass("row_even");
			$('#menu_admin').click();
		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Headers", "Main", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { title = "Policy Other Group Headers" })%> &gt;
Delete &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.Label) %>
</asp:Content>