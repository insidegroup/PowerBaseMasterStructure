<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.MeetingAdviceLanguagesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Groups - <%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %></div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="45%" class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = Model.Meeting.MeetingID, meetingAdviceTypeId = ViewData["MeetingAdviceTypeId"], page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="45%"><%= Html.RouteLink(ViewData["MeetingAdviceTypeLabelName"].ToString(), "ListMain", new { id = Model.Meeting.MeetingID, meetingAdviceTypeId = ViewData["MeetingAdviceTypeId"], page = 1, sortField = "MeetingAdvice", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.MeetingAdviceLanguages) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LanguageName)%></td>                    
                    <td class="wrap-text"><%= Html.Encode(item.MeetingAdvice) %></td>
                    <td> <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.MeetingID, meetingAdviceTypeId = item.MeetingAdviceTypeId,  languageCode = item.LanguageCode }, new { title = "Edit" })%><% } %> </td>
                    <td> <%if (ViewData["Access"] == "WriteAccess") { %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.MeetingID, meetingAdviceTypeId = item.MeetingAdviceTypeId, languageCode = item.LanguageCode }, new { title = "Delete" })%><% } %> </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="4" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.MeetingAdviceLanguages.HasPreviousPage){ %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List",  id = Model.Meeting.MeetingID, meetingAdviceTypeId = ViewData["MeetingAdviceTypeId"], page = (Model.MeetingAdviceLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.MeetingAdviceLanguages.HasNextPage){ %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List",  id = Model.Meeting.MeetingID, meetingAdviceTypeId = ViewData["MeetingAdviceTypeId"], page = (Model.MeetingAdviceLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.MeetingAdviceLanguages.TotalPages > 0){ %>Page <%=Model.MeetingAdviceLanguages.PageIndex%> of <%=Model.MeetingAdviceLanguages.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
					<a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
			    <td class="row_footer_blank_right" colspan="3">
			     <%if (ViewData["Access"] == "WriteAccess")
                { %>
                <%= Html.RouteLink("Create Advice", "Main", new { action = "Create", id = Model.Meeting.MeetingID, meetingAdviceTypeId = ViewData["MeetingAdviceTypeId"] }, new { @class = "red", title = "Create Advice" })%>
			    <%} %></td>  
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
<%=Html.Encode(ViewData["MeetingAdviceTypeName"]) %>
</asp:Content>