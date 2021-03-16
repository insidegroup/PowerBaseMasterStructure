<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSEndWarningConfigurationLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - GDS Response Message Advice</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Delete Response Message Advice</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GDS Response Message Advice</th> 
		        </tr> 
		        <tr>
                    <td><label for="GDSEndWarningConfigurationLanguage_LanguageName">Language</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSEndWarningConfigurationLanguage.Language.LanguageName)%></td>
                </tr>
				<tr>
                    <td><label for="GDSEndWarningConfigurationLanguage_AdviceMessage">GDS Response Message Advice</label></td>
                    <td colspan="2"><%= Html.Encode(Model.GDSEndWarningConfigurationLanguage.AdviceMessage)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Delete GDS Response Message Advice" title="Delete GDS Response Message Advice" class="red"/>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfigurationLanguage.VersionNumber)%>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId)%>
							<%= Html.HiddenFor(model => model.GDSEndWarningConfigurationLanguage.LanguageCode)%>
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
			$("#breadcrumb").css("width", "auto");
		});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("GDS Response Message", "Main", new { controller = "GDSEndWarningConfiguration", action = "List" }, new { title = "GDS Response Message Advice" })%> &gt;
<%=CWTDesktopDatabase.Helpers.CWTStringHelpers.TrimString(Model.GDSEndWarningConfiguration.IdentifyingWarningMessage, 30) %> &gt;
GDS Response Message Advice &gt;
Delete GDS Response Message Advice
</asp:Content>