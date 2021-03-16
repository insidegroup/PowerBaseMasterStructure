<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitMatrixDPCodesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Matrix DP Codes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Matrix DP Codes</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="30%" class="row_header_left"><%= Html.RouteLink("Hierarchy Item", "ListMain", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "HierarchyItem" }, new { title = "Hierarchy Item" })%></th>
			        <th width="30%"><%= Html.RouteLink("Hierarchy Type", "ListMain", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "HierarchyType" }, new { title = "Hierarchy Type" })%></th>
			        <th width="30%"><%= Html.RouteLink("Matrix DP Code", "ListMain", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = 1, sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "MatrixDPCode" }, new { title = "Matrix DP Code" })%></th>
			        <th width="5%">&nbsp;</th> 
			        <th width="5%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.MatrixDPCodes)
                    { 
                %>
                    <tr>
                        <td><%= Html.Encode(item.HierarchyItem)%></td>
                        <td><%= Html.Encode(item.HierarchyType) %></td>
                        <td><%= Html.Encode(item.MatrixDPCode) %></td>
                        <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Edit", "Edit", new { id = Model.ClientTopUnit.ClientTopUnitGuid, hierarchyCode = item.HierarchyCode, hierarchyType = item.HierarchyType }, new { title = "Edit" })%><%} %></td>
                        <td><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Delete", "Delete", new { id = Model.ClientTopUnit.ClientTopUnitGuid, hierarchyCode = item.HierarchyCode, hierarchyType = item.HierarchyType }, new { title = "Delete" })%><%} %></td>                
                    </tr>
                <% 
                } 
                %>
                 <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.MatrixDPCodes.HasPreviousPage)
                             { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.MatrixDPCodes.PageIndex - 1), sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "MatrixDPCode"  }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.MatrixDPCodes.HasNextPage)
                            { %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.MatrixDPCodes.PageIndex + 1), sortOrder = ViewData["NewSortOrder"].ToString(), sortField = "MatrixDPCode"  }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.MatrixDPCodes.TotalPages > 0) { %>Page <%=Model.MatrixDPCodes.PageIndex%> of <%=Model.MatrixDPCodes.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>
					<td class="row_footer_blank_right" colspan="10">
						<%if (ViewData["Access"] == "WriteAccess"){%>
							<%= Html.RouteLink("Create Matrix DP Code", "Main", new { id = Model.ClientTopUnit.ClientTopUnitGuid, action = "Create" }, new { @class = "red", title = "Create Matrix DP Codes" })%>
						<%} %>
			        </td>  
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
Matrix DP Codes
</asp:Content>
