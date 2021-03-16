<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderTableNameLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Name Translations</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Table Name Translations</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Table Name Translation</th> 
		        </tr> 
				<tr>
					<td><label for="PolicyOtherGroupHeader_TableName">Table Name</label></td>
					<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableName)%></td>
                </tr>	
				<tr>
                    <td><label for="PolicyOtherGroupHeaderLabelTranslation_LanguageCode">Label Language</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.LanguageCode)%></td>
                </tr> 
				<tr>
                    <td><label for="PolicyOtherGroupHeaderLabelTranslation_TableNameTranslation">Translation</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Table Name Translation" class="red" title="Create Table Name Translation"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
		<% } %>
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
Create
</asp:Content>