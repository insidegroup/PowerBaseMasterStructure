<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.AdditionalBookingCommentVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Additional Booking Comment</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
            <%= Html.AntiForgeryToken() %>      
            <table cellpadding="0" cellspacing="0" width="100%">
		        <tr> 
			        <th class="row_header" colspan="3">Create Additional Booking Comment</th> 
		        </tr> 
				<tr>
                    <td width="30%"><label for="AdditionalBookingComment_Language">Language</label></td>
                    <td width="50%"><%= Html.DropDownListFor(model => model.AdditionalBookingComment.LanguageCode, Model.Languages, "Please Select...")%><span class="error"> *</span></td>
                    <td width="20%"><%= Html.ValidationMessageFor(model => model.AdditionalBookingComment.LanguageCode)%></td>
                </tr>
				<tr>
                    <td><label for="AdditionalBookingComment_AdditionalBookingCommentDescription">Additional Booking Comment</label></td>
                    <td><%= Html.TextAreaFor(model => model.AdditionalBookingComment.AdditionalBookingCommentDescription, new { maxlength = "1500", @class="AdditionalBookingCommentDescription" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.AdditionalBookingComment.AdditionalBookingCommentDescription)%></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Additional Booking Comment" class="red" title="Create Additional Booking Comment"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.AdditionalBookingComment.BookingChannelId)%>
			<%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid) %>
		<% } %>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		//Navigation
		$('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#breadcrumb').css('width', 'auto');
	});
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Model.ClientSubUnit.ClientSubUnitName%>
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%= Html.RouteLink("Additional Booking Comments", "Main", new { controller = "BookingChannel", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid} , new { title = "Additional Booking Comments" })%> &gt; 
Create Additional Booking Comment
</asp:Content>