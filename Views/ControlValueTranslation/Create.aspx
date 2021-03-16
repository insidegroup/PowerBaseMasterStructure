<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ControlValueLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Control Value Translations</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr> 
                    <th class="row_header" colspan="3">Create Contorl Value Translation</th> 
                </tr>  
                <tr>
                    <td>Control Value</td>
                    <td><%= ViewData["ControlValue"].ToString() %></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="LanguageCode">Language</label></td>
                    <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
                </tr> 
                <tr>
                    <td><label for="ControlValueTranslation">Translation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.ControlValueTranslation, new { maxlength="256"})%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.ControlValueTranslation)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", new { id = Model.ControlValueId }, new { @class = "red", title = "Back To List" })%></td>
                    <td></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Translation" title="Create Translation" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.ControlValueId) %>
    <% } %>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    $('#menu_admin').click();
			$("tr:odd").addClass("row_odd");
	$("tr:even").addClass("row_even");
    })
 </script></asp:Content>

 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Contorl Values", "Main", new { controller = "ControlValue", action = "List", }, new { title = "Control Values" })%> &gt;
<%=Html.RouteLink(ViewData["ControlValue"].ToString(), "Main", new { controller = "ControlValue", action = "View", id = ViewData["ControlValueId"].ToString() }, new { title = ViewData["ControlValue"].ToString() })%> &gt;
Translations
</asp:Content>