<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectPhraseTranslations_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Phrase Translations</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="33%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = ViewData["PhraseId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="52%"><%=  Html.RouteLink("Phrase Name", "List", new { id = ViewData["PhraseId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "PhraseName" }, new { title = "Sort By Phrase Name" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 		        
		    </tr> 
            <%               
            foreach (var item in Model) {                   
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName) %></td>
                <td><%= Html.Encode(item.PhraseTranslation)%></td>
                <td>
                        <%=  Html.RouteLink("View", "Main", new { action = "ViewItem", languageCode = item.LanguageCode, id = item.PhraseId }, new { title = "View" })%>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, id = item.PhraseId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, id = item.PhraseId }, new { title = "Delete" })%>
                    <%} %>
                </td>             
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["PhraseId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["PhraseId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="4">
			    <%if (ViewData["Access"] == "WriteAccess"){ %>
			    <%= Html.RouteLink("Create Phrase Translation", "Main", new { action = "Create", id = ViewData["PhraseId"] }, new { @class = "red", title = "Create Car Advice" })%>
			    <% } %> 
			    </td>  
		    </tr> 	        
        </table>    
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
<%=Html.RouteLink("Phrases", "Main", new { controller = "Phrase", action = "List", }, new { title = "Phrases" })%> &gt;
<%=Html.RouteLink(ViewData["PhraseName"].ToString(), "Main", new { controller = "Phrase", action = "ViewItem", id = ViewData["PhraseId"].ToString() }, new { title = ViewData["PhraseName"].ToString() })%> &gt;
Phrase Translations
</asp:Content>

