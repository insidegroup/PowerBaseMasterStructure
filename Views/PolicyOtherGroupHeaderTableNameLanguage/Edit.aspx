<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderTableNameLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Name Translation</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Table Name Translation</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
						<th class="row_header" colspan="3">Edit Table Name Translation</th> 
					</tr> 
					<tr>
						<td><label for="PolicyOtherGroupHeader_TableName">Table Name</label></td>
						<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableName)%></td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeaderTableNameLanguage_LanguageCode">Label Language</label></td>
						<td><%= Html.DropDownListFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.LanguageCode)%></td>
					</tr>
					<tr>
						<td><label for="PolicyOtherGroupHeaderTableNameLanguage_TableNameTranslation">Translation</label></td>
						<td><%= Html.TextBoxFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation, new { maxlength = "50" })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation)%></td>
					</tr>  
     				<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
					<tr>
						<td class="row_footer_blank_left" colspan="2">
							<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
							<a href="javascript:window.print();" class="red" title="Print">Print</a>
						</td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Table Name Label Translation" title="Edit Table Name Translation" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
   <script type="text/javascript">
   		$(document).ready(function () {
   			$("tr:odd").addClass("row_odd");
   			$("tr:even").addClass("row_even");
   			$('#menu_admin').click();
   			$('#search').hide();
   			$('#breadcrumb').css('width', 'auto');
   		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Header", "Main", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { title = "Policy Other Group Header" })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.Label, "Main", new { controller = "PolicyOtherGroupHeaderLabelLanguage", action = "List", id = Model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId }, new { title = Model.PolicyOtherGroupHeader.Label })%> &gt;
<%=Html.RouteLink(Model.PolicyOtherGroupHeader.TableName, "Main", new { controller = "PolicyOtherGroupHeaderTableNameLanguage", action = "List", id = Model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId }, new { title = Model.PolicyOtherGroupHeader.TableName })%> &gt;
Table Name Trans &gt;
Edit
</asp:Content>