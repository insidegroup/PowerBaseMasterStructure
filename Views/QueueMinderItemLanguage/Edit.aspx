<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.QueueMinderItemLanguageVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Alternate Descriptions</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Alternate Description</th> 
		        </tr>  
                 <tr>
                    <td>Language</td>
                    <td><%= Html.Encode(Model.QueueMinderItemLanguage.Language.LanguageName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="QueueMinderItemLanguage_QueueMinderItemLanguageItineraryDescription">Description</label></td>
                    <td> <%= Html.TextBoxFor(model => model.QueueMinderItemLanguage.QueueMinderItemLanguageItineraryDescription, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td> <%= Html.ValidationMessageFor(model => model.QueueMinderItemLanguage.QueueMinderItemLanguageItineraryDescription)%> </td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Alternate Description" title="Edit Alternate Description" class="red"/></td>
                </tr>
            </table>
<%= Html.HiddenFor(model => model.QueueMinderItemLanguage.QueueMinderItemId)%>
<%= Html.HiddenFor(model => model.QueueMinderItemLanguage.LanguageCode)%>
<%= Html.HiddenFor(model => model.QueueMinderItemLanguage.VersionNumber)%>
    <% } %>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_ticketqueuegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName, "Default", new { controller = "FollowUpQueueGroup", action = "View", id = Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderGroupId }, new { title = Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderGroup.QueueMinderGroupName })%> &gt;
<%=Html.RouteLink("Queue Minder Items", "Default", new { controller = "QueueMinderItem", action = "List", id = Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderGroupId }, new { title = "Queue Minder Items" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderItemDescription, "Default", new { controller = "QueueMinderItem", action = "View", id = Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderItemId }, new { title = Model.QueueMinderItemLanguage.QueueMinderItem.QueueMinderItemDescription })%>
</asp:Content>