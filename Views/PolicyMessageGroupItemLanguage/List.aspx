<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyMessageGroupItemLanguagesVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Message</div></div>
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr> 
			    <th width="30%" class="row_header_left"><%=  Html.RouteLink("Language", "List", new { id = Model.PolicyMessageGroupItemId, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "LanguageName" }, new { title = "Sort By Language" })%></th>
                <th width="55%"><%=  Html.RouteLink("Policy Message", "List", new { id = Model.PolicyMessageGroupItemId, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "CountryAdvice" }, new { title = "Sort By Policy Message" })%></th>
			    <th width="4%">&nbsp;</th> 
			    <th width="4%">&nbsp;</th> 
			    <th width="7%" class="row_header_right">&nbsp;</th> 
		    </tr> 
            <%
                string controllerName = "PolicyMessageGroupItemLanguageAir";
                switch (Model.ProductId)
                {
                    case 3:
                        controllerName = "PolicyMessageGroupItemLanguageCar";
                        break;
                    case 2:
                        controllerName = "PolicyMessageGroupItemLanguageHotel";
                        break;
                }
                
            foreach (var item in Model.PolicyMessageGroupItemLanguages) {
               
            %>
            <tr>
                <td><%= Html.Encode(item.LanguageName)%></td>
                <td><%= Html.Encode(CWTStringHelpers.TrimString(item.PolicyMessageGroupItemTranslationMarkdown,50))%></td>
                <td><%=  Html.RouteLink("View", "LanguageView", new { controller = controllerName, action = "View", languageCode = item.LanguageCode, id = Model.PolicyMessageGroupItemId }, new { title = "View" })%></td>
                <td>
                   <%if (Model.HasWriteAccess){ %>
                        <%=  Html.RouteLink("Edit", "LanguageView", new { controller = controllerName, action = "Edit", languageCode = item.LanguageCode, id = Model.PolicyMessageGroupItemId }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                   <%if (Model.HasWriteAccess){ %>
                        <%=  Html.RouteLink("Delete", "LanguageView", new { controller = controllerName, action = "Delete", languageCode = item.LanguageCode, id = item.PolicyMessageGroupItemId }, new { title = "Delete" })%>
                    <%} %>
                </td>             
            </tr>
            <% 
            } 
            %>
            <tr>
            <td colspan="5" class="row_footer">
                <div class="paging_container">
                    <div class="paging_left"><% if (Model.PolicyMessageGroupItemLanguages.HasPreviousPage){ %><%=  Html.RouteLink("<<Previous Page", "List", new { id = ViewData["PolicyCountryGroupItemId"], page = (Model.PolicyMessageGroupItemLanguages.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), }, new { title = "Previous Page" })%><%}%></div>
                    <div class="paging_right"> <% if (Model.PolicyMessageGroupItemLanguages.HasNextPage)
                                                  { %><%=  Html.RouteLink("Next Page>>", "List", new { id = ViewData["PolicyCountryGroupItemId"], page = (Model.PolicyMessageGroupItemLanguages.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%><%}%> </div>
                    <div class="paging_centre"><%if (Model.PolicyMessageGroupItemLanguages.TotalPages > 0)
                                                 { %>Page <%=Model.PolicyMessageGroupItemLanguages.PageIndex%> of <%=Model.PolicyMessageGroupItemLanguages.TotalPages%><%} %></div>
                </div>
            </td>
        </tr> 
		<tr> 
            <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			<td class="row_footer_blank_right" colspan="4">
			    <%if (Model.HasWriteAccess){ %>
			    <%= Html.RouteLink("Create Policy Message", "Default", new { controller = controllerName, action = "Create", id = Model.PolicyMessageGroupItemId }, new { @class = "red", title = "Create Policy Message" })%>
			    <% } %> 
			</td> 
		</tr> 
    </table>    
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        //for pages with long breadcrumb and no search box
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%> &gt;
<%=Html.RouteLink(Model.PolicyGroupName, "Default", new { controller = "PolicyGroup", action = "View", id = Model.PolicyGroupId }, new { title = Model.PolicyGroupName })%> &gt;
<%=Html.RouteLink("Policy Message Group Items", "Default", new { controller = "PolicyMessageGroupItem", action = "List", id = Model.PolicyGroupId }, new { title = "Policy Message Group Items" })%> &gt;
<%=Html.RouteLink(Model.PolicyMessageGroupItemName, "Default", new { controller = "PolicyMessageGroupItem" + Model.ProductName, action = "View", id = Model.PolicyMessageGroupItemId }, new { title = Model.PolicyMessageGroupItemName })%> &gt;
Policy Message
</asp:Content>