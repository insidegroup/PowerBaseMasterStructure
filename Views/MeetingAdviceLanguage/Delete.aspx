<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingAdviceLanguageVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">Delete Meeting <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></th> 
		    </tr> 
            <tr>
                <td>Language</td>
                <td><%= Html.Encode(Model.MeetingAdviceLanguage.Language.LanguageName)%></td>
                <td></td>
            </tr>
			<tr>
                <td><%=Html.Encode(ViewData["MeetingAdviceTypeLabelName"]) %></td>
                <td class="wrap-text"><%= Html.Encode(Model.MeetingAdviceLanguage.MeetingAdvice)%></td>
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
						<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.LanguageCode) %>
						<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.MeetingID) %>
						<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.MeetingAdviceTypeId) %>
						<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.VersionNumber) %>
					<%}%>
                </td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_meetings').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListUnDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.Meeting.ClientTopUnit.ClientTopUnitName) %> &gt;
<%=Html.Encode(Model.Meeting.MeetingName) %> &gt;
<%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %> &gt;
Delete
</asp:Content>
