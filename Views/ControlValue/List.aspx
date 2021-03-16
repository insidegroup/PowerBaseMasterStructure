<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectControlValues_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Control Values</div></div>
        <div id="content">
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
                    <th width="10%" class="row_header_left"><%=  Html.RouteLink("DisplayOrder", "ListMain", new { controller = "ControlValue", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "DisplayOrder" }, new { title = "Value" })%></th>
                    <th width="35%"><%=  Html.RouteLink("Value", "ListMain", new { controller = "ControlValue", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ControlValue" }, new { title = "ControlValue" })%></th>
                    <th width="28%"><%=  Html.RouteLink("Name", "ListMain", new { controller = "ControlValue", action = "List", page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], sortField = "ControlName" }, new { title = "ControlName" })%></th>
			        <th width="10%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
               
                foreach (var item in Model) { 
               
                %>
                <tr>
                    <td><%= Html.Encode(item.DisplayOrder) %></td>
                    <td><%= Html.Encode(item.ControlValue) %></td>
                    <td><%= Html.Encode(item.ControlName) %></td>
                    <td><%= Html.RouteLink("Translations", "List", new { controller="ControlValueTranslation", action="List", id = item.ControlValueId }, new { title = "Translations" })%> </td>
                    <td><%= Html.RouteLink("View", "Default", new { id = item.ControlValueId, action = "View" }, new { title = "View" })%> </td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Edit", "Default", new { id = item.ControlValueId, action = "Edit" }, new { title="Edit"})%><%} %></td>
                    <td><%if (ViewData["Access"] == "WriteAccess"){%><%= Html.RouteLink("Delete", "Default", new { id = item.ControlValueId, action = "Delete" }, new { title = "Delete" })%><%} %></td>
                </tr>
                <% 
                } 
                %>
                <tr>
                    <td colspan="7" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                                <% if (Model.HasPreviousPage){ %>
                            <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ControlValue", action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                                <% if (Model.HasNextPage){ %>
                            <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ControlValue", action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
                <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="5">
                    <%if (ViewData["Access"] == "WriteAccess"){%>
                    <%= Html.RouteLink("Create Control Value", "Main", new { action = "Create" }, new { @class = "red", title = "Create Control Value" })%><%} %></td> 
                </tr> 
        </table>    
        </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $('#menu_admin').click();
    })
    colspan = "5"
    //Search
    $('#btnSearch').click(function() {
	
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
        $("#frmSearch").attr("action", "/ControlValue.mvc/List");
        $("#frmSearch").submit();

    });
 </script>
</asp:Content>
