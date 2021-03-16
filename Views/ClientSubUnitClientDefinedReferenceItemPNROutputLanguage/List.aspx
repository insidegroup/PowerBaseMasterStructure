<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteFullWidth.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientDefinedReferenceItemPNROutputLanguages_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Sub Units - CDRs - PNR Formats - Remark Translations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Remark Translations</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
		        <th width="30%" class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { action = "List", id = ViewData["ClientDefinedReferenceItemPNROutputId"], csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "LanguageName", }, new { title = "Language" })%></th> 
		        <th width="58%"><%= Html.RouteLink("Translation", "ListMain", new { action = "List", id = ViewData["ClientDefinedReferenceItemPNROutputId"], csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "RemarkTranslation", }, new { title = "Translation" })%></th> 
				<th width="6%">&nbsp;</th> 
				<th width="6%" class="row_header_right">&nbsp;</th> 
	        </tr> 
            <%
            foreach (var item in Model) { 
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName) %></td>
                <td><%= Html.Encode(item.RemarkTranslation)%></td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess" ){%>
							<%= Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientDefinedReferenceItemPNROutputId, languageCode = item.LanguageCode, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { title = "Edit" })%>
					<% } %>
				</td>
				<td>
					<%if(ViewData["Access"] == "WriteAccess"){%>
						<%= Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientDefinedReferenceItemPNROutputId, languageCode = item.LanguageCode, csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"]}, new { title = "Delete" })%>
					<% } %>
				</td>
            </tr>        
            <% 
            } 
            %>
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.HasNextPage) {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientDefinedReferenceItem", action = "List", id = ViewData["ClientSubUnitGuid"].ToString(), page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
	        <tr> 
	            <td class="row_footer_blank_left" colspan="1">
					<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a>
	            </td>
		        <td class="row_footer_blank_right" colspan="3">
					<%if(ViewData["Access"] == "WriteAccess"){%>
						<%= Html.RouteLink("Create Translation", "Main", new { action="Create", id = ViewData["ClientDefinedReferenceItemPNROutputId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString(), can = ViewData["ClientAccountNumber"], ssc = ViewData["SourceSystemCode"]}, new { @class = "red", title = "Create Translation" })%>
					<% } %>
		        </td> 
	        </tr> 
        </table>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', 'auto');

    	//Search
        $('#search').hide();

    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientTopUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientTopUnitName"].ToString() })%> &gt;
<%=Html.RouteLink("ClientSubUnits", "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "ClientSubUnits" })%> &gt;
<%=Html.RouteLink(ViewData["ClientSubUnitName"].ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = ViewData["ClientTopUnitGuid"].ToString() }, new { title = ViewData["ClientSubUnitName"].ToString() })%> &gt;
<%=Html.RouteLink(ViewData["DisplayNameAlias"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItem", action = "ListBySubUnit", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["DisplayNameAlias"].ToString() })%> &gt;
<%=Html.RouteLink("PNR Formats", "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List", id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = "PNR Formats" })%> &gt;
<%=Html.RouteLink(ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"].ToString(), "Main", new { controller = "ClientSubUnitClientDefinedReferenceItemPNROutput", action = "List",  id = ViewData["ClientDefinedReferenceItemId"].ToString(), csu = ViewData["ClientSubUnitGuid"].ToString() }, new { title = ViewData["ClientDefinedReferenceItemPNROutputDefaultRemark"].ToString() })%> &gt;
Translations
</asp:Content>