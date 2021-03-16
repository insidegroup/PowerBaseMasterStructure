<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectOptionalFieldLookupValues_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Optional Field Look Up Values</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Look Up Values</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="44%"><%= Html.RouteLink("Optional Field Look Up Values", "ListMain", new { id = ViewData["OptionalFieldId"], page = 1, sortField = "OptionalFieldLookupValueLabel", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
					<th width="44%" class="row_header_left"><%= Html.RouteLink("Language", "ListMain", new { id = ViewData["OptionalFieldId"], page = 1, sortField = "LanguageName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.OptionalFieldLookupValueLabel)%></td>                    
                    <td><%= Html.Encode(item.LanguageName) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.OptionalFieldLookupValueId }, new { title = "View" })%></td>
                    <td> <%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.OptionalFieldLookupValueId }, new { title = "Edit" })%><% } %> </td>
                    <td> <%if (ViewData["Access"] == "WriteAccess") { %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.OptionalFieldLookupValueId }, new { title = "Delete" })%><% } %> </td>
                </tr>        
                <% 
                } 
                %>
                <tr>
                    <td colspan="8" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left">
                            <% if (Model.HasPreviousPage){ %>
                            <%=Html.RouteLink("<<Previous Page", "ListMain", new { id =  ViewData["OptionalFieldId"], action = "List", page = (Model.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                            <%}%>
                            </div>
                            <div class="paging_right">
                            <% if (Model.HasNextPage){ %>
                            <%=Html.RouteLink("Next Page>>", "ListMain", new { id =  ViewData["OptionalFieldId"], action = "List", page = (Model.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
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
                <%= Html.RouteLink("Create Optional Field Look Up Value", "Main", new { action = "Create", id = ViewData["OptionalFieldId"] }, new { @class = "red", title = "Create Optional Field Look Up Value" })%>
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
		$('#ft').attr('name', 'id');
		$("#frmSearch input[name='id']").val(<%=ViewData["OptionalFieldId"]%>);

	})
	$('#btnSearch').click(function () {
		if ($('#filter').val() == "") {
			alert("No Search Text Entered");
			return false;
		}
		$("#frmSearch").attr("action", "/OptionalFieldLookupValue.mvc/List");
		$("#frmSearch").submit();

	});

 </script>
 </asp:Content>

 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Definitions", "Main", new { controller = "OptionalField", action = "List", }, new { title = "Optional Field Definitions" })%> &gt;
<%=Html.RouteLink(ViewData["OptionalFieldName"].ToString(), new { controller = "OptionalFieldLookupValue", action = "List", id = ViewData["OptionalFieldId"] }, new { title = ViewData["OptionalFieldName"] })%>  &gt;
Look Up Values
</asp:Content>
