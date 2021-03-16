<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectReasonCodeProductTypeTravelerDescriptions_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Traveler Descriptions</div></div>
    <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="30%" class="row_header_left"><%=  Html.RouteLink("Language", "ListMain", new { productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="59%"><%=  Html.RouteLink("Traveler Description", "ListMain", new { productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"], page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "ReasonCodeProductTypeDescription" }, new { title = "Sort By Traveler Description" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
			        
		    </tr> 
            <%               
            foreach (var item in Model) {                   
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName)%></td>
                <td><%= Html.Encode(item.ReasonCodeProductTypeTravelerDescription)%></td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", languageCode = item.LanguageCode, productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"] }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){%>
                        <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", languageCode = item.LanguageCode, productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"] }, new { title = "Delete" })%>
                    <%} %>
                </td>             
            </tr>
            <% 
            } 
            %>
            <tr>
                <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"><% if (Model.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "ListMain", new { productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"], page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                        <div class="paging_right"> <% if (Model.HasNextPage){ %><%=  Html.RouteLink("Next Page>>", "ListMain", new { productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"], page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                        <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		    <tr> 
                <td class="row_footer_blank_left" ><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="3">
			    <%if (ViewData["Access"] == "WriteAccess"){ %>
			    <%= Html.RouteLink("Create Traveler Description", "Main", new { action = "Create", productId = ViewData["ProductId"], reasonCode = ViewData["ReasonCode"], reasonCodeTypeId = ViewData["ReasonCodeTypeId"] }, new { @class = "red", title = "Create Traveler Description" })%>
			    <% } %> 
			    </td> 
		    </tr> 	        
        </table>    
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#menu_reasoncodes').click();
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Product Types", "Main", new { controller = "ReasonCodeProductType", action = "List", }, new { title = "Reason Code Product Types" })%> &gt;
<%=ViewData["ReasonCodeItem"].ToString()%> &gt;
Traveler Descriptions
</asp:Content>
