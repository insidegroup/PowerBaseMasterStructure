<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CityLanguage>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">City Translation</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create City Translation</th> 
		        </tr>  
                <tr>
                    <td>City Name</td>
                    <td><%= Html.Encode(ViewData["CityName"].ToString())%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="LanguageCode">Language Code</label></td>
                    <td><%= Html.DropDownList("LanguageCode", ViewData["Languages"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.LanguageCode)%></td>
                </tr> 
                 <tr>
                    <td><label for="CityName">City Name Translation</label></td>
                    <td> <%= Html.TextBoxFor(model => model.CityName, new {maxlength="256"})%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.CityName)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create City Translation" title="Create City Translation" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.CityCode) %>
    <% } %>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Cities", "Main", new { controller = "City", action = "List", }, new { title = "Cities" })%> &gt;
<%=Html.RouteLink(ViewData["CityName"].ToString(), "Main", new { controller = "City", action = "View", id = ViewData["CityCode"].ToString() }, new { title = ViewData["CityName"].ToString() })%> &gt;
City Translations
</asp:Content>