<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldLookupValueVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Look Up Values</asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Look Up Values</div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Optional Field Look Up Values</th> 
		        </tr>  
                <tr>
                    <td><label for="OptionalFieldLanguage_LabelText">Optional Field Look Up Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel, new { maxlength = "50", autocomplete = "off", style="width:250px;" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueLabel)%></td>
                </tr>
				<tr>
                    <td><label for="OptionalFieldLanguage_OptionalFieldLanguage">Language</label></td>
                    <td><%= Html.DropDownListFor(model => model.OptionalFieldLookupValueLanguage.LanguageCode, Model.OptionalFieldLookupValueLanguages, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OptionalFieldLookupValueLanguage.LanguageCode)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
               </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Optional Field Look Up Value" title="Edit Optional Field Look Up Value" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueId) %>
			<%= Html.HiddenFor(model => model.OptionalFieldLookupValueLanguage.LanguageCode) %>
			<%= Html.HiddenFor(model => model.OptionalFieldLookupValueLanguage.VersionNumber) %>
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
<%=Html.RouteLink(ViewData["OptionalFieldName"].ToString(), new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLookupValue.OptionalFieldId }, new { title = ViewData["OptionalFieldName"] })%>  &gt;
<%=Html.RouteLink("Definition Values", "Main", new { controller = "OptionalFieldLanguage", action = "List", id = Model.OptionalFieldLookupValue.OptionalFieldId }, new { title = "Definition Values" })%>  &gt;
<%=Model.OptionalFieldLookupValueLanguage.Language.LanguageName %>
</asp:Content>


