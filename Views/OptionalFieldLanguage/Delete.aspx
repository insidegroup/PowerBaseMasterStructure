<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Language Definitions</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Optional Field Language Definitions</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Optional Field Language Definition</th> 
		    </tr> 
            <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.Language.LanguageName)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Label Text</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.OptionalFieldLabel)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Tool Tip Text</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.OptionalFieldTooltip)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Default Text</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.OptionalFieldDefaultText)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Optional Field Validation</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.OptionalFieldValidationRegularExpression)%></td>
                <td></td>
            </tr>
			<tr>
                <td>Validation Failed Message</td>
                <td><%= Html.Encode(Model.OptionalFieldLanguage.OptionalFieldValidationFailureMessage)%></td>
                <td></td>
            </tr>
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
					</td>
					<td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red" />
							<%= Html.HiddenFor(model => model.OptionalFieldLanguage.LanguageCode) %>
							<%= Html.HiddenFor(model => model.OptionalFieldLanguage.OptionalFieldId) %>
							<%= Html.HiddenFor(model => model.OptionalFieldLanguage.VersionNumber) %>
						<%}%>
                    </td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_passivesegmentbuilder').click();
		$('#menu_passivesegmentbuilder_optionalfields').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Definitions", "Main", new { controller = "OptionalField", action = "List", }, new { title = "Optional Field Definitions" })%> &gt;
<%=Html.ActionLink(ViewData["OptionalFieldName"].ToString(), "List", "OptionalFieldLanguage", new { id = Model.OptionalFieldLanguage.OptionalFieldId }, new { title = ViewData["OptionalFieldName"] })%>  &gt;
<%=Html.RouteLink("Definition Values", new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLanguage.OptionalFieldId }, new { title = "Definition Values" })%>  &gt;
<%=Model.OptionalFieldLanguage.Language.LanguageName %>
</asp:Content>

