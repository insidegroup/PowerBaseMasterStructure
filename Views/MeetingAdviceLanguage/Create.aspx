<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingAdviceLanguageVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></asp:Content>


<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></div></div>
    <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Meeting <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></th> 
		        </tr>  
                <tr>
					<td><label for="MeetingAdviceLanguage_LanguageCode">Language</label></td>
					<td><%= Html.DropDownListFor(model => model.MeetingAdviceLanguage.LanguageCode, Model.MeetingAdviceLanguages, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.MeetingAdviceLanguage.LanguageCode)%></td>
				</tr>  
				<tr>
					<td><label for="MeetingAdviceLanguage_MeetingAdvice"><%=Html.Encode(ViewData["MeetingAdviceTypeLabelName"]) %></label></td>
					<td><%= Html.TextAreaFor(model => model.MeetingAdviceLanguage.MeetingAdvice, new { maxlength = "1500", autocomplete = "off", style="height:500px;width:300px;" })%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.MeetingAdviceLanguage.MeetingAdvice)%></td>
				</tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Advice" title="Create Advice" class="red"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.MeetingID) %>
			<%= Html.HiddenFor(model => model.MeetingAdviceLanguage.MeetingAdviceTypeId) %>
    <% } %>
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
Create
</asp:Content>
