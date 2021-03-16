<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Helpers.CWTPaginatedList<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectClientFees_v1Result>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Base Client Fees</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr> 
			        <th width="43%" class="row_header_left"><%= Html.RouteLink("Fee Description", "ListMain", new { page = 1, sortField = "ClientFeeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="17%"><%= Html.RouteLink("FeeType", "ListMain", new { page = 1, sortField = "FeeTypeDescription", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="12%"><%= Html.RouteLink("Context", "ListMain", new { page = 1, sortField = "ContextName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="5%"><%= Html.RouteLink("GDS", "ListMain", new { page = 1, sortField = "GDSName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"] })%></th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model) { 
                %>
                <tr>
                    <td><%= Html.Encode(item.ClientFeeDescription)%></td>                    
                    <td><%
                        if(item.FeeTypeDescription=="Client Fee"){
                            %>Transaction Fee<%
                            }else{
                            %><%= Html.Encode(item.FeeTypeDescription)%><%
                            }%>
                     </td>
                    <td><%= Html.Encode(item.ContextName) %></td>
                    <td><%= Html.Encode(item.GDSName) %></td>
                    <td><%= Html.RouteLink("View", "Default", new { action = "View", id = item.ClientFeeId }, new { title = "View" })%> </td>
                    <td><%= Html.RouteLink("Alt Desc", "Default", new { controller = "ClientFeeLanguage", action = "List", id = item.ClientFeeId }, new { title = "Alternate Descriptions" })%></td>
                    <td> <%if (ViewData["Access"] == "WriteAccess")
          { %><%= Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientFeeId }, new { title = "Edit" })%><%
                            }%> </td>
                    <td> <%if (ViewData["Access"] == "WriteAccess")
          { %><%= Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientFeeId }, new { title = "Delete" })%><%
                            }%> </td>
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
                <%= Html.RouteLink("Create Base Client Fee", "Main", new { action = "Create" }, new { @class = "red", title = "Create Base Client Fee" })%>
			    <%} %></td>  
		    </tr> 
            </table>
        </div>
    </div> 
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();

    })
    $('#btnSearch').click(function() {
        if ($('#filter').val() == "") {
            alert("No Search Text Entered");
            return false;
        }
      $("#frmSearch").attr("action", "/ClientFee.mvc/List");
      $("#frmSearch").submit();

  });

 </script>
 </asp:Content>