<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Message  Advice</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">GDS Response Message  Advice</div></div>
        <div id="content">
            <% Html.EnableClientValidation(); %>
			<% Html.EnableUnobtrusiveJavaScript(); %>
			<% using (Html.BeginForm()) {%>
				<%= Html.AntiForgeryToken() %>
				<table cellpadding="0" border="0" width="100%" cellspacing="0"> 
					<tr> 
						<th class="row_header" colspan="3">Edit GDS Response Message Advice</th> 
					</tr> 
					<tr>
						<td><label for="GDSEndWarningConfigurationLanguage_Language">Language</label></td>
						<td><%= Html.DropDownListFor(model => model.GDSEndWarningConfigurationLanguage.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSEndWarningConfigurationLanguage.LanguageCode)%></td>
					</tr>
					<tr valign="top">
						<td><label for="GDSEndWarningConfigurationLanguage_GDSEntry">GDS Response Message Advice</label></td>
						<td><%= Html.TextAreaFor(model => model.GDSEndWarningConfigurationLanguage.AdviceMessage, new { maxlength = "255"  })%><span class="error"> *</span></td>
						<td><%= Html.ValidationMessageFor(model => model.GDSEndWarningConfigurationLanguage.AdviceMessage)%></td>
					</tr>
					<tr>
						<td width="30%" class="row_footer_left"></td>
						<td width="40%" class="row_footer_centre"></td>
						<td width="30%" class="row_footer_right"></td>
					</tr>
				   <tr>
						<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
						<td class="row_footer_blank_right">
							<input type="submit" value="Edit GDS Response Message" title="Edit GDS Response Message" class="red"/>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfigurationLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId)%>
						</td>
					</tr>
			   </table>
			<%}%>
        </div>
    </div>
   <script type="text/javascript">
   	$(document).ready(function () {
   		//Navigation
   		$('#menu_admin').click();
   		$("tr:odd").addClass("row_odd");
   		$("tr:even").addClass("row_even");
   		$("#breadcrumb").css("width", "auto");
   	});
    </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("GDS Response Message", "Main", new { controller = "GDSEndWarningConfiguration", action = "List" }, new { title = "GDS Response Messages" })%> &gt;
<%=CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Model.GDSEndWarningConfiguration.IdentifyingWarningMessage, 30) %> &gt;
GDS Response Message Advice &gt;
Edit GDS Response Message Advice
</asp:Content>