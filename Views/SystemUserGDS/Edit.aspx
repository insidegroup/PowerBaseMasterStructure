<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUserGDS>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - System Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">System User GDS</div></div>
    <div id="content">
    <% Html.EnableClientValidation(); %>
    <% Html.EnableUnobtrusiveJavaScript(); %>
    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>        
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Edit System User GDS</th> 
		    </tr> 
            <tr>
                <td>System User Name</td>
                <td><strong><%= Html.Label(Model.SystemUserName)%></strong></td>
                <td></td>
            </tr>  
            <tr>
                <td>GDS</td>
                <td><strong><%= Html.Label(Model.GDSName)%></strong></td>
                <td></td>
            </tr>
            <%if(Model.GDSCode !="ALL"){ %>
                <tr>
                    <td><label for="PseudoCityOrOfficeId">Home Pseudo City or Office ID</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeId, new { maxlength = "9" })%><span class="error" id="errorPseudoCityOrOfficeId"></span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeId)%><label id="lblPseudoCityOrOfficeIdMsg"></label></td>
             </tr>         
             <tr>
                <td><label for="GDSSignOn">GDS Sign On ID</label></td>
                <td><%= Html.TextBoxFor(model => model.GDSSignOn, new { maxlength = "10" })%><span class="error"></span></td>
                <td><%= Html.ValidationMessageFor(model => model.GDSSignOn)%></td>
            </tr>
            <% } %> 
            <tr>
                <td>Default GDS?</td>
                <td><%= Html.CheckBoxFor(model => model.DefaultGDS)%></td>
                <td></td>
            </tr>           
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right"><input type="submit" value="Edit System User GDS" title="Edit System User GDS" class="red"/></td>
            </tr>
        </table>
        <%= Html.HiddenFor(model => model.VersionNumber) %>
        <%= Html.HiddenFor(model => model.SystemUserGuid) %>
        <%= Html.HiddenFor(model => model.GDSCode) %>
    <% } %>
    </div>
</div>
<script src="<%=Url.Content("~/Scripts/ERD/SystemUserGDS.js")%>" type="text/javascript"></script>
</asp:Content>


