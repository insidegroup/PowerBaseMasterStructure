<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Language Definitions</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Language Definitions</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Optional Field Language Definitions</th> 
		        </tr>  
                <tr>
                    <td>Language</td>
					<td><%= Html.Encode(Model.OptionalFieldLanguage.Language.LanguageName)%></td>
					<td></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_LabelText">Label Text</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldLanguage.OptionalFieldLabel, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLanguage.OptionalFieldLabel)%></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_TooltipText">Tool Tip Text</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldLanguage.OptionalFieldTooltip, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLanguage.OptionalFieldTooltip)%></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_DefaultText">Default Text</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldLanguage.OptionalFieldDefaultText, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLanguage.OptionalFieldDefaultText)%></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_OptionalFieldValidation">Optional Field Validation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldLanguage.OptionalFieldValidationRegularExpression, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLanguage.OptionalFieldValidationRegularExpression)%></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_ValidationFailedMessage">Validation Failed Message</label></td>
                    <td> <%= Html.TextBoxFor(model => model.OptionalFieldLanguage.OptionalFieldValidationFailureMessage, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLanguage.OptionalFieldValidationFailureMessage)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
               </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Optional Field Language Definition" title="Edit Optional Field Language Definition" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.OptionalFieldLanguage.LanguageCode) %>
			<%= Html.HiddenFor(model => model.OptionalFieldLanguage.OptionalFieldId) %>
			<%= Html.HiddenFor(model => model.OptionalFieldLanguage.VersionNumber) %>
    <% } %>
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
<%=Html.RouteLink(ViewData["OptionalFieldName"].ToString(), new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLanguage.OptionalFieldId }, new { title = ViewData["OptionalFieldName"] })%>  &gt;
<%=Html.RouteLink("Definition Values", "Main", new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLanguage.OptionalFieldId }, new { title = "Definition Values" })%>  &gt;
<%=Model.OptionalFieldLanguage.Language.LanguageName %>
</asp:Content>


