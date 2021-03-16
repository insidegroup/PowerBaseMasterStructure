<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectFormOfPaymentAdviceMessageGroupItemTranslations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - FOP Advice Message Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">FOP Advice Message Translations</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="33%" class="row_header_left"><%=  Html.RouteLink("Language", "ListMain", new { id = ViewData["FormOfPaymentAdviceMessageGroupItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="52%"><%=  Html.RouteLink("Translation", "ListMain", new { id = ViewData["FormOfPaymentAdviceMessageGroupItemId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "FormOfPaymentAdviceMessageTranslation" }, new { title = "Sort By Translation" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th>
		    </tr> 
            <%
            foreach (var item in Model) {
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName) %></td>
                <td class="wrap-text"><%= Html.Encode(item.FormOfPaymentAdviceMessageTranslation) %></td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, id = item.FormOfPaymentAdviceMessageGroupItemId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, id = item.FormOfPaymentAdviceMessageGroupItemId }, new { title = "Delete" })%>
                    <%} %>
                </td>             
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupItemId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["FormOfPaymentAdviceMessageGroupItemId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left">
                    <a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    <a href="javascript:window.print();" class="red" title="Print">Print</a>
                </td>
			    <td class="row_footer_blank_right" colspan="3">
			    <%if (ViewData["Access"] == "WriteAccess"){ %>
			        <%= Html.RouteLink("Create", "Main", new { action = "Create", id = ViewData["FormOfPaymentAdviceMessageGroupItemId"] }, new { @class = "red", title = "Create" })%>
			    <% } %> 
			    </td>  
		    </tr> 	        
        </table>    
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_fopadvicemessages').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');
        $('#search_wrapper').css('height', 'auto');
    })
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("FOP Advice Message Groups", "Main", new { controller = "FormOfPaymentAdviceMessageGroup", action = "ListUnDeleted", }, new { title = "FOP Advice Message Groups" })%> &gt;
<%=Html.RouteLink(ViewData["FormOfPaymentAdviceMessageGroupName"].ToString(), "Default", new { controller = "FormOfPaymentAdviceMessageGroup", action = "View", id = ViewData["FormOfPaymentAdviceMessageGroupId"].ToString() }, new { title = ViewData["FormOfPaymentAdviceMessageGroupName"].ToString() })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "FormOfPaymentAdviceMessageGroupItem", action = "List", id = ViewData["FormOfPaymentAdviceMessageGroupId"].ToString() }, new { title = "Items" })%> &gt;
Translations
</asp:Content>