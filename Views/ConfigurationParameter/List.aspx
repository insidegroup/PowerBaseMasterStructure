<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ConfigurationParametersVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Configuration Parameters</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Configuration Parameters</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="22%" class="row_header_left"><%=  Html.RouteLink("Configuration Parameter", "ListMain", new { page = 1, sortField = "ConfigurationParameterName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Configuration Parameter" })%></th>
                    <th width="20%"><%=  Html.RouteLink("Value", "ListMain", new { page = 1, sortField = "ConfigurationParameterValue", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Value" })%></th>
                    <th width="12%"><%=  Html.RouteLink("Application", "ListMain", new { page = 1, sortField = "ContextName", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Applicaiton" })%></th>
                    <th width="18%"><%=  Html.RouteLink("Updated", "ListMain", new { page = 1, sortField = "LastUpdateTimestamp", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Update Date" })%></th>
                    <th width="18%"><%=  Html.RouteLink("Updated By", "ListMain", new { page = 1, sortField = "SystemUserLoginIdentifier", sortOrder = ViewData["NewSortOrder"].ToString() }, new { title = "Sort By Update User" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="5%" class="row_header_right">&nbsp;</th> 
			        
		        </tr> 
                <%
                foreach (var item in Model.ConfigurationParameters) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ConfigurationParameterName) %></td>
                    <td><%= Html.Encode(item.ConfigurationParameterValue)%></td>
                    <td><%= Html.Encode(item.ContextName)%></td>                    
                    <td><%= Html.Encode(item.LastUpdateTimestamp.HasValue ? item.LastUpdateTimestamp.Value.ToString("MMM dd, yyyy") : "")%></td>
                    <td><%= Html.Encode(item.SystemUserLoginIdentifier)%></td>
                    <td><%=  Html.RouteLink("View", "Main", new { action = "ViewItem", id = item.ConfigurationParameterName }, new { title = "View" })%></td>
                    <td>
                        <%if(Model.HasWriteAccess){%>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ConfigurationParameterName }, new { title = "Edit" })%>
                        <%} %>
                    </td>           
                </tr>
                <% 
                } 
                %>
                <tr>
                <td colspan="7" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.ConfigurationParameters.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.ConfigurationParameters.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.ConfigurationParameters.HasNextPage)
                           {  %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.ConfigurationParameters.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString() }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.ConfigurationParameters.TotalPages > 0)
                                                     { %>Page <%=Model.ConfigurationParameters.PageIndex%> of <%=Model.ConfigurationParameters.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
            
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="6"></td> 
		        </tr> 
        </table>    
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_configparameters').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();

    })



    //Search
    $('#btnSearch').click(function () {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ConfigurationParameter.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>
