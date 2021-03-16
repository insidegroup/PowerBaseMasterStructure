<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.TravelPortLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Travel Port Languages</div></div>
        <div id="content">
       <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr> 
                    <th class="row_header" colspan="3">Create Travel Port Language</th> 
                </tr>  
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.TravelPortCodeTravelPortName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="LanguageCode">Type</label></td>
                    <td><%= Html.DropDownList("TravelPortTypeId", ViewData["TravelPortTypeList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.TravelPortTypeId)%></td>
                </tr> 
                <tr>
                    <td><label for="LanguageCode">Language</label></td>
                    <td><%= Html.DropDownList("LanguageCode", ViewData["LanguageList"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
                </tr> 
                <tr>
                    <td><label for="TravelPortName">Name Translation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.TravelPortName, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.TravelPortName)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Travel Port Language" title="Create Travel Port Language" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.TravelPortCode) %>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/TravelPortLanguage.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Travel Ports", "Main", new { controller = "TravelPort", action = "List", }, new { title = "TravelPorts" })%> &gt;
<%=Html.RouteLink(Model.TravelPortCodeTravelPortName, "Main", new { controller = "TravelPort", action = "ViewItem", id = Model.TravelPortCode }, new { title = Model.TravelPortCodeTravelPortName })%> &gt;
<%=Html.RouteLink("Travel Port Languages", "Default", new { controller = "TravelPortLanguage", action = "View", id = Model.TravelPortCode }, new { title = "TravelPort Languages" })%>
</asp:Content>
