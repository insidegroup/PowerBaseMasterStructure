<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderTableNameLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Table Name Translations</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Table Name Translation</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Table Name Translation</th> 
		        </tr> 
		        <tr>
					<td><label for="PolicyOtherGroupHeader_TableName">Table Name</label></td>
					<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableName)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeaderTableNameLanguage_TableNameTranslation">Translation</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeaderTableNameLanguage.TableNameTranslation)%></td>
                </tr>
				<tr>
                    <td><label for="PolicyOtherGroupHeaderTableNameLanguages_Language">Language</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeaderTableNameLanguage.Language.LanguageName) %></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Delete Table Name Translation" title="Delete Table Name Translation" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId)%>
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
Delete
</asp:Content>