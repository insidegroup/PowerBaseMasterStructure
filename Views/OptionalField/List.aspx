<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectOptionalFields_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Definitions</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Definitions</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="29%" class="row_header_left"><%= Html.RouteLink("Name", "ListMain", new { page = 1, sortField = "OptionalFieldDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="23%"><%= Html.RouteLink("Style", "ListMain", new { page = 1, sortField = "OptionalFieldStyleDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="15%">&nbsp;</th>
                    <th width="15%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="10%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.OptionalFieldName)%></td>                    
                    <td><%= Html.Encode(item.OptionalFieldStyleDescription) %></td>
                    <td><%= Html.RouteLink("Definition Values", new { controller = "OptionalFieldLanguage", action = "List", id = item.OptionalFieldId }, new { title = "Definition Values" })%></td>
					<td><%= Html.RouteLink("Look Up Values", new { controller = "OptionalFieldLookupValue", action = "List", id = item.OptionalFieldId }, new { title = "Look Up Values" })%></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.OptionalFieldId }, new { title = "View" })%> </td>
                    <td> <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.OptionalFieldId }, new { title = "Edit" })%><% } %> </td>
                    <td> <%if (ViewData["Access"] == "WriteAccess") { %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.OptionalFieldId }, new { title = "Delete" })%><% } %> </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="8" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                            <%}%> 
                            </div>
                            <div class="paging_centre"><%if (Model.TotalPages > 0){ %>Page <%=Model.PageIndex%> of <%=Model.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        <tr> 
                <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			    <td class="row_footer_blank_right" colspan="7">
			     <%if (ViewData["Access"] == "WriteAccess")
                { %>
                <%= Html.RouteLink("Create Optional Field", "Main", new { action = "Create" }, new { @class = "red", title = "Create Optional Field" })%>
			    <%} %></td>  
		    </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_passivesegmentbuilder').click();
        $('#menu_passivesegmentbuilder_optionalfields').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();

    })
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
      $("#frmSearch").attr("action", "/OptionalField.mvc/List");
      $("#frmSearch").submit();

  });

 </script>
 </asp:Content>